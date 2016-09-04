using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asterion.Presentors
{
    class PresenterDiary
    {
        Models.Diary diary = null;
        MainWindow mainWindow = null;

        public PresenterDiary( MainWindow mainWindow )
        {
            this.mainWindow = mainWindow;
            this.mainWindow.diaryEvent += new EventHandler( mainWindow_diary );  
        }

        void mainWindow_diary( object sender, System.EventArgs e )
        {
            diary = new Models.Diary();
            diary.Open( mainWindow.richTextBox );
        }

    }
}
