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
        object lockObject = new object(); //Объект для синхронизации потоков

        int timerHour = 0;
        int timerMinute = 0;
        bool timerIsStart = true;
        


        public int TimerHour
        {
            get
            {
                return timerHour;
            }

            set
            {
                timerHour = value;
            }
        }
        public int TimerMinute
        {
            get
            {
                return timerMinute;
            }

            set
            {
                timerMinute = value;
            }
        }

        public bool TimerIsStart
        {
            get
            {
                return timerIsStart;
            }

            set
            {
                timerIsStart = value;
            }
        }

        
        

        public void StartTimer( object pat)
        {
            Asterion.Presentors.PresenterAlarmTimer presenterAlarmTimer = pat as Asterion.Presentors.PresenterAlarmTimer;
            timerHour = presenterAlarmTimer.currentHourChange;
            timerMinute = presenterAlarmTimer.currentMinutesChange;
            TimerIsStart = !presenterAlarmTimer.isStopTimer;

            while( timerMinute != 0 || timerHour != 0 )
            {               
                presenterAlarmTimer.TimerStatusText = MyMessage.lastTime + timerHour + " час(ов),  " + timerMinute + "минут(а/ы)";
                Thread.Sleep( TimeSpan.FromSeconds( 1 ) );
                timerMinute--;
                if( timerMinute <0 && timerHour > 0)
                {
                    timerMinute = 59;
                    timerHour--;
                }
            }
            TimerIsStart = false;
        }
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

