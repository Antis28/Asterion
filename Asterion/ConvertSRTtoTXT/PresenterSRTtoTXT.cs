#define Debug
#define framewok_4_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using WpfFolderBrowser;

namespace Asterion.ConvertSRTtoTXT
{
    //Объявление типа делегата для 2-го потока
    delegate void UpdateProgressBarDelegate( DependencyProperty dp, object value );

    /// <summary>
    /// MVP - model-view-presenter
    /// Presenter служит для связывание логики и UI
    /// </summary>
    class PresenterSRTtoTXT
    {
        object lockObject = new object(); //Объект для синхронизации потоков
        //ChellForWebP chellForWebP = null;
        MainWindow mainWindow = null;

        string[] pathFileNames;
        public string[] PathFileNames
        {
            get
            {
                return pathFileNames;
            }

            set
            {
                pathFileNames = value;
            }
        }

        public PresenterSRTtoTXT( MainWindow mainWindow )
        {
            //this.chellForWebP = new ChellForWebP();
            this.mainWindow = mainWindow;
            this.mainWindow.startConvertEvent += new EventHandler(mainWindow_startConvert);
            this.mainWindow.srtOpenFolderDialogEvent += new EventHandler(mainWindow_openFolderDialog);
            this.mainWindow.srtOpenFileDialogEvent += new EventHandler(mainWindow_openFileDialog);            
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

        private void mainWindow_openFileDialog( object sender, System.EventArgs e )
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name    
            dlg.Multiselect = true;
            dlg.Filter = "Субтитры| *.SRT;";// Filter files by extension.mp3

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if( result == true )
            {
                // Open document
                mainWindow.tbx_srtAddressField.Text = System.IO.Path.GetDirectoryName(dlg.FileName);
                PathFileNames = dlg.FileNames;
                ExistPath();
            }
        }
        private void mainWindow_openFolderDialog( object sender, System.EventArgs e )
        {
            var dialog = new WpfFolderBrowserDialog("Выберите каталог для обработки");
            bool? result = dialog.ShowDialog();
            if( result == true )
            {
                mainWindow.tbx_srtAddressField.Text = dialog.FileName;
                ExistPath();
            }
        }
        public void ExistPath()
        {
            if( System.IO.Directory.Exists(mainWindow.tbx_srtAddressField.Text) )
            {
                mainWindow.btn_convert.IsEnabled = true;
                if( mainWindow.cb_srtIsDirectory.IsChecked == true )
                    mainWindow.tb_srtSelectedValue.Text =
                        FilterExtension(mainWindow.tbx_srtAddressField.Text)
                        .Length.ToString();
                else
                    mainWindow.tb_srtSelectedValue.Text = PathFileNames.Length.ToString();
            }
            else
            {
                mainWindow.tbx_addressField.Text = "Директория не существует";
                mainWindow.btn_convert.IsEnabled = false;
                mainWindow.tb_srtSelectedValue.Text = "0";
            }
            mainWindow.tb_srtSelectedValue.Text += " файлов";
        }

        public static string[] FilterExtension( string pathDirectory )
        {
#if framewok_4_0
            //For .NET 4.0 and later, 
            var files = Directory.EnumerateFiles(pathDirectory, "*.*", SearchOption.TopDirectoryOnly)
            .Where(s =>
            s.EndsWith(".SRT", StringComparison.OrdinalIgnoreCase)
            );
#endif
#if framewok_do_4_0
            //For earlier versions of .NET,
            var files = Directory.GetFiles("C:\\path", "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".srt"));
#endif
            return files.ToArray<string>();
        }

        private void mainWindow_startConvert( object sender, EventArgs e )
        {
            throw new NotImplementedException();
        }
    }
}
