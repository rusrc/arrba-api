using Arrba.Services.Logger;
using Newtonsoft.Json;
using StackExchange.Redis;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Arrba.Services
{
    public class CacheService
    {
        private static Lazy<ILogService> LogService => new Lazy<ILogService>(() => new LogService());
        //const string redisConnection = "arrba.redis.cache.windows.net:6380,password=9J71riJ6rPp72AhqdOhAHLVRITfjS9BdJqetZjMOxgo=,ssl=True,abortConnect=False";
        private static Lazy<ConfigurationOptions> configOptions
                = new Lazy<ConfigurationOptions>(() =>
                {
                    var options = new ConfigurationOptions
                    {
                        EndPoints = { { "arrba.redis.cache.windows.net:6380" } },
                        Password = "6cV6QvQnRljPUhp145JwyVPZAOPvPvlts1NHLttfRRA=",
                        Ssl = true,
                        ClientName = "ARRBA_CACHE_SERVICE",
                        ConnectTimeout = 100000,
                        SyncTimeout = 100000,
                        AbortOnConnectFail = false,
                    };
                    return options;
                });

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection
            = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configOptions.Value));

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static string Ping()
        {
            var db = Connection.GetDatabase();
            var result = db.Execute("ping").ToString();

            return result;
        }

        public static string FlushDb()
        {
            var db = Connection.GetDatabase();
            var result = db.Execute("flushdb").ToString();

            return result;
        }

        /// <summary>
        /// Get data from cache
        /// </summary>
        /// <typeparam name="TObject">Resulted object</typeparam>
        /// <param name="key">Name of key in cache</param>
        /// <returns></returns>
        public static TObject GetData<TObject>(string key) where TObject : class
        {
            IDatabase db = Connection.GetDatabase();

            if (db.KeyExists(key))
            {
                var cachedResult = db.StringGet(key).ToString();
                var objectResult = JsonConvert.DeserializeObject<TObject>(cachedResult);

                return objectResult;
            }

            return null;
        }

        public static bool SetData(string key, object value)
        {
            IDatabase db = Connection.GetDatabase();
            var jsonValue = JsonConvert.SerializeObject(value);

            return db.StringSet(key, jsonValue);
        }

        public static Task SetDataAsync(string key, object value, TimeSpan? expiry = null)
        {
            IDatabase db = Connection.GetDatabase();
            var jsonValue = JsonConvert.SerializeObject(value);
            var result = db.StringSetAsync(key, jsonValue, expiry)
                    .ContinueWith(async t =>
                    {
                        if (t.IsFaulted is false)
                        {
                            LogService.Value.Info("Set cache by key: " + key + " successfully");
                        }

                        if (t.IsFaulted)
                        {
                            await DeleteKeyAsync(key);
                            LogService.Value.Error(t.Exception?.Message, t.Exception);
                        }
                    });

            return result;
        }

        public static async Task<IEnumerable<T>> GetListAsync<T>(string key) where T : class
        {
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                //MissingMemberHandling = MissingMemberHandling.Ignore
            };

            IDatabase db = Connection.GetDatabase();
            if (db.KeyExists(key))
            {
                var cachedList = await db.ListRangeAsync(key);

                return cachedList.Select(e =>
                {
                    var r = e.ToString();
                    var o = JsonConvert.DeserializeObject<T>(r, settings);
                    return o;
                });
            }

            return null;
        }

        public static void SetList<T>(string key, IEnumerable<T> list) where T : class
        {
            IDatabase db = Connection.GetDatabase();
            if (db.KeyExists(key) == false)
            {
                foreach (var item in list)
                {
#if DEBUG
                    var sw = new Stopwatch(); sw.Start(); 
#endif

                    var stringObject = JsonConvert.SerializeObject(item);
                    var result = db.ListRightPush(key, stringObject);

#if DEBUG
                    sw.Stop();
                    LogService.Value.Info($"RPUSH {key}; result: {result}; length: {list.Count()}, TimeMs: {sw.ElapsedMilliseconds}"); 
#endif
                }
            }
        }

        public static async Task SetListAsync<T>(string key, IEnumerable<T> list) where T : class
        {
            IDatabase db = Connection.GetDatabase();
            if ((await db.KeyExistsAsync(key)) is false)
            {
                foreach (var item in list)
                {
#if DEBUG
                    var sw = new Stopwatch(); sw.Start();
#endif

                    var stringObject = JsonConvert.SerializeObject(item);
                    var result = db.ListRightPushAsync(key, stringObject);

#if DEBUG
                    sw.Stop();
                    LogService.Value.Info($"RPUSH {key}; result: {result}; length: {list.Count()}, TimeMs: {sw.ElapsedMilliseconds}");
#endif
                }
            }
        }

        public static bool DeleteKey(string key)
        {
            IDatabase db = Connection.GetDatabase();
            return db.KeyDelete(key);
        }

        public static async Task<bool> DeleteKeyAsync(string key)
        {
            IDatabase db = Connection.GetDatabase();
            return await db.KeyDeleteAsync(key);
        }
    }
}
