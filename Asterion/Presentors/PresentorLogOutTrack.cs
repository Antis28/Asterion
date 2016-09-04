using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Asterion.Models;

namespace Asterion.Presentors
{
    class PresenterLogOutTrack
    {        
        MainWindow mainWindow = null;

        public PresenterLogOutTrack( MainWindow mainWindow )
        {
            this.mainWindow = mainWindow;
            this.mainWindow.logOutTrackEvent += new EventHandler( mainWindow_logOutTrack );            
        }

        void mainWindow_logOutTrack( object sender, System.EventArgs e )
        {
            
        }

    }
}








