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

        public void BeginStartConvert( string pathDirectory )
        {
            this.pathDirectory = pathDirectory;
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "SrtToTxt";
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }
        //------------- private -----------------------------//
        private void Start()
        {
            ExtractPathsFiles(pathDirectory); // получить адреса файлов
            string path = pathToInputFiles[0];
            string text = ReadFromSrtFile(path);
            WriteToTextFile(path, text);
            WriteToSrtFile(path);

        }

        private void WriteToSrtFile( string writePath )
        {
            string readPath = string.Empty;
            string text = string.Empty;
            string fileName = string.Empty;
            string FullFileName = string.Empty;
            string folderName = string.Empty;

            readPath = Path.GetFileNameWithoutExtension(writePath) + ".txt";
            fileName = Path.GetFileNameWithoutExtension(writePath);
            folderName = Path.GetDirectoryName(writePath);
            FullFileName = folderName + "\\" + fileName + "-ru.srt";
            int counter = 1;

            Regex rxTime = new Regex(@"\d+:\d+"); // 00:00
            Regex rxNum = new Regex(@"^\d+"); // 00
            try
            {   // Open the text file using a stream reader.
                using( StreamWriter swResult = new StreamWriter(File.Create(FullFileName), System.Text.Encoding.UTF8) )
                {
                    using( StreamReader srOriginal = new StreamReader(writePath, System.Text.Encoding.UTF8) )
                    {
                        using( StreamReader srSource = new StreamReader(readPath, System.Text.Encoding.UTF8) )
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
            writePath = Path.GetFileNameWithoutExtension(writePath) + ".txt";
            try
            {
                using( StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.UTF8) )
                {
                    sw.WriteLine(text);
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
