namespace Arrba.Services
{
    public interface IDeclineService
    {
        /// <summary>
        /// Prepositional
        /// Предложный падеж.
        /// <example>
        ///     Москве, Санкт-Петербурге
        /// </example>
        /// </summary>
        /// <param name="noun"></param>
        /// <returns></returns>
        string GetPrepositional(string noun);
    }
}
