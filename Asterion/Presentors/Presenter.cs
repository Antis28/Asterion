using System;
using Asterion.Models;


namespace Asterion.Presentors
{
    /// <summary>
    /// MVP - model-view-presenter
    /// Presenter служит для связывание логики и UI
    /// </summary>
    class Presenter
    {
        AlarmTimer alarmTimer = null;
        MainWindow mainWindow = null;

        public Presenter( MainWindow mainWindow )
        {
            this.alarmTimer = new AlarmTimer();
            this.mainWindow = mainWindow;
            this.mainWindow.startAlarmEvent += new EventHandler( mainWindow_startAlarmEvent );
        }

        void mainWindow_startAlarmEvent( object sender, System.EventArgs e )
        {
            //alarmTimer.Logic();

            
        }
    }
}
