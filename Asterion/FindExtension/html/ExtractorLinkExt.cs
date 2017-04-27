using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using HtmlAgilityPack;

namespace ExtensionStore
{
    /// <summary>
    /// Получает ссылки со страницы
    /// </summary>
    class ExtractorLinkExt
    {
        private string siteAddress;
        private string fileTempAddress;

        public Encoding encoding;

        ///////////////////////////////////////////////////////////////////////

        //события заполнения словаря на категории форматов
        public event Action<Dictionary<string, string>> CompleteGenLinkParseEvent;
        public event Action<int> MaxValueGeneralEvent;
        public event Action ChangeValueGeneralEvent;

        protected void OnMaxValueGeneral( int maxValue )
        {
            if( MaxValueGeneralEvent != null )
                MaxValueGeneralEvent(maxValue);
        }
        protected void OnChangeValueGen()
        {
            if( ChangeValueGeneralEvent != null )
                ChangeValueGeneralEvent();
        }
        protected void OnCompleteGenLinkParse( Dictionary<string, string> list )
        {
            CompleteGenLinkParseEvent(list);
        }

        ///////////////////////////////////////////////////////////////////////

        //события заполнения словаря ссылок на все форматы
        public event Action<Dictionary<string, List<string>>> CompleteAllLinkParseEvent;
        public event Action<int> MaxValueAllEvent;
        public event Action ChangeValueAllEvent;

        protected void OnCompleteAllLinkParse( Dictionary<string, List<string>> list )
        {
            if( CompleteAllLinkParseEvent != null )
                CompleteAllLinkParseEvent(list);
        }
        protected void OnMaxValueAll( int maxValue )
        {
            if( MaxValueAllEvent != null )
                MaxValueAllEvent(maxValue);
        }
        protected void OnChangeValueAll()
        {
            if( ChangeValueAllEvent != null )
                ChangeValueAllEvent();
        }

        ///////////////////////////////////////////////////////////////////////

        //события заполнения списка объектов на расширения
        public event Action<List<ExtInfo>> CompleteExtParseEvent;
        public event Action<int> MaxValueExtParseEvent;
        public event Action ChangeValueExtParseEvent;

        protected void OnCompleteExtParse( List<ExtInfo> list )
        {
            if( CompleteExtParseEvent != null )
                CompleteExtParseEvent(list);
        }
        protected void OnMaxValueExtParse( int maxValue )
        {
            if( MaxValueExtParseEvent != null )
                MaxValueExtParseEvent(maxValue);
        }
        protected void OnChangeValueExtParse()
        {
            if( ChangeValueExtParseEvent != null )
                ChangeValueExtParseEvent();
        }

        ///////////////////////////////////////////////////////////////////////
        public ExtractorLinkExt()
        {
            siteAddress = "http://open-file.ru/";
            fileTempAddress = "temp.txt";
            encoding = Encoding.GetEncoding(1251);
        }

        public ExtractorLinkExt( string siteAddress, Encoding encoding )
        {
            this.siteAddress = siteAddress;
            this.encoding = encoding;
        }

        public void BeginParse()
        {
            Thread backgroundThread = new Thread(new ThreadStart(Start));
            backgroundThread.Name = "SiteParser";
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        private void Start()
        {
            Dictionary<string, string> linkList = GetTypeLinkList();
            OnCompleteGenLinkParse(linkList);
            Dictionary<string, List<string>> allLinkDict = GetAllLinkList(linkList);
            List<ExtInfo> extList = GetExtensionList(allLinkDict);
            CompleteExtParseEvent(extList);
        }

        private Dictionary<string, string> GetTypeLinkList()
        {
            Dictionary<string, string> ExtList = new Dictionary<string, string>();

            //xPathQuery
            string xpq_allWorks = "//ul[@class=\"nav-list\"]"; //ul class="nav-list"

            HtmlDocument allHTML = new HtmlDocument();
            HtmlDocument currentHTML = new HtmlDocument();

            fileTempAddress = "links.txt";
            if( File.Exists(fileTempAddress) )
            {
                allHTML.LoadHtml(HtmlToString.ReadCacheFile(fileTempAddress, encoding));
            }
            else
            {
                allHTML.LoadHtml(HtmlToString.Read(siteAddress, encoding, fileTempAddress));
            }

            var TableNodes = allHTML.DocumentNode.SelectSingleNode(xpq_allWorks).SelectNodes("li");

            OnMaxValueGeneral(TableNodes.Count());
            foreach( var lineNode in TableNodes )
            {
                if( lineNode.Name == "#text" )
                    continue;

                HtmlNode tdNodes = lineNode.SelectSingleNode("a");

                HtmlAttribute href = tdNodes.Attributes["href"];
                string name = tdNodes.InnerText;
                string link = href.Value;
                ExtList.Add(name, link);
                OnChangeValueGen();
            }
            return ExtList;
        }
        private Dictionary<string, List<string>> GetAllLinkList( Dictionary<string, string> linkList )
        {
            Dictionary<string, List<string>> allLinkDict =
                                            new Dictionary<string, List<string>>();

            HtmlDocument allHTML = new HtmlDocument();
            HtmlDocument currentHTML = new HtmlDocument();
            
            OnMaxValueAll(linkList.Count);
            foreach( KeyValuePair<string, string> item in linkList )
            {
                fileTempAddress = "linkTypes\\" + item.Key + ".txt";
                List<string> templinkType = new List<string>();
                if( !allLinkDict.ContainsKey(item.Key) )
                {
                    allLinkDict.Add(item.Key, templinkType);
                }
                else
                {
                    allLinkDict[item.Key] = templinkType;
                }

                if( File.Exists(fileTempAddress) )
                {
                    allHTML.LoadHtml(HtmlToString.ReadCacheFile(fileTempAddress, encoding));
                }
                else
                {
                    allHTML.LoadHtml(HtmlToString.Read(siteAddress + item.Value, encoding, fileTempAddress));
                }
                string xpq_allWorks;
                HtmlNodeCollection TableNodes;
                xpq_allWorks = "//table[@class=\"tbl tablecol-2\"]/tbody/tr";
                TableNodes = allHTML.DocumentNode.SelectNodes(xpq_allWorks);
                if( TableNodes == null )
                {
                    xpq_allWorks = "//table[@class=\"tbl tablecol-2\"]/tr";
                    TableNodes = allHTML.DocumentNode.SelectNodes(xpq_allWorks);
                    if( TableNodes == null )
                    {
                        MessageBox.Show("Ошибка - " + item.Key);
                        continue;
                    }
                }
                foreach( var lineNode in TableNodes )
                {
                    HtmlNodeCollection tdNodes = lineNode.SelectNodes("td");
                    if( tdNodes == null )
                        continue;
                    HtmlNode elA = tdNodes[0].SelectSingleNode("a");
                    HtmlAttribute href = elA.Attributes["href"];
                    templinkType.Add(href.Value);
                }
                OnChangeValueAll();
            }

            return allLinkDict;
        }
        private ExtInfo GetDescriptionExtension( string link )
        {
            ExtInfo ext = new ExtInfo();
            HtmlDocument allHTML = new HtmlDocument();
            HtmlDocument currentHTML = new HtmlDocument();
            string siteAddress = "";
            if( link.Contains(this.siteAddress) )
                siteAddress = link;
            else
                siteAddress = this.siteAddress + link;
            ext.Link = siteAddress;
            fileTempAddress = siteAddress.Replace("http://open-file.ru/types/", "");
            fileTempAddress = "types\\" + fileTempAddress + ".txt";
            if( File.Exists(fileTempAddress) )
            {
                allHTML.LoadHtml(HtmlToString.ReadCacheFile(fileTempAddress, encoding));
            }
            else
            {
                string s = HtmlToString.Read(siteAddress, encoding, fileTempAddress);
                if( s == "" )
                    return ext;
                allHTML.LoadHtml(s);
            }
            //xPathQuery
            //table class="desc"
            string xpq_allWorks;
            HtmlNodeCollection TableNodes;

            xpq_allWorks = "//table[@class=\"desc\"]/*/*/*/td|//table[@class=\"desc\"]/*/*/td|//table[@class=\"desc\"]/*/td|//table[@class=\"desc\"]/td|//table[@class=\"desc\"]/th";
            TableNodes = allHTML.DocumentNode.SelectNodes(xpq_allWorks);
            if( TableNodes == null )
            {
                MessageBox.Show("Ошибка - " + link);
                return null;
            }


            foreach( var tdNode in TableNodes )
            {
                string key = null;
                key = tdNode.InnerText;
                int index = TableNodes.IndexOf(tdNode);

                if( key.Contains("Формат") )
                {
                    Match m = Regex.Match(tdNode.InnerText, @"\.[a-zа-я0-9]*");
                    if( m.Value != string.Empty )
                        ext.Name = m.Value.Remove(0, 1);
                }
                else if( key.Contains("Тип файла") )
                {
                    if( ext.TypeFile == string.Empty )
                        ext.TypeFile = TableNodes[index + 1].InnerText;
                    else
                        ext.TypeFile += ",\n" + TableNodes[index + 1].InnerText;
                }
                else if( key.Contains("на русском") )
                {
                    if( ext.RusDescription == string.Empty )
                        ext.RusDescription = TableNodes[index + 1].InnerText;
                    else
                        ext.RusDescription += ",\n" + TableNodes[index + 1].InnerText;
                }
                else if( key.Contains("на английском") )
                {
                    if( ext.EngDescription == string.Empty )
                        ext.EngDescription = TableNodes[index + 1].InnerText;
                    else
                        ext.EngDescription += ",\n" + TableNodes[index + 1].InnerText;
                }
                else if( key.Contains("Подробное описание") )
                {
                    if( ext.DetailedDescription == string.Empty )
                        ext.DetailedDescription = TableNodes[index + 1].InnerText;
                    else
                        ext.DetailedDescription += ",\n " + TableNodes[index + 1].InnerText;
                }
                else if( key.Contains("ASCII:") )
                {
                    ext.InfoHeaderFile.Add(key);
                }
                else if( key.Contains("HEX:") )
                {
                    ext.InfoHeaderFile.Add(key);
                }
                else if( key == " Windows" || key == "Windows" )
                {
                    ext.WhatOpen = TableNodes[index + 1].InnerText;

                    HtmlNodeCollection li_s = TableNodes[index + 1].SelectNodes("*/li|*/*/li");
                    foreach( HtmlNode li in li_s )
                    {
                        ext.WhatOpenWindows.Add(li.InnerText);
                    }
                }
                else if( key == " Linux" || key == "Linux" )
                {
                    ext.WhatOpen += TableNodes[index + 1].InnerText;
                    HtmlNodeCollection li_s = TableNodes[index + 1].SelectNodes("*/li");

                    foreach( HtmlNode li in li_s )
                    {
                        ext.WhatOpenLinux.Add(li.InnerText);
                    }

                }
                else if( key == " MacOS" )
                {
                    ext.WhatOpen += TableNodes[index + 1].InnerText;
                    HtmlNodeCollection li_s = TableNodes[index + 1].SelectNodes("*/li");
                    foreach( HtmlNode li in li_s )
                    {
                        ext.WhatOpenMac.Add(li.InnerText);
                    }
                }
            }
            if( ext.EngDescription == null
                || ext.RusDescription == null
                || ext.Name == null
                || ext.TypeFile == null
                || ext.Link == null
                )
                MessageBox.Show("Не найдены данные");

            return ext;
        }
        private List<ExtInfo> GetExtensionList( Dictionary<string, List<string>> AllLink )
        {
            List<ExtInfo> extList = new List<ExtInfo>();

            int count = 0;
            foreach( KeyValuePair<string, List<string>> item in AllLink )
            {
                count += item.Value.Count;
            }
            OnMaxValueExtParse(count);
            foreach( KeyValuePair<string, List<string>> item in AllLink )
            {
                foreach( string link in item.Value )
                //for( int i = 0; i < 5; i++ )
                {
                    //ExtInfo ext = GetDescriptionExtension(item.Value[i]);
                    ExtInfo ext = GetDescriptionExtension(link);
                    extList.Add(ext);
                    OnChangeValueExtParse();
                }
            }
            return extList;
        }
    }
}
//OnChangeValue();