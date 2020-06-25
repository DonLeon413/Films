using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonLEonFilms.Models.DataTable
{
    public class ColumnModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "data" )]
        public String Data
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "name" )]
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "searchable" )]
        public Boolean Searchable
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "orderable" )]
        public Boolean Orderable
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "search" )]
        public SearchModel Search
        {
            get;
            set;
        }
    }
}
