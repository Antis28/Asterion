using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Asterion.Presentors
{
    class ControllerDiary
    {
        //Main window(form)
        MainWindow mainWindow = null;

        Models.Diary diary;

        public ControllerDiary( MainWindow mainWindow )
        {
            diary = new Models.Diary();
            this.mainWindow = mainWindow;            
            this.mainWindow.diaryOpenEvent += new EventHandler( mainWindow_diaryOpen );
            this.mainWindow.diarySaveEvent += new EventHandler( mainWindow_diarySave );
        }

        void mainWindow_diaryOpen( object sender, System.EventArgs e )
        {
            diary = new Models.Diary();
        }

        void mainWindow_diarySave( object sender, System.EventArgs e )
        {
           
            diary.Save( mainWindow.txb_Diary.Text );
        }
    }
}
