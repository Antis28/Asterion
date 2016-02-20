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
                new Thread( StartTimer ) { IsBackground = true };                
            } else
            {
                ButtonContentInvert();                
            }
        }

        private void StartTimer()
        {
            //Thread threadTimer = new Thread( alarmTimer.StartTimer ) { IsBackground = true };

            while( !isStopTimer )
            {
                Thread.Sleep( TimeSpan.FromSeconds( 1 ) );
            }
            ButtonContentInvert();
            //threadTimer.Abort();
        }

        private void ButtonContentInvert()
        {
            if( isStopTimer ){
                mainWindow.startTimerButton.Foreground = System.Windows.Media.Brushes.Red;
                mainWindow.startTimerButton.Background = System.Windows.Media.Brushes.Yellow;
                mainWindow.startTimerButton.Content = "Таймер включен";
                isStopTimer = false;
            } else{
                mainWindow.startTimerButton.Foreground = System.Windows.Media.Brushes.Black;
                mainWindow.startTimerButton.Background = new System.Windows.Media.SolidColorBrush( System.Windows.Media.Color.FromArgb( 0xFF, 0xE3, 0xE3, 0xE3 ) );               
                mainWindow.startTimerButton.Content = "Включить таймер";
                isStopTimer = true;
            }
        }

        private void ChangedTimeInTimer(  )
        {
            if( this.mainWindow.hoursComboBox != null && this.mainWindow.minutesComboBox != null )
            {
                currentHourChange = (int)this.mainWindow.hoursComboBox.SelectedValue;
                currentMinutesChange = (int)this.mainWindow.minutesComboBox.SelectedValue;
            }
        }
    }
}
