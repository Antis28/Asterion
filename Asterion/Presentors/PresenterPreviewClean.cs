using System;
using System.Windows.Controls;
using Asterion.Models;

namespace Asterion.Presentors
{
    class PresenterClean
    {
        System.Windows.Threading.DispatcherTimer timerAutoClean = new System.Windows.Threading.DispatcherTimer();
        
        LogicCleanTemp cleanTemp = null;
        MainWindow mainWindow = null;
        CheckBox checkBox = null;
        ComboBox comboBox = null;      

        public PresenterClean( MainWindow mainWindow)
        {
            this.cleanTemp = new LogicCleanTemp();

            this.checkBox = mainWindow.checkBoxCleanTemp;
            this.comboBox = mainWindow.comboBox;   

            this.mainWindow = mainWindow;
            this.mainWindow.previewCleanEvent += new EventHandler( mainWindow_previewCleanEvent );
            this.mainWindow.CleanEvent += new EventHandler( mainWindow_CleanEvent );
            this.mainWindow.checkBoxCleanEvent += new EventHandler( mainWindow_checkBoxCleanEvent );
            
            LogicCleanTemp.console = mainWindow.consoleListView;
        }

        //private void mainWindow_sizeDiskProgressBarEvent( object sender, EventArgs e )
        //{
        //    UpdateFreeSpaceOnDisk();            

        //    timerDiskSpace.Tick += new EventHandler( timerDiskSpaceTick );
        //    timerDiskSpace.Interval = new TimeSpan( 0, 0, 5 );
        //    timerDiskSpace.Start();
        //}
        //private void timerDiskSpaceTick( object sender, EventArgs e )
        //{
        //    UpdateFreeSpaceOnDisk();
        //}
        //void UpdateFreeSpaceOnDisk()
        //{
        //    System.IO.DriveInfo driveC = new System.IO.DriveInfo( @"C:\" );
        //    try
        //    {
        //        sizeDiskProgressBar.Maximum = driveC.TotalSize;
        //        sizeDiskProgressBar.Value = driveC.TotalFreeSpace;
        //        int dielitel = 1024 * 1024;
        //        long max = driveC.TotalSize / dielitel;
        //        long current = driveC.TotalFreeSpace / dielitel;
        //        long percent = (long)(current * 1.0 / max * 100);
        //        sizeDiskText.Text = percent + "% свободно.";
        //    } catch( System.Exception e )
        //    {
        //        System.Windows.MessageBox.Show( e.Message );
        //    }
        //}

        private void mainWindow_CleanEvent( object sender, EventArgs e )
        {            
            cleanTemp.Clean();
        }

        private void mainWindow_previewCleanEvent( object sender, System.EventArgs e )
        {
            //LogicCleanTemp.console = mainWindow.consoleListView;
            cleanTemp.PreviewClean();
        }

        private void mainWindow_checkBoxCleanEvent( object sender, System.EventArgs e )
        {
            if( checkBox.IsChecked == true )
            {
                timerAutoClean.Tick += new EventHandler( timerAutoCleanTick );
                timerAutoClean.Interval = new TimeSpan( (int)comboBox.SelectedValue, 0, 0 );

                timerAutoClean.Start();
            } else
            {
                timerAutoClean.Stop();
            }
        }

        private void timerAutoCleanTick( object sender, EventArgs e )
        {            
            cleanTemp.Clean();
        }
    }
    class SpaceDiskInPercent
    {
        System.Windows.Threading.DispatcherTimer timerDiskSpace = new System.Windows.Threading.DispatcherTimer();
        MainWindow mainWindow = null;
        ProgressBar sizeDiskProgressBar = null;
        TextBlock sizeDiskText = null;

        public SpaceDiskInPercent( MainWindow mainWindow )
        {
            this.mainWindow = mainWindow;
            this.sizeDiskProgressBar = mainWindow.sizeDiskProgressBar;
            this.sizeDiskText = mainWindow.sizeDiskText;

            this.mainWindow.sizeDiskProgressBarEvent += new EventHandler( mainWindow_sizeDiskProgressBarEvent );
        }
        private void mainWindow_sizeDiskProgressBarEvent( object sender, EventArgs e )
        {
            UpdateFreeSpaceOnDisk();

            timerDiskSpace.Tick += new EventHandler( timerDiskSpaceTick );
            timerDiskSpace.Interval = new TimeSpan( 0, 0, 5 );
            timerDiskSpace.Start();
        }
        private void timerDiskSpaceTick( object sender, EventArgs e )
        {
            UpdateFreeSpaceOnDisk();
        }
        private void UpdateFreeSpaceOnDisk()
        {
            System.IO.DriveInfo driveC = new System.IO.DriveInfo( @"C:\" );
            try
            {
                sizeDiskProgressBar.Maximum = driveC.TotalSize;
                sizeDiskProgressBar.Value = driveC.TotalFreeSpace;
                int dielitel = 1024 * 1024;
                long max = driveC.TotalSize / dielitel;
                long current = driveC.TotalFreeSpace / dielitel;
                long percent = (long)(current * 1.0 / max * 100);
                sizeDiskText.Text = percent + "% свободно.";
            } catch( System.Exception e )
            {
                System.Windows.MessageBox.Show( e.Message );
            }
        }
    }
}
