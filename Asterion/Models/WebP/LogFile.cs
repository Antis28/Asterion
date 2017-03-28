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

        public LogFile()
        {
            fileLogOut = new StreamWriter(
                Environment.CurrentDirectory + "\\log-error.txt", true
                );
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
        public void EndRecordToLog(int countFiles, bool isComplete = true )
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
            //Пишем в файл(поток)                
            fileLogOut.WriteLine(data);
        }

        public void SeparateRecord( bool isPrint = true )
        {
            if( isPrint )
                fileLogOut.WriteLine(new string('=', 50));
        }
    }
}
