#define Debug

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;

namespace Asterion.Models
{
    public class ChellForWebP
    {
        //------------- public -----------------------------//
        // Объявляем событие
        public event Action ChangeValueEvent;
        public event Action<int> MaxValueEvent;
        public event Action CompleteConvertEvent;
        public event Action CanceledConvertEvent;

        // Выполняется?
        public bool isRunning = false;
        // Все файлы конвертировать?
        public bool isAllFiles = true;

        public string[] pathFileNames;

        // Качество изображения
        public int quality = 85;
        public Resolution resolution;
        public int qualityAlpha = 100;

        //Process для консольного приложения
        private Process myProcess = null;
        StreamWriter fileLogOut;

        // Команда которую будет выполнять
        string command = string.Empty;

        private string pathToWebp = @"cwebp.exe";

        private string pathDirectory = "";
        private List<string> pathToInputFiles;

        //------------- public -----------------------------//
        public ChellForWebP()
        {

        }

        /// <summary>
        /// Конвертация выполняется в другом потоке
        /// </summary>
        /// <param name="pathDirectory"></param>
        public void BeginStartConvert( string pathDirectory )
        {
            this.pathDirectory = pathDirectory;
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "Вторичный";
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        /// <summary>
        /// Начать конвертацию в том же потоке
        /// </summary>
        /// <param name="pathDirectory"></param>
        public void StartConvert( string pathDirectory )
        {
            this.pathDirectory = pathDirectory;
            Start();
        }

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

        //------------- private -----------------------------//


        private void Start()
        {
            ExtractPathsFiles(pathDirectory); // получить адреса файлов

            if( !Directory.Exists(pathDirectory + @"\output") )
            {
                Directory.CreateDirectory(pathDirectory + @"\output");
            }

            string commandParameters = BuildParams();
            List<string> commands = BuildComands(commandParameters);

            //string timpName = DateTime.Now.ToString("HH-mm-ss");
            using( fileLogOut = new StreamWriter(Environment.CurrentDirectory + "\\log-error.txt", true) )
            {
                StartRecordToLog();
                foreach( var command in commands )
                {
                    fileLogOut.WriteLine(new string('=', 50));
                    PrepareConsole(command);
                    if( !isRunning )
                    {
                        OnCanceledConvert();
                        break;
                    }
                    OnChangeValue();
                    fileLogOut.WriteLine(new string('=', 50));
                }
                EndRecordToLog();
            }
            if( isRunning )
                OnCompleteConvert();

            // Очистка старых событий;
            ClearEvents();
        }        

        private void StartRecordToLog()
        {
            fileLogOut.WriteLine(new string('*', 50));
            fileLogOut.WriteLine();
            fileLogOut.WriteLine("Начало конвертации " + DateTime.Now);
            fileLogOut.WriteLine();
        }

        private void EndRecordToLog()
        {
            fileLogOut.WriteLine();
            fileLogOut.WriteLine("Конец конвертации " + DateTime.Now);
            fileLogOut.WriteLine();
            fileLogOut.WriteLine(new string('*', 50));
        }

        /// <summary>
        /// Получение полных путей файлов
        /// </summary>
        /// <param name="pathDirectory"></param>
        private void ExtractPathsFiles( string pathDirectory )
        {
            FileInfo[] files;
            pathToInputFiles = new List<string>();
            if( isAllFiles )
            {
                DirectoryInfo dir = new System.IO.DirectoryInfo(pathDirectory);
                files = dir.GetFiles();
            }
            else
            {
                int length = this.pathFileNames.Length;
                files = new FileInfo[length];
                for( int i = 0; i < length; i++ )
                {
                    files[i] = new FileInfo(this.pathFileNames[i]);
                }
            }
            foreach( var item in files )
            {
                pathToInputFiles.Add(item.FullName);
            }
            OnMaxValue(pathToInputFiles.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandParameters"></param>
        /// <returns>готовые аргуменнты для webP</returns>
        private List<string> BuildComands( string commandParameters )
        {
            List<string> commands = new List<string>();

            foreach( var currentFile in pathToInputFiles )
            {
                // Компановка команды для Webp конвертера
                command = string.Format(" {1} \"{2}\" {3}{4}.webP\"",
                    "/C",               // {0} Ключ /C - выполнение команды
                    pathToWebp,         // {1} Команда которую будет выполнять
                    currentFile,        // {2} Файл для конвертации
                    commandParameters,  // {3}
                    Path.GetFileNameWithoutExtension(currentFile)  //{4} имя для выходного файла
                    );
                commands.Add(command);
            }
            return commands;
        }

        private string BuildParams()
        {
            // Параметры для Webp конвертера
            StringBuilder sb = new StringBuilder();
            if( resolution.height > 100 && resolution.width > 100 )
            {
                sb.Append("-resize ");              // -resize
                sb.Append(resolution.ToString());   // <w> <h>
            }

            //sb.Append(" -progress ");          // -progress  report encoding progress 
            sb.Append(" -v ");                   // -v         verbose, e.g. print encoding/decoding times


            sb.Append(" -q ");          // -q       качество изображения от 0 до 100
            sb.Append(quality);
            sb.Append(" -alpha_q ");    // -alpha_q качество изображения для альфа канала от 0 до 100   
            sb.Append(qualityAlpha);
            sb.Append(" -o ");          // -o       адрес вывода файла
            sb.Append("\"");
            sb.Append(pathDirectory);
            sb.Append(@"\output\");     //          каталог вывода

            //commandParameters = string.Format(" -q {0} -alpha_q {1} {4} -o \"{2}{3}",
            //        quality,            //{0} -q       качество изображения от 0 до 100
            //        qualityAlpha,       //{1} -alpha_q качество изображения для альфа канала от 0 до 100
            //        pathDirectory,      //{2}  -o      адрес вывода файла
            //        @"\output\",        //{3}          каталог вывода
            //       "-resize " +
            //       resolution.ToString()//{4} -resize
            //    );
            return sb.ToString();
        }

        private string convertToCp866( string input )
        {
            Encoding cp866 = Encoding.GetEncoding(866);
            Encoding unicode = Encoding.Unicode;

            // Convert the string into a byte array.
            byte[] unicodeBytes = unicode.GetBytes(input);

            // Perform the conversion from one encoding to the other.
            byte[] cp866Bytes = Encoding.Convert(unicode, cp866, unicodeBytes);

            // Convert the new byte[] into a char[] and then into a string.
            char[] cp866Chars = new char[cp866.GetCharCount(cp866Bytes, 0, cp866Bytes.Length)];
            cp866.GetChars(cp866Bytes, 0, cp866Bytes.Length, cp866Chars, 0);
            string cp866String = new string(cp866Chars);

            return cp866String;
        }

        /// <summary>
        /// Подготовка передачи комманд в консоль
        /// </summary>
        /// <param name="command"></param>
        private void PrepareConsole( string command )
        {
            // Запускаем через cmd с параметрами command
            ProcessStartInfo startinfo = new ProcessStartInfo();

            InitStartInfo(startinfo);

            myProcess = new Process();
            myProcess.StartInfo = startinfo;
            myProcess.OutputDataReceived += cmd_DataReceived;
            myProcess.ErrorDataReceived += cmd_DataError;
            myProcess.EnableRaisingEvents = true;

            // запускаем процесс
            myProcess.Start();
            myProcess.BeginOutputReadLine();
            myProcess.BeginErrorReadLine();

            StartWebP(command);
            myProcess.WaitForExit();

            // Освобождаем
            myProcess = null;
            startinfo = null;
        }

        /// <summary>
        /// Ввод команд в консоль
        /// </summary>
        /// <param name="command">параметры для Webp</param>
        private void StartWebP( string command )
        {
            myProcess.StandardInput.Write(pathToWebp);
            // кодировка для русского языка
            byte[] buffer = Encoding.GetEncoding(866).GetBytes(command);
            myProcess.StandardInput.BaseStream.Write(buffer, 0, buffer.Length);
            myProcess.StandardInput.Write("\n");

            myProcess.StandardInput.WriteLine("exit");
        }

        /// <summary>
        /// Настройка ProcessStartInfo
        /// </summary>
        /// <param name="startinfo"></param>
        private void InitStartInfo( ProcessStartInfo startinfo )
        {
            startinfo.StandardOutputEncoding = Encoding.GetEncoding(866);

            startinfo.FileName = @"C:\Windows\System32\cmd.exe";
            // скрываем окно запущенного процесса
            startinfo.WindowStyle = ProcessWindowStyle.Hidden;
            // Перенаправить вывод
            startinfo.RedirectStandardOutput = true;
            startinfo.RedirectStandardInput = true;
            startinfo.RedirectStandardError = true;
            // Не используем shellexecute
            startinfo.UseShellExecute = false;
            // Не надо окон
            startinfo.CreateNoWindow = true;
        }

        private void ClearEvents()
        {
            ChangeValueEvent = null;
            MaxValueEvent = null;
            CompleteConvertEvent = null;
            CanceledConvertEvent = null;
        }

        void cmd_DataReceived( object sender, DataReceivedEventArgs e )
        {
            try
            {
                using( StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\log1.txt", true, Encoding.UTF8) )
                {
                    //Выводим                
                    sw.WriteLine(e.Data);
                    sw.Close();
                }
            } catch { }
        }

        void cmd_DataError( object sender, DataReceivedEventArgs e )
        {
            try
            {
                //Пишем в файл(поток)                
                fileLogOut.WriteLine(e.Data);
            } catch { }
        }


        // Используем метод для запуска события
        private void OnChangeValue()
        {
            ChangeValueEvent();
        }
        private void OnMaxValue( int maxValue )
        {
            MaxValueEvent(maxValue);
        }
        private void OnCompleteConvert()
        {
            CompleteConvertEvent();
        }
        private void OnCanceledConvert()
        {
            CanceledConvertEvent();
        }
        public struct Resolution
        {
            public int width;
            public int height;
            public Resolution( int w, int h )
            {
                width = w;
                height = h;
            }
            public override string ToString()
            {
                return width + " " + height;
            }
        }
    }
}

