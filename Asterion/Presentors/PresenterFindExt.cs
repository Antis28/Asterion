using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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

            mainWindow.extDescriptionCategory.Text = mainWindow.extDescription.Text = "";
        }

        void mainWindow_findExtInBaseDate( object sender, EventArgs e )
        {            
            this.ExtInBaseDate = new FindExtInBaseDate( mainWindow.extInput.Text );
            ExtInBaseDate.FindIn_TXT();
            mainWindow.extDescriptionCategory.Text = ExtInBaseDate.descriptExt.Category;
            mainWindow.extDescription.Text = ExtInBaseDate.descriptExt.Description;
            mainWindow.listViewExtDescription.Items.Clear();
            foreach( var item in ExtInBaseDate.descriptExtList )
            {
                mainWindow.listViewExtDescription.Items.Add( item.Category );
                mainWindow.listViewExtDescription.Items.Add( item.Description );
                mainWindow.listViewExtDescription.Items.Add( new string( '-', 50 ));
            }
            
           
        }


    }
}
