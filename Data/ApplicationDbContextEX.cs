using DonLEonFilms.Data.Entitys;
using DonLEonFilms.Models;
using DonLEonFilms.Models.DataTable;
using Microsoft.CodeAnalysis.Classification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DonLEonFilms.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApplicationDbContextEX
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="dataTableAjaxModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static DataTableAjaxResultModel<FilmRowModel> GetFilms( this ApplicationDbContext _this,
                                                                      DataTableAjaxPostModel dataTableAjaxModel,
                                                                      String userId )
       {            
            var query = _this.Films.AsQueryable();

            if( dataTableAjaxModel.Search != null && 
                false == String.IsNullOrWhiteSpace( dataTableAjaxModel.Search.Value ) )
            {
                var search_value = dataTableAjaxModel.Search.Value.Trim().ToLower();

                query = query.Where( f => f.Name.ToLower().Contains( search_value ) ||
                                          f.Producer.ToLower().Contains( search_value ) ||  
                                          f.Year.ToString().Contains( search_value )
                );
            }

            String column_name = "id";
            Boolean ascending = true;
            Expression<Func<Film, Object>> expression = _FilmExpressions[column_name];

            if( dataTableAjaxModel.Order != null && dataTableAjaxModel.Order.Count > 0 )
            {
                var column_index = dataTableAjaxModel.Order[0].Column;
                if( column_index >= 0 && column_index < dataTableAjaxModel.Columns.Count )
                {
                    try
                    {
                        var column_info = dataTableAjaxModel.Columns[column_index];
                        column_name = column_info.Data.Trim();
                        ascending = ( String.Compare( dataTableAjaxModel.Order[0].Dir, "asc", true ) == 0 );                            
                    }
                    catch( Exception ex )
                    {  // Sort default by Id
                        Debug.WriteLine( String.Format("Sort by ID => Error: {0}", ex.Message ) );
                        column_name = "id";
                        ascending = true;
                    }
                }

                if( _FilmExpressions.ContainsKey( column_name ) )
                {
                    expression = _FilmExpressions[ column_name ];
                }              
            }

            if( null != expression )
            {
                if( ascending )
                {
                    query = query.OrderBy( expression );
                }
                else
                {
                    query = query.OrderByDescending( expression ); 
                }
            }

            var total_items = query.Count();

            var items = query.Skip( dataTableAjaxModel.Start ).
                              Take( dataTableAjaxModel.Length ).
                              ToList().
                              Select( f => new FilmRowModel()
                              {
                                  Name = f.Name,
                                  Id = f.Id,
                                  Year = f.Year,
                                  Produser = f.Producer,
                                  IsReadOnly = ( 0 != String.Compare( f.ApplicationUserId, userId ) )
                              } );

            return new DataTableAjaxResultModel<FilmRowModel>()
            {
                Draw = dataTableAjaxModel.Draw,
                RecordsTotal = total_items,
                RecordsFiltered = total_items,
                Data = items
            };
       }      
    
       /// <summary>
       /// 
       /// </summary>
       /// <param name="_this"></param>
       /// <param name="userId"></param>
       /// <param name="id"></param>
       public static String DeleteFilm( this ApplicationDbContext _this,
                                      String userId, Int32 id )
       {
            String file_name = String.Empty;

            var film_bbdd = _this.Films.Where( f => f.Id == id ).FirstOrDefault();
            if( null == film_bbdd )
            {
                throw new Exception( String.Format( "Film by id={0} not found ", id ) );
            }

            if( String.Compare( film_bbdd.ApplicationUserId, userId, true ) != 0 )
            {
                throw new Exception( "Film '{0}' cannot be  deleted " );
            }

            file_name = film_bbdd.FileName; // for delete

            _this.Films.Remove( film_bbdd );
            _this.SaveChanges();

            return file_name;
       }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static FilmModel LoadFilm( this ApplicationDbContext _this, Int32 id, 
                                                       String userId )
       {            

            Film film_bbdd = _this.Films.Where( f => f.Id == id ).
                                         FirstOrDefault();

            if( film_bbdd == null )
            {
                throw new Exception( "Film not found" );
            }

            return new FilmModel()
            {
                Id = film_bbdd.Id,
                Name = film_bbdd.Name,
                Year = film_bbdd.Year,
                Description = film_bbdd.Description,
                Producer = film_bbdd.Producer,
                FileName = film_bbdd.FileName,
                IsReadOnly = ( String.Compare(userId, film_bbdd.ApplicationUserId, true ) != 0 )
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
       public static String UpdateFilm( this ApplicationDbContext _this, FilmModel model, String userId )
       {
            String old_file = String.Empty; // old file name

            Film film_bbdd = null;
            if( model.Id == 0 )
            {
                film_bbdd = new Film()
                {
                    Name = model.Name,
                    Year = model.Year,
                    FileName = model.FileName,
                    Producer = model.Producer,
                    Description = model.Description,
                    ApplicationUserId = userId
                };
                _this.Films.Add( film_bbdd );
            }            
            else
            {
                film_bbdd = _this.Films.Where( f => f.Id == model.Id ).FirstOrDefault();
                if( film_bbdd == null )
                { // Not Found
                    throw new Exception( String.Format("Unable update - film '{0}' not found!", 
                                                        model.Name ) );
                }

                if( 0 != String.Compare(film_bbdd.ApplicationUserId, userId, true ) )
                { // NO EDIT RIGHT
                    throw new Exception( String.Format("No edit rights '{0}'", film_bbdd.Name ) );
                }                

                // Update
                film_bbdd.Name = model.Name;
                if( false == String.IsNullOrWhiteSpace( model.FileName ) )
                {
                    old_file = film_bbdd.FileName;
                    film_bbdd.FileName = model.FileName;                    
                }
                film_bbdd.Producer = model.Producer;
                film_bbdd.Description = model.Description;
                film_bbdd.Year = model.Year;
            }

            _this.SaveChanges();

            return old_file;
       }

        #region MAPS
        private static Dictionary<String, Expression<Func<Film, Object>>> _FilmExpressions =
                    new Dictionary<string, Expression<Func<Film, object>>>()
                    {
                        {  "id", f=> f.Id },
                        { "name", f=> f.Name },
                        { "year", f=> f.Year },
                        { "Produser", f => f.Producer }
                    };
        #endregion
    }
}
