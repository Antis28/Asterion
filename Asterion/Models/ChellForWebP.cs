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
    class ChellForWebP
    {
        // Объявляем событие
        public event Action ChangeValueEvent;
        public event Action<int> MaxValueEvent;
        public event Action CompleteConvertEvent;
        public event Action CanceledConvertEvent;
        
        // Используем метод для запуска события
        public void OnChangeValue()
        {
            ChangeValueEvent();
        }
        public void OnMaxValue( int maxValue )
        {
            MaxValueEvent( maxValue );
        }
        public void OnCompleteConvert()
        {
            CompleteConvertEvent();
        }
        public void OnCanceledConvert()
        {
            CanceledConvertEvent();
        }

        // Выполняется?
        public bool isRunning = false;
        //Process для консольного приложения
        Process myProcess;
        string pathToWebp = @"cwebp.exe";
        string pathDirectory = "";
        List<string> pathToInputFiles;
        public int quality = 85;

        public ChellForWebP()
        {
            Initialization();

        }

        void Initialization()
        {
            myProcess = new Process();
        }
        //получение полных путей файлов
        public void ExtractPathsFiles( string pathDirectory )
        {
            DirectoryInfo dir = new System.IO.DirectoryInfo( pathDirectory );
            pathToInputFiles = new List<string>();
            FileInfo[] files = dir.GetFiles();
            foreach( var item in files )
            {
                pathToInputFiles.Add( item.FullName );
            }
        }

        public void BeginStartConvert( string pathDirectory )
        {
            this.pathDirectory = pathDirectory;
            Thread backgroundThread = new Thread( new ThreadStart( Start ) );
            backgroundThread.Name = "Вторичный";
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }
        public void StartConvert( string pathDirectory )
        {
            this.pathDirectory = pathDirectory;
            Start();
        }

        private void Start()
        {
            ExtractPathsFiles( pathDirectory );
            if( !Directory.Exists( pathDirectory + @"\output" ) )
            {
                Directory.CreateDirectory( pathDirectory + @"\output" );
            }
            List<string> commands = new List<string>();
            OnMaxValue( pathToInputFiles.Count );
            foreach( var currentFile in pathToInputFiles )
            {
                // Компановка команды для Webp конвертера
                string command = "/C " + pathToWebp + " \"" + currentFile + "\" -q " + quality + " -alpha_q 100 -o \"" + pathDirectory + @"\output\" + Path.GetFileNameWithoutExtension( currentFile ) + ".webP\"";
                // преобразование кодировки для консоли
                //command = convertToCp866( command );
                commands.Add( command );
            }

            foreach( var command in commands )
            {
                OnChangeValue();
                convertFileToWebP( command );
                if( !isRunning )
                    break;
            }
            if( !isRunning )
                OnCanceledConvert();
            else
                OnCompleteConvert();
            // Очистка старых событий;
            ClearEvents();
        }
        string convertToCp866( string input )
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
        void convertFileToWebP( string command )
        {
            // создаем процесс cmd.exe с параметрами command
            ProcessStartInfo psiOpt = new ProcessStartInfo( @"cmd.exe", command );
            // скрываем окно запущенного процесса
            psiOpt.WindowStyle = ProcessWindowStyle.Hidden;
            psiOpt.RedirectStandardOutput = true;
            psiOpt.UseShellExecute = false;
            psiOpt.CreateNoWindow = true;
            // запускаем процесс
            Process procCommand = Process.Start( psiOpt );
            // получаем ответ запущенного процесса
            StreamReader srIncoming = procCommand.StandardOutput;
            //StreamReader srOutcoming = procCommand.StandardInput;
            // выводим результат
            //MessageBox.Show( srIncoming.ReadToEnd() );
            // закрываем процесс
            procCommand.WaitForExit();
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

