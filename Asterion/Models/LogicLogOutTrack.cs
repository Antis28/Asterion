/*
Аналогом класса Form в Windows Forms является класс Window, в котором нет метода WndProc, который мы могли бы переопределить. Возвращаемся в Windows Forms? Нет, решение всё-таки существует.

Решение

Для решения нашей задачи предназначен замечательный класс HwndSource, который находится в пространстве имен System.Windows.Interop. HwndSource — класс, который представляет Win32-окно, в котором находится контент WPF. Именно им нужно будет пользоваться, если нам нужно вызывать какие-либо функции WinAPI, которые требуют дескриптор окна в качестве одного из параметров. Также в экземпляре класса HwndSource есть два метода, которые позволяют получить доступ к сообщениям Windows:
public void AddHook(HwndSourceHook hook);
public void RemoveHook(HwndSourceHook hook);
Эти методы позволяют соответственно установить и удалить функцию-перехватчик. Сигнатура HwndSourceHook выглядит так:
public delegate IntPtr HwndSourceHook(
 IntPtr hwnd, // дескриптор окна
 int msg, // сообщение Windows
 IntPtr wParam, // параметры
 IntPtr lParam, // сообщения
 ref bool handled // обработано ли сообщение
)
Чтобы создать экземпляр класса HwndSource, мы можем воспользоваться его конструктором и создать новое окно, сообщения которому мы и будем перехватывать. Однако, можно сделать и по-другому. Поскольку HwndSource является наследником класса PresentationSource, достаточно будет воспользоваться статическим методом PresentationSource.FromVisual(Visual) с нашим главным окном в качестве параметра и привести результат к типу HwndSource.
В этом моменте я допустил ошибку, вызвав FromVisual в конструкторе формы. В момент вызова окно ещё не было полностью инициализировано, и результатом стало исключение. После этой попытки я переопределил вызов OnSourceInitialized и завершил инициализвцию HwndSource в нём.
Код

Полный код перехвата сообщений Windows выглядит так:
public partial class MainWindow : Window
{
    private const int WM_DRAWCLIPBOARD = 0x0308;
    private HwndSource hwndSource;

    // функция-перехватчик
    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_DRAWCLIPBOARD)
        {
            // обрабатываем сообщение
        }
        return IntPtr.Zero;
    }

    public MainWindow()
    {
        InitializeComponent();   
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        // создаем экземпляр HwndSource
        hwndSource = PresentationSource.FromVisual(this) as HwndSource; 
        // и устанавливаем перехватчик
        hwndSource.AddHook(WndProc);
    }
}
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Windows.Interop;

/*
Момент блокировки или разблокировки компьютера 
можно отлавливать при помощи события SystemEvents.SessionSwitch.
*/
namespace Asterion.Models
{
    public class LogicLogOutTrack
    {
        #region
        //private void SystemEvents_SessionSwitch( object sender, SessionSwitchEventArgs e )
        //{
        //    //If the reason for the session switch is lock or unlock  
        //    //send the message to mute or unmute the system volume
        //    if (e.Reason == SessionSwitchReason.SessionLock)
        //    {
        //        SendMessageW( this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        //    }
        //    else if (e.Reason == SessionSwitchReason.SessionUnlock)
        //    {
        //        SendMessageW( this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        //    }
        //}

        //private void SystemEvents_SessionSwitch( object sender, SessionSwitchEventArgs e )
        //{
        //    //If the reason for the session switch is lock or unlock 
        //    //send the message to mute or unmute the system volume
        //    if( e.Reason == SessionSwitchReason.SessionLock )
        //        System.Windows.MessageBox.Show( "Я отработал" );
        //    System.Windows.MessageBox.Show( "simple" );
        //}
        #endregion
        /*
               private static int WM_QUERYENDSESSION = 0x11;
               private static bool systemShutdown = false;

               protected override void WndProc( ref System.Windows.Forms.Message m )
               {
                   if( m.Msg == WM_QUERYENDSESSION )
                   {
                       System.Windows.MessageBox.Show( "queryendsession: this is a logoff, shutdown, or reboot" );
                       systemShutdown = true;
                   }

                   // If this is WM_QUERYENDSESSION, the closing event should be
                   // raised in the base WndProc.
                   base.WndProc( ref m );

               } //WndProc 

               private void Form1_Closing(
                                            System.Object sender,
                                            System.ComponentModel.CancelEventArgs e )
               {
                   if( systemShutdown )
                   // Reset the variable because the user might cancel the 
                   // shutdown.
                   {
                       systemShutdown = false;
                       if( DialogResult.Yes == MessageBox.Show( "My application",
                           "Do you want to save your work before logging off?",
                           MessageBoxButtons.YesNo ) )
                       {
                           e.Cancel = true;
                       } else
                       {
                           e.Cancel = false;
                       }
                   }
               }
        */

        #region Полный код перехвата сообщений Windows выглядит так:
        public const int WM_DRAWCLIPBOARD = 0x0308;
        public HwndSource hwndSource;

        // функция-перехватчик
        public IntPtr WndProc( IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled )
        {
            if( msg == WM_DRAWCLIPBOARD )
            {
                // обрабатываем сообщение
                System.Windows.MessageBox.Show( "Я отработал" );
            }
            return IntPtr.Zero;
        }    
        #endregion
      
    }
}
