using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonLEonFilms.Data.Entitys
{
    /// <summary>
    /// 
    /// </summary>
   public class ApplicationUser: IdentityUser
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Film> Films 
        { 
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ApplicationUser()
        {

        }
   }
}
