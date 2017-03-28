﻿using System;
using System.Threading;
using Asterion.Models;
using Ookii.Dialogs.Wpf;
using WPFFolderBrowser;
using System.Windows;
using System.Collections.Generic;
using Asterion.Models.WebP;

namespace Asterion.Presentors
{
    //Объявление типа делегата для 2-го потока
    delegate void UpdateProgressBarDelegate( DependencyProperty dp, object value );

    /// <summary>
    /// MVP - model-view-presenter
    /// Presenter служит для связывание логики и UI
    /// </summary>
    class PresenterChellForWebp
    {

        object lockObject = new object(); //Объект для синхронизации потоков
        ChellForWebP chellForWebP = null;
        MainWindow mainWindow = null;

        public int currentHourChange = 0;
        public int currentMinutesChange = 0;

        public volatile bool isStopTimer = true;
        //string convertStatusText = "";

        public volatile string pathToMusicFile = "";



        public PresenterChellForWebp( MainWindow mainWindow )
        {
            this.chellForWebP = new ChellForWebP();
            this.mainWindow = mainWindow;
            this.mainWindow.startConvertEvent += new EventHandler(mainWindow_startConvert);
            this.mainWindow.openFolderDialogEvent += new EventHandler(mainWindow_openFolderDialog);
            this.mainWindow.openFileDialogToConverterEvent += new EventHandler(mainWindow_openFileDialog);
        }

        bool isRunning = false;
        private void mainWindow_startConvert( object sender, EventArgs e )
        {
            if( !isRunning )
            {
                isRunning = !isRunning;
                chellForWebP.isRunning = isRunning;
                mainWindow.btn_convert.Content = "Остановить";                
                {                    
                    int tmpWidth = 0, tmpHeight = 0, tmpQuality = 0;

                    int.TryParse(mainWindow.tb_qualityValue.Text, out tmpQuality);

                    // присвоение параметров из оболочки
                    chellForWebP.parameters = new WebPParams()
                    {
                        quality = tmpQuality,
                        qualityAlpha = 100,
                        IsQuiet = true,
                    };
                    if( (bool)mainWindow.cb_isChangeResolution.IsChecked )
                    {
                        int.TryParse(mainWindow.tbx_resolution_w.Text, out tmpWidth);
                        int.TryParse(mainWindow.tbx_resolution_h.Text, out tmpHeight);
                        chellForWebP.parameters.resolution = 
                                    new WebPParams.Resolution(tmpWidth, tmpHeight);
                    }
                    if( mainWindow.cb_isNoAlpha.IsChecked.Value )
                        chellForWebP.parameters.IsNoalpha = true;

                    if( mainWindow.cb_IsLossless.IsChecked.Value )
                        chellForWebP.parameters.IsLossless = true;
                }
                // Добавляем обработчик события             
                chellForWebP.MaxValueEvent += onInitialValue;
                chellForWebP.ChangeValueEvent += onChangeIndicator;
                chellForWebP.CompleteConvertEvent += onCompleteConvert;
                chellForWebP.CanceledConvertEvent += onCanceledConvert;

                if( mainWindow.cb_isDirectory.IsChecked.Value )
                {
                    chellForWebP.SwitchOnAllFiles();
                }
                else
                {
                    chellForWebP.SwitchOnSelectedFiles(PathFileNames);
                }
                

                chellForWebP.BeginStartConvert(mainWindow.tbx_addressField.Text);

            }
            else
            {
                isRunning = !isRunning;
                mainWindow.btn_convert.Content = "Начать";
                chellForWebP.isRunning = isRunning;

            }
        }
        void onChangeIndicator()
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        mainWindow.pb_percentConvert.Value += 1;
                        string currentValue;
                        if( mainWindow.isPercent )
                            currentValue = (int)(mainWindow.pb_percentConvert.Value / mainWindow.pb_percentConvert.Maximum * 100) + " %";
                        else
                            currentValue = mainWindow.pb_percentConvert.Value + " из " + mainWindow.pb_percentConvert.Maximum;

                        mainWindow.tb_percentConvert.Text = currentValue;//;
                    });

        }
        void onCompleteConvert()
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        mainWindow.tb_percentConvert.Text = "Конвертировние завершено";
                        isRunning = !isRunning;
                        mainWindow.btn_convert.Content = "Начать";
                    });

        }
        void onCanceledConvert()
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        mainWindow.tb_percentConvert.Text = "Конвертировние отменено";
                        mainWindow.btn_convert.Content = "Начать";
                    });

        }
        void onInitialValue( int maximum )
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        mainWindow.pb_percentConvert.Value = 0;
                        mainWindow.pb_percentConvert.Maximum = maximum;
                    });

        }

        string[] PathFileNames;
        private void mainWindow_openFileDialog( object sender, System.EventArgs e )
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name    
            dlg.Multiselect = true;
            dlg.Filter = "Графические файлы| *.PNG; *.JPG; *.JPEG; *.TIFF;";// Filter files by extension.mp3

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if( result == true )
            {
                // Open document
                mainWindow.tbx_addressField.Text = System.IO.Path.GetDirectoryName(dlg.FileName);
                PathFileNames = dlg.FileNames;
                ExistPath();
            }
        }
        private void mainWindow_openFolderDialog( object sender, System.EventArgs e )
        {
            var dialog = new WPFFolderBrowserDialog("Выберите каталог для обработки");
            bool? result = dialog.ShowDialog();
            if( result == true )
            {
                mainWindow.tbx_addressField.Text = dialog.FileName;
                ExistPath();
            }
        }
        public void ExistPath()
        {
            if( System.IO.Directory.Exists(mainWindow.tbx_addressField.Text) )
            {
                mainWindow.btn_convert.IsEnabled = true;
                if( mainWindow.cb_isDirectory.IsChecked == true )
                    mainWindow.tb_selectedValue.Text = System.IO.Directory.GetFiles(mainWindow.tbx_addressField.Text).Length.ToString();
                else
                    mainWindow.tb_selectedValue.Text = PathFileNames.Length.ToString();
            }
            else
            {
                mainWindow.tbx_addressField.Text = "Директория не существует";
                mainWindow.btn_convert.IsEnabled = false;
                mainWindow.tb_selectedValue.Text = "0";
            }
            mainWindow.tb_selectedValue.Text += " файлов";
        }
    }
}
