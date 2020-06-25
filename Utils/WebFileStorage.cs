using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonLEonFilms.Utils
{
    public class WebFileStorage
    {
        public static String SaveFile(String rootPath,  String fileName, Stream stream )
        {
            String storage_full = FileNameToFileNameStorage( fileName );

            String os_full = rootPath + "\\" + storage_full;

            var folder = Path.GetDirectoryName( os_full );

            if( false == Directory.Exists( folder ) )
            {
                System.IO.Directory.CreateDirectory( folder );
            }

            using( var file_stream = new FileStream( os_full, FileMode.Create, FileAccess.Write ) )
            {
                stream.Seek( 0, SeekOrigin.Begin );
                stream.CopyTo( file_stream );
            }

            return storage_full;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static String FileNameToFileNameStorage( String fileName )
        { 
            String md5 = FileToMD4( fileName );
            String file_name = Md5ToPath( md5 );
            String extension = Path.GetExtension( fileName );

            return file_name + "." + extension;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="md5String"></param>
        /// <returns></returns>
        private static String Md5ToPath( String md5String )
        {
            String path_l1 = md5String.Substring( 0, 2 );
            String path_l2 = md5String.Substring( 2, 2 );
            String file_name = md5String.Substring( 4 );

            String result = path_l1 + "\\" + path_l2 + "\\" + file_name;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static String FileToMD4( String fileName)
        {
            fileName += DateTime.Now.Ticks.ToString(); // так то лучше считать не из имени а из данных

            using( System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create() )
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes( fileName );
                byte[] hashBytes = md5.ComputeHash( inputBytes );
                                
                StringBuilder sb = new StringBuilder();
                for( int i = 0; i < hashBytes.Length; i++ )
                {
                    sb.Append( hashBytes[i].ToString( "X2" ) );
                }
                return sb.ToString();
            }
        }
    }
}
