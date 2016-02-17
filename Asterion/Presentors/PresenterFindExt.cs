using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asterion.Models;

namespace Asterion.Presentors
{
    class PresenterFindExt
    {
        FindExtInBaseDate ExtInBaseDate = null;
        MainWindow mainWindow = null;

        public PresenterFindExt( MainWindow mainWindow )
        {            
            this.mainWindow = mainWindow;
            this.mainWindow.findExtInBaseDateEvent += new EventHandler( mainWindow_findExtInBaseDate );
        }

        void mainWindow_findExtInBaseDate( object sender, System.EventArgs e )
        {
            this.ExtInBaseDate = new FindExtInBaseDate( mainWindow.extInput.Text );
            ExtInBaseDate.FindIn_TXT();
            mainWindow.extDescriptionCategory.Text = ExtInBaseDate.descriptExt.Category;
            mainWindow.extDescription.Text = ExtInBaseDate.descriptExt.Description;
        }


    }
}
