using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Asterion.Models
{
    class LogicCleanTemp
    {
        string windowsTemp;
        string appTemp;
        string internetTemp;
        List<string> directoryAtClear;
        public static ListView console;

        public void Clean()
        {
            try
            {
                Initialization();

                Thread th = new Thread( CleanTempFolder );
                th.Start();

            } catch( System.Exception ex )
            {
                MessageBox.Show( ex.Message );
            }

        }

        public void PreviewClean()
        {
            try
            {
                Initialization();
                PreviewCleanTempFolder();
            } catch( System.Exception ex )
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void CleanTempFolder()
        {
            console.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (System.Action)delegate
            {
                console.Items.Clear();
            } );
            DirectoryInfo cleanFolder;
            foreach( var pathDir in directoryAtClear )
            {
                cleanFolder = new DirectoryInfo( pathDir );

                console.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (System.Action)delegate
                {
                    //console.Items.Add( new string( '-', 100 ) );
                    console.Items.Add( "Содержимое " + pathDir + ":" );
                } );

                CleanFolderAndFiles( cleanFolder );

                console.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (System.Action)delegate
                {
                    console.Items.Add( new string( '-', 100 ) );
                } );
            }
        }

        private void PreviewCleanTempFolder()
        {
            console.Items.Clear();
            DirectoryInfo cleanFolder;
            foreach( var pathDir in directoryAtClear )
            {
                cleanFolder = new DirectoryInfo( pathDir );

                console.Items.Add( new string( '-', 100 ) );
                console.Items.Add( "Содержимое " + pathDir + ":" );
                PreviewFolderAndFiles( cleanFolder );
                console.Items.Add( new string( '-', 100 ) );

            }
        }

        private static void CleanFolderAndFiles( DirectoryInfo cleanFolder )
        {
            // Size in Byte
            long sizeFiles = 0;

            if( !Directory.Exists( cleanFolder.FullName ) )
            {
                console.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (System.Action)delegate
                {
                    var tb = new TextBlock();
                    tb.Text = "Директория " + cleanFolder.FullName + " не найдена!!!";

                    tb.Foreground = System.Windows.Media.Brushes.DarkRed;
                    console.Items.Add( tb );
                } );
            } else
            {
                console.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (System.Action)delegate
                {
                    console.Items.Add( new string( '*', 100 ) );
                    console.Items.Add( "Файлы:" );
                } );

                foreach( FileInfo file in cleanFolder.GetFiles() )
                {
                    try
                    {
                        sizeFiles = file.Length;
                        file.Delete();
                        console.Items.Add( file.FullName );
                    } catch { }
                    console.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (System.Action)delegate
                    {
                        var tb = new TextBlock();
                        tb.Text = "Файл " + file.FullName + " удалить не удалось.";
                        tb.Foreground = System.Windows.Media.Brushes.Red;

                        console.Items.Add( tb );
                    } );
                }

                console.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (System.Action)delegate
                {
                    console.Items.Add( "Размер файлов: " + (sizeFiles * 1.0 / (1024 * 1024)).ToString( "####0" ) + " Mb" );
                    console.Items.Add( new string( '*', 100 ) );
                    console.Items.Add( "Директории:" );
                } );
                foreach( DirectoryInfo dir in cleanFolder.GetDirectories() )
                {
                    try
                    {
                        dir.Delete( true );
                        console.Items.Add( dir.FullName );
                    } catch { }
                    console.Dispatcher.BeginInvoke( DispatcherPriority.Normal, (System.Action)delegate
                    {
                        var tb = new TextBlock();
                        tb.Text = "Директорию " + dir.FullName + " удалить не удалось.";

                        tb.Foreground = System.Windows.Media.Brushes.DarkRed;
                        console.Items.Add( tb );

                    } );
                }
            }
        }

        private static void PreviewFolderAndFiles( DirectoryInfo cleanFolder )
        {
            // Size in Byte
            long sizeFiles = 0;

            //console.Items.Add( new string( '*', 100 ) );
            //console.Items.Add( "Файлы:" );
            if( !Directory.Exists( cleanFolder.FullName ) )
            {
                var tb = new TextBlock();
                tb.Text = "Директория " + cleanFolder.FullName + " не найдена!!!";

                tb.Foreground = System.Windows.Media.Brushes.DarkRed;
                console.Items.Add( tb );
            } else
            {
                foreach( FileInfo file in cleanFolder.GetFiles() )
                {
                    console.Items.Add( file.FullName );
                }
                console.Items.Add( sizeFiles / 1024 * 1024 + " Mb" );
                console.Items.Add( new string( '*', 100 ) );
                console.Items.Add( "Директории:" );
                foreach( DirectoryInfo dir in cleanFolder.GetDirectories() )
                {
                    console.Items.Add( dir.FullName );
                }
            }
        }

        private void Initialization()
        {
            if( System.Environment.OSVersion.Platform == System.PlatformID.Win32NT )
            {
                windowsTemp = System.Environment.GetFolderPath( System.Environment.SpecialFolder.Windows );
                appTemp = System.Environment.GetFolderPath( System.Environment.SpecialFolder.UserProfile );
                internetTemp = System.Environment.GetFolderPath( System.Environment.SpecialFolder.InternetCache );
            } else
                windowsTemp = System.Environment.ExpandEnvironmentVariables( "%HOMEDRIVE%%HOMEPATH%" );

            windowsTemp += @"\temp";
            appTemp += @"\AppData\Local\Temp\";

            LoadClearedList();
        }

        private void LoadClearedList()
        {
            directoryAtClear = new List<string>();
            var file = new FileStream( "Settings.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite );
            try
            {
                var write = new StreamWriter( file, System.Text.Encoding.GetEncoding( 1251 ) );
                try
                {
                    write.WriteLine( windowsTemp );
                    write.WriteLine( appTemp );
                    write.WriteLine( internetTemp );

                } catch( Exception e )
                {
                    console.Items.Add( e.Message );
                } finally
                {
                    write.Close();
                }

                var reader = File.OpenText( "Settings.txt" );
                try
                {
                    string input;
                    while( (input = reader.ReadLine()) != null )
                    {
                        directoryAtClear.Add( input );
                    }
                } catch( Exception e )
                {
                    console.Items.Add( e.Message );
                } finally
                {
                    reader.Close();
                }
            } catch( Exception e )
            {
                console.Items.Add( e.Message );
            } finally
            {
                file.Close();
            }
        }
    }
}
