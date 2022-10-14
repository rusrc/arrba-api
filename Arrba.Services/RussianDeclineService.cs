using Cyriller;

namespace Arrba.Services
{
    /// <summary>
    /// Used https://github.com/miyconst/Cyriller/blob/master/Cyriller.Samples/Program.cs#L48
    /// </summary>
    public class RussianDeclineService : IDeclineService
    {
        private readonly CyrNounCollection _cyrNounCollection;
        public RussianDeclineService()
        {
            this._cyrNounCollection = new CyrNounCollection();
        }

        public string GetPrepositional(string noun)
        {
            var result = _cyrNounCollection
                .Get(noun, out _, out _)
                .Decline()
                .Prepositional;

            return result;
        }
    }
}
