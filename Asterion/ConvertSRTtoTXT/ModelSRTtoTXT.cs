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
        internal bool isRunning;
        private bool isAllFiles;
        private string[] pathFileNames;
        private string pathDirectory;
        private List<string> pathToInputFiles;
        enum TypeConvert { SRT, TEXT };
        TypeConvert TargetType = TypeConvert.SRT;

        // Объявляем событие
        public event Action ChangeValueEvent;
        public event Action<int> MaxValueEvent;
        public event Action CompleteConvertEvent;
        public event Action CanceledConvertEvent;

        /// <summary>
        /// Обрабатываются все файлы в папке
        /// </summary>
        public void SwitchOnAllFiles()
        {
            pathFileNames = null;
            isAllFiles = true;
        }

        /// <summary>
        /// Обрабатываются только выбранные файлы в папке
        /// </summary>
        /// <param name="pathFileNames"></param>
        public void SwitchOnSelectedFiles( string[] pathFileNames )
        {
            this.pathFileNames = pathFileNames;
            isAllFiles = false;
        }

        public void BeginConvertSrtToTxt( string pathDirectory )
        {
            this.pathDirectory = pathDirectory;
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "SrtToTxt";
            backgroundThread.IsBackground = true;
            TargetType = TypeConvert.TEXT;
            backgroundThread.Start();
        }
        public void BeginConvertTxtToSrt( string pathDirectory )
        {
            this.pathDirectory = pathDirectory;
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "TxtToSrt";
            backgroundThread.IsBackground = true;
            TargetType = TypeConvert.SRT;
            backgroundThread.Start();
        }
        //------------- private -----------------------------//
        private void Start()
        {
            ExtractPathsFiles(pathDirectory); // получить адреса файлов
            string path = pathToInputFiles[0];

            if( TargetType == TypeConvert.TEXT )
            {
                string text = ReadFromSrtFile(path);
                WriteToTextFile(path, text);
            }
            else if( TargetType == TypeConvert.SRT )
            {
                WriteToSrtFile(path);
            }


        }

        private void WriteToSrtFile( string originalFilePath )
        {
            string tempFilePath = string.Empty;
            string text = string.Empty;
            string resultFileName = string.Empty;            
            string directoryName = string.Empty;
            string tempNameFile = string.Empty;

            directoryName = Path.GetDirectoryName(originalFilePath);

            tempNameFile = Path.GetFileNameWithoutExtension(originalFilePath) + ".txt";
            tempFilePath = directoryName + "\\" + tempNameFile;

            resultFileName = Path.GetFileNameWithoutExtension(originalFilePath);            
            resultFileName = directoryName + "\\" + resultFileName + "-ru.srt";
            int counter = 1;

            Regex rxTime = new Regex(@"\d+:\d+"); // 00:00
            Regex rxNum = new Regex(@"^\d+"); // 00
            try
            {   // Open the text file using a stream reader.
                using( StreamWriter swResult = new StreamWriter(File.Create(resultFileName), System.Text.Encoding.UTF8) )
                {
                    using( StreamReader srOriginal = new StreamReader(originalFilePath, System.Text.Encoding.UTF8) )
                    {
                        using( StreamReader srSource = new StreamReader(tempFilePath, System.Text.Encoding.UTF8) )
                        {
                            while( true )
                            {
                                // Читаем строку из файла во временную переменную.
                                string temp = srOriginal.ReadLine();

                                // Если достигнут конец файла, прерываем считывание.
                                if( temp == null )
                                    break;
                                if( temp == string.Empty || rxNum.IsMatch(temp) )
                                {
                                    swResult.WriteLine(temp);
                                    continue;
                                }

                                if( rxTime.IsMatch(temp) )
                                {
                                    //swResult.WriteLine(counter++);
                                    //swResult.WriteLine();
                                    swResult.WriteLine(temp);
                                    swResult.WriteLine();
                                    swResult.WriteLine(srSource.ReadLine());
                                    //swResult.WriteLine();
                                }
                                else
                                {
                                    swResult.WriteLine(srSource.ReadLine());
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

        private void WriteToTextFile( string writePath, string text )
        {
            string directoryName = Path.GetDirectoryName(writePath);
            string tempNameFile = Path.GetFileNameWithoutExtension(writePath) + ".txt";
            writePath = directoryName + "\\" + tempNameFile;

            try
            {
                using( StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.UTF8) )
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
            //OnMaxValue(pathToInputFiles.Count);
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
    }
}
