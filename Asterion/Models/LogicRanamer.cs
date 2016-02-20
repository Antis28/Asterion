using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;

namespace Asterion.Models
{
    class LogicRanamer
    {
        static public event Asterion.Presentors.ProgressHandler Progress;
        static public event Asterion.Presentors.ConsoleHandler ConsoleEvent;

        //public static ListBox _myConsole;  // пишет отчет о проделаной работе
        //public static CheckBox _isOnlyJPG;

        public static bool isFirst = true;
        static string[] _selectFiles = new string[] {"",""};

        volatile bool _isOnlyJPG = false;

        static int number = 1;//переменная для добавления номера к файлу

        public bool IsOnlyJPG
        {
            get
            {
                return _isOnlyJPG;
            }

            set
            {
                _isOnlyJPG = value;
            }
        }

        /// <summary>
        /// Переименовывает файл в соответсвии с параметрами
        /// </summary>
        /// <param name="selectedPath"></param>  
        public void StartRenameToDateFormatWithoutDot( string selectedPath )
        {
            DirectoryInfo directoryInfo = new DirectoryInfo( selectedPath );

            //new Thread( () =>
            //NameAdded_2( directoryInfo )
            //).Start();
        }



        /// <summary>
        /// Проверка имени файла на совпадение с одним из шаблонов
        /// </summary>
        /// <param name="myDirectoryInfo"></param>
        /// <returns></returns>
        private void NameAdded_2( DirectoryInfo myDirectoryInfo )
        {
            int currentItem = 0;

            foreach( FileInfo fileInfo in myDirectoryInfo.GetFiles() )
            {
                currentItem = UpdateProgressBar( myDirectoryInfo, currentItem );
                //если хочу переименовывать только jpg
                if( (fileInfo.Extension != ".jpg" && IsOnlyJPG) ||
                    fileInfo.Extension == ".ini" ||
                    fileInfo.Extension == ".ico" )
                    continue;
                SeparationFilename.SeparateName( fileInfo );
                if( SeparationFilename.flagYYYYMMDD )
                    continue;
                if( SeparationFilename.date == "" )
                    addPrefixDate( fileInfo, currentItem );
                else
                    number = RenameMethod_2( fileInfo, SeparationFilename.name, SeparationFilename.date );
            }
            if( ConsoleEvent != null )
            {
                ConsoleEvent( new string( '-', 100 ) );
                ConsoleEvent( "Переименование завершено!!!" );
                ConsoleEvent( "Переименовано файлов: " + (number - 1) );
                ConsoleEvent( new string( '-', 100 ) );
            }
        }

        private static class SeparationFilename
        {
            public static string date = "";
            public static string name = "";
            public static string extension = "";

            // флаги указывающие каккой шаблон активен
            public static bool flagDDdotMMdotYY;
            public static bool flagYYYYtireMMtireDD;
            public static bool flagYYYYdotMMdotDD;
            public static bool flagYYYYMMDD;
            // Шаблоны формата дат
            static Regex DDdotMMdotYY = new Regex( @"\d\d\.\d\d\.\d\d" );//дд.мм.гг  
            static Regex YYYYtireMMtireDD = new Regex( @"\d\d\d\d\-\d\d\-\d\d" );// гггг-мм-дд  
            static Regex YYYYdotMMdotDD = new Regex( @"\d\d\d\d\.\d\d\.\d\d" );// гггг.мм.дд 
            static Regex YYYYMMDD = new Regex( @"\d\d\d\d\d\d" );// ггггммдд 
            // Коллекция шаблонов
            public static IEnumerable PaternCollection()
            {
                flagYYYYMMDD = true;
                yield return YYYYMMDD;

                flagYYYYMMDD = false;
                flagYYYYdotMMdotDD = true;
                yield return YYYYdotMMdotDD;

                flagYYYYdotMMdotDD = false;
                flagDDdotMMdotYY = true;
                yield return DDdotMMdotYY;

                flagDDdotMMdotYY = false;
                flagYYYYtireMMtireDD = true;
                yield return YYYYtireMMtireDD;


            }

            /// <summary>
            /// Разделяет файл на:
            /// Дату
            /// имя
            /// </summary>
            /// <param name="fileInfo"></param>
            public static void SeparateName( FileInfo fileInfo )
            {
                name = "";
                date = "";
                extension = "";
                flagDDdotMMdotYY = false;
                flagYYYYtireMMtireDD = false;
                flagYYYYdotMMdotDD = false;
                flagYYYYMMDD = false;


                // Проверка имени файла на совпадение с одним из шаблонов
                foreach( var item in PaternCollection() )
                {
                    Regex matchItem = item as Regex;
                    //MatchCollection matches = matchItem.Matches( fileInfo.Name );
                    Match match = matchItem.Match( fileInfo.Name );

                    if( match.Value != "" )
                    {
                        if( !flagYYYYMMDD )
                            DeleteDateInNameFile( fileInfo, match ); //найти имя без даты и расширения
                        break;
                    }
                }

            }
            public static void DeleteExtensionFile( FileInfo fileInfo )
            {
                //убираем расширение из имени файла 
                string pattern = fileInfo.Extension;    //".jpg";
                string[] substrings = Regex.Split( fileInfo.Name, pattern );

                foreach( var item in substrings )
                {
                    if( item != "" )
                        name = item;
                }
            }

            static void DeleteDateInNameFile( FileInfo fileInfo, Match match )
            {
                string[] substrings;

                DeleteExtensionFile( fileInfo );
                //убираем дату из имени файла
                date = match.Value;
                substrings = Regex.Split( name, date );
                name = "";
                foreach( var item in substrings )
                {
                    if( item != "" )
                        name += item;
                }

                DeleteSeparate();
            }
            // только jpg
            static void PatternFindExtensionFile( FileInfo fileInfo )
            {
                Regex rex = new Regex( @"\.jpg" );//дд.мм.гг  
                MatchCollection matches = rex.Matches( fileInfo.Name );
                if( matches.Count > 0 )
                    foreach( var item in matches )
                    {
                        extension = item.ToString();
                    }

            }

            static void DeleteSeparate()
            {
                String[] expressions = new string[0];
                if( flagDDdotMMdotYY || flagYYYYdotMMdotDD )
                {
                    expressions = date.Split( new char[] { '.' } );
                } else
                {
                    expressions = date.Split( new char[] { '-' } );
                }

                date = "";
                if( !flagYYYYdotMMdotDD && !flagYYYYtireMMtireDD )
                {
                    for( var i = expressions.Length - 1; i >= 0; i-- )
                    {
                        date += expressions[i];
                    }
                } else
                {
                    for( var i = 0; i < expressions.Length; i++ )
                    {
                        Regex patternYYYY = new Regex( @"\d\d\d\d" );// гггг.мм.дд 
                        Match YYYY = patternYYYY.Match( expressions[i] );
                        if( YYYY.Value != "" )
                        {
                            patternYYYY = new Regex( @"20" );// гггг.мм.дд        
                            string[] substrings = patternYYYY.Split( YYYY.Value, 2 );
                            foreach( string match in substrings )
                            {
                                if( match != "" )
                                    date = match;
                            }
                        } else
                            date += expressions[i];
                    }
                }
            }
        }

        private  void addPrefixDate( FileInfo myFileInfo, int currentItem )
        {
            SeparationFilename.DeleteExtensionFile( myFileInfo );
            string name = SeparationFilename.name;

            DateTime fileTime = myFileInfo.LastWriteTime;
            string startName;
            if( fileTime.Year.ToString() == "2015" )
                startName = "15";
            else
                startName = fileTime.Year.ToString();

            if( fileTime.Month.ToString().Length < 2 )
                startName += "0" + fileTime.Month;
            else
                startName += fileTime.Month.ToString();
            if( fileTime.Day.ToString().Length < 2 )
                startName += "0" + fileTime.Month;
            else
                startName += fileTime.Day.ToString();

            //само переименование            
            number = RenameMethod_2( myFileInfo, name, startName );
        }

        private static int RenameMethod_2( FileInfo myDirectoryInfo, string name, string startName = "" )
        {
            string path = myDirectoryInfo.DirectoryName;
            //string name = myDirectoryInfo.Name;


            if( startName == "" )
                startName = "NoName";

            //само переименование
            try
            {
                File.Move( path + @"\" + myDirectoryInfo.Name, path + @"\" + startName + "__" + name + myDirectoryInfo.Extension );
            } catch( Exception )
            {
                //File.Move( path + @"\" + myDirectoryInfo.Name, path + @"\" + startName + "__" + name + "__" + number + myDirectoryInfo.Extension );
            }           
            number++;//увеличиваем каждый раз номер  
            return number;
        }

        private static int UpdateProgressBar( DirectoryInfo DI, int currentItem )
        {
            #region Обновляет прогрессБар
            // Обновляет прогрессБар
            int allItem = DI.GetFiles().Length;
            currentItem++;

            int progress = (Int32)((currentItem * 1d / allItem) * 100d);
            if( progress > 100 )
                progress = 100;
            else if( progress < 0 )
                progress = 0;

            if( Progress != null )
                Progress( progress );
            #endregion
            return currentItem;
        }

    }
}

