using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Asterion.Models
{
    class LogicRestartProcess
    {
        string targetProcess = "explorer";

        public void StartKillProcess()
        {
            Thread thr = new Thread( new ThreadStart( KillProcess ) );
            thr.Priority = ThreadPriority.AboveNormal;
            thr.IsBackground = false;
            thr.Start();
        }

        void KillProcess()
        {
            Process[] processInfo = Process.GetProcessesByName( targetProcess );
            try
            {
                foreach( Process p in processInfo )
                    p.Kill();
            } catch( Exception e) { MessageBox.Show( e.Message ); }
        }
    }
}
