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
    public class DataTableAjaxPostModel
    {
        /// <summary>
        /// 
        /// </summary>
        public DataTableAjaxPostModel( )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "draw" )]
        public Int32 Draw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "start" )]
        public Int32 Start
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "length" )]
        public Int32 Length
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "columns" )]
        public List<ColumnModel> Columns
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

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "order" )]
        public List<OrderModel> Order
        {
            get;
            set;
        }
    }
}
