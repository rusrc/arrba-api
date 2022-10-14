using log4net;
using log4net.Config;

using System;
using System.Reflection;
using System.Xml;

namespace Arrba.Services.Logger
{
    // TODO add configuration into web.config file https://www.eyecatch.no/blog/logging-with-log4net-in-c/
    public class LogService : ILogService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(LogService));
        public LogService()
        {
            var xml = @"<log4net>
                        <appender name=""PapertrailRemoteSyslogAppender"" type=""log4net.Appender.RemoteSyslogAppender"">
                          <facility value=""Local6"" />
                          <identity value=""%date{yyyy-MM-ddTHH:mm:ss.ffffffzzz} %P{log4net:HostName} Arrba.API"" />
                          <layout type=""log4net.Layout.PatternLayout"" value=""%level - %message%newline"" />
                          <remoteAddress value=""logs2.papertrailapp.com"" />
                          <remotePort value=""16323"" />
                        </appender>
                        <root>
                          <level value=""DEBUG"" />
                          <appender-ref ref=""PapertrailRemoteSyslogAppender"" />
                        </root>
                      </log4net>";


            var repo = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlConfigurator.Configure(repo, doc.DocumentElement);
        }

        public void Info(string msg)
        {
            _log.Info(msg);
        }

        public void Info(string msg, Exception ex)
        {
            _log.Info(msg, ex);
        }

        public void Debug(string msg)
        {
            _log.Debug(msg);
        }

        public void Debug(string msg, Exception ex)
        {
            _log.Debug(msg, ex);
        }

        public void Warn(string msg)
        {
            _log.Warn(msg);
        }

        public void Warn(string msg, Exception ex)
        {
            _log.Warn(msg, ex);
        }

        public void Error(string msg)
        {
            _log.Error(msg);
        }

        public void Error(string msg, Exception ex)
        {
            _log.Error(msg, ex);
        }
    }
}
