using System;
using Asterion.Models;

namespace Asterion.Presentors
{
    public delegate void ProgressHandler( int progress );
    public delegate void ConsoleHandler( string progress );

    class PresenterRenamer
    {
        LogicRanamer logicRanamer = null;
        MainWindow mainWindow = null;

        public PresenterRenamer( MainWindow mainWindow )
        {
            this.mainWindow = mainWindow;
            this.mainWindow.logicRanamerEvent += new EventHandler( mainWindow_logicRanamerEvent );            
        }

        void mainWindow_logicRanamerEvent( object sender, System.EventArgs e )
        {

        }
    }
}
