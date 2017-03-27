using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.IO;
using System.Text;

namespace Asterion.Models
{
  public class ChellForWebP
    {
        bool iDebug = false;
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

        public int qualityAlpha = 100;

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

        //Process для консольного приложения
        private Process myProcess = null;

        // Команда которую будет выполнять
        string command = string.Empty;
        string commandParameters = string.Empty;

        private string pathToWebp = @"cwebp.exe";        

        private string pathDirectory = "";
        private List<string> pathToInputFiles;

        //------------- public -----------------------------//
        public ChellForWebP()
        {
            Initialization();

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
                
        //получение полных путей файлов
        public void ExtractPathsFiles( string pathDirectory )
        {
            FileInfo[] files;
            pathToInputFiles = new List<string>();
            if( isAllFiles )
            {
                DirectoryInfo dir = new System.IO.DirectoryInfo( pathDirectory );                
                files = dir.GetFiles();
            } else
            {
                int length = this.pathFileNames.Length;
                files = new FileInfo[length];
                for( int i = 0; i < length; i++ )                
                {
                    files[i] = new FileInfo( this.pathFileNames[i] );
                }                 
            }
            foreach( var item in files )
            {
                pathToInputFiles.Add( item.FullName );
            }
        }

        //------------- private -----------------------------//
        private void Initialization()
        {
            myProcess = new Process();
        }

        private void Start()
        {
            ExtractPathsFiles( pathDirectory );
            if( !Directory.Exists( pathDirectory + @"\output" ) )
            {
                Directory.CreateDirectory( pathDirectory + @"\output" );
            }
            // Параметры для Webp конвертера
            commandParameters = string.Format("{0} {1}",

                    // -q качество изображения от 0 до 100
                    "\" -q ",
                    quality,
                    // качество изображения для альфа канала от 0 до 100
                    " -alpha_q 100 -o \"",
                    qualityAlpha,
                    pathDirectory,
                    @"\output\"                    
                );

            List<string> commands = new List<string>();

            OnMaxValue( pathToInputFiles.Count );
            foreach( var currentFile in pathToInputFiles )
            {
                // Компановка команды для Webp конвертера
                //string command = "/C " + pathToWebp + " \"" + currentFile + "\" -q " + quality + " -alpha_q 100 -o \"" + pathDirectory + @"\output\" + Path.GetFileNameWithoutExtension( currentFile ) + ".webP\"";
                command = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                    "/C ",          // Ключ /C - выполнение команды
                    pathToWebp,     // Команда которую будет выполнять
                    " \"",
                    currentFile,
                    commandParameters,
                    // имя для выходного файла
                    Path.GetFileNameWithoutExtension(currentFile),
                    ".webP\""
                    );
                // преобразование кодировки для консоли
                //command = convertToCp866( command );
                commands.Add( command );
            }

            foreach( var command in commands )
            {
                OnChangeValue();
                convertFileToWebP( command );
                if( !isRunning )
                {
                    OnCanceledConvert();
                    break;
                }
            }
            if( isRunning )
                OnCompleteConvert();

            // Очистка старых событий;
            ClearEvents();
        }
        private string convertToCp866( string input )
        {
            Encoding cp866 = Encoding.GetEncoding( 866 );
            Encoding unicode = Encoding.Unicode;

            // Convert the string into a byte array.
            byte[] unicodeBytes = unicode.GetBytes( input );

            // Perform the conversion from one encoding to the other.
            byte[] cp866Bytes = Encoding.Convert( unicode, cp866, unicodeBytes );

            // Convert the new byte[] into a char[] and then into a string.
            char[] cp866Chars = new char[cp866.GetCharCount( cp866Bytes, 0, cp866Bytes.Length )];
            cp866.GetChars( cp866Bytes, 0, cp866Bytes.Length, cp866Chars, 0 );
            string cp866String = new string( cp866Chars );

            return cp866String;
        }

        private void convertFileToWebP( string command )
        {
            // Запускаем через cmd с параметрами command
            ProcessStartInfo psiOpt = new ProcessStartInfo( @"cmd.exe", command );
            if( iDebug )
            {                
                psiOpt.WindowStyle = ProcessWindowStyle.Normal;
                psiOpt.RedirectStandardOutput = false;
                psiOpt.UseShellExecute = true;
                psiOpt.CreateNoWindow = true;
            } else
            {
                // скрываем окно запущенного процесса
                psiOpt.WindowStyle = ProcessWindowStyle.Hidden;
                psiOpt.RedirectStandardOutput = true;
                psiOpt.UseShellExecute = false;
                psiOpt.CreateNoWindow = true;
            }
            
            // запускаем процесс
            Process procCommand = Process.Start( psiOpt );
            // получаем ответ запущенного процесса
            StreamReader srIncoming;
            if( !iDebug )
                srIncoming = procCommand.StandardOutput;
            //StreamReader srOutcoming = procCommand.StandardInput;
            // выводим результат
            //MessageBox.Show( srIncoming.ReadToEnd() );
            // закрываем процесс            
            procCommand.WaitForExit();
            if( iDebug )
                procCommand.WaitForInputIdle( 4000 );
        }
        private void ClearEvents()
        {
            ChangeValueEvent = null;
            MaxValueEvent = null;
            CompleteConvertEvent = null;
            CanceledConvertEvent = null;
        }        
    }
}

