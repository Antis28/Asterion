using System;
using System.Threading;
using Asterion.Models;
using Ookii.Dialogs.Wpf;
using WPFFolderBrowser;
using System.Threading;
using System.Windows;

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
        string convertStatusText = "";

        public volatile string pathToMusicFile = "";

        public string ConvertStatusText
        {
            get
            {
                mainWindow.Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,
                   (Action)delegate
                   {
                       convertStatusText = mainWindow.statusText.Text;
                   } );
                return convertStatusText;
            }

            set
            {
                convertStatusText = value;
                mainWindow.Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        mainWindow.statusText.Text = convertStatusText;
                    } );
            }
        }

        public PresenterChellForWebp( MainWindow mainWindow )
        {
            this.chellForWebP = new ChellForWebP();
            this.mainWindow = mainWindow;
            this.mainWindow.startConvertEvent += new EventHandler( mainWindow_startConvert );
            this.mainWindow.openFolderDialogEvent += new EventHandler( mainWindow_openFolderDialog );
        }

        private void mainWindow_startConvert( object sender, EventArgs e )
        {
            chellForWebP.quality = int.Parse( mainWindow.tb_qualityValue.Text );
            // Добавляем обработчик события          
            chellForWebP.MaxValueEvent += onInitialValue;
            chellForWebP.ChangeValueEvent += onChangeIndicator;
            chellForWebP.CompleteConvertEvent += onCompleteConver;
            chellForWebP.BeginStartConvert( mainWindow.tb_addressField.Text );
        }
        void onChangeIndicator()
        {
            mainWindow.Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        mainWindow.pb_percentConvert.Value += 1;
                        string currentValue;
                        if( mainWindow.isPercent )
                            currentValue = (int)( mainWindow.pb_percentConvert.Value / mainWindow.pb_percentConvert.Maximum * 100 ) + " %";
                        else
                            currentValue = mainWindow.pb_percentConvert.Value + " из " + mainWindow.pb_percentConvert.Maximum;

                        mainWindow.tb_percentConvert.Text = currentValue;//;
                    } );

        }
        void onCompleteConver()
        {
            mainWindow.Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        mainWindow.tb_percentConvert.Text = "Конвертировние завершено";
                    } );

        }
        void onInitialValue( int maximum )
        {
            mainWindow.Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        mainWindow.pb_percentConvert.Value = 0;
                        mainWindow.pb_percentConvert.Maximum = maximum;
                    } );

        }

        private void StartTimer()
        {
            /*
            Thread threadTimer = new Thread( new ParameterizedThreadStart( alarmTimer.StartTimer ) )
            { IsBackground = true, Name = "StartTimer" };
            threadTimer.Start( this );
            while( !isStopTimer )
            {
                Thread.Sleep( TimeSpan.FromSeconds( 1 ) );
                if( !isStopTimer )
                    isStopTimer = !alarmTimer.TimerIsStart;
            }
            mainWindow.Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,
                                                new Action( DisableTimer ) );            
            threadTimer.Abort();

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            if( System.IO.File.Exists(pathToMusicFile) )
                process.StartInfo.FileName = pathToMusicFile;
            else if( System.IO.File.Exists( @"\Alarm01.wav" ) )
            {
                process.StartInfo.FileName = @"\Alarm.wav";
            } else
            { process.StartInfo.FileName = @"C:\Windows\Media\Alarm02.wav"; }

                process.Start();
                */
        }

        private void DisableTimer()
        {
            /*
            mainWindow.startTimerButton.Foreground = System.Windows.Media.Brushes.Black;
            mainWindow.startTimerButton.Background = new System.Windows.Media.SolidColorBrush( System.Windows.Media.Color.FromArgb( 0xFF, 0xE3, 0xE3, 0xE3 ) );
            mainWindow.startTimerButton.Content = "Включить таймер";
            TimerStatusText = "Cтатус: таймер выключен";
            isStopTimer = true;
            */
        }

        private void EnableTimer()
        {
            /*
            mainWindow.startTimerButton.Foreground = System.Windows.Media.Brushes.Red;
            mainWindow.startTimerButton.Background = System.Windows.Media.Brushes.Yellow;
            mainWindow.startTimerButton.Content = "Таймер включен";            
            isStopTimer = false;
            */
        }

        private void ChangedTimeInTimer()
        {
            /*
            if( this.mainWindow.hoursComboBox != null && this.mainWindow.minutesComboBox != null )
            {
                currentHourChange = (int)this.mainWindow.hoursComboBox.SelectedValue;
                currentMinutesChange = (int)this.mainWindow.minutesComboBox.SelectedValue;
            }
            */
        }

        private void mainWindow_openFileDialog( object sender, System.EventArgs e )
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name            
            dlg.Filter = "Музыкальные файлы| *.mp3; *.wav; *.flac; *.wma; *.ogg|Видео файлы|*.avi; *.mp4|Все файлы (*.*)|*.*"; // Filter files by extension.mp3

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if( result == true )
            {
                // Open document
                mainWindow.pathToFile.Text = dlg.FileName;
            }
        }
        private void mainWindow_openFolderDialog( object sender, System.EventArgs e )
        {
            var dialog = new WPFFolderBrowserDialog( "Выберите каталог для обработки" );
            bool? result = dialog.ShowDialog();
            if( result == true )
            {
                mainWindow.tb_addressField.Text = dialog.FileName;
                ExistPath();
            }
        }
        public void ExistPath()
        {
            if( System.IO.Directory.Exists( mainWindow.tb_addressField.Text ) )
            {
                mainWindow.btn_convert.IsEnabled = true;
            } else
            {
                mainWindow.tb_addressField.Text = "Директория не существует";
                mainWindow.btn_convert.IsEnabled = false;
            }
        }
    }
}
