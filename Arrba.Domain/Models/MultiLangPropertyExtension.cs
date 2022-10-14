using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Arrba.Helpers;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class MultiLangPropertyExtension
    {
        [JsonIgnore]
        [Display(Name = "Multi Lang Json")]
        [DataType(DataType.MultilineText)]
        public string NameMultiLangJson { get; set; }
        public virtual string Name { get; set; }

        #region NotMapped properties   
        [NotMapped]
        [JsonIgnore]
        public NameMultiLangJson NameMultiLangJsonObject => this.MakeNameMultiLangJson(this.NameMultiLangJson);
        [NotMapped]
        public string NameMultiLang
        {
            get { return NameMultiLangJsonObject.Value; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
            }
        }
        #endregion

        #region helpers
        protected NameMultiLangJson MakeNameMultiLangJson(string nameMultiLangJson)
        {
            //Default result
            var result = new NameMultiLangJson
            {
                LangName = CultureHelper.GetDefaultCulture(),
                Value = Name
            };

            // TODO add unit test
            // If property not null or empty
            if (!string.IsNullOrEmpty(nameMultiLangJson))
            {
                var currentLang = CultureHelper.GetCurrentCulture();
                var langNameList = JsonConvert
                    .DeserializeObject
                    <List<NameMultiLangJson>>(nameMultiLangJson);

                if ((langNameList != null) && langNameList.Any(l => l.LangName.StartsWith(currentLang, StringComparison.InvariantCultureIgnoreCase)))
                {
                    result = langNameList.SingleOrDefault
                        (
                            predicate: l => l.LangName.StartsWith(currentLang, StringComparison.InvariantCultureIgnoreCase)
                        );

                    if (result != null && string.IsNullOrEmpty(result.Value))
                        result.Value = Name;
                }
            }


            return result;
        }
        #endregion
    }
}