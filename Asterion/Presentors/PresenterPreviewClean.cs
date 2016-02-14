using System;
using Asterion.Models;

namespace Asterion.Presentors
{
    class PresenterClean
    {
        LogicCleanTemp cleanTemp = null;
        MainWindow mainWindow = null;

        public PresenterClean( MainWindow mainWindow )
        {
            this.cleanTemp = new LogicCleanTemp();
            this.mainWindow = mainWindow;
            this.mainWindow.previewCleanEvent += new EventHandler( mainWindow_previewCleanEvent );
            this.mainWindow.CleanEvent += new EventHandler( mainWindow_CleanEvent );
        }

        private void mainWindow_CleanEvent( object sender, EventArgs e )
        {
            
        }

        void mainWindow_previewCleanEvent( object sender, System.EventArgs e )
        {
            LogicCleanTemp.console = mainWindow.consoleListView;
            cleanTemp.PreviewClean();
        }
    }
}
