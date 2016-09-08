using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace Asterion.Presentors
{
    class ControllerDiary
    {
        //Main window(form)
        MainWindow mainWindow = null;

        Models.Diary diary;

        public ControllerDiary( MainWindow mainWindow )
        {
            diary = new Models.Diary();
            listRecords = diary.ReadDiary();
            this.mainWindow = mainWindow;
            this.mainWindow.diaryOpenEvent += new EventHandler( mainWindow_diaryOpen );
            this.mainWindow.diarySaveEvent += new EventHandler( mainWindow_diarySave );
            this.mainWindow.diaryShowEvent += new EventHandler( mainWindow_diaryShow );
        }

        List<string> listRecords;
        private void mainWindow_diaryShow( object sender, EventArgs e )
        {
            FileChange();
            if( mainWindow.txb_Diary.Visibility == Visibility.Visible )
            {
                mainWindow.txb_Diary.Visibility = Visibility.Collapsed;
                mainWindow.lvDiary.Visibility = Visibility.Visible;
                mainWindow.btToggleDiary.Content = "Новая запись";
                mainWindow.lvDiary.Items.Clear();
                foreach( var item in listRecords )
                {
                    mainWindow.lvDiary.Items.Add( MyLabel( item ) );
                }

            } else
            {
                mainWindow.txb_Diary.Visibility = Visibility.Visible;
                mainWindow.lvDiary.Visibility = Visibility.Collapsed;
                mainWindow.btToggleDiary.Content = "Показать дневник";
            }
        }

        void mainWindow_diaryOpen( object sender, System.EventArgs e )
        {
            diary = new Models.Diary();
        }

        void mainWindow_diarySave( object sender, System.EventArgs e )
        {

            diary.Save( mainWindow.txb_Diary.Text );
        }

        private System.Windows.Controls.Label MyLabel( string text )
        {
            System.Windows.Controls.Label tb = new System.Windows.Controls.Label()
            {
                FontSize = 22,
                //Height = 25,                                
                Content = text
            };
            tb.MouseDoubleClick += Tb_MouseDoubleClick;
            return tb;
        }

        private void Tb_MouseDoubleClick( object sender, System.Windows.Input.MouseButtonEventArgs e )
        {
            
            string s = e.OriginalSource.ToString();
            
            System.Windows.MessageBox.Show( s );
        }

        System.IO.FileSystemWatcher fsw;
        private void FileChange()
        {
           fsw = new System.IO.FileSystemWatcher( 
                System.IO.Directory.GetCurrentDirectory(), diary.pathToXmlDocument
                ); //следим за diary.pathToXmlDocument
            fsw.Changed += new System.IO.FileSystemEventHandler( fsw_Changed );
            fsw.EnableRaisingEvents = true;
        }

        void fsw_Changed( object sender, System.IO.FileSystemEventArgs e )
        {
            listRecords = diary.ReadDiary();
            
            try
            {
                System.Windows.MessageBox.Show( "Файл изменен!" );
                fsw.EnableRaisingEvents = false; //отключаем слежение
            } finally
            {
                fsw.EnableRaisingEvents = true; //переподключаем слежение
            }
        }
    }
}
//Просто проверка работы события слежения за файлом