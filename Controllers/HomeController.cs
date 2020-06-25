using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DonLEonFilms.Models;
using Microsoft.AspNetCore.Authorization;
using DonLEonFilms.Models.DataTable;
using DonLEonFilms.Data;
using System.Security.Claims;
using DonLEonFilms.Data.Entitys;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using DonLEonFilms.Utils;
using Newtonsoft.Json.Schema;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace DonLEonFilms.Controllers
{
    public class HomeController: Controller
    {
        
        private readonly ApplicationDbContext _DBContext;
        private UserManager<ApplicationUser> _UserManager;
        private readonly IWebHostEnvironment _Env;
        private readonly IConfiguration _IConfig; 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userManager"></param>
        public HomeController( ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
                               IWebHostEnvironment env, IConfiguration iConfig )
        {
            this._DBContext = dbContext;
            this._UserManager = userManager;
            this._Env = env;
            this._IConfig = iConfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Index( )
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Films( DataTableAjaxPostModel ajaxModel )
        {
            try
            {
                var user = await this._UserManager.GetUserAsync( HttpContext.User );

                var result = this._DBContext.GetFilms( ajaxModel, user.Id );

                return new JsonResult( result );
            }
            catch( Exception ex )
            {
                return StatusCode( 406, ex.Message );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FilmDelete( [FromBody] Int32 id )
        {
            try
            {
                var user = await this._UserManager.GetUserAsync( HttpContext.User );

                this._DBContext.DeleteFilm( user.Id, id );

                return new JsonResult( new { result = true } );

            }catch( Exception ex )
            {
                return StatusCode( 406, ex.Message );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult>  Film( [FromBody] Int32 id )
        {
            try
            {
                var user = await this._UserManager.GetUserAsync( HttpContext.User );

                FilmModel model;

                if( id == 0 )
                {
                    model = new FilmModel();
                }
                else
                {
                    model = this._DBContext.LoadFilm( id, user.Id );
                }

                if( String.IsNullOrWhiteSpace( model.FileName ) )
                { // Set default image
                    model.FileName = this._IConfig.GetValue<string>( "DefaultImage" );
                }
                else
                {
                    String path = this._IConfig.GetValue<string>( "Storage" );
                    model.FileName = path + "\\" + model.FileName;
                }

                return new JsonResult( model );

            }catch( Exception ex )
            {
                return StatusCode( 406, ex.Message );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveFilm( FilmModel obj, IFormFile file )
        {
            try
            {
                var user = await this._UserManager.GetUserAsync( HttpContext.User );
                obj.FileName = String.Empty; // не меняем файл

                String message="";

                if( null != file )
                {
                    try
                    {
                        using( MemoryStream memory_stream = new MemoryStream() )
                        {
                            String storage_path = this._IConfig.GetValue<string>( "Storage" );
                            String root_path = this._Env.WebRootPath + "\\" + storage_path;

                            file.CopyTo( memory_stream );
                            obj.FileName = WebFileStorage.SaveFile( root_path, file.FileName, memory_stream );

                            // Файл сохранен успешно - имя в BBDD
                        }

                    }
                    catch( Exception ex )
                    {
                        obj.FileName = String.Empty; // не меняем файл
                        message = String.Format( "File save error: {0}", ex.Message );
                    }
                }

                try
                {
                    
                    this._DBContext.UpdateFilm( obj, user.Id );

                }catch( Exception ex )
                {   // Цто то случилось случилось при сохранении!!!

                    if( false == String.IsNullOrWhiteSpace( obj.FileName ) )
                    {   // Здесь удалить файл из хранилища 

                        // не реализовано - это ведь всего лишь тестовое задание
                    }

                    throw; // Исключение летит дальше - сообщение отправляется пользователю
                }

                return new JsonResult( new { message = message } );

            }catch( Exception ex )
            {   
                // что то серьезное
                return StatusCode( 406, ex.Message );
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy( )
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ResponseCache( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true )]
        public IActionResult Error( )
        {
            return View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
        }
    }
}
