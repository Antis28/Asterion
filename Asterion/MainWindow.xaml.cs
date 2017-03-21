using System;
using System.Windows;
using System.Windows.Forms;
using Asterion.Presentors;
using Asterion.WpfExtensions;
using System.Windows.Interop;

namespace Asterion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        System.Windows.Threading.DispatcherTimer timerDiskSpace = new System.Windows.Threading.DispatcherTimer();
        PresenterChellForWebp presenterChellForWebp;
        public MainWindow()
        {
            InitializeComponent();
            new PresenterAlarmTimer( this );
            new PresenterRestartProcess( this );
            new PresenterClean( this );
            new PresenterFindExt( this );
            new PresenterRenamer( this );
            new PresenterFindExtInXMLBaseDate( this );
            presenterChellForWebp = new PresenterChellForWebp( this );
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
        public event EventHandler FindExtInXMLBaseDateEvent= null;
        private void startSerach_Click( object sender, RoutedEventArgs e )
        {
            if( isSearchXML.IsChecked != true )
                findExtInBaseDateEvent.Invoke( sender, e );
            else
                FindExtInXMLBaseDateEvent.Invoke( sender, e );
        }

        public event EventHandler openFileDialogRenamerEvent = null;
        private void buttonOpenFileDialog_Click( object sender, RoutedEventArgs e )
        {
            openFileDialogRenamerEvent.Invoke( sender, e );
        }

        public event EventHandler startTimerEvent = null;
        private void startTimer_Click( object sender, RoutedEventArgs e )
        {
            startTimerEvent.Invoke( sender, e );
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

        public event EventHandler openFileDialogEvent = null;
        private void openFileDialogToAlarmAndTimer_Click( object sender, RoutedEventArgs e )
        {
            openFileDialogEvent.Invoke( sender, e );
        }


        public event EventHandler logOutTrackEvent = null;
        protected override void OnSourceInitialized( EventArgs e )
        {
            base.OnSourceInitialized( e );
            // создаем экземпляр HwndSource
            Asterion.Models.LogicLogOutTrack LOT = new Asterion.Models.LogicLogOutTrack();
            LOT.hwndSource = PresentationSource.FromVisual( this ) as HwndSource;
            // и устанавливаем перехватчик
            LOT.hwndSource.AddHook( LOT.WndProc );
            // logOutTrackEvent - ?
            if( e.ToString() == "" )
                logOutTrackEvent.Invoke(this, e);
        }

        public event EventHandler openFolderDialogEvent = null;
        public event EventHandler openFileDialogToConverterEvent = null;

        private void btn_addAddress_Click( object sender, RoutedEventArgs e )
        {
            if( cb_isDirectory.IsChecked == true )
                openFolderDialogEvent.Invoke( sender, e );
            else
                openFileDialogToConverterEvent.Invoke( sender, e );
        }
        public event EventHandler startConvertEvent= null;
        private void btn_convert_Click( object sender, RoutedEventArgs e )
        {
            startConvertEvent.Invoke( sender, e );
        }

        private void tb_addressField_DragEnter( object sender, System.Windows.DragEventArgs e )
        {
            System.Windows.MessageBox.Show( e.ToString() );
        }

        private void tb_addressField_LostFocus( object sender, RoutedEventArgs e )
        {
            presenterChellForWebp.ExistPath();
        }

        public bool isPercent = false;
        private void pb_percentConvert_MouseUp( object sender, System.Windows.Input.MouseButtonEventArgs e )
        {
            isPercent = !isPercent;
        }
    }
}
