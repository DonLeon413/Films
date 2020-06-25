using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DonLEonFilms.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class FilmModel
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public FilmModel()
        {
            this.IsReadOnly = true;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("id")]
        public Int32 Id
        {
            get;
            set;
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
        [JsonPropertyName("description")]
        [DisplayFormat( ConvertEmptyStringToNull = true )]
        public String Description
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("year")]
        public Int32 Year
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("file_name")]
        public String FileName
        {
            get;
            set;
        }

        [JsonPropertyName("producer")]
        public String Producer
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("is_read_only")]
        public Boolean IsReadOnly
        {
            get;
            set;
        }
    }
}
