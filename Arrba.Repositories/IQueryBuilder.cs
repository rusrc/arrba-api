namespace Arrba.Repositories
{
    public interface IQueryBuilder
    {
        string Select(long countryId, long regionId = 0, long cityId = 0);
        Enums.TypeField TypeField { get; set; }
    }
}