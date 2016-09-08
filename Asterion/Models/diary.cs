using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;

namespace Asterion.Models
{
    class Diary
    {
        public string pathToXmlDocument  = "DiaryBase.xml";
        XmlDocument xmlDoc;
        XmlNode root;


        public Diary()
        {
            if( !System.IO.File.Exists( pathToXmlDocument ) )
                CreateBase();
            else
                Open();
        }

        private void Open()
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load( pathToXmlDocument );

            root = xmlDoc.SelectSingleNode( "Diary" ); // найти корневой элемент            
        }

        private void CreateBase()
        {
            XmlTextWriter xmlWriter = new XmlTextWriter( pathToXmlDocument, System.Text.Encoding.UTF8 )
            {
                Formatting = Formatting.Indented,   // Включить форматирование документа (с отступом).
                IndentChar = '\t',                  // Для выделения уровня элемента использовать табуляцию.
                Indentation = 1,                    // использовать один символ табуляции.
                //QuoteChar = '\''                  // способ записи ковычек
            };

            xmlWriter.WriteStartDocument();         //<? xml version = "1.0" ?>

            xmlWriter.WriteComment( "My diary" );
            xmlWriter.WriteStartElement( "Diary" );
            //xmlWriter.WriteComment( "Date stamps" );
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        public void Add( string msg, double timestamp )
        {
            XmlElement timestampElement;
            XmlElement msgElement;
            string name = "Section_" + timestamp;
            string xPath = "Diary/Section_" + timestamp;

            // Метка времени существует?
            XmlNode timestampNode = xmlDoc.SelectSingleNode( xPath );
            if( timestampNode == null )
            {
                // Нет. Создать элемент с меткой времени
                timestampElement = xmlDoc.CreateElement( name );
                // Создать элемент с сообщением
                msgElement = xmlDoc.CreateElement( "Message" );
                msgElement.InnerText = msg;

                timestampElement.AppendChild( msgElement );
                root.AppendChild( timestampElement );
            } else
            {
                // Создать элемент с сообщением
                msgElement = xmlDoc.CreateElement( "Message" );
                msgElement.InnerText = msg;
                timestampNode.AppendChild( msgElement );
            }
            xmlDoc.Save( pathToXmlDocument );
        }

        internal void Save( string msg )
        {
            double timestamp = MyLibrary.UnixDateStamp();
            Add( msg, timestamp );
            DateTime dt = MyLibrary.ConvertFromUnixTimestamp( timestamp );
            MessageBox.Show( "Элемент с меткой = " + timestamp + " = "
                + dt.Day + "." + dt.Month + "." + dt.Year + " добавлен." );
        }

        public List<string> ReadDiary(string pathToXmlDocument = "DiaryBase.xml" )
        {
            var document = new XmlDocument();
            List<string> resultMsg = new List<string>();

            try
            {
                document.Load( pathToXmlDocument );
                foreach( XmlElement messages in root )
                {
                    double timestamp = double.Parse( messages.Name.Replace( "Section_", "" ) );
                    DateTime date = MyLibrary.ConvertFromUnixTimestamp( timestamp );
                    string fDate = date.Day + "." + date.Month + "." + date.Year;

                    resultMsg.Add( fDate );
                    foreach( XmlElement message in messages )
                        resultMsg.Add( message.InnerText );
                    resultMsg.Add( new string( '-', 30 ) );
                }
            } catch( System.IO.IOException ) { }
            return resultMsg;
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
        public void Open()//RichTextBox richTextBox )
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

        public void Save()//RichTextBox richTextBox )
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
