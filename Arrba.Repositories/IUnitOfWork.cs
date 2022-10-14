using System;
using System.Threading.Tasks;

namespace Arrba.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ISuperCategoryRepository SuperCategoryRepository { get; }
        IHotAdRepository HotAdRepository { get; }
        IItemTypeRepository ItemTypeRepository { get; }
        IBrandRepository BrandRepository { get; }
        IPropertyRepository PropertyRepository { get; }
        IModelRepository ModelRepository { get; }
        IAdVehicleRepository AdVehicleRepository { get; }
        ISearchFormRepository SearchFormRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICategoryTypeRepository CategoryTypeRepository { get; }
        IPropertyCategoryRepository PropertyCategoryRepository { get; }
        ICategoryBrandRepository CategoryBrandRepository { get; }
        ICountryRepository CountryRepository { get; }
        ICityRepository CityRepository { get; }
        ICurrencyRepository CurrencyRepository { get; }
        IUserRepository UserRepository { get; }
        IUserPhoneRepository UserPhoneRepository { get; }
        IDealershipRepository DealershipRepository { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}
