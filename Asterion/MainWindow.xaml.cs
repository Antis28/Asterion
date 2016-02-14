﻿using System;
using System.Windows;
using Asterion.Presentors;

namespace Asterion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            new PresenterAlarmTimer( this );
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
    }
}