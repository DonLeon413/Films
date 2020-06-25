using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonLEonFilms.Models.DataTable
{
    public class DataTableAjaxResultModel<T>
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("draw")]
        public Int32 Draw
        {
            get;
            set;
        } 
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("recordsTotal")]
        public Int32 RecordsTotal
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("recordsFiltered")]
        public Int32 RecordsFiltered
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data")]
        public  IEnumerable<T> Data
        {
            get;
            set;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public DataTableAjaxResultModel()
        {
        }
    }
}
