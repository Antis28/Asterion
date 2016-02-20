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

        public PresenterAlarmTimer( MainWindow mainWindow )
        {
            this.alarmTimer = new LogicAlarmTimer();
            this.mainWindow = mainWindow;
            this.mainWindow.startAlarmEvent += new EventHandler( mainWindow_startAlarmEvent );            
        }

        void mainWindow_startAlarmEvent( object sender, System.EventArgs e )
        {
            ChangedTimeInTimer();
            Thread threadTimer = new Thread( StartTimer );

        }

        private void StartTimer()
        {
            
            
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
