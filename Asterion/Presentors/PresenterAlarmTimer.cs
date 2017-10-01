using System;
using System.Threading;
using Asterion.Models;
using Asterion.UIHelpers;

namespace Asterion.Presentors
{
    /// <summary>
    /// MVP - model-view-presenter
    /// Presenter служит для связывание логики и UI
    /// </summary>
    class PresenterAlarmTimer
    {
        object lockObject = new object(); //Объект для синхронизации потоков
        LogicAlarmTimer alarmTimer = null;
        MainWindow mainWindow = null;

        public int currentHourChange = 0;
        public int currentMinutesChange = 0;
        
        public volatile bool isStopTimer = true;
        string timerStatusText = "";

        public volatile string pathToMusicFile = "";

        public string TimerStatusText
        {
            get
            {
                 mainWindow.Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        timerStatusText = mainWindow.statusText.Text;
                    } );
                return timerStatusText;
            }

            set
            {
                timerStatusText = value;
                mainWindow.Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                        mainWindow.statusText.Text = timerStatusText;
                    } );                
            }
        }

        public PresenterAlarmTimer( MainWindow mainWindow )
        {
            this.alarmTimer = new LogicAlarmTimer();
            this.mainWindow = mainWindow;
            this.mainWindow.startAlarmEvent += new EventHandler( mainWindow_startAlarmEvent );
            this.mainWindow.startTimerEvent += new EventHandler( mainWindow_startTimerEvent );
            this.mainWindow.openFileDialogEvent += new EventHandler( mainWindow_openFileDialog );

        }

        private void mainWindow_startAlarmEvent( object sender, EventArgs e )
        {
            throw new NotImplementedException();
        }

        void mainWindow_startTimerEvent( object sender, System.EventArgs e )
        {
            if( isStopTimer )
            {
                pathToMusicFile = mainWindow.pathToFile.Text;
                EnableTimer();
                ChangedTimeInTimer();
                new Thread( StartTimer ) { IsBackground = true, Name = "PreStartTimer" }.Start();
            } else
            {
                DisableTimer();
            }
        }

        private void StartTimer()
        {
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
        }        

        private void DisableTimer()
        {
            mainWindow.startTimerButton.Foreground = System.Windows.Media.Brushes.Black;
            mainWindow.startTimerButton.Background = new System.Windows.Media.SolidColorBrush( System.Windows.Media.Color.FromArgb( 0xFF, 0xE3, 0xE3, 0xE3 ) );
            mainWindow.startTimerButton.Content = ButtonsName.ON_TIMER;
            TimerStatusText = LabelText.STATUS_OFF_TIMER;
            isStopTimer = true;
        }

        private void EnableTimer()
        {
            mainWindow.startTimerButton.Foreground = System.Windows.Media.Brushes.Red;
            mainWindow.startTimerButton.Background = System.Windows.Media.Brushes.Yellow;
            mainWindow.startTimerButton.Content = ButtonsName.OFF_TIMER;
            isStopTimer = false;
        }

        private void ChangedTimeInTimer()
        {
            if( this.mainWindow.hoursComboBox != null && this.mainWindow.minutesComboBox != null )
            {
                currentHourChange = (int)this.mainWindow.hoursComboBox.SelectedValue;
                currentMinutesChange = (int)this.mainWindow.minutesComboBox.SelectedValue;
            }
        }

        void mainWindow_openFileDialog( object sender, System.EventArgs e )
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
    }
}
