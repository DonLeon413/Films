using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DonLEonFilms.Models
{
    /// <summary>
    /// Folm row model
    /// </summary>
    public class FilmRowModel
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public FilmRowModel()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("name")]
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName( "year" )]
        public Int32 Year
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName( "produser" )]
        public String Produser
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName( "id" )]
        public Int32 Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName( "is_read_only" )]
        public Boolean IsReadOnly
        {
            get;
            set;
        }
    }
}
