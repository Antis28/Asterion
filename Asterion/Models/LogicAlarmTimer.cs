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
        bool timerIsStart = false;
        string timerStatusText = "";


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

        public void StartTimer()
        {
            while( timerMinute != 0 && timerMinute != 0 )
            {
                lock( lockObject )
                {
                    timerStatusText = MyMessage.lastTime + timerHour + " час(ов),  " + timerMinute + "минут(а/ы)";
                }
            }
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

