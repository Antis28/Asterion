using System;
using System.Net;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Asterion.Models
{
    class LogicAlarmTimer
    {
        /*
        void StartTimer( object ob )
        {
            ParamsTimer pt = ob as ParamsTimer;

            int countHour = pt.hour;
            int countMinute = pt.minute;


            while( countHour != 0 || countMinute != 0 )
            {
                
                statusText.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (Action)delegate
                {
                    statusText.Text = MyMessage.lastTime + countHour + " час(ов),  " + countMinute + "минут(а/ы)";
                } );
                
                Thread.Sleep( TimeSpan.FromMinutes( 1 ) );
                if( countMinute < 0 && countHour > 0 )
                {
                    countHour--;
                    countMinute = 59;
                } else
                {
                    countMinute--;
                }

            }


            if( pt.sleepFlag )
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "cmd";
                process.StartInfo.Arguments = "/C rundll32 powrprof.dll,SetSuspendState 0,1,0";
                process.Start();
            } else
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();

                    adressFile.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (Action)delegate
                    {
                        if( System.IO.File.Exists( adressFile.Text ) )
                            process.StartInfo.Arguments = adressFile.Text;
                        else
                            process.StartInfo.Arguments = @"M:\zvezdnie_vojni_-_imperskij_marsh.mp3";
                        //process.StartInfo.Arguments = @"M:\Music_(67 ГБ)\+boots randolph yakety sax.mp3";

                        if( System.IO.File.Exists( adressPlayer.Text ) )
                            process.StartInfo.FileName = adressPlayer.Text;
                        else
                            process.StartInfo.FileName = @"P:\Work_Program\KMPlayer\KMPlayer.exe";//@"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe";
                    } );
                       
                    while( !System.IO.File.Exists( process.StartInfo.FileName ) )
                    { Thread.Sleep( TimeSpan.FromSeconds( 1 ) ); }

                    process.Start();
                } catch( Exception e )
                {
                    MessageBox.Show( e.Message );
                }
            }
            startTimer.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (Action)delegate
            {
                startTimer.IsEnabled = true;
            } );

        }
        */
    }

    public class ParamsTimer
    {
        public int hour, minute;
        public bool sleepFlag;

        public ParamsTimer( int hours, int minutes )
        {
            this.hour = hours;
            this.minute = minutes;
        }

    }
}

