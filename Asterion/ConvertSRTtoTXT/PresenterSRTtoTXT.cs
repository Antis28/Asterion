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
        ModelSRTtoTXT modelSRTtoTXT = null;
        MainWindow mainWindow = null;

        string[] pathFileNames;
        private bool isRunning;

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
            this.modelSRTtoTXT = new ModelSRTtoTXT();
            this.mainWindow = mainWindow;
            this.mainWindow.srtStartConvertEvent += new EventHandler(mainWindow_startConvert);
            this.mainWindow.srtOpenFolderDialogEvent += new EventHandler(mainWindow_openFolderDialog);
            this.mainWindow.srtOpenFileDialogEvent += new EventHandler(mainWindow_openFileDialog);            
            this.mainWindow.WebpDragEnterEvent += new DragEventHandler(mainWindow_WebpDragEnter);
            this.mainWindow.WebpPreviewDropEvent += new DragEventHandler(mainWindow_WebpPreviewDrop);
        }
        // ***************************************************** //
        #region Window Handlers
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
                mainWindow.btn_srtConvert.IsEnabled = true;
                if( mainWindow.cb_srtIsDirectory.IsChecked == true )
                    mainWindow.tb_srtSelectedValue.Text =
                        modelSRTtoTXT.FilterExtension(mainWindow.tbx_srtAddressField.Text)
                        .Length.ToString();
                else
                    mainWindow.tb_srtSelectedValue.Text = PathFileNames.Length.ToString();
            }
            else
            {
                mainWindow.tbx_addressField.Text = "Директория не существует";
                mainWindow.btn_srtConvert.IsEnabled = false;
                mainWindow.tb_srtSelectedValue.Text = "0";
            }
            mainWindow.tb_srtSelectedValue.Text += " файлов";
        }

        

        private void mainWindow_startConvert( object sender, EventArgs e )
        {
            if( !isRunning )
            {
                isRunning = !isRunning;
                modelSRTtoTXT.isRunning = isRunning;
                mainWindow.btn_srtConvert.Content = "Остановить";

                // Добавляем обработчик события             
                modelSRTtoTXT.MaxValueEvent += onInitialValue;
                modelSRTtoTXT.ChangeValueEvent += onChangeIndicator;
                modelSRTtoTXT.CompleteConvertEvent += onCompleteConvert;
                modelSRTtoTXT.CanceledConvertEvent += onCanceledConvert;

                if( !mainWindow.cb_srtIsDirectory.IsChecked.Value )
                    modelSRTtoTXT.SetSelectedFiles(PathFileNames);
                if( mainWindow.rb_srtToText.IsChecked.Value )
                {
                    modelSRTtoTXT.BeginConvertSrtToTxt(mainWindow.tbx_srtAddressField.Text);
                }else if( mainWindow.rb_textToSrt.IsChecked.Value )
                {
                    modelSRTtoTXT.BeginConvertTxtToSrt(mainWindow.tbx_srtAddressField.Text);
                }

            }
            else
            {
                isRunning = !isRunning;
                mainWindow.btn_srtConvert.Content = "Начать";
                modelSRTtoTXT.isRunning = false;
            }
        }
        #endregion
        // ***************************************************** //
        #region Convert Handlers
        private void onCanceledConvert()
        {
            throw new NotImplementedException();
        }

        private void onCompleteConvert()
        {
            throw new NotImplementedException();
        }

        private void onChangeIndicator()
        {
            throw new NotImplementedException();
        }

        private void onInitialValue( int obj )
        {
            throw new NotImplementedException();
        }
        #endregion
        // ***************************************************** //
    }
}
