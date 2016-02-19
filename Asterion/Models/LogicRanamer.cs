using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;

namespace Asterion.Models
{
    class LogicRanamer
    {
        class RenameFile
        {
            //static public event ProgressHandler Progress;
            //static public event ConsoleHandler ConsoleEvent;

            //public static ListBox _myConsole;  // пишет отчет о проделаной работе
            //public static CheckBox _isOnlyJPG;

            public static bool isFirst = true;
            static string[] _selectFiles = new string[] {"",""};


            const string _startName = "Разборка ";

            static int number = 1;//переменная для добавления номера к файлу


            /// <summary>
            /// Переименовывает файл в соответсвии с параметрами
            /// </summary>
            /// <param name="selectedPath"></param>
            /// <param name="MyConsole"></param>
            public static void StartRename_2( string selectedPath )
            {
                DirectoryInfo directoryInfo = new DirectoryInfo( selectedPath );

                if( isFirst )
                {
                    isFirst = false;
                }
               

                //new Thread( () =>
                //NameAdded_2( directoryInfo )
                //).Start();
            }

            /*
            private static int UpdateProgressBar( DirectoryInfo DI, int currentItem )
            {
                #region Обновляет прогрессБар
                // Обновляет прогрессБар
                int allItem = DI.GetFiles().Length;
                currentItem++;

                int progress = (Int32)((currentItem * 1d / allItem) * 100d);
                if( progress > 100 )
                    progress = 100;
                else if( progress < 0 )
                    progress = 0;

                if( Progress != null )
                    Progress( progress );
                #endregion
                return currentItem;
            }
           */
           
            
        }
    }
}
