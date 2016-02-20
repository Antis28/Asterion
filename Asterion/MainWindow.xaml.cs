using System;
using System.Windows;
using System.Windows.Forms;
using Asterion.Presentors;
using Asterion.WpfExtensions;

namespace Asterion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        System.Windows.Threading.DispatcherTimer timerDiskSpace = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            new PresenterAlarmTimer( this );
            new PresenterRestartProcess( this );
            new PresenterClean( this );
            new PresenterFindExt( this );
            new PresenterRenamer( this );
        }

        public event EventHandler startAlarmEvent = null;
        private void startAlarm_Click( object sender, RoutedEventArgs e )
        {
            startAlarmEvent.Invoke( sender, e );
        }

        public event EventHandler restartProcessEvent = null;
        private void buttonRestartProcess_Click( object sender, RoutedEventArgs e )
        {
            restartProcessEvent.Invoke( sender, e );
        }

        public event EventHandler previewCleanEvent = null;
        private void previewCleanButton_Click( object sender, RoutedEventArgs e )
        {
            previewCleanEvent.Invoke( sender, e );
        }

        public event EventHandler CleanEvent = null;
        private void cleanButton_Click( object sender, RoutedEventArgs e )
        {
            CleanEvent.Invoke( sender, e );
        }

        private void comboBox_Initialized( object sender, EventArgs e )
        {
            for( int i = 1; i < 5; i++ )
            {
                comboBox.Items.Add( i );
            }
            comboBox.SelectedIndex = 0;
        }

        public event EventHandler checkBoxCleanEvent = null;
        private void checkBox_Click( object sender, RoutedEventArgs e )
        {
            checkBoxCleanEvent.Invoke( sender, e );
        }
        public event EventHandler sizeDiskProgressBarEvent = null;
        private void sizeDiskProgressBar_Initialized( object sender, EventArgs e )
        {
            new SpaceDiskInPercent( this );
            sizeDiskProgressBarEvent.Invoke( sender, e );
        }
        public event EventHandler findExtInBaseDateEvent = null;
        private void startSerach_Click( object sender, RoutedEventArgs e )
        {
            findExtInBaseDateEvent.Invoke( sender, e );
        }

        public event EventHandler openFileDialogEvent = null;
        private void buttonOpenFileDialog_Click( object sender, RoutedEventArgs e )
        {
            openFileDialogEvent.Invoke( sender, e );
        }

        public event EventHandler startTimerEvent = null;
        private void startTimer_Click( object sender, RoutedEventArgs e )
        {

        }

        
        public event EventHandler logicRanamerEvent = null;
        private void buttonRenameFiles_Click( object sender, RoutedEventArgs e )
        {
            logicRanamerEvent.Invoke( sender, e );
        }

        private void hoursComboBox_Initialized( object sender, EventArgs e )
        {
            for( int i = 0; i < 24; i++ )
            {
                hoursComboBox.Items.Add( i );
            }
            hoursComboBox.SelectedIndex = 0;
        }        

        private void minutesComboBox_Initialized( object sender, EventArgs e )
        {
            for( int i = 0; i < 60; i++ )
            {
                minutesComboBox.Items.Add( i );
            }
            minutesComboBox.SelectedIndex = 0;
        }
        
    }
}
