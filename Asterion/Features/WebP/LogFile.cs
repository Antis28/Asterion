using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Asterion.Models.WebP
{
    class LogFile
    {
        StreamWriter fileLogOut = null;
        string pathFolder = Environment.CurrentDirectory + @"\Log";
        public string pathFile = @"\log-error.txt";

        public LogFile()
        {

            if( !Directory.Exists(pathFolder) )
            {
                Directory.CreateDirectory(pathFolder);
            }
            string fullPath = pathFolder + pathFile;
            fileLogOut = new StreamWriter(fullPath, true);
        }

        public void StartRecordToLog()
        {
            fileLogOut.WriteLine(new string('*', 50));
            fileLogOut.WriteLine();
            fileLogOut.WriteLine("Начало конвертации " + DateTime.Now);
            fileLogOut.WriteLine();
        }
        /// <summary>
        /// закрывает файл
        /// </summary>
        public void EndRecordToLog( int countFiles, bool isComplete = true )
        {
            fileLogOut.WriteLine();
            if( isComplete )
                fileLogOut.WriteLine("Конец конвертации " + DateTime.Now);
            else
                fileLogOut.WriteLine("Конвертация отменена " + DateTime.Now);
            fileLogOut.WriteLine();
            fileLogOut.WriteLine("Конвертировано файлов {0}", countFiles);
            fileLogOut.WriteLine();
            fileLogOut.WriteLine(new string('*', 50));
            fileLogOut.Close();
        }

        public void RecordLine( string data )
        {
            if( data == null )
                return;
            fileLogOut.WriteLine(data);

            //byte[] buffer = Encoding.UTF8.GetBytes(data);
            ////Пишем в файл(поток)
            //foreach( var item in buffer )
            //{
            //    fileLogOut.BaseStream.WriteByte(item);
            //}
        }

        public void SeparateRecord( bool isPrint = true )
        {
            if( isPrint )
                fileLogOut.WriteLine(new string('=', 50));
        }
    }
}
