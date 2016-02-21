using System;
using System.Threading;
using Asterion.Models;


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

        int currentHourChange = 0;
        int currentMinutesChange = 0;
        volatile bool isStopTimer = true;

        public PresenterAlarmTimer( MainWindow mainWindow )
        {            
            this.alarmTimer = new LogicAlarmTimer();
            this.mainWindow = mainWindow;
            this.mainWindow.startAlarmEvent += new EventHandler( mainWindow_startAlarmEvent );
            this.mainWindow.startTimerEvent += new EventHandler( mainWindow_startTimerEvent );
        }

        private void mainWindow_startAlarmEvent( object sender, EventArgs e )
        {
            throw new NotImplementedException();
        }

        void mainWindow_startTimerEvent( object sender, System.EventArgs e )
        {
            if( isStopTimer )
            {
                ButtonContentInvert();
                ChangedTimeInTimer();
                new Thread( StartTimer ) { IsBackground = true, Name = "PreStartTimer" }.Start();
            } else
            {
                ButtonContentInvert();
            }
        }

        private void StartTimer()
        {
            Thread threadTimer = new Thread( alarmTimer.StartTimer )
            { IsBackground = true, Name = "StartTimer" };
            threadTimer.Start();
            while( !isStopTimer )
            {
                Thread.Sleep( TimeSpan.FromSeconds( 1 ) );
                isStopTimer = !alarmTimer.TimerIsStart;
            }
            mainWindow.Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,
                                                new Action( DisbleTimer ) );
            threadTimer.Abort();
        }

        
        private void ButtonContentInvert()
        {
            if( isStopTimer )
            {                
                
                EnableTimer();
            } else
            {
                DisbleTimer();
            }
        }

        private void DisbleTimer()
        {
            mainWindow.startTimerButton.Foreground = System.Windows.Media.Brushes.Black;
            mainWindow.startTimerButton.Background = new System.Windows.Media.SolidColorBrush( System.Windows.Media.Color.FromArgb( 0xFF, 0xE3, 0xE3, 0xE3 ) );
            mainWindow.startTimerButton.Content = "Включить таймер";
            isStopTimer = true;
        }

        private void EnableTimer()
        {
            mainWindow.startTimerButton.Foreground = System.Windows.Media.Brushes.Red;
            mainWindow.startTimerButton.Background = System.Windows.Media.Brushes.Yellow;
            mainWindow.startTimerButton.Content = "Таймер включен";
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
    }
}
