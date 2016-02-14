using System;
using Asterion.Models;

namespace Asterion.Presentors
{
    class PresenterRestartProcess
    {
        LogicRestartProcess alarmTimer = null;
        MainWindow mainWindow = null;

        public PresenterRestartProcess( MainWindow mainWindow )
        {
            this.alarmTimer = new LogicRestartProcess();
            this.mainWindow = mainWindow;
            this.mainWindow.restartProcessEvent += new EventHandler( mainWindow_restartProcessEvent );
        }

        void mainWindow_restartProcessEvent( object sender, System.EventArgs e )
        {    
        }
    }
}
