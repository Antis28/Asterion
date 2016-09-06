using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Asterion.Models;

namespace Asterion.Presentors
{
    class PresenterFindExtInXMLBaseDate
    {
        LogicFindExtInXMLBaseDate XmlBaseDate = null;
        MainWindow mainWindow = null;

        public PresenterFindExtInXMLBaseDate( MainWindow mainWindow )
        {
            this.XmlBaseDate = new LogicFindExtInXMLBaseDate();
            this.mainWindow = mainWindow;
            this.mainWindow.FindExtInXMLBaseDateEvent += new EventHandler( mainWindow_findExtInBaseDate );

            // mainWindow.extDescriptionCategory.Text = mainWindow.extDescription.Text = "";
        }

        void mainWindow_findExtInBaseDate( object sender, System.EventArgs e )
        {
            this.XmlBaseDate = new LogicFindExtInXMLBaseDate();
            XmlBaseDate.Extract( mainWindow.extInput.Text );
            mainWindow.listViewExtDescription.Items.Clear();

            int InfoHeaderIterator = 0;
            int DetailedDescriptionIterator = 0;
            string isHex = "";

            if( XmlBaseDate.Header.Count > 0 )
            {
                for( int i = 0; i < XmlBaseDate.Header.Count; i++ )
                {

                    mainWindow.listViewExtDescription.Items.Add( NewTextBlock( XmlBaseDate.Header[i], mainWindow.Width ) );
                    mainWindow.listViewExtDescription.Items.Add( NewTextBlock( XmlBaseDate.DrusDescription + ": ", mainWindow.Width ) );
                    mainWindow.listViewExtDescription.Items.Add( NewTextBlock( XmlBaseDate.RusDescription[i], mainWindow.Width ) );

                    mainWindow.listViewExtDescription.Items.Add( NewTextBlock( XmlBaseDate.DtypeFile + ": ", mainWindow.Width ) );
                    mainWindow.listViewExtDescription.Items.Add( NewTextBlock( XmlBaseDate.TypeFile[i], mainWindow.Width ) );

                    // Заголовки файлов, если есть
                    if( XmlBaseDate.InfoHeaderFile.Count > InfoHeaderIterator )
                    {
                        mainWindow.listViewExtDescription.Items.Add( NewTextBlock( XmlBaseDate.DinfoHeaderFile + ": ", mainWindow.Width ) );
                        while( XmlBaseDate.InfoHeaderFile.Count > InfoHeaderIterator && isHex != "HEX:" )
                        {
                            mainWindow.listViewExtDescription.Items.Add( NewTextBlock( XmlBaseDate.InfoHeaderFile[InfoHeaderIterator++], mainWindow.Width ) );

                            if( XmlBaseDate.InfoHeaderFile.Count > InfoHeaderIterator )
                                isHex = XmlBaseDate.InfoHeaderFile[InfoHeaderIterator].Remove( 4 );
                            else
                                break;
                        }
                        isHex = "";
                    }
                    // Детальное описание, если есть
                    if( XmlBaseDate.DetailedDescription.Count > DetailedDescriptionIterator )
                    {
                        mainWindow.listViewExtDescription.Items.Add( NewTextBlock( XmlBaseDate.DdetailedDescription + ": ", mainWindow.Width ) );
                        mainWindow.listViewExtDescription.Items.Add( NewTextBlock( XmlBaseDate.DetailedDescription[DetailedDescriptionIterator++], mainWindow.Width ) );
                    }

                    mainWindow.listViewExtDescription.Items.Add( NewTextBlock( XmlBaseDate.DwhatOpen + ": " + XmlBaseDate.WhatOpen[i], mainWindow.Width ) );
                    //Отделить разные форматы
                    mainWindow.listViewExtDescription.Items.Add( "" );
                }
                //mainWindow.listViewExtDescription.Items.Add( XmlBaseDate.);
            } else
            { mainWindow.listViewExtDescription.Items.Add( NewTextBlock("Не найдено совпадений.") ); }

        }

        private static System.Windows.Controls.TextBlock NewTextBlock( string text, Double widthWin = 400 )
        {
            return new System.Windows.Controls.TextBlock()
            {
                FontSize = 22,
                Width = widthWin,
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = text
            };
        }
    }
}
