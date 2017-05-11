using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionStore
{
    class HtmlToString
    {
        /// <summary>
        /// Запрашивает интернет страницу по url
        /// и преобразует ее в строку.
        /// если задано имя файла сохраняет страницу в файл
        /// для последующего использования в ReadFileHTML()
        /// </summary>
        /// <param name="url">Url сайта</param>
        /// <param name="encoding">кодировка сайта</param>
        /// <param name="fileTemp">имя файла для кэширования страницы </param>
        /// <returns>html код</returns>
        public static string Read( string url, Encoding encoding, string fileTemp = null )
        {
            string result = "";
            // Создать объект запроса
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response;
            try
            {
                // Получить ответ с сервера
                response = (HttpWebResponse)request.GetResponse();

                // Получаем поток данных из ответа
                using( StreamReader stream = new StreamReader(response.GetResponseStream(), encoding) )
                {
                    // Выводим исходный код страницы
                    result = stream.ReadToEnd();
                }
                if( fileTemp != null )
                {
                    using( StreamWriter streawr = new StreamWriter(
                        Environment.CurrentDirectory + "\\" + fileTemp
                        , false
                        , encoding) )
                    {
                        streawr.WriteLine(result);
                    }
                }
            } catch {
                using( StreamWriter streawr = new StreamWriter(
                        Environment.CurrentDirectory + "\\" + "logError"
                        , true
                        , encoding) )
                {
                    //streawr.WriteLine(fileTemp);
                }
            }
            //
            // Получаем некоторые данные о сервере
            //string messageServer = "Целевой URL: \t" + 
            //                            request.RequestUri + 
            //                            "\nМетод запроса: \t" + 
            //                            request.Method +
            //                            "\nТип полученных данных: \t" + 
            //                            response.ContentType + 
            //                            "\nДлина ответа: \t" + 
            //                            response.ContentLength + 
            //                            "\nЗаголовки";
            //Console.WriteLine( messageServer );
            //
            xmlSanitizingString sanizer = new xmlSanitizingString();
            return sanizer.SanitizeXmlString(result);
        }
        public static string ReadHTML( string url = "http://www.professorweb.ru" )
        {
            return Read(url, Encoding.UTF8);
        }
        public static string ReadCacheFile( string fileСache, Encoding encoding = null )
        {
            string result = "";

            if( encoding == null )
                encoding = Encoding.UTF8;

            // Получаем поток данных из ответа
            using( StreamReader stream = new StreamReader(
                Environment.CurrentDirectory + "\\" + fileСache
                 , encoding) )
            {
                // Выводим исходный код страницы
                result = stream.ReadToEnd();
            }
            xmlSanitizingString sanizer = new xmlSanitizingString();            
            return sanizer.SanitizeXmlString(result);
        }
    }
}
