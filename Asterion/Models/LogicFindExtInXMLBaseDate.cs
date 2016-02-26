/*
        Вытаскивает из Xml информацию о расширении файла
*/

using System.Collections.Generic;
using System.Xml;

namespace Asterion.Presentors
{
    class LogicFindExtInXMLBaseDate
    {
        List<string> header;
        List<string> rusDescription;
        List<string> engDescription;
        List<string> infoHeaderFile;
        List<string> typeFile;
        List<string> detailedDescription;
        List<string> whatOpen;

        public readonly string DrusDescription = "Описание файла на русском";
        public readonly string DengDescription = "Описание файла на английском";
        public readonly string DinfoHeaderFile = "Информация о заголовке файла";
        public readonly string DtypeFile = "Тип файла";
        public readonly string DdetailedDescription = "Подробное описание";
        public readonly string DwhatOpen = "Как, чем открыть файл";

        public List<string> WhatOpen
        {
            get
            {
                return whatOpen;
            }

            set
            {
                whatOpen = value;
            }
        }

        public List<string> TypeFile
        {
            get
            {
                return typeFile;
            }

            set
            {
                typeFile = value;
            }
        }

        public List<string> EngDescription
        {
            get
            {
                return engDescription;
            }

            set
            {
                engDescription = value;
            }
        }

        public List<string> RusDescription
        {
            get
            {
                return rusDescription;
            }

            set
            {
                rusDescription = value;
            }
        }

        public List<string> Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
            }
        }

        public List<string> DetailedDescription
        {
            get
            {
                return detailedDescription;
            }

            set
            {
                detailedDescription = value;
            }
        }

        public List<string> InfoHeaderFile
        {
            get
            {
                return infoHeaderFile;
            }

            set
            {
                infoHeaderFile = value;
            }
        }

        public LogicFindExtInXMLBaseDate()
        {
            header = new List<string>();
            rusDescription = new List<string>();
            engDescription = new List<string>();
            InfoHeaderFile = new List<string>();
            DetailedDescription = new List<string>();
            typeFile = new List<string>();
            whatOpen = new List<string>();
        }

        public void Extract( string zapros = "2sf", string pathToXmlDocument = @"6000-ExtensionsBase.xml" )
        {
            var document = new XmlDocument(); 
           
            document.Load( pathToXmlDocument );

            //// <ListOfExtension>
            XmlElement root = document.DocumentElement; //<?xml version="1.0"?>
            
            zapros = "RASHIRENIE" + zapros;
            XmlElement resultNode = null;
            foreach( XmlElement item in root )
            {
                if( zapros == item.Name )
                {
                    resultNode = item;
                    break;
                }
            }
            //category = resultNode.Name;
            if( resultNode != null )
            {
                foreach( XmlElement item in resultNode )
                {
                    switch( item.Name )
                    {
                        case "Header":
                            Header.Add( item.GetAttribute( "Value" ) );
                            break;
                        case "rusDescription":
                            RusDescription.Add( item.GetAttribute( "Value" ) );
                            break;
                        case "engDescription":
                            EngDescription.Add( item.GetAttribute( "Value" ) );
                            break;
                        case "typeFile":
                            TypeFile.Add( item.GetAttribute( "Value" ) );
                            break;
                        case "WhatOpen":
                            WhatOpen.Add( item.GetAttribute( "Value" ) );
                            break;
                        case "infoHeaderFile":
                            infoHeaderFile.Add( item.GetAttribute( "Value" ) );
                            break;
                        case "detailedDescription":
                            detailedDescription.Add( item.GetAttribute( "Value" ) );
                            break;
                    }
                }
            }
            
        }
    }
}
//!= ".2sf"