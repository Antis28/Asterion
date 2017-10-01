using System;
using System.Net;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using Asterion.UIHelpers;

namespace Asterion.Models
{
    class LogicCheckInternet
    {
        double checkInterval = 10d;
        double pauseSleepInterval = 5d;
        TextBlock textBlock;
        Button buttonStart;

        public LogicCheckInternet( TextBlock textBlock, Button buttonStart )
        {
            this.textBlock = textBlock;
            this.buttonStart = buttonStart;
        }

        // Проверка доступности сайта или сервера - strServer
        public bool ConnectionAvailable( string strServer )
        {
            try
            {
                HttpWebRequest reqFP = (HttpWebRequest)HttpWebRequest.Create( strServer );

                HttpWebResponse rspFP = (HttpWebResponse)reqFP.GetResponse();
                if( HttpStatusCode.OK == rspFP.StatusCode )
                {
                    // HTTP = 200 - Интернет безусловно есть! 
                    rspFP.Close();
                    return true;
                } else
                {
                    // сервер вернул отрицательный ответ, возможно что инета нет
                    rspFP.Close();
                    return false;
                }
            } catch( WebException )
            {
                // Ошибка, значит интернета у нас нет. Плачем :'(
                return false;
            }
        }

        // Гибернация при отсутсвии интернета
        public void isInternetSleep( object ob )
        {
            ParamsSleep pr = ob as ParamsSleep;
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();

            while( ConnectionAvailable( "http://www.google.com" ) && pr.internetSleepFlag )
            {
                Thread.Sleep( TimeSpan.FromSeconds( checkInterval ) );

            }
            if( pr.internetSleepFlag )
            {
                textBlock.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (Action)delegate ()
                {
                    textBlock.Text = "Интернета нет, засыпаю...";
                    synthesizer.Speak( "Интернета нет, засыпаю..." );
                    //buttonStart.Content = ButtonsName.START;
                } );


                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd";
                // p.StartInfo.Arguments = "/C ping 127.0.0.1"; //чтобы программно нажать enter
                /**чтобы консоль сразу не закрывалась, чтобы её закрыть - закоментируй эту строку*/
                // p.StartInfo.Arguments = "/K ping 127.0.0.1";                
                Thread.Sleep( TimeSpan.FromSeconds( pauseSleepInterval ) );
                p.StartInfo.Arguments = "/C rundll32 powrprof.dll,SetSuspendState 0,1,0";
                p.Start();
            }
            buttonStart.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (Action)delegate ()
            {
                buttonStart.Content = ButtonsName.START;
                textBlock.Text = MyMessage.status + MyMessage.stop;
            } );
        }
    }

    public class ParamsSleep
    {
        public bool internetSleepFlag;
        public ParamsSleep( bool flag )
        {
            this.internetSleepFlag = flag;
        }

    }

    class mySpeeak
    {
        public void Start()
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Speak( MyMessage.start );
        }
        public void Stop()
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Speak( MyMessage.stop );
        }
    }

    static public class MyMessage
    {
        static public string status = "Статус: ";
        static public string stop = "отслеживать интернет прекращаю.";
        static public string start = "начинаю следить за интернетом.";
        static public string lastTime = "Осталось: ";
    }
}
