#define Debug
#define framewok_4_0

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Asterion.ConvertSRTtoTXT
{
    class ModelSRTtoTXT
    {
        #region fields
        internal bool isRunning;
        private bool isAllFiles = true;
        private string[] pathFileNames = null;
        private string pathDirectory;
        private List<string> pathToInputFiles;
        enum TypeConvert { SRT, TEXT };
        TypeConvert TargetType = TypeConvert.SRT;
        #endregion
        #region Events
        // Объявляем событие
        public event Action ChangeValueEvent;
        public event Action<int> MaxValueEvent;
        public event Action CompleteConvertEvent;
        public event Action CanceledConvertEvent;

        private void OnCompleteConvert()
        {
            if( CompleteConvertEvent != null )
                CompleteConvertEvent();
        }
        private void OnChangeValueEvent()
        {
            if( ChangeValueEvent != null )
                ChangeValueEvent();
        }
        private void OnMaxValueEvent( int value )
        {
            if( MaxValueEvent != null )
                MaxValueEvent(value);
            Thread.Sleep(100);
        }
        private void OnCanceledConvertEvent()
        {
            if( CanceledConvertEvent != null )
                CanceledConvertEvent();
        }
        #endregion
        #region public methods

        /// <summary>
        /// Обрабатываются только выбранные файлы в папке
        /// </summary>
        /// <param name="pathFileNames"></param>        
        public void BeginConvertSrtToTxt( string[] pathFileNames )
        {
            this.pathFileNames = pathFileNames;
            isAllFiles = false;
            BeginConvertSrtToTxt(Path.GetDirectoryName(pathFileNames[0]));
        }
        /// <summary>
        /// Обрабатываются все файлы в папке
        /// </summary>
        /// <param name="pathFileNames"></param>   
        public void BeginConvertSrtToTxt( string pathDirectory )
        {
            this.pathDirectory = pathDirectory;
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "SrtToTxt";
            backgroundThread.IsBackground = true;
            TargetType = TypeConvert.TEXT;
            backgroundThread.Start();
        }
        /// <summary>
        /// Обрабатываются только выбранные файлы в папке
        /// </summary>
        /// <param name="pathFileNames"></param>
        public void BeginConvertTxtToSrt( string[] pathFileNames )
        {
            this.pathFileNames = pathFileNames;
            isAllFiles = false;
            BeginConvertTxtToSrt(Path.GetDirectoryName(pathFileNames[0]));
        }
        /// <summary>
        /// Обрабатываются все файлы в папке
        /// </summary>
        /// <param name="pathFileNames"></param>   
        public void BeginConvertTxtToSrt( string pathDirectory )
        {
            this.pathDirectory = pathDirectory;
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "TxtToSrt";
            backgroundThread.IsBackground = true;
            TargetType = TypeConvert.SRT;
            backgroundThread.Start();
        }

        public string[] FilterExtension( string pathDirectory )
        {
#if framewok_4_0
            //For .NET 4.0 and later, 
            var files = Directory.EnumerateFiles(pathDirectory, "*.*", SearchOption.TopDirectoryOnly)
            .Where(s =>
            s.EndsWith(".SRT", StringComparison.OrdinalIgnoreCase)
            );
#endif
#if framewok_do_4_0
                    //For earlier versions of .NET,
                    var files = Directory.GetFiles("C:\\path", "*.*", SearchOption.AllDirectories)
                    .Where(s => s.EndsWith(".srt"));
#endif
            return files.ToArray<string>();
        }
        #endregion
        # region private methods
        private void Start()
        {
            ExtractPathsFiles(pathDirectory); // получить адреса файлов
            if( TargetType == TypeConvert.TEXT )
            {
                foreach( string path in pathToInputFiles )
                {
                    string text = ReadFromSrtFile(path);
                    WriteToTextFile(path, text);
                    OnChangeValueEvent();
                }
            }
            else if( TargetType == TypeConvert.SRT )
            {
                foreach( string path in pathToInputFiles )
                {
                    WriteToSrtFile(path);
                    OnChangeValueEvent();
                }
            }
            else
            {
                OnCanceledConvertEvent();
            }
            OnCompleteConvert();
        }

        private void WriteToSrtFile( string originalFilePath )
        {
            string directoryName = Path.GetDirectoryName(originalFilePath);
            string NameTxtFile = Path.GetFileNameWithoutExtension(originalFilePath) + ".txt";
            string PathTxtFile = directoryName + "\\" + NameTxtFile;
            string NameSrtFile = Path.GetFileNameWithoutExtension(originalFilePath);
            NameSrtFile = directoryName + "\\" + NameSrtFile + "-ru.srt";
            //int counter = 1;
            try
            {   // Open the text file using a stream reader.
                using( StreamWriter srtTarget = new StreamWriter(File.Create(NameSrtFile), System.Text.Encoding.UTF8) )
                {
                    using( StreamReader srOriginal = new StreamReader(originalFilePath, System.Text.Encoding.UTF8) )
                    {
                        using( StreamReader txtSource = new StreamReader(PathTxtFile, System.Text.Encoding.UTF8) )
                        {
                            while( true )
                            {
                                // Читаем строку из файла оригинала
                                string originalText = srOriginal.ReadLine();

                                // Достигнут конец файла, прерываем считывание.
                                if( originalText == null )
                                    break;
                                if( originalText == string.Empty )
                                    continue;
                                // Cтрока начинается с цифры
                                if( StartNumber(srtTarget, originalText) )
                                    continue;
                                // Не с таймкода - просто записать в файл
                                if( !StartWithTimeCode(srtTarget, txtSource, originalText) )
                                {
                                    srtTarget.WriteLine(txtSource.ReadLine());
                                }

                            }
                        }
                    }
                }
            } catch( Exception e )
            {
                Console.WriteLine(e.Message);
            }
        }

        private void WriteToTextFile( string txtPath, string text )
        {
            string directoryName = Path.GetDirectoryName(txtPath);
            string txtNameFile = Path.GetFileNameWithoutExtension(txtPath) + ".txt";
            txtPath = directoryName + "\\" + txtNameFile;

            try
            {
                using( StreamWriter sw = new StreamWriter(txtPath, false, System.Text.Encoding.UTF8) )
                {
                    sw.Write(text);
                }
            } catch( Exception e )
            {
                Console.WriteLine(e.Message);
            }
        }

        private string ReadFromSrtFile( string readPath )
        {
            string text = "";
            Regex rxTime = new Regex(@"^\d+:\d+"); // 00:00
            Regex rxNum = new Regex(@"^\d+"); // 00
            try
            {   // Open the text file using a stream reader.
                using( StreamReader sr = new StreamReader(readPath) )
                {
                    while( true )
                    {
                        // Читаем строку из файла во временную переменную.
                        string temp = sr.ReadLine();
                        // Если достигнут конец файла, прерываем считывание.
                        if( temp == null )
                            break;

                        if( temp == string.Empty || rxNum.IsMatch(temp) )
                            continue;

                        bool isDigit = rxTime.IsMatch(temp);

                        if( !isDigit )
                        {
                            // Пишем считанную строку в итоговую переменную.
                            text += temp + "\n";
                        }
                    }

                }
            } catch( Exception e )
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return text;
        }

        /// <summary>
        /// Получение полных путей файлов
        /// </summary>
        /// <param name="pathDirectory"></param>
        private void ExtractPathsFiles( string pathDirectory )
        {
            pathToInputFiles = new List<string>();
            if( isAllFiles )
            {
                pathFileNames = FilterExtension(pathDirectory);
            }
            foreach( var item in pathFileNames )
            {
                pathToInputFiles.Add(item);
            }
            OnMaxValueEvent(pathToInputFiles.Count);
        }

        private bool StartWithTimeCode( StreamWriter srtTarget,
                                                StreamReader txtSource,
                                                string text )
        {
            //Regex rxTime = new Regex(@"^\d+:\d+"); // 00:00
            //00:00:04,130 --> 00:00:09,389 align:middle line:90%
            Regex rxTime = new Regex(@"^[\d]{2}[:]{1}[\d]{2}([:]?)?([\d]{0,2})?([,]?)?([\d]{0,3})?( [-]{2})?([>]{1})?( [\d]{2})?([:]{1})?([\d]{2})?([:]{1})?([\d]{2})?([,]{1})?([\d]{3})?( [a-z]{5})?([:]?)?([a-z]{0,6})?( [a-z]{4})?([:]?)?([\d]?)?([%\d]?)?([%]?)?$");
            Regex rxForRemove = new Regex(@"[a-z]{5}[:]{1}[a-z]{6} [a-z]{4}[:]{1}[\d]{2}[%]{1}");
            bool withTimeCode = rxTime.IsMatch(text);
            string cleanedText = rxForRemove.Replace(text, "");

            if( !withTimeCode )
                return false;

            srtTarget.WriteLine();
            srtTarget.WriteLine(cleanedText);
            
            srtTarget.WriteLine(txtSource.ReadLine());
            return true;
        }

        private bool StartNumber( StreamWriter sWriter, string record )
        {
            Regex rxNum = new Regex(@"^\d+$"); // 00
            bool startNumber = rxNum.IsMatch(record);
            if( startNumber )
            {
                sWriter.WriteLine(record);                
                return startNumber;
            }
            return startNumber;
        }
        #endregion
    }
}
