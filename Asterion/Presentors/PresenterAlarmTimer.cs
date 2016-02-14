using System;
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

        public PresenterAlarmTimer( MainWindow mainWindow )
        {
            this.alarmTimer = new LogicAlarmTimer();
            this.mainWindow = mainWindow;
            this.mainWindow.startAlarmEvent += new EventHandler( mainWindow_startAlarmEvent );
        }

        void mainWindow_startAlarmEvent( object sender, System.EventArgs e )
        {
        }
    }
}
