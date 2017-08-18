using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Asterion.ConvertSRTtoTXT
{
    class PresenterSRTtoTXT
    {
        object lockObject = new object(); //Объект для синхронизации потоков
        //ChellForWebP chellForWebP = null;
        MainWindow mainWindow = null;

        string[] pathFileNames;

        public PresenterSRTtoTXT( MainWindow mainWindow )
        {
            //this.chellForWebP = new ChellForWebP();
            this.mainWindow = mainWindow;
            this.mainWindow.startConvertEvent += new EventHandler(mainWindow_startConvert);
            this.mainWindow.openFolderDialogEvent += new EventHandler(mainWindow_openFolderDialog);
            this.mainWindow.openFileDialogToConverterEvent += new EventHandler(mainWindow_openFileDialog);
            this.mainWindow.profileSelectedEvent += new EventHandler(mainWindow_profileSelected);
            this.mainWindow.WebpDragEnterEvent += new DragEventHandler(mainWindow_WebpDragEnter);
            this.mainWindow.WebpPreviewDropEvent += new DragEventHandler(mainWindow_WebpPreviewDrop);
        }

        private void mainWindow_WebpPreviewDrop( object sender, DragEventArgs e )
        {
            throw new NotImplementedException();
        }

        private void mainWindow_WebpDragEnter( object sender, DragEventArgs e )
        {
            throw new NotImplementedException();
        }

        private void mainWindow_profileSelected( object sender, EventArgs e )
        {
            throw new NotImplementedException();
        }

        private void mainWindow_openFileDialog( object sender, EventArgs e )
        {
            throw new NotImplementedException();
        }

        private void mainWindow_openFolderDialog( object sender, EventArgs e )
        {
            throw new NotImplementedException();
        }

        private void mainWindow_startConvert( object sender, EventArgs e )
        {
            throw new NotImplementedException();
        }
    }
}
