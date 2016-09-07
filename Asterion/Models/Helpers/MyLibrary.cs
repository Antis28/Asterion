using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Asterion.Models
{
    class MyLibrary
    {
        /// <summary>
        /// Функция конвертирования Unix Timestamp в DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ConvertFromUnixTimestamp( double timestamp )
        {
            DateTime origin = new DateTime( 1970, 1, 1, 0, 0, 0, 0 );
            return origin.AddSeconds( timestamp );
        }
        /// <summary>
        /// Функция обратного конвертирования DateTime в Unix Timestamp
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double ConvertToUnixTimestamp( DateTime date )
        {
            DateTime origin = new DateTime( 1970, 1, 1, 0, 0, 0, 0 );
            TimeSpan diff = date - origin;
            return Math.Floor( diff.TotalSeconds );
        }

        /// <summary>
        /// Метка с учетом только даты
        /// </summary>
        /// <returns></returns>
        public static double UnixDateStamp( )
        {
            DateTime date = DateTime.Now;
            DateTime origin = new DateTime( 1970, 1, 1, 0, 0, 0, 0 );
            TimeSpan diff = date.Date - origin;
            return Math.Floor( diff.TotalSeconds );
        }

        /// <summary>
        /// Замена кавычек на "ёлочки" в C#
        /// </summary>
        /// <param name="m">Исходная строка с текстом</param>
        /// <returns>Текст с замененными кавычками</returns>
        private string replace_quotes( string m )
        {
            int at = 0;
            int pos = 0;
            while( (pos < m.Length) && (at > -1) )
            {
                at = m.IndexOf( '"', pos );
                if( at == -1 )
                    break; // Выход из цикла если больше нет нужного символа в строке   
                pos = at + 1;

                Regex R = new Regex( @"[^wd]" );

                // Если конец строки заканчивается кавычкой
                if( pos == m.Length )
                { m = R.Replace( m, "&raquo;", 1, at ); break; }

                // Если впереди кавычки идут буквы и цифры, - ставим открывающую "«"
                // Если любые другие символы, - ставим закрывающую "»"
                Match match = R.Match( m, pos, 1 );
                if( match.Success )
                { m = R.Replace( m, "&raquo;", 1, at ); } else
                { m = R.Replace( m, "&laquo;", 1, at ); }
            }

            return m;
        }
    }
}
