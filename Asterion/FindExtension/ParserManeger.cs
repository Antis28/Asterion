using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using Asterion;

namespace ExtensionStore
{
    /// <summary>
    /// Управляет порядком парсинга
    /// </summary>
    class ParserManeger
    {
        MainWindow mainWindow;
        string linkTypes;
        string linkFormats;
        string formats;

        public ParserManeger( MainWindow mainWindow )
        {
            this.mainWindow = mainWindow;
        }
        public void Parse()
        {

            linkTypes = mainWindow.tb_genLinks.Text;
            linkFormats = mainWindow.tb_allLinks.Text;
            formats = mainWindow.tb_allExtension.Text;

            Encoding codePage = Encoding.GetEncoding(1251);

            ExtractorLinkExt extractor = new ExtractorLinkExt("http://open-file.ru", codePage);

            //события для ссылок на категории форматов
            extractor.CompleteGenLinkParseEvent += Extractor_CompleteConvertEvent;
            extractor.ChangeValueGeneralEvent += Extractor_ChangeValueGeneralEvent;
            extractor.MaxValueGeneralEvent += Extractor_MaxValueGeneralEvent;

            //события для ссылок всех форматов
            extractor.MaxValueAllEvent += Extractor_MaxValueAllEvent;
            extractor.ChangeValueAllEvent += Extractor_ChangeValueAllEvent;
            extractor.CompleteAllLinkParseEvent += Extractor_CompleteAllLinkParseEvent;

            //события заполнения списка объектов на расширения
            extractor.MaxValueExtParseEvent += Extractor_MaxValueExtParseEvent;
            extractor.ChangeValueExtParseEvent += Extractor_ChangeValueExtParseEvent;
            extractor.CompleteExtParseEvent += Extractor_CompleteExtParseEvent;

            extractor.BeginParse();

        }

        public void ExtractExt()
        {
            Encoding codePage = Encoding.GetEncoding(1251);
            XmlExtractor xmlE = new XmlExtractor();
            xmlE.CompleteExtractEvent += XmlE_CompleteExtractEvent;
            // поле для ввода искомого расширения
            xmlE.ExtractExt(mainWindow.extInput.Text);
        }
        ////////////////////////////////////////////////////////////////
        private void XmlE_CompleteExtractEvent( ExtInfo obj )
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                   (Action)delegate
                   {
                       ShowResult(obj);
                   });
        }
        void ShowResult( ExtInfo currentExt )
        {
            BindProp(mainWindow.tb_formatfile, currentExt, "Name");
            BindProp(mainWindow.tb_descripEng, currentExt, "EngDescription");
            BindProp(mainWindow.tb_descripRus, currentExt, "RusDescription");
            BindProp(mainWindow.tb_typefile, currentExt, "TypeFile");
            BindProp(mainWindow.tb_FullDescrip, currentExt, "DetailedDescription");
            BindProp(mainWindow.lb_InfoHeaderFile, currentExt, "InfoHeaderFile");
            BindProp(mainWindow.lb_WhatOpenWindows, currentExt, "WhatOpenWindows");
            BindProp(mainWindow.lb_WhatOpenLinux, currentExt, "WhatOpenLinux");
            BindProp(mainWindow.lb_WhatOpenMac, currentExt, "WhatOpenMac");
        }
        void BindProp( TextBlock tb, ExtInfo currentExt, string property )
        {
            Binding bind = new Binding();
            bind.Source = currentExt;
            bind.Path = new PropertyPath(property);
            bind.Mode = BindingMode.OneWay;
            tb.SetBinding(TextBlock.TextProperty, bind);
        }
        void BindProp( ListBox lb, ExtInfo currentExt, string property )
        {
            Binding bind = new Binding();
            bind.Source = currentExt;
            bind.Path = new PropertyPath(property);
            bind.Mode = BindingMode.OneWay;
            lb.SetBinding(ListBox.ItemsSourceProperty, bind);
        }

        ////////////////////////////////////////////////////////////////

        private void Extractor_CompleteExtParseEvent( List<ExtInfo> extList )
        {
            XmlConstructor constructor = new XmlConstructor();
            foreach( var item in extList )
            {
                if( item == null )
                    continue;
                constructor.AddToCategory(item);
            }

            constructor.Close();
        }

        private void Extractor_ChangeValueExtParseEvent()
        {
        }

        private void Extractor_MaxValueExtParseEvent( int obj )
        {
        }

        private void Extractor_CompleteAllLinkParseEvent( Dictionary<string, List<string>> obj )
        {
            System.Windows.MessageBox.Show("Fine");
        }

        private void Extractor_ChangeValueAllEvent()
        {
        }

        private void Extractor_MaxValueAllEvent( int obj )
        {
        }

        private void Extractor_ChangeValueGeneralEvent()
        {
        }
        private void Extractor_MaxValueGeneralEvent( int obj )
        {
        }

        private void Extractor_CompleteConvertEvent( Dictionary<string, string> obj )
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {
                    });
        }

        private void Parser_CompleteConvertEvent( ExtInfo obj )
        {
            mainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate
                    {

                    });
        }
    }
}
