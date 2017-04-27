using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;

namespace ExtensionStore
{
    /// <summary>
    /// Создает или переписывает xml
    /// в соответсвии с данными
    /// </summary>
    class XmlConstructor
    {
        string fileName = "ExtensionsBase.xml";

        XmlTextWriter xmlTWriter;
        XDocument doc;
        XElement root;

        public XmlConstructor()
        {
            /////////////////////////////////////////////////////////////////
            try
            {
                doc = XDocument.Load(fileName);
                doc.Declaration = new XDeclaration("1.0", "utf-8", "yes");
                root = doc.Element("ListOfExtension");
            } catch
            {
                xmlTWriter = new XmlTextWriter(fileName, Encoding.UTF8)
                {
                    Formatting = Formatting.Indented,   // Включить форматирование документа (с отступом).
                    IndentChar = '\t',                  // Для выделения уровня элемента использовать табуляцию.
                    Indentation = 1,                    // использовать один символ табуляции.
                    //QuoteChar = '\''                  // способ записи ковычек
                };
                xmlTWriter.WriteStartDocument(true);         //<? xml version = "1.0" ?>

                xmlTWriter.WriteComment("Список всех расширений.");
                xmlTWriter.WriteStartElement("ListOfExtension");
                xmlTWriter.Close();
            }
            doc = XDocument.Load(fileName);
            //doc.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            root = doc.Element("ListOfExtension");
        }

        public void AddToCategory( ExtInfo itemExt )
        {
            if( itemExt.TypeFile == null )
                return;
            foreach( KeyValuePair<string, string> cat in Category.categoryes2 )
            {
                // если найдена категория из словаря
                if( itemExt.TypeFile.Contains(cat.Value) )
                {
                    // то ищем ее в xml
                    XElement category = root.Element(cat.Key);

                    XmlWriter xmlTWriter;
                    // категория найдена?
                    if( category != null )
                    {
                        // ищем в xml полученое разрешение
                        foreach( var item in category.Elements("ext") )
                        {
                            bool coincidence = item.Attribute("Name").Value == itemExt.Name;
                            // если xml уже есть такое расширение
                            if( coincidence )
                            {
                                AddInfoExtension(itemExt, item);
                                return;
                            }
                        }

                        xmlTWriter = category.CreateWriter();
                        AddNewExtension(itemExt, xmlTWriter);
                    }
                    else
                    {
                        xmlTWriter = root.CreateWriter();

                        xmlTWriter.WriteStartElement(cat.Key);
                        AddNewExtension(itemExt, xmlTWriter);
                        xmlTWriter.WriteEndElement();
                    }
                    xmlTWriter.Close();
                    break;
                }
            }
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
        /// <summary>
        /// Добавляет новый элемент в xml
        /// </summary>
        /// <param name="itemExt"></param>
        /// <param name="xmlTWriter"></param>
        void AddNewExtension( ExtInfo itemExt, XmlWriter xmlTWriter )
        {
            string element = "e";
            string eName = "Name";
            string eTypeFile = "typeFile";
            string eRusDescription = "rusDescription";
            string eEngDescription = "engDescription";
            string eDetailedDescription = "detailedDescription";
            string eInfoHeaderFile = "infoHeaderFile";
            string eWhatOpenWindows = "WhatOpenWindows";
            string eWhatOpenLinux = "WhatOpenLinux";
            string eWhatOpenMac = "WhatOpenMac";

            xmlTWriter.WriteStartElement("ext");
            {
                xmlTWriter.WriteAttributeString(eName, itemExt.Name);
                xmlTWriter.WriteAttributeString(eTypeFile, itemExt.TypeFile);
                xmlTWriter.WriteAttributeString(eRusDescription, itemExt.RusDescription);
                xmlTWriter.WriteAttributeString(eEngDescription, itemExt.EngDescription);
                xmlTWriter.WriteAttributeString(eDetailedDescription, itemExt.DetailedDescription);

                if( itemExt.InfoHeaderFile.Count > 0 )
                {
                    xmlTWriter.WriteStartElement(eInfoHeaderFile);
                    foreach( var item in itemExt.InfoHeaderFile )
                    {
                        xmlTWriter.WriteElementString(element, item);
                    }
                    xmlTWriter.WriteEndElement();
                }
                if( itemExt.WhatOpenWindows.Count > 0 )
                {
                    xmlTWriter.WriteStartElement(eWhatOpenWindows);
                    foreach( var item in itemExt.WhatOpenWindows )
                    {
                        xmlTWriter.WriteElementString(element, item);
                    }
                    xmlTWriter.WriteEndElement();
                }
                if( itemExt.WhatOpenLinux.Count > 0 )
                {
                    xmlTWriter.WriteStartElement(eWhatOpenLinux);
                    foreach( var item in itemExt.WhatOpenLinux )
                    {
                        xmlTWriter.WriteElementString(element, item);
                    }
                    xmlTWriter.WriteEndElement();
                }
                if( itemExt.WhatOpenMac.Count > 0 )
                {
                    xmlTWriter.WriteStartElement(eWhatOpenMac);
                    foreach( var item in itemExt.WhatOpenMac )
                    {
                        xmlTWriter.WriteElementString(element, item);
                    }
                    xmlTWriter.WriteEndElement();
                }
            }
            xmlTWriter.WriteEndElement();
        }

        /// <summary>
        /// Проверяет xml аттрибут с данными в объекте,
        /// если отличаются то добавляются новые данные
        /// </summary>
        /// <param name="itemExt">объект с данными</param>
        /// <param name="element">элемент с именем как в объекте</param>
        void AddInfoExtension( ExtInfo itemExt, XElement element )
        {
            XAttribute attr;
            bool coincidence;

            attr = element.Attribute("detailedDescription");
            coincidence = attr.Value == itemExt.DetailedDescription;
            if( !coincidence )
            {
                attr.Value = itemExt.DetailedDescription;
            }
        }

        public void Close()
        {
            if( xmlTWriter != null )
                xmlTWriter.Close();
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
