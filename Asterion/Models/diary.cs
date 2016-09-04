using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Asterion.Models
{
    class Diary
    {
        /*      Загрузка файла
        *   Ниже представлен фрагмент простого кода, 
        *   который преобразует .rtf-документ в выборку текста в объекте TextRange, 
        *   после чего вставляет этот текст в RichTextBox:
        */
        public void Open( RichTextBox richTextBox )
        {
            System.Windows.Forms.OpenFileDialog openFile =
                new System.Windows.Forms.OpenFileDialog();

            openFile.Filter = "RichText files (*.rtf)|*.rtf|All files (*.*)|*.*";

            if( openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                TextRange tr = new TextRange(
                       richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd );

                using( FileStream fs = File.Open( openFile.FileName, FileMode.Open ) )
                {
                    tr.Load( fs, System.Windows.DataFormats.Rtf );
                }
            }
        }
    }
}
