#define Debug

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;
using Asterion.Models.WebP;

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
        public WebPParams parameters;

        //Process для консольного приложения
        private Process myProcess = null;
        private LogFile log;

        // Команда которую будет выполнять
        string command = string.Empty;

        private string pathToWebp = @"Dlls\cwebp.exe";

        private string pathDirectory = "";
        private List<string> pathToInputFiles;

        //------------- public -----------------------------//

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

            string commandParameters = parameters.BuildParams(pathDirectory);
            List<string> commands = BuildComands(commandParameters);

            log = new LogFile();
            log.StartRecordToLog();
            foreach( var command in commands )
            {
                if( !isRunning )
                    break;
                log.SeparateRecord(!parameters.IsQuiet);
                PrepareConsole(command);
                OnChangeValue();
                log.SeparateRecord(!parameters.IsQuiet);
            }

            if( isRunning )
            {
                log.EndRecordToLog(pathToInputFiles.Count);
                OnCompleteConvert();
            }
            else
            {
                log.EndRecordToLog(pathToInputFiles.Count, false);
                OnCanceledConvert();
            }
            // Очистка старых событий;
            ClearEvents();
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
                command = string.Format("{1} \"{2}\" {3}{4}.webP\"",
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
            //myProcess.StandardInput.Write(pathToWebp);
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
            startinfo.StandardErrorEncoding = Encoding.GetEncoding(866);

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
            //try
            //{
            //    using( StreamWriter sw = new StreamWriter(
            //        Environment.CurrentDirectory + @"\Log" + "\\log1.txt", true, Encoding.UTF8) )
            //    {
            //        sw.WriteLine(new string('*', 50));
            //        sw.WriteLine("Конвертация " + DateTime.Now);
            //        //Выводим                
            //        sw.WriteLine(e.Data);
            //        sw.WriteLine(new string('*', 50));
            //        sw.Close();
            //    }
            //} catch { }
        }

        void cmd_DataError( object sender, DataReceivedEventArgs e )
        {
            log.RecordLine(e.Data);
        }

        // Используем метод для запуска события
        private void OnChangeValue()
        {
            if( ChangeValueEvent != null )
                ChangeValueEvent();
        }
        private void OnMaxValue( int maxValue )
        {
            if( MaxValueEvent != null )
                MaxValueEvent(maxValue);
        }
        private void OnCompleteConvert()
        {
            if( CompleteConvertEvent != null )
                CompleteConvertEvent();
        }
        private void OnCanceledConvert()
        {
            if( CanceledConvertEvent != null )
                CanceledConvertEvent();
        }
    }
}

