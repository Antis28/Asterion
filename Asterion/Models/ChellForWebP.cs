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
        Process myProcess;
        string pathToWebp = @"cwebp.exe";
        List<string> pathToInputFiles;
        int quality = 85;

        object lockObject = new object(); //Объект для синхронизации потоков

        public ChellForWebP()
        {
            Initialization();
        }

        void Initialization()
        {
            myProcess = new Process();
        }
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
        public void Start( string pathDirectory )
        {
            ExtractPathsFiles( pathDirectory );
            /*
            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                // You can start any process.
                myProcess.StartInfo.FileName = "";//pathToWebp;
                myProcess.StartInfo.Arguments = pathToInputFiles[0];
                myProcess.StartInfo.WorkingDirectory = pathDirectory;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                // This code assumes the process you are starting will terminate itself. 
                // Given that is is started without a window so you cannot terminate it 
                // on the desktop, it must terminate itself or you can do it programmatically
                // from this application using the Kill method.
            } catch( Exception e )
            {
                MessageBox.Show( e.Message );
            }
            */
            //////////////////////////////////////////
            if(!Directory.Exists( pathDirectory + @"\output" )){
                Directory.CreateDirectory( pathDirectory + @"\output" );
            }
            string command = "/C " + pathToWebp +" \"" + pathToInputFiles[0] + "\" -q " + quality + " -alpha_q 100 -o \"" + pathDirectory + @"\output\"  + Path.GetFileNameWithoutExtension(pathToInputFiles[0])+".webP\"";            

            //string command2 = @"/C cwebp.exe T:\2\40181.jpg -q 85 -alpha_q 100 -o T:\2\output\40181.webP";
            command = convertToCp866( command );
           
            // создаем процесс cmd.exe с параметрами "ipconfig /all"
            ProcessStartInfo psiOpt = new ProcessStartInfo( @"cmd.exe", command );//, @"/C ipconfig /all" );
                       
            // скрываем окно запущенного процесса
            psiOpt.WindowStyle = ProcessWindowStyle.Hidden;
            psiOpt.RedirectStandardOutput = true;
            //psiOpt.RedirectStandardOutput = false;
            //psiOpt.RedirectStandardInput = true;
            psiOpt.UseShellExecute = false;
            psiOpt.CreateNoWindow = true;
            //psiOpt.CreateNoWindow = false;
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
        string convertToCp866(string input )
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
    }
}

