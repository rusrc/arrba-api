using System;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.Cache;
using Arrba.Services.Logger;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    using Settings = Constants.SETTINGS;
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbArrbaContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogService _log;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UnitOfWork(
            DbArrbaContext context,
            UserManager<User> userManager,
            ILogService log,
            IMapper mapper,
            IConfiguration configuration)
        {
            this._context = context;
            this._userManager = userManager;
            this._log = log;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        public ISuperCategoryRepository SuperCategoryRepository => new Lazy<SuperCategoryRepository>(() =>
            UseCache
            ? new CachedSuperCategoryRepository(_context, _log)
            : new SuperCategoryRepository(_context, _log)
        ).Value;

        public IHotAdRepository HotAdRepository => new Lazy<HotAdRepository>(() => new HotAdRepository(_context)).Value;

        public IItemTypeRepository ItemTypeRepository => new Lazy<IItemTypeRepository>(() =>
            UseCache
                ? new CachedItemTypeRepository(_context, _log)
                : new ItemTypeRepository(_context, _log)
        ).Value;

        public IBrandRepository BrandRepository => new Lazy<BrandRepository>(() =>
            UseCache
            ? new CachedBrandRepository(_context, _log)
            : new BrandRepository(_context, _log)
        ).Value;

        public IPropertyRepository PropertyRepository => new Lazy<PropertyRepository>(() =>
            UseCache
            ? new CachedPropertyRepository(_context)
            : new PropertyRepository(_context)
        ).Value;

        public IModelRepository ModelRepository => new Lazy<IModelRepository>(() => new ModelRepository(_context)).Value;

        public IAdVehicleRepository AdVehicleRepository => new Lazy<IAdVehicleRepository>(() =>
            UseCache
            ? new CachedAdVehicleRepository(_context, QueryString, SearchFormRepository, CurrencyRepository)
            : new AdVehicleRepository(_context, QueryString, SearchFormRepository, CurrencyRepository)
        ).Value;

        public ISearchFormRepository SearchFormRepository => new Lazy<ISearchFormRepository>(() =>
            false
            ? new CachedSearchFormRepository(_context, _log)
            : new SearchFormRepository(_context, _log)
        ).Value;

        public ICategoryRepository CategoryRepository => new Lazy<ICategoryRepository>(() =>
            UseCache
            ? new CachedCategoryRepository(_context)
            : new CategoryRepository(_context)
        ).Value;

        public ICategoryTypeRepository CategoryTypeRepository => new Lazy<ICategoryTypeRepository>(() => new CategoryTypeRepository(_context)).Value;

        public IQueryString QueryString => new Lazy<IQueryString>(() => new QueryString(_context)).Value;

        public IPropertyCategoryRepository PropertyCategoryRepository => new Lazy<IPropertyCategoryRepository>(() =>
            UseCache
            ? new CachedPropertyCategoryRepository(_context)
            : new PropertyCategoryRepository(_context)
        ).Value;

        public ICategoryBrandRepository CategoryBrandRepository => new Lazy<ICategoryBrandRepository>(() =>
            UseCache
            ? new CachedCategoryBrandRepository(_context)
            : new CategoryBrandRepository(_context)
            ).Value;

        public ICountryRepository CountryRepository => new Lazy<ICountryRepository>(() =>
            UseCache
            ? new CachedCountryRepository(_context)
            : new CountryRepository(_context)
        ).Value;

        public ICityRepository CityRepository => new Lazy<ICityRepository>(() =>
             UseCache
            ? new CachedCityRepository(_context, _log)
            : new CityRepository(_context)
        ).Value;

        public ICurrencyRepository CurrencyRepository => new Lazy<ICurrencyRepository>(() =>
            UseCache
        ? new CachedCurrencyRepository(_context)
        : new CurrencyRepository(_context)
        ).Value;

        public IUserRepository UserRepository => new Lazy<IUserRepository>(() => new UserRepository(_context, _userManager, _log)).Value;

        public IUserPhoneRepository UserPhoneRepository => new Lazy<IUserPhoneRepository>(() => new UserPhoneRepository(_context)).Value;

        public IDealershipRepository DealershipRepository => new Lazy<IDealershipRepository>(() => new DealershipRepository(_context)).Value;

        public int Complete() => _context.SaveChanges();

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        private bool UseCache => this._configuration.GetValue<bool>(nameof(Settings.UseMemaryCaching));

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) if (disposing) _context.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
