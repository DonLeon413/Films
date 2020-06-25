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
    public class OrderModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "column" )]
        public Int32 Column
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "dir" )]
        public String Dir
        {
            get;
            set;
        }
    }
}
