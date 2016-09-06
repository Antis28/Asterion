using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Asterion.Models
{
    class Diary
    {
        public Diary()
        {
            Open();
        }
        private void Open()
        {
            
        }

        internal void Save()
        {
          
        }
    }

    class DiaryReachBox
    {

        RichTextBox _richTextBox;
        TextBox _txb_xaml;

        public DiaryReachBox( RichTextBox richTextBox, TextBox txb_xaml )
        {
            this._richTextBox = richTextBox;
            _txb_xaml = txb_xaml;
        }

        /*      Загрузка файла
        *   Ниже представлен фрагмент простого кода, 
        *   который преобразует .rtf-документ в выборку текста в объекте TextRange, 
        *   после чего вставляет этот текст в RichTextBox:
        */
        public void Open( )//RichTextBox richTextBox )
        {
            System.Windows.Forms.OpenFileDialog openFile =
                new System.Windows.Forms.OpenFileDialog();

            openFile.Filter = "RichText files (*.rtf)|*.rtf|All files (*.*)|*.*";

            if( openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                TextRange tr = new TextRange(
                       _richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd );

                using( FileStream fs = File.Open( openFile.FileName, FileMode.Open ) )
                {
                    tr.Load( fs, System.Windows.DataFormats.Rtf );
                }
            }
            copyInMemory();

        }
        private void copyInMemory()
        {
            // Копирование содержимого документа в MemoryStream. 
            using( MemoryStream stream = new MemoryStream() )
            {
                TextRange range = new TextRange( _richTextBox.Document.ContentStart,
                    _richTextBox.Document.ContentEnd );
                range.Save( stream, DataFormats.Xaml );
                stream.Position = 0;
                readFromMemory( stream );
            }
        }
        private void readFromMemory( MemoryStream stream )
        {
            // Чтение содержимого из потока и вывод его в текстовом поле. 
            using( StreamReader r = new StreamReader( stream ) )
            {
                string line;
                while( (line = r.ReadLine()) != null )
                    _txb_xaml.Text += line + "\n";
            }
        }

        public void Save( )//RichTextBox richTextBox )
        {
            Microsoft.Win32.SaveFileDialog save = new Microsoft.Win32.SaveFileDialog();
            save.Filter =
                "Файл XAML (*.xaml)|*.xaml|RTF-файл (*.rtf)|*.rtf";

            if( save.ShowDialog() == true )
            {
                // Создание контейнера TextRange для всего документа
                TextRange documentTextRange = new TextRange(
                    _richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd );

                // Если такой файл существует, он перезаписывается, 
                using( FileStream fs = File.Create( save.FileName ) )
                {
                    if( System.IO.Path.GetExtension( save.FileName ).ToLower() == ".rtf" )
                    {
                        documentTextRange.Save( fs, DataFormats.Rtf );
                    } else
                    {
                        documentTextRange.Save( fs, DataFormats.Xaml );
                    }
                }
            }
        }
    }
}
