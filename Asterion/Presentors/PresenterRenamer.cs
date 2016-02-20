using System;
using System.IO;
using Asterion.Models;

namespace Asterion.Presentors
{
    public delegate void ProgressHandler( int progress );
    public delegate void ConsoleHandler( string progress );    

    class PresenterRenamer
    {
        LogicRanamer logicRanamer = null;
        MainWindow mainWindow = null;

        string filename = null;

        public PresenterRenamer( MainWindow mainWindow )
        {
            this.mainWindow = mainWindow;
            logicRanamer = new LogicRanamer();

            this.mainWindow.logicRanamerEvent += new EventHandler( mainWindow_logicRanamerEvent );
            this.mainWindow.openFileDialogEvent += new EventHandler( mainWindow_openFileDialog );
        }

        void mainWindow_logicRanamerEvent( object sender, System.EventArgs e )
        {
            if(!File.Exists( filename ) )            
            { mainWindow_openFileDialog( sender, e ); }

            //Здесь подставить checkbox.isChecked
            logicRanamer.IsOnlyJPG = false;
            logicRanamer.StartRenameToDateFormatWithoutDot( filename);
        }

        void mainWindow_openFileDialog( object sender, System.EventArgs e )
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name            
            dlg.Filter = "Графические файлы|*.jpg;*.png;*.gif;*.bmp|Все файлы (*.*)|*.*|Text documents (.txt) | *.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if( result == true )
            {
                // Open document
                filename = dlg.FileName;
            }
        }
    }
}
