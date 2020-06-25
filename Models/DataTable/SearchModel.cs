using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonLEonFilms.Models.DataTable
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "value" )]
        public String Value
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "regex" )]
        public String Regex
        {
            get;
            set;
        }
    }
}
