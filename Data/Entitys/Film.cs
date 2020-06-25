using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DonLEonFilms.Data.Entitys
{
    /// <summary>
    /// 
    /// </summary>
    [Table("Films")]
    public class Film
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("id")]
        [Key]
        public Int32 Id
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Column("Name")]
        [MaxLength(256)]
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Column("Year")]        
        public Int32 Year
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Column("Prodeucer")]
        [MaxLength(256)]
        public String Producer
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Column("Description")]
        [MaxLength(1024)]
        public String Description
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Column("UserId")]                
        public string ApplicationUserId
        { 
            get;
            set; 
        }

        /// <summary>
        /// 
        /// </summary> 
        [ForeignKey( "UserId" )]
        public virtual ApplicationUser ApplicationUser 
        { 
            get;
            set; 
        }

        [Column("FileName")]
        [MaxLength(512)]
        public String FileName
        {
            get;
            set;
        }
    }
}
