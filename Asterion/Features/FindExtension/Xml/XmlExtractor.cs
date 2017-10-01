using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;
using System.Windows;
using ExtensionStore.Xml;

namespace ExtensionStore
{
    /// <summary>
    /// Достает данные из xml
    /// </summary>
    class XmlExtractor
    {
        //события заполнения словаря на категории форматов
        public event Action<ExtInfo> CompleteExtractEvent;
        public event Action<int> MaxValueEvent;
        public event Action ChangeValueEvent;

        protected void OnMaxValue( int maxValue )
        {
            if( MaxValueEvent != null )
                MaxValueEvent(maxValue);
        }
        protected void OnChangeValue()
        {
            if( ChangeValueEvent != null )
                ChangeValueEvent();
        }
        protected void OnComplete(ExtInfo ext )
        {
            if( CompleteExtractEvent != null )
                CompleteExtractEvent(ext);           
        }

        string fileName = "ExtensionsBase.xml";
        XDocument doc;
        XElement root;

        bool isReady = false;

        public XmlExtractor()
        {
            /////////////////////////////////////////////////////////////////
            try
            {
                doc = XDocument.Load(fileName);
                doc.Declaration = new XDeclaration("1.0", "utf-8", "yes");
                root = doc.Element("ListOfExtension");
                isReady = true;
            } catch
            {
                MessageBox.Show("Файл: " + fileName + ", не найден!");
            }

        }

        public void ExtractExt( string word )
        {
            if( !isReady )
                return;
           List<XElement> foundItems = new List<XElement>();            

            foreach( KeyValuePair<string, string> cat in Category.categoryes2 )
            {
                XElement category = root.Element(cat.Key);
                foreach( XElement item in category.Elements(XmlStruct.eExt) )
                {
                    bool condit = item.Attribute(XmlStruct.eName).Value == word;
                    if( condit )
                    {
                        foundItems.Add(item);
                    }
                }
            }

            if( foundItems.Count > 0 )
            {
                ExtInfo extInfo = new ExtInfo();
                foreach( XElement foundItem in foundItems )
                {
                    extInfo += Extract(foundItem);
                }

                OnComplete(extInfo);
            }
            else
            {
                MessageBox.Show("Расширение " + word + " не найдено");
            }
        }

        ExtInfo Extract( XElement foundItem )
        {
            string element = XmlStruct.element;
            string eName = XmlStruct.eName;
            string eTypeFile = XmlStruct.eTypeFile;
            string eRusDescription = XmlStruct.eRusDescription;
            string eEngDescription = XmlStruct.eEngDescription;
            string eDetailedDescription = XmlStruct.eDetailedDescription;
            string eInfoHeaderFile = XmlStruct.eInfoHeaderFile;
            string eWhatOpenWindows = XmlStruct.eWhatOpenWindows;
            string eWhatOpenLinux = XmlStruct.eWhatOpenLinux;
            string eWhatOpenMac = XmlStruct.eWhatOpenMac;

            ExtInfo extInfo = new ExtInfo()
            {
                Name = foundItem.Attribute(eName).Value,
                TypeFile = foundItem.Attribute(eTypeFile).Value,
                RusDescription = foundItem.Attribute(eRusDescription).Value,
                EngDescription = foundItem.Attribute(eEngDescription).Value,
                DetailedDescription = foundItem.Attribute(eDetailedDescription).Value
            };
            XElement xElem = foundItem.Element(eInfoHeaderFile);
            if( xElem != null )
            {
                extInfo.InfoHeaderFile = new List<string>();
                foreach( var item in xElem.Elements(element) )
                {
                    extInfo.InfoHeaderFile.Add(item.Value);
                }
            }
            xElem = foundItem.Element(eWhatOpenWindows);
            if( xElem != null )
            {
                extInfo.WhatOpenWindows = new List<string>();
                foreach( var item in xElem.Elements(element) )
                {
                    extInfo.WhatOpenWindows.Add(item.Value);
                }
            }
            xElem = foundItem.Element(eWhatOpenLinux);
            if( xElem != null )
            {
                extInfo.WhatOpenLinux = new List<string>();
                foreach( var item in xElem.Elements(element) )
                {
                    extInfo.WhatOpenLinux.Add(item.Value);
                }
            }
            xElem = foundItem.Element(eWhatOpenMac);
            if( xElem != null )
            {
                extInfo.WhatOpenMac = new List<string>();
                foreach( var item in xElem.Elements(element) )
                {
                    extInfo.WhatOpenMac.Add(item.Value);
                }
            }
            return extInfo;
        }

        class Category
        {
            public string audio = "Аудио-файлы";
            public string video = "Видео-файлы";
            public string other = "Другие файлы";
            public string picture = "Рисунки, изображения";
            public string rastr = "Растровые изображения";
            public string vector = "Векторные изображения";
            public string cad = "CAD-файлы";
            public string threeD = "3D-модели, изображения";
            public string text = "Текст, документы";
            public string game = "Файлы игр";
            public string archive = "Архивы, сжатые файлы";
            public string exe = "Исполняемые файлы";
            public string internet = "Интернет, web файлы";
            public string setting = "Файлы настроек";
            public string imageDisk = "Образы дисков";
            public string system = "Системные файлы";
            public string font = "Файлы шрифтов";
            public string encrypted = "Зашифрованные файлы";
            public string marked = "Размеченные документы";
            public string backup = "Файлы резервных копий";
            public string data = "Файлы данных";
            public string dataBase = "Файлы баз данных";
            public string source = "Скрипты, исходный код";
            public string plugin = "Подключаемые модули";
            public string geo = "Географические файлы, карты";

            public static List<string> categoryes = new List<string>()
            {
                "Аудио-файлы",
                "Видео-файлы",
                "Другие файлы",
                "Рисунки, изображения",
                "Растровые изображения",
                "Векторные изображения",
                "CAD-файлы",
                "3D-модели, изображения",
                "Текст, документы",
                "Файлы игр",
                "Архивы, сжатые файлы",
                "Исполняемые файлы",
                "Интернет, web файлы",
                "Файлы настроек",
                "Образы дисков",
                "Системные файлы",
                "Файлы шрифтов",
                "Зашифрованные файлы",
                "Размеченные документы",
                "Файлы резервных копий",
                "Файлы данных",
                "Файлы баз данных",
                "Скрипты, исходный код",
                "Подключаемые модули",
                "Географические файлы, карты",
        };
            public static Dictionary<string, string> categoryes2 = new Dictionary<string, string>()
            {
                { "audio" , "Аудио-файлы" },
                { "video" , "Видео-файлы" },
                { "other" , "Другие файлы" },
                { "picture", "Рисунки, изображения" },
                { "rastr" , "Растровые изображения" },
                { "vector" , "Векторные изображения" },
                { "cad" , "CAD-файлы" },
                { "threeD" , "3D-модели, изображения" },
                { "text" , "Текст, документы" },
                { "game" , "Файлы игр" },
                { "archive" , "Архивы, сжатые файлы" },
                { "exe" , "Исполняемые файлы" },
                { "internet" , "Интернет, web файлы" },
                { "setting" , "Файлы настроек" },
                { "imageDisk" , "Образы дисков" },
                { "system" , "Системные файлы" },
                { "font" , "Файлы шрифтов" },
                { "encrypted" , "Зашифрованные файлы" },
                { "marked" , "Размеченные документы" },
                { "backup" , "Файлы резервных копий"},
                { "data" , "Файлы данных"},
                { "dataBase" , "Файлы баз данных"},
                { "source" , "Скрипты, исходный код"},
                { "plugin" , "Подключаемые модули"},
                { "geo" , "Географические файлы, карты"}
        };
        }
       
        public void Close()
        {            
            if( doc != null )
            {
                doc.Save(fileName);
            }
        }
    }
}

/*

МЕТАСИМВОЛЫ - это символы для составления Шаблона поиска.

\d    Определяет символы цифр. 
\D 	Определяет любой символ, который не является цифрой. 
\w 	Определяет любой символ цифры, буквы или подчеркивания. 
\W    Определяет любой символ, который не является цифрой, буквой или подчеркиванием. 
\s 	Определяет любой непечатный символ, включая пробел. 
\S 	Определяет любой символ, кроме символов табуляции, новой строки и возврата каретки.
.    Определяет любой символ кроме символа новой строки. 
\.    Определяет символ точки.

*/


