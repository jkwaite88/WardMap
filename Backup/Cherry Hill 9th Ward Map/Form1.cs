using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Collections;
using JouniHeikniemi.Tools.Text;






namespace Cherry_Hill_9th_Ward_Map
{
    public partial class Form1 : Form
    {
        //class instance variables      
        private PrintDocument printDoc = new PrintDocument();
        private PageSettings pgSettings = new PageSettings();
        private PrinterSettings prtSettings = new PrinterSettings();
        private const int MAX_PAGES = 2;
        private const float MIN_FONT_SIZE = 6;
        private const float PAGE_TITLE_FONT_SIZE = 40;
        private const float DEFAULT_NAME_FONT_SIZE = 12;
        private const float DEFAULT_ADDRESSW_FONT_SIZE = 10;
        private const bool PRINT_ADDRESS_BOX = false;
        private const bool PRINT_NAME_BOX = false;
        private const int DEFAULT_LINE_SPACE_HEIGHT = 0;
        private const int MIN_LINE_SPACE_HEIGHT = 0;
        public enum MapArea { SandyBrook = 1, Condos, _100East, _1700South, Church };
        private MapArea StartPage;
        private MapArea StopPage;
        private MapArea CurrentPage;
        private int NumAreas;
        private Font pageTitleFont = new Font("Ariel", PAGE_TITLE_FONT_SIZE, FontStyle.Bold);
        private Font addressFont = new Font("Ariel", DEFAULT_ADDRESSW_FONT_SIZE);
        private Font nameFont = new Font("Ariel", DEFAULT_NAME_FONT_SIZE, FontStyle.Bold);
        private Font stringFont = new Font("Courier New", 12, FontStyle.Bold);
        private Font streetFont = new Font("Ariel", 14, FontStyle.Bold);
        private Pen streetPen = new Pen(Color.Black, 3);
        private Pen propertyBoundPen = new Pen(Color.Black, 1);
        private Pen addressBoundPen = new Pen(Color.Green, 1);
        private Pen nameBoundPen = new Pen(Color.Red, 1);
        private WardList WardList = new WardList();

        ProgramSettings programRegSettings = new ProgramSettings();
        
        //Default Constructor
        public Form1()
        {
            InitializeComponent();
            MenuItem fileMenuItem = new MenuItem("&File");
            MenuItem filePageSetupMenuItem = new MenuItem("Page Set&up...", new EventHandler(filePageSetupMenuItem_Click));
            MenuItem filePrintPreviewMenuItem = new MenuItem("Print Pre&view", new EventHandler(filePrintPreviewMenuItem_Click));
            MenuItem filePrintMenuItem = new MenuItem("&Print...", new EventHandler(filePrintMenuItem_Click), Shortcut.CtrlP);
          

            fileMenuItem.MenuItems.Add(filePageSetupMenuItem);
            fileMenuItem.MenuItems.Add(filePrintPreviewMenuItem);
            fileMenuItem.MenuItems.Add(filePrintMenuItem);

            this.Menu = new MainMenu();
            this.Menu.MenuItems.Add(fileMenuItem);
            printDoc.PrintPage += new PrintPageEventHandler(
                  printDoc_PrintPage);
            initFilePaths(); 
 
            WardList.populateDtOnWardList();
            WardList.populateDtNotOnWardList();
            OnWardList.DataSource = WardList.dtOnWardList;
            NotOnWardList.DataSource = WardList.dtNotOnWardList;
            string tmpStr = WardList.dtOnWardList.Rows.Count.ToString();
            lblNumFamsOnList.Text = "Number of Families: " + tmpStr;
            tmpStr = WardList.dtNotOnWardList.Rows.Count.ToString();
            lblNumFamsNotOnList.Text = "Number of Families: " + tmpStr;

            OnWardList_ColumnWidthChanged();
        }

        // -------------- event handlers ---------------------------------
        
       private void filePrintMenuItem_Click(Object sender,
                EventArgs e)
        {
            printDoc.DefaultPageSettings = pgSettings;
            PrintDialog dlg = new PrintDialog();
            dlg.Document = printDoc;
            dlg.AllowSomePages = true;
            //printDoc.DefaultPageSettings.PrinterSettings.MaximumPage = 4;
            //printDoc.DefaultPageSettings.PrinterSettings.MinimumPage = 1;
            dlg.Document.PrinterSettings.FromPage = 1;
            dlg.Document.PrinterSettings.ToPage = 5 ;// MAX_PAGES;
            dlg.Document.PrinterSettings.MaximumPage = 5;// MAX_PAGES;
            dlg.Document.PrinterSettings.MinimumPage = 1;
           if (dlg.ShowDialog() == DialogResult.OK)
            {
                StartPage = (MapArea)dlg.Document.PrinterSettings.FromPage;
                StopPage = (MapArea)dlg.Document.PrinterSettings.ToPage;
                CurrentPage = StartPage;
                printDoc.Print();
            }
        }

        private void initFilePaths()
        {
            string pathOnWardList = "";
            string pathNotOnWardList = "";
            
            //get path from registry
            int i = 0;
            pathOnWardList = programRegSettings.WardListFile.ToString();
            pathNotOnWardList = programRegSettings.NotONWardListFile.ToString();
            //debug
            //pathOnWardList = "C:\\Documents and Settings\\Jonathan\\Desktop\\Cherry Hill 9th Ward member directory.csv";
            //pathNotOnWardList = "C:\\Documents and Settings\\Jonathan\\Desktop\\Not On Ward List.csv";
            //pathOnWardList = @"C:\Documents and Settings\Jonathan\Desktop\Cherry Hill 9th Ward member directory.csv";
            //pathNotOnWardList = @"C:\Documents and Settings\Jonathan\Desktop\Not On Ward List.csv";
            //debug

            //update paths
            updateOnWardListPath(pathOnWardList);
            updateNotOnWardListPath(pathNotOnWardList);
        }

        private void updateOnWardListPath(string pathOnWardList)
        {
            if (File.Exists(pathOnWardList))
            {
                //update OnWardList registry with path
                //todo

                //set path in ward list
                WardList.onWardListPath = pathOnWardList;

                //set text box
                wardListPathTextBox.Text = pathOnWardList;
            }
        }
        private void updateNotOnWardListPath(string pathNotOnWardList)
        {
         if (File.Exists(pathNotOnWardList))
            {
                //update NotOnWardList registry with path
                //todo

                //set path in ward list
                WardList.notOnWardListPath = pathNotOnWardList;

                //set text box
                notOnwardListPathTextBox.Text = pathNotOnWardList;
            }
        }

    

        private void filePrintPreviewMenuItem_Click(Object sender,
                EventArgs e)
        {
            StartPage = (MapArea) 1;
            StopPage = (MapArea) 5;
            CurrentPage = StartPage;
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            
            dlg.Document = printDoc;
            
            dlg.Width = 1000;
            dlg.Height = 650;
            dlg.ShowDialog();

        }

        private void filePageSetupMenuItem_Click(Object sender,
                EventArgs e)
        {
            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.PageSettings = pgSettings;
            pageSetupDialog.PrinterSettings = prtSettings;
            pageSetupDialog.AllowOrientation = true;
            pageSetupDialog.AllowMargins = true;
            pageSetupDialog.ShowDialog();
        }

        private void printDoc_PrintPage(Object sender,
                PrintPageEventArgs e)
        {
            DrawPage(CurrentPage, e);

            e.HasMorePages = ++CurrentPage <= StopPage;
        }

        private void DrawPage(MapArea Page, PrintPageEventArgs e)
        {
            switch (Page)
            {
                case MapArea.SandyBrook:
                    {
                        DrawSandyBrook(e);
                        DrawSandyBrookTextAndStuff(e);
                        break;
                    }
                case MapArea.Condos:
                    {
                        DrawCondos(e);
                        DrawCondosTextAndStuff(e);
                        break;
                    }
                case MapArea._100East:
                    {
                        Draw_100E(e);
                        Draw100EastTextAndStuff(e);
                        break;
                    }
                case MapArea._1700South:
                    {
                        Draw_1700South(e);
                        Draw1700SouthTextAndStuff(e);
                        break;
                    }
                case MapArea.Church:
                    {
                        DrawChurchArea(e);
                        DrawChurchAreaTextAndStuff(e);
                        break;
                    }
                default:
                    break;
            }
        }

        private void DrawChurchAreaTextAndStuff(PrintPageEventArgs e)
        {
            Point upperLeftPoint = new Point();
            String textToPrint;
            SizeF tempSize = new SizeF();
            textToPrint = "Church Area";
            tempSize = e.Graphics.MeasureString(textToPrint, pageTitleFont);
            //center text
            upperLeftPoint.X = e.PageBounds.Width / 2 - (int)tempSize.Width / 2-150;
            upperLeftPoint.Y = 650;
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(upperLeftPoint.X, upperLeftPoint.Y);
            e.Graphics.RotateTransform(-90);
            DrawTitleAndStuff(textToPrint, new Point(0, 0), e);
            //drawCompass
            e.Graphics.ResetTransform();
            drawCompass(e, new Point(750, 200));
            drawNotOnWardListLegend(e);
        }

        private void Draw1700SouthTextAndStuff(PrintPageEventArgs e)
        {
            Point upperLeftPoint = new Point();
            String textToPrint;
            SizeF tempSize = new SizeF();
            e.Graphics.ResetTransform();
            textToPrint = "1700 South";
            tempSize = e.Graphics.MeasureString(textToPrint, pageTitleFont);
            //center text
            upperLeftPoint.X = e.PageBounds.Width / 2 - (int)tempSize.Width / 2;
            upperLeftPoint.Y = 1000;
            DrawTitleAndStuff(textToPrint, upperLeftPoint, e);
            //drawCompass
            drawCompass(e, new Point(750, 200));
            drawNotOnWardListLegend(e);
        }
        private void Draw_1700South(PrintPageEventArgs e)
        {
            Rectangle propertyRect = new Rectangle();
            Rectangle addressRect = new Rectangle();
            Rectangle nameRect = new Rectangle();
            Rectangle tRect = new Rectangle();
            StringFormat addressFormat = new StringFormat();
            Point p1 = new Point();
            Point p2 = new Point();
            string tempString;
            string[] addressStringsToFind = new string[2];
            string[] stringsToWrite;
            SizeF stringSize = new SizeF();
            int unit_h;
            int unit_w;
            int defaultUnit_h = 60;
            int defaultUnit_w = 99;
            int temp1;

            int bldg_138E_x = 120;
            int bldg_138E_y = 400;
            int bldg_138E_w = 70;// helps if divisible by number units across
            int bldg_138E_h = 300; // helps if divisible by number units high

            int bldg_148E_x = 230;
            int bldg_148E_y = 160;
            int bldg_148E_w = 70;// helps if divisible by number units across
            int bldg_148E_h = 60; // helps if divisible by number units high

            int bldg_140E_x = 205;
            int bldg_140E_y = 270;
            int bldg_140E_w = 70;// helps if divisible by number units across
            int bldg_140E_h = 60; // helps if divisible by number units high

            int bldg_144E_x = 290;
            int bldg_144E_y = 270;
            int bldg_144E_w = 70;// helps if divisible by number units across
            int bldg_144E_h = 60; // helps if divisible by number units high

            int bldg_164E_x = 305;
            int bldg_164E_y = 170;
            int bldg_164E_w = 70;// helps if divisible by number units across
            int bldg_164E_h = 60; // helps if divisible by number units high

            int bldg_168E_x = 305;
            int bldg_168E_y = 255;
            int bldg_168E_w = 70;// helps if divisible by number units across
            int bldg_168E_h = 60; // helps if divisible by number units high

            int bldg_172E_x = 475;
            int bldg_172E_y = 185;
            int bldg_172E_w = 70;// helps if divisible by number units across
            int bldg_172E_h = 60; // helps if divisible by number units high

            int bldg_176E_x = 475;
            int bldg_176E_y = 100;
            int bldg_176E_w = 70;// helps if divisible by number units across
            int bldg_176E_h = 60; // helps if divisible by number units high

            int bldg_184E_x = 560;
            int bldg_184E_y = 160;
            int bldg_184E_w = 70;// helps if divisible by number units across
            int bldg_184E_h = 60; // helps if divisible by number units high

            int bldg_1624S_x = 575;
            int bldg_1624S_y = 170;
            int bldg_1624S_w = 70;// helps if divisible by number units across
            int bldg_1624S_h = 60; // helps if divisible by number units high

            int bldg_1632S_x = 575;
            int bldg_1632S_y = 255;
            int bldg_1632S_w = 70;// helps if divisible by number units across
            int bldg_1632S_h = 60; // helps if divisible by number units high

            int bldg_1650S_x = 305;
            int bldg_1650S_y = 400;
            int bldg_1650S_w = 130;// helps if divisible by number units across
            int bldg_1650S_h = 330; // helps if divisible by number units high

            int bldg_189E_x = 565;
            int bldg_189E_y = 415;
            int bldg_189E_w = 70;// helps if divisible by number units across
            int bldg_189E_h = 60; // helps if divisible by number units high

            int bldg_175E_x = 480;
            int bldg_175E_y = 415;
            int bldg_175E_w = 70;// helps if divisible by number units across
            int bldg_175E_h = 60; // helps if divisible by number units high

            int bldg_161E_x = 395;
            int bldg_161E_y = 415;
            int bldg_161E_w = 70;// helps if divisible by number units across
            int bldg_161E_h = 60; // helps if divisible by number units high

            int bldg_147E_x = 310;
            int bldg_147E_y = 415;
            int bldg_147E_w = 70;// helps if divisible by number units across
            int bldg_147E_h = 60; // helps if divisible by number units high

            int bldg_135E_x = 225;
            int bldg_135E_y = 415;
            int bldg_135E_w = 70;// helps if divisible by number units across
            int bldg_135E_h = 60; // helps if divisible by number units high

            int bldg_125E_x = 140;
            int bldg_125E_y = 415;
            int bldg_125E_w = 70;// helps if divisible by number units across
            int bldg_125E_h = 60; // helps if divisible by number units high

            int bldg_1714S_x = 575;
            int bldg_1714S_y = 595;
            int bldg_1714S_w = 70;// helps if divisible by number units across
            int bldg_1714S_h = 60; // helps if divisible by number units high

            int bldg_1722S_x = 575;
            int bldg_1722S_y = 680;
            int bldg_1722S_w = 70;// helps if divisible by number units across
            int bldg_1722S_h = 60; // helps if divisible by number units high

            int bldg_1734S_x = 575;
            int bldg_1734S_y = 765;
            int bldg_1734S_w = 70;// helps if divisible by number units across
            int bldg_1734S_h = 60; // helps if divisible by number units high

            int bldg_1744S_x = 575;
            int bldg_1744S_y = 850;
            int bldg_1744S_w = 70;// helps if divisible by number units across
            int bldg_1744S_h = 60; // helps if divisible by number units high

            int bldg_180E_x = 560;
            int bldg_180E_y = 585;
            int bldg_180E_w = 70;// helps if divisible by number units across
            int bldg_180E_h = 60; // helps if divisible by number units high

            int bldg_164E1700S_x = 475;
            int bldg_164E1700S_y = 585;
            int bldg_164E1700S_w = 70;// helps if divisible by number units across
            int bldg_164E1700S_h = 60; // helps if divisible by number units high

            int bldg_1713S_x = 390;
            int bldg_1713S_y = 525;
            int bldg_1713S_w = 70;// helps if divisible by number units across
            int bldg_1713S_h = 60; // helps if divisible by number units high

            int bldg_1725S_x = 430;
            int bldg_1725S_y = 610;
            int bldg_1725S_w = 70;// helps if divisible by number units across
            int bldg_1725S_h = 60; // helps if divisible by number units high

            int bldg_1731S_x = 460;
            int bldg_1731S_y = 695;
            int bldg_1731S_w = 70;// helps if divisible by number units across
            int bldg_1731S_h = 60; // helps if divisible by number units high

            int bldg_1743S_x = 440;
            int bldg_1743S_y = 830;
            int bldg_1743S_w = 70;// helps if divisible by number units across
            int bldg_1743S_h = 60; // helps if divisible by number units high

            int bldg_1742S_x = 295;
            int bldg_1742S_y = 865;
            int bldg_1742S_w = 70;// helps if divisible by number units across
            int bldg_1742S_h = 60; // helps if divisible by number units high

            int bldg_1710S_x = 220;
            int bldg_1710S_y = 595;
            int bldg_1710S_w = 70;// helps if divisible by number units across
            int bldg_1710S_h = 60; // helps if divisible by number units high

            int bldg_1722S145E_x = 220;
            int bldg_1722S145E_y = 680;
            int bldg_1722S145E_w = 70;// helps if divisible by number units across
            int bldg_1722S145E_h = 60; // helps if divisible by number units high

            int bldg_1732S_x = 220;
            int bldg_1732S_y = 765;
            int bldg_1732S_w = 70;// helps if divisible by number units across
            int bldg_1732S_h = 60; // helps if divisible by number units high

            int bldg_124E_x = 205;
            int bldg_124E_y = 585;
            int bldg_124E_w = 70;// helps if divisible by number units across
            int bldg_124E_h = 60; // helps if divisible by number units high

            int bldg_202E_x = 100;
            int bldg_202E_y = 990;
            int bldg_202E_w = 100;// helps if divisible by number units across
            int bldg_202E_h = 585; // helps if divisible by number units high

            addressFormat.Alignment = StringAlignment.Center;
            addressFormat.LineAlignment = StringAlignment.Center;

            //draw roads
            //1600 S
            temp1 = 40;
            e.Graphics.ResetTransform();
            p1 = new Point(bldg_138E_x -bldg_138E_w - temp1, bldg_138E_y - bldg_138E_h);
            p2 = new Point(bldg_148E_x + 10 , bldg_148E_y - bldg_148E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);

            p1.X = p2.X; p1.Y = bldg_148E_y + 10;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.Y = p1.Y; p2.X = bldg_140E_x - 40;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = p2.X; p1.Y = bldg_140E_y - bldg_140E_h - 10;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.Y = p1.Y; p2.X = bldg_164E_x - 10;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = p2.X; p1.Y = bldg_164E_y - bldg_140E_w;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.Y = p1.Y; p2.X = bldg_164E_x + bldg_164E_h;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = p2.X; p1.Y = bldg_168E_y - temp1;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.Y = bldg_168E_y; p2.X = (bldg_168E_x + bldg_172E_x)/2;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = bldg_172E_x - bldg_172E_h;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.X = p1.X; p2.Y = bldg_176E_y ;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = bldg_1624S_x + bldg_1624S_h; p1.Y = p2.Y;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.X = p1.X; p2.Y = bldg_189E_y + bldg_189E_h;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = bldg_125E_x - temp1; p1.Y = p2.Y;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = bldg_124E_x - bldg_124E_w - temp1; p1.Y = bldg_124E_y - bldg_124E_h;
            p2.X = bldg_1710S_x + bldg_1710S_h; p2.Y = bldg_1710S_y - bldg_1713S_w;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = p2.X; p1.Y = bldg_1732S_y - 20;
            e.Graphics.DrawLine(streetPen, p1, p2);
            tRect = new Rectangle(bldg_1732S_x + bldg_1732S_h, bldg_1732S_y - 85, 115, 120);//x, y, w, h
            //e.Graphics.DrawRectangle(streetPen, tRect);
            e.Graphics.DrawArc(streetPen, tRect, 180f, -250f);
            p1.X = bldg_1713S_x - bldg_1713S_h; p1.Y = bldg_1713S_y + bldg_1713S_w;
            p2.X = p1.X + 28; p2.Y = p1.Y + 90;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.X = p1.X; p2.Y = bldg_1713S_y;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = bldg_1714S_x + bldg_1714S_h; p1.Y = p2.Y;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.X = p1.X; p2.Y = bldg_1744S_y + 10;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = p2.X + 50; p1.Y = p2.Y;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.X = p1.X; p2.Y = bldg_1624S_y - bldg_1624S_w;
            e.Graphics.DrawLine(streetPen, p1, p2);
            //draw 1800 S
            p1.X = bldg_202E_x + bldg_202E_h; p1.Y = bldg_202E_y - 25;
            p2.X = p1.X + 100; p2.Y = p1.Y;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.X = p1.X; p2.Y = p1.Y - 50;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = p2.X + 100; p1.Y = p2.Y;
            e.Graphics.DrawLine(streetPen, p1, p2);

            //set up e for 138 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_138E_x, bldg_138E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_138E_h / 5;
            unit_w = bldg_138E_w / 1;
            //draw units
            DrawUnit(0, 0, "138 E", "138 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "134 E", "134 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, 2*unit_h, "130 E", "130 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, 3*unit_h, "126 E", "126 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, 4*unit_h, "122 E", "122 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //set up e for 148 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_148E_x, bldg_148E_y);
            e.Graphics.RotateTransform(-180);
            unit_h = bldg_148E_h / 1;
            unit_w = bldg_148E_w / 1;
            //draw units
            DrawUnit(0, 0, "148 E", "148 E", "1600 S", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_148E_w / 2, bldg_148E_h + 25);
            e.Graphics.DrawString("1600 South", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 140 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_140E_x, bldg_140E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_140E_h / 1;
            unit_w = bldg_140E_w / 1;
            //draw units
            DrawUnit(0, 0, "140 E", "140 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //set up e for 144 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_144E_x, bldg_144E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_144E_h / 1;
            unit_w = bldg_144E_w / 1;
            //draw units
            DrawUnit(0, 0, "144 E", "144 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //set up e for 164 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_164E_x, bldg_164E_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_164E_h / 1;
            unit_w = bldg_164E_w / 1;
            //draw units
            DrawUnit(0, 0, "164 E", "164 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //set up e for 168 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_168E_x, bldg_168E_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_168E_h / 1;
            unit_w = bldg_168E_w / 1;
            //draw units
            DrawUnit(0, 0, "168 E", "168 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //set up e for 172 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_172E_x, bldg_172E_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_172E_h / 1;
            unit_w = bldg_172E_w / 1;
            //draw units
            DrawUnit(0, 0, "172 E", "172 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //set up e for 176 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_176E_x, bldg_176E_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_176E_h / 1;
            unit_w = bldg_176E_w / 1;
            //draw units
            DrawUnit(0, 0, "176 E", "176 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //set up e for 184 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_184E_x, bldg_184E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_184E_h / 1;
            unit_w = bldg_184E_w / 1;
            //draw units
            DrawUnit(0, 0, "184 E", "184 E", "1600 S", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_184E_w / 2, bldg_184E_h + 25);
            e.Graphics.DrawString("1600 South", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 1624 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1624S_x, bldg_1624S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1624S_h / 1;
            unit_w = bldg_1624S_w / 1;
            //draw units
            DrawUnit(0, 0, "1624 S", "1624 S", "200 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1632 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1632S_x, bldg_1632S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1632S_h / 1;
            unit_w = bldg_1632S_w / 1;
            //draw units
            DrawUnit(0, 0, "1632 S", "1632 S", "200 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_1632S_w / 2, bldg_1632S_h + 25);
            e.Graphics.DrawString("200 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 1650 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1650S_x, bldg_1650S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1650S_h / 1;
            unit_w = bldg_1650S_w / 1;
            //draw units
            DrawUnit(0, 0, "1650 S", "1650 S", "200 E", unit_w, unit_h, addressFormat, e);
            p1.X = unit_w / 2;
            p1.Y = unit_h / 2;
            e.Graphics.DrawString("Church", addressFont, Brushes.Black, p1, addressFormat);

            //set up e for 189 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_189E_x, bldg_189E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_189E_h / 1;
            unit_w = bldg_189E_w / 1;
            //draw units
            DrawUnit(0, 0, "189 E", "189 E", "1700 S", unit_w, unit_h, addressFormat, e);

            //set up e for 175 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_175E_x, bldg_175E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_175E_h / 1;
            unit_w = bldg_175E_w / 1;
            //draw units
            DrawUnit(0, 0, "175 E", "175 E", "1700 S", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_175E_w / 2, bldg_175E_h + 25);
            e.Graphics.DrawString("1700 South", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 161 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_161E_x, bldg_161E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_161E_h / 1;
            unit_w = bldg_161E_w / 1;
            //draw units
            DrawUnit(0, 0, "161 E", "161 E", "1700 S", unit_w, unit_h, addressFormat, e);

            //set up e for 147 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_147E_x, bldg_147E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_147E_h / 1;
            unit_w = bldg_147E_w / 1;
            //draw units
            DrawUnit(0, 0, "147 E", "147 E", "1700 S", unit_w, unit_h, addressFormat, e);

            //set up e for 135 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_135E_x, bldg_135E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_135E_h / 1;
            unit_w = bldg_135E_w / 1;
            //draw units
            DrawUnit(0, 0, "135 E", "135 E", "1700 S", unit_w, unit_h, addressFormat, e);

            //set up e for 125 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_125E_x, bldg_125E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_125E_h / 1;
            unit_w = bldg_125E_w / 1;
            //draw units
            DrawUnit(0, 0, "125 E", "125 E", "1700 S", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_125E_w / 2, bldg_125E_h + 25);
            e.Graphics.DrawString("1700 South", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 1714 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1714S_x, bldg_1714S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1714S_h / 1;
            unit_w = bldg_1714S_w / 1;
            //draw units
            DrawUnit(0, 0, "1714 S", "1714 S", "200 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1722 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1722S_x, bldg_1722S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1722S_h / 1;
            unit_w = bldg_1722S_w / 1;
            //draw units
            DrawUnit(0, 0, "1722 S", "1722 S", "200 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_1722S_w / 2, bldg_1722S_h + 25);
            e.Graphics.DrawString("200 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 1734 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1734S_x, bldg_1734S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1734S_h / 1;
            unit_w = bldg_1734S_w / 1;
            //draw units
            DrawUnit(0, 0, "1734 S", "1734 S", "200 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1744 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1744S_x, bldg_1744S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1744S_h / 1;
            unit_w = bldg_1744S_w / 1;
            //draw units
            DrawUnit(0, 0, "1744 S", "1744 S", "200 E", unit_w, unit_h, addressFormat, e);

            //set up e for 180 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_180E_x, bldg_180E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_180E_h / 1;
            unit_w = bldg_180E_w / 1;
            //draw units
            DrawUnit(0, 0, "180 E", "180 E", "1700 S", unit_w, unit_h, addressFormat, e);

            //set up e for 164 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_164E1700S_x, bldg_164E1700S_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_164E1700S_h / 1;
            unit_w = bldg_164E1700S_w / 1;
            //draw units
            DrawUnit(0, 0, "164 E", "164 E", "1700 S", unit_w, unit_h, addressFormat, e);

            //set up e for 1713 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1713S_x, bldg_1713S_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_1713S_h / 1;
            unit_w = bldg_1713S_w / 1;
            //draw units
            DrawUnit(0, 0, "1713 S", "1713 S", "145 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1725 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1725S_x, bldg_1725S_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_1725S_h / 1;
            unit_w = bldg_1725S_w / 1;
            //draw units
            DrawUnit(0, 0, "1725 S", "1725 S", "145 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1731 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1731S_x, bldg_1731S_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_1731S_h / 1;
            unit_w = bldg_1731S_w / 1;
            //draw units
            DrawUnit(0, 0, "1731 S", "1731 S", "145 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1743 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1743S_x, bldg_1743S_y);
            e.Graphics.RotateTransform(150);
            unit_h = bldg_1743S_h / 1;
            unit_w = bldg_1743S_w / 1;
            //draw units
            DrawUnit(0, 0, "1743 S", "1743 S", "145 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1742 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1742S_x, bldg_1742S_y);
            e.Graphics.RotateTransform(-150);
            unit_h = bldg_1742S_h / 1;
            unit_w = bldg_1742S_w / 1;
            //draw units
            DrawUnit(0, 0, "1742 S", "1742 S", "145 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1710 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1710S_x, bldg_1710S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1710S_h / 1;
            unit_w = bldg_1710S_w / 1;
            //draw units
            DrawUnit(0, 0, "1710 S", "1710 S", "145 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_1710S_w / 2 - 30, bldg_1710S_h + 25);
            e.Graphics.DrawString("145 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 1722 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1722S145E_x, bldg_1722S145E_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1722S145E_h / 1;
            unit_w = bldg_1722S145E_w / 1;
            //draw units
            DrawUnit(0, 0, "1722 S", "1722 S", "145 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1732 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1732S_x, bldg_1732S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1732S_h / 1;
            unit_w = bldg_1732S_w / 1;
            //draw units
            DrawUnit(0, 0, "1732 S", "1732 S", "145 E", unit_w, unit_h, addressFormat, e);

            //set up e for 124 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_124E_x, bldg_124E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_124E_h / 1;
            unit_w = bldg_124E_w / 1;
            //draw units
            DrawUnit(0, 0, "124 E", "124 E", "1700 S", unit_w, unit_h, addressFormat, e);

            //set up e for 202 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_202E_x, bldg_202E_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_202E_h / 1;
            unit_w = bldg_202E_w / 1;
            //draw units
            DrawUnit(0, 0, "202 E", "202 E", "1800 S", unit_w, unit_h, addressFormat, e);
            //draw street name
            e.Graphics.RotateTransform(90);
            //p1 = new Point(bldg_202E_w / 2 - 30, bldg_202E_h + 25);
            p1 = new Point(bldg_202E_h + 50, -bldg_202E_w / 2);
            e.Graphics.DrawString("1800 S", streetFont, Brushes.Black, p1, addressFormat);
        
        }

        private void Draw100EastTextAndStuff(PrintPageEventArgs e)
        {
            Point upperLeftPoint = new Point();
            String textToPrint;
            SizeF tempSize = new SizeF();
            e.Graphics.ResetTransform();
            textToPrint = "100 East";
            tempSize = e.Graphics.MeasureString(textToPrint, pageTitleFont);
            //center text
            upperLeftPoint.X = e.PageBounds.Width / 2 - (int)tempSize.Width / 2;
            upperLeftPoint.X = upperLeftPoint.X + 200; // uncenter text for looks
            upperLeftPoint.Y = 400;
            DrawTitleAndStuff(textToPrint, upperLeftPoint, e);
            //drawCompass
            drawCompass(e, new Point(750, 200));
            drawNotOnWardListLegend(e);
        }

        private void DrawTitleAndStuff(string textToPrint, Point upperLeftPoint, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(textToPrint, pageTitleFont, Brushes.Black, upperLeftPoint);
        }

        private void DrawSandyBrookTextAndStuff(PrintPageEventArgs e)
        {
            Point upperLeftPoint = new Point();
            String textToPrint;
            SizeF tempSize = new SizeF();
            e.Graphics.ResetTransform();
            textToPrint = "Sandy Brooke";
            tempSize = e.Graphics.MeasureString(textToPrint, pageTitleFont);
            //center text
            upperLeftPoint.X = e.PageBounds.Width / 2 - (int)tempSize.Width / 2;
            upperLeftPoint.X = upperLeftPoint.X - 185; // uncenter text for looks
            upperLeftPoint.Y = 150;
            DrawTitleAndStuff(textToPrint, upperLeftPoint, e);
            //drawCompass
            drawCompass(e, new Point(750, 200));
            drawNotOnWardListLegend(e);
        }

        private void DrawChurchArea(PrintPageEventArgs e)
        {
            Rectangle propertyRect = new Rectangle();
            Rectangle addressRect = new Rectangle();
            Rectangle nameRect = new Rectangle();
            Rectangle tRect = new Rectangle();
            StringFormat addressFormat = new StringFormat();
            Point p1 = new Point();
            Point p2 = new Point();
            string tempString;
            string[] addressStringsToFind = new string[2];
            string[] stringsToWrite;
            SizeF stringSize = new SizeF();
            int unit_h;
            int unit_w;
            int defaultUnit_h = 60;
            int defaultUnit_w = 99;
            int temp1;
            //////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////
            int bldg_187E_x = 650;
            int bldg_187E_y = 70;
            int bldg_187E_w = 70;// helps if divisible by number units across
            int bldg_187E_h = 60; // helps if divisible by number units high

            int bldg_175E_x = 520;
            int bldg_175E_y = 60;
            int bldg_175E_w = 70;// helps if divisible by number units across
            int bldg_175E_h = 60; // helps if divisible by number units high

            int bldg_155E_x = 435;
            int bldg_155E_y = 80;
            int bldg_155E_w = 70;// helps if divisible by number units across
            int bldg_155E_h = 60; // helps if divisible by number units high

            int bldg_186E_x = 700;
            int bldg_186E_y = 215;
            int bldg_186E_w = 70;// helps if divisible by number units across
            int bldg_186E_h = 60; // helps if divisible by number units high

            int bldg_174E_x = 590;
            int bldg_174E_y = 275;
            int bldg_174E_w = 70;// helps if divisible by number units across
            int bldg_174E_h = 60; // helps if divisible by number units high

            int bldg_1845S_x = 505;
            int bldg_1845S_y = 190;
            int bldg_1845S_w = 70;// helps if divisible by number units across
            int bldg_1845S_h = 60; // helps if divisible by number units high

            int bldg_1853S_x = 505;
            int bldg_1853S_y = 275;
            int bldg_1853S_w = 70;// helps if divisible by number units across
            int bldg_1853S_h = 60; // helps if divisible by number units high

            int bldg_173E_x = 520;
            int bldg_173E_y = 285;
            int bldg_173E_w = 70;// helps if divisible by number units across
            int bldg_173E_h = 60; // helps if divisible by number units high

            int bldg_189E_x = 605;
            int bldg_189E_y = 285;
            int bldg_189E_w = 70;// helps if divisible by number units across
            int bldg_189E_h = 60; // helps if divisible by number units high

            int bldg_156E_x = 515;
            int bldg_156E_y = 455;
            int bldg_156E_w = 70;// helps if divisible by number units across
            int bldg_156E_h = 60; // helps if divisible by number units high

            int bldg_172E_x = 600;
            int bldg_172E_y = 455;
            int bldg_172E_w = 70;// helps if divisible by number units across
            int bldg_172E_h = 60; // helps if divisible by number units high

            int bldg_188E_x = 685;
            int bldg_188E_y = 455;
            int bldg_188E_w = 70;// helps if divisible by number units across
            int bldg_188E_h = 60; // helps if divisible by number units high

            int bldg_161E_x = 445;
            int bldg_161E_y = 480;
            int bldg_161E_w = 70;// helps if divisible by number units across
            int bldg_161E_h = 60; // helps if divisible by number units high

            int bldg_171E_x = 530;
            int bldg_171E_y = 470;
            int bldg_171E_w = 70;// helps if divisible by number units across
            int bldg_171E_h = 60; // helps if divisible by number units high

            int bldg_179E_x = 660;
            int bldg_179E_y = 470;
            int bldg_179E_w = 70;// helps if divisible by number units across
            int bldg_179E_h = 60; // helps if divisible by number units high

            int bldg_178E_x = 705;
            int bldg_178E_y = 620;
            int bldg_178E_w = 70;// helps if divisible by number units across
            int bldg_178E_h = 60; // helps if divisible by number units high

            int bldg_160E_x = 560;
            int bldg_160E_y = 650;
            int bldg_160E_w = 115;// helps if divisible by number units across
            int bldg_160E_h = 60; // helps if divisible by number units high

            int bldg_159E_x = 445;
            int bldg_159E_y = 675;
            int bldg_159E_w = 70;// helps if divisible by number units across
            int bldg_159E_h = 60; // helps if divisible by number units high

            int bldg_169E_x = 530;
            int bldg_169E_y = 665;
            int bldg_169E_w = 70;// helps if divisible by number units across
            int bldg_169E_h = 60; // helps if divisible by number units high

            int bldg_177E_x = 665;
            int bldg_177E_y = 670;
            int bldg_177E_w = 70;// helps if divisible by number units across
            int bldg_177E_h = 60; // helps if divisible by number units high

            int bldg_176E_x = 705;
            int bldg_176E_y = 815;
            int bldg_176E_w = 70;// helps if divisible by number units across
            int bldg_176E_h = 60; // helps if divisible by number units high

            int bldg_158E_x = 560;
            int bldg_158E_y = 845;
            int bldg_158E_w = 115;// helps if divisible by number units across
            int bldg_158E_h = 60; // helps if divisible by number units high

            int bldg_142E_x = 420;
            int bldg_142E_y = 910;
            int bldg_142E_w = 70;// helps if divisible by number units across
            int bldg_142E_h = 60; // helps if divisible by number units high

            int bldg_140E_x = 420;
            int bldg_140E_y = 980;
            int bldg_140E_w = 70;// helps if divisible by number units across
            int bldg_140E_h = 60; // helps if divisible by number units high

            int bldg_136E_x = 360;
            int bldg_136E_y = 1050;
            int bldg_136E_w = 70;// helps if divisible by number units across
            int bldg_136E_h = 60; // helps if divisible by number units high

            int bldg_116E_x = 290;
            int bldg_116E_y = 1040;
            int bldg_116E_w = 70;// helps if divisible by number units across
            int bldg_116E_h = 60; // helps if divisible by number units high

            int bldg_120E_x = 330;
            int bldg_120E_y = 970;
            int bldg_120E_w = 70;// helps if divisible by number units across
            int bldg_120E_h = 60; // helps if divisible by number units high

            int bldg_84E_x = 260;
            int bldg_84E_y = 970;
            int bldg_84E_w = 70;// helps if divisible by number units across
            int bldg_84E_h = 60; // helps if divisible by number units high

            int bldg_66E_x = 190;
            int bldg_66E_y = 970;
            int bldg_66E_w = 70;// helps if divisible by number units across
            int bldg_66E_h = 60; // helps if divisible by number units high

            int bldg_46E_x = 120;
            int bldg_46E_y = 970;
            int bldg_46E_w = 70;// helps if divisible by number units across
            int bldg_46E_h = 60; // helps if divisible by number units high

            int bldg_1826S_x = 245;
            int bldg_1826S_y = 210;
            int bldg_1826S_w = 105;// helps if divisible by number units across
            int bldg_1826S_h = 150; // helps if divisible by number units high

            int bldg_1850S_x = 245;
            int bldg_1850S_y = 315;
            int bldg_1850S_w = 105;// helps if divisible by number units across
            int bldg_1850S_h = 150; // helps if divisible by number units high

            int bldg_1876S_x = 245;
            int bldg_1876S_y = 420;
            int bldg_1876S_w = 105;// helps if divisible by number units across
            int bldg_1876S_h = 150; // helps if divisible by number units high

            int bldg_1920S_x = 245;
            int bldg_1920S_y = 525;
            int bldg_1920S_w = 105;// helps if divisible by number units across
            int bldg_1920S_h = 150; // helps if divisible by number units high

            int bldg_135E_x = 245; //church
            int bldg_135E_y = 525;
            int bldg_135E_w = 150;// helps if divisible by number units across
            int bldg_135E_h = bldg_142E_y - 50 - bldg_1920S_y; // helps if divisible by number units high

            addressFormat.Alignment = StringAlignment.Center;
            addressFormat.LineAlignment = StringAlignment.Center;
            //Draw Roads
            temp1 = 40;
            e.Graphics.ResetTransform();
            tRect = new Rectangle(bldg_155E_x +bldg_155E_w, bldg_155E_y + bldg_174E_h -20, 120, 95);//x, y, w, h
            //e.Graphics.DrawRectangle(streetPen, tRect);
            e.Graphics.DrawArc(streetPen, tRect, -150f, 307f);
            p1 = new Point(bldg_1845S_x - bldg_1845S_h - 50, bldg_155E_y + bldg_155E_h);
            p2 = new Point(bldg_175E_x-2, bldg_155E_y + bldg_155E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(bldg_1845S_x - bldg_1845S_h, bldg_1845S_y);
            p2 = new Point(bldg_174E_x-bldg_174E_w-6, bldg_1845S_y);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2 = new Point(bldg_1853S_x-bldg_1853S_h, bldg_1853S_y+bldg_1853S_w);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(bldg_189E_x + bldg_189E_w+temp1, bldg_189E_y + bldg_189E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);

            p1 = new Point(bldg_1845S_x-bldg_1845S_h-50, bldg_155E_y + bldg_155E_h);
            p2 = new Point(p1.X, bldg_142E_y - 50);
            e.Graphics.DrawLine(streetPen, p1, p2);

            p1 = new Point(bldg_188E_x+temp1, bldg_188E_y - bldg_188E_h);
            p2 = new Point(bldg_156E_x-bldg_156E_w, bldg_156E_y-bldg_156E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(bldg_161E_x, bldg_161E_y + bldg_161E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2 = new Point(bldg_161E_x + bldg_161E_w + 67, bldg_161E_y + bldg_161E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            tRect = new Rectangle(bldg_171E_x + bldg_171E_w -40, bldg_171E_y  + 65, 85, 70);//x, y, w, h
            //e.Graphics.DrawRectangle(streetPen, tRect);
            e.Graphics.DrawArc(streetPen, tRect, -125f, 277f);
            p1 = new Point(bldg_160E_x - bldg_160E_w, bldg_160E_y - bldg_160E_h);
            p2 = new Point(bldg_160E_x + 7, bldg_160E_y - bldg_160E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2 = new Point(bldg_159E_x, bldg_159E_y + bldg_160E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(bldg_159E_x+bldg_159E_w+67, bldg_159E_y + bldg_160E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            tRect = new Rectangle(bldg_169E_x + bldg_169E_w - 40, bldg_169E_y + 65, 85, 70);//x, y, w, h
            //e.Graphics.DrawRectangle(streetPen, tRect);
            e.Graphics.DrawArc(streetPen, tRect, -125f, 277f);
            p1 = new Point(bldg_158E_x - bldg_158E_w, bldg_158E_y - bldg_158E_h);
            p2 = new Point(bldg_158E_x + 7, bldg_158E_y - bldg_158E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2 = new Point(p1.X,bldg_142E_y-50);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(p2.X+ 50, p2.Y);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.Y = bldg_142E_y; p2.X = bldg_142E_x - bldg_142E_h;
            p1.Y = p2.Y;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = p2.X; p1.Y = bldg_136E_y - bldg_140E_h;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.Y = p1.Y; p2.X = bldg_116E_x;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = p2.X; p1.Y = bldg_120E_y +5;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.Y = p1.Y; p2.X = bldg_120E_x;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1.X = p2.X; p1.Y = bldg_120E_y - bldg_120E_h;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.Y = p1.Y; p2.X = bldg_46E_x-bldg_46E_w-20;
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2.Y = p2.Y-50;
            p1.X = bldg_158E_x-bldg_158E_w-50; p1.Y = p2.Y;
            e.Graphics.DrawLine(streetPen, p1, p2);

            //set up e for 187 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_187E_x, bldg_187E_y);
            e.Graphics.RotateTransform(45);
            unit_h = bldg_187E_h / 1; unit_w = bldg_187E_w / 1;
            //draw units
            DrawUnit(0, 0, "187 E", "187 E", "1825 S", unit_w, unit_h, addressFormat, e);

            //set up e for 175 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_175E_x, bldg_175E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_175E_h / 1; unit_w = bldg_175E_w / 1;
            //draw units
            DrawUnit(0, 0, "175 E", "175 E", "1825 S", unit_w, unit_h, addressFormat, e);

            //set up e for 155 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_155E_x, bldg_155E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_155E_h / 1; unit_w = bldg_155E_w / 1;
            //draw units
            DrawUnit(0, 0, "155 E", "155 E", "1825 S", unit_w, unit_h, addressFormat, e);
           
            //draw street name
            p1 = new Point(bldg_155E_w / 2+50, bldg_155E_h + 25);
            e.Graphics.DrawString("1825 South", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 186 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_186E_x, bldg_186E_y);
            e.Graphics.RotateTransform(135);
            unit_h = bldg_186E_h / 1; unit_w = bldg_186E_w / 1;
            //draw units
            DrawUnit(0, 0, "186 E", "186 E", "1825 S", unit_w, unit_h, addressFormat, e);

            //set up e for 174 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_174E_x, bldg_174E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_174E_h / 1; unit_w = bldg_174E_w / 1;
            //draw units
            DrawUnit(0, 0, "174 E", "174 E", "1825 S", unit_w, unit_h, addressFormat, e);

            //set up e for 1845 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1845S_x, bldg_1845S_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_1845S_h / 1; unit_w = bldg_1845S_w / 1;
            //draw units
            DrawUnit(0, 0, "1845 S", "1845 S", "140 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1853 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1853S_x, bldg_1853S_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_1853S_h / 1; unit_w = bldg_1853S_w / 1;
            //draw units
            DrawUnit(0, 0, "1853 S", "1853 S", "140 E", unit_w, unit_h, addressFormat, e);

            //set up e for 173 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_173E_x, bldg_173E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_173E_h / 1; unit_w = bldg_173E_w / 1;
            //draw units
            DrawUnit(0, 0, "173 E", "173 E", "1864 S", unit_w, unit_h, addressFormat, e);

            //draw street name
            p1 = new Point(bldg_173E_w / 2, bldg_173E_h + 25);
            e.Graphics.DrawString("1864 South", streetFont, Brushes.Black, p1, addressFormat);


            //set up e for 189 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_189E_x, bldg_189E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_189E_h / 1; unit_w = bldg_189E_w / 1;
            //draw units
            DrawUnit(0, 0, "189 E", "189 E", "1864 S", unit_w, unit_h, addressFormat, e);

            //set up e for 156 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_156E_x, bldg_156E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_156E_h / 1; unit_w = bldg_156E_w / 1;
            //draw units
            DrawUnit(0, 0, "156 E", "156 E", "1864 S", unit_w, unit_h, addressFormat, e);

            //set up e for 172 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_172E_x, bldg_172E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_172E_h / 1; unit_w = bldg_172E_w / 1;
            //draw units
            DrawUnit(0, 0, "172 E", "172 E", "1864 S", unit_w, unit_h, addressFormat, e);

            //set up e for 188 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_188E_x, bldg_188E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_188E_h / 1; unit_w = bldg_188E_w / 1;
            //draw units
            DrawUnit(0, 0, "188 E", "188 E", "1864 S", unit_w, unit_h, addressFormat, e);

            //set up e for 161 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_161E_x, bldg_161E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_161E_h / 1; unit_w = bldg_161E_w / 1;
            //draw units
            DrawUnit(0, 0, "161 E", "161 E", "1910 S", unit_w, unit_h, addressFormat, e);

            //draw street name
            p1 = new Point(bldg_161E_w / 2 + 50, bldg_161E_h + 25);
            e.Graphics.DrawString("1910 South", streetFont, Brushes.Black, p1, addressFormat);
            
            //set up e for 171 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_171E_x, bldg_171E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_171E_h / 1; unit_w = bldg_171E_w / 1;
            //draw units
            DrawUnit(0, 0, "171 E", "171 E", "1910 S", unit_w, unit_h, addressFormat, e);

            //set up e for 179 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_179E_x, bldg_179E_y);
            e.Graphics.RotateTransform(45);
            unit_h = bldg_179E_h / 1; unit_w = bldg_179E_w / 1;
            //draw units
            DrawUnit(0, 0, "179 E", "179 E", "1910 S", unit_w, unit_h, addressFormat, e);

            //set up e for 178 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_178E_x, bldg_178E_y);
            e.Graphics.RotateTransform(135);
            unit_h = bldg_178E_h / 1; unit_w = bldg_178E_w / 1;
            //draw units
            DrawUnit(0, 0, "178 E", "178 E", "1910 S", unit_w, unit_h, addressFormat, e);

            //set up e for 160 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_160E_x, bldg_160E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_160E_h / 1; unit_w = bldg_160E_w / 1;
            //draw units
            DrawUnit(0, 0, "160 E", "160 E", "1910 S", unit_w, unit_h, addressFormat, e);

            //set up e for 159 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_159E_x, bldg_159E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_159E_h / 1; unit_w = bldg_159E_w / 1;
            //draw units
            DrawUnit(0, 0, "159 E", "159 E", "1950 S", unit_w, unit_h, addressFormat, e);
            //draw street name

            p1 = new Point(bldg_159E_w / 2 + 50, bldg_159E_h + 25);
            e.Graphics.DrawString("1950 South", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 169 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_169E_x, bldg_169E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_169E_h / 1; unit_w = bldg_169E_w / 1;
            //draw units
            DrawUnit(0, 0, "169 E", "169 E", "1950 S", unit_w, unit_h, addressFormat, e);

            //set up e for 177 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_177E_x, bldg_177E_y);
            e.Graphics.RotateTransform(45);
            unit_h = bldg_177E_h / 1; unit_w = bldg_177E_w / 1;
            //draw units
            DrawUnit(0, 0, "177 E", "177 E", "1950 S", unit_w, unit_h, addressFormat, e);

            //set up e for 176 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_176E_x, bldg_176E_y);
            e.Graphics.RotateTransform(135);
            unit_h = bldg_176E_h / 1; unit_w = bldg_176E_w / 1;
            //draw units
            DrawUnit(0, 0, "176 E", "176 E", "1950 S", unit_w, unit_h, addressFormat, e);

            //set up e for 158 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_158E_x, bldg_158E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_158E_h / 1; unit_w = bldg_158E_w / 1;
            //draw units
            DrawUnit(0, 0, "158 E", "158 E", "1950 S", unit_w, unit_h, addressFormat, e);

            //set up e for 140 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_140E_x, bldg_140E_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_140E_h / 1; unit_w = bldg_140E_w / 1;
            //draw units
            DrawUnit(0, 0, "140 E", "140 E", "2000 S", unit_w, unit_h, addressFormat, e);

            //set up e for 142 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_142E_x, bldg_142E_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_142E_h / 1; unit_w = bldg_142E_w / 1;
            //draw units
            DrawUnit(0, 0, "142 E", "142 E", "2000 S", unit_w, unit_h, addressFormat, e);

            //set up e for 136 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_136E_x, bldg_136E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_136E_h / 1; unit_w = bldg_136E_w / 1;
            //draw units
            DrawUnit(0, 0, "136 E", "136 E", "2000 S", unit_w, unit_h, addressFormat, e);

            //set up e for 116 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_116E_x, bldg_116E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_116E_h / 1; unit_w = bldg_116E_w / 1;
            //draw units
            DrawUnit(0, 0, "116 E", "116 E", "2000 S", unit_w, unit_h, addressFormat, e);

            //set up e for 120 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_120E_x, bldg_120E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_120E_h / 1; unit_w = bldg_120E_w / 1;
            //draw units
            DrawUnit(0, 0, "120 E", "120 E", "2000 S", unit_w, unit_h, addressFormat, e);

            //set up e for 84 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_84E_x, bldg_84E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_84E_h / 1; unit_w = bldg_84E_w / 1;
            //draw units
            DrawUnit(0, 0, "84 E", "84 E", "2000 S", unit_w, unit_h, addressFormat, e);

            //set up e for 66 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_66E_x, bldg_66E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_66E_h / 1; unit_w = bldg_66E_w / 1;
            //draw units
            DrawUnit(0, 0, "66 E", "66 E", "2000 S", unit_w, unit_h, addressFormat, e);

            //set up e for 46 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_46E_x, bldg_46E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_46E_h / 1; unit_w = bldg_46E_w / 1;
            //draw units
            DrawUnit(0, 0, "46 E", "46 E", "2000 S", unit_w, unit_h, addressFormat, e);

            //set up e for 1826 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1826S_x, bldg_1826S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1826S_h / 1; unit_w = bldg_1826S_w / 1;
            //draw units
            DrawUnit(0, 0, "1826 S", "1826 S", "140 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1850 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1850S_x, bldg_1850S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1850S_h / 1; unit_w = bldg_1850S_w / 1;
            //draw units
            DrawUnit(0, 0, "1850 S", "1850 S", "140 E", unit_w, unit_h, addressFormat, e);

            //draw street name
            p1 = new Point(bldg_1850S_w / 2, bldg_1850S_h + 25);
            e.Graphics.DrawString("140 East", streetFont, Brushes.Black, p1, addressFormat);


            //set up e for 1876 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1876S_x, bldg_1876S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1876S_h / 1; unit_w = bldg_1876S_w / 1;
            //draw units
            DrawUnit(0, 0, "1876 S", "1876 S", "140 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1920 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1920S_x, bldg_1920S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1920S_h / 1; unit_w = bldg_1920S_w / 1;
            //draw units
            DrawUnit(0, 0, "1920 S", "1920 S", "140 E", unit_w, unit_h, addressFormat, e);

            //draw street name
            p1 = new Point(bldg_1920S_w / 2 - 200, bldg_1920S_h + 25);
            e.Graphics.DrawString("140 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 135 E Bldg -- Church
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_135E_x, bldg_135E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_135E_h / 1; unit_w = bldg_135E_w / 1;
            //draw units
            DrawUnit(0, 0, "135 E", "135x E", "2000 S", unit_w, unit_h, addressFormat, e);
            p1.X = unit_w / 2;
            p1.Y = unit_h / 2;
            e.Graphics.DrawString("Church", addressFont, Brushes.Black, p1, addressFormat);
            //draw street name
            p1 = new Point(bldg_135E_w / 2-100, bldg_135E_h + 25);
            e.Graphics.DrawString("2000 South", streetFont, Brushes.Black, p1, addressFormat);

        }


        private void Draw_100E(PrintPageEventArgs e)
        {
            Rectangle propertyRect = new Rectangle();
            Rectangle addressRect = new Rectangle();
            Rectangle nameRect = new Rectangle();
            StringFormat addressFormat = new StringFormat();
            Point p1 = new Point();
            Point p2 = new Point();
            Point p3 = new Point();
            Point p4 = new Point();
            Point p5 = new Point();
            Point p6 = new Point();
            Point p7 = new Point();
            Point p8 = new Point();
            Point[] points = new Point[10];
            string tempString;
            string[] addressStringsToFind = new string[2];
            string[] stringsToWrite;
            SizeF stringSize = new SizeF();
            int unit_h;
            int unit_w;
            int defaultUnit_h = 60;
            int defaultUnit_w = 99;
            int temp1;
            int temp2;
            int temp3;
            int temp4;
  
            int bldg_1480_x = 50;
            int bldg_1480_y = 130;
            int bldg_1480_w = 60;// helps if divisible by number units across
            int bldg_1480_h = 100; // helps if divisible by number units high

            int bldg_1490_x = 100;
            int bldg_1490_y = 205;
            int bldg_1490_w = 60;// helps if divisible by number units across
            int bldg_1490_h = 90; // helps if divisible by number units high

            int bldg_85E_x = 40;
            int bldg_85E_y = 145;
            int bldg_85E_w = 50;// helps if divisible by number units across
            int bldg_85E_h = 60; // helps if divisible by number units high

            int bldg_98E_x = 170;
            int bldg_98E_y = 315;
            int bldg_98E_w = 120;// helps if divisible by number units across
            int bldg_98E_h = 60; // helps if divisible by number units high

            int bldg_ParkAveTown_x = 500;
            int bldg_ParkAveTown_y = 315;
            int bldg_ParkAveTown_w = 280;// helps if divisible by number units across
            int bldg_ParkAveTown_h = 60; // helps if divisible by number units high

            int bldg_1533S_x = 340;
            int bldg_1533S_y = 330;
            int bldg_1533S_w = 140;// helps if divisible by number units across
            int bldg_1533S_h = 120; // helps if divisible by number units high

            int bldg_1551S_x = 340;
            int bldg_1551S_y = 485;
            int bldg_1551S_w = 140;// helps if divisible by number units across
            int bldg_1551S_h = 120; // helps if divisible by number units high

            int bldg_1563S_x = 340;
            int bldg_1563S_y = 640;
            int bldg_1563S_w = 140;// helps if divisible by number units across
            int bldg_1563S_h = 120; // helps if divisible by number units high

            int bldg_1575S_x = 340;
            int bldg_1575S_y = 795;
            int bldg_1575S_w = 70;// helps if divisible by number units across
            int bldg_1575S_h = 120; // helps if divisible by number units high

            int bldg_1585S_x = 280;
            int bldg_1585S_y = 880;
            int bldg_1585S_w = 70;// helps if divisible by number units across
            int bldg_1585S_h = 60; // helps if divisible by number units high

            int bldg_123E_x = 220;
            int bldg_123E_y = 965;
            int bldg_123E_w = 60;// helps if divisible by number units across
            int bldg_123E_h = 60; // helps if divisible by number units high

            int bldg_137E_x = 295;
            int bldg_137E_y = 905;
            int bldg_137E_w = 60;// helps if divisible by number units across
            int bldg_137E_h = 120; // helps if divisible by number units high

            int bldg_145E_x = 370;
            int bldg_145E_y = 905;
            int bldg_145E_w = 60;// helps if divisible by number units across
            int bldg_145E_h = 120; // helps if divisible by number units high

            int bldg_173E_x = 470;
            int bldg_173E_y = 905;
            int bldg_173E_w = 120;// helps if divisible by number units across
            int bldg_173E_h = 120; // helps if divisible by number units high

            int bldg_183E_x = 605;
            int bldg_183E_y = 905;
            int bldg_183E_w = 120;// helps if divisible by number units across
            int bldg_183E_h = 120; // helps if divisible by number units high

            int bldg_195E_x = 740;
            int bldg_195E_y = 965;
            int bldg_195E_w = 60;// helps if divisible by number units across
            int bldg_195E_h = 60; // helps if divisible by number units high

            int bldg_1584S_x = 110;
            int bldg_1584S_y = 1025;
            int bldg_1584S_w = 70;// helps if divisible by number units across
            int bldg_1584S_h = 60; // helps if divisible by number units high

            int bldg_1576S_x = 110;
            int bldg_1576S_y = 935;
            int bldg_1576S_w = 70;// helps if divisible by number units across
            int bldg_1576S_h = 60; // helps if divisible by number units high

            int bldg_1564S_x = 110;
            int bldg_1564S_y = 845;
            int bldg_1564S_w = 70;// helps if divisible by number units across
            int bldg_1564S_h = 60; // helps if divisible by number units high

            int bldg_1546S_x = 110;
            int bldg_1546S_y = 755;
            int bldg_1546S_w = 70;// helps if divisible by number units across
            int bldg_1546S_h = 60; // helps if divisible by number units high

            int bldg_1538S_x = 50;
            int bldg_1538S_y = 665;
            int bldg_1538S_w = 140;// helps if divisible by number units across
            int bldg_1538S_h = 120; // helps if divisible by number units high

            int bldg_1530S_x = 110;
            int bldg_1530S_y = 505;
            int bldg_1530S_w = 70;// helps if divisible by number units across
            int bldg_1530S_h = 60; // helps if divisible by number units high

            int bldg_1520S_x = 110;
            int bldg_1520S_y = 415;
            int bldg_1520S_w = 70;// helps if divisible by number units across
            int bldg_1520S_h = 60; // helps if divisible by number units high

            addressFormat.Alignment = StringAlignment.Center;
            addressFormat.LineAlignment = StringAlignment.Center;
            
            //draw roads
            //1500 S
            temp4 = 40; 
            e.Graphics.ResetTransform();
            p1 = new Point(bldg_85E_x-20, bldg_85E_y + bldg_98E_h);
            p2 = new Point(bldg_1490_x + bldg_1490_h, bldg_1490_y);
            e.Graphics.DrawLine(streetPen, p1, p2);
            temp1 = 10;
            p1 = new Point(bldg_1490_x + bldg_1490_h, bldg_1490_y - bldg_1490_w - temp1);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2 = new Point(bldg_1490_x + bldg_1490_h, bldg_1480_y - bldg_1480_w - temp1);
            temp2 = 50;
            p3 = new Point(bldg_1490_x + bldg_1490_h +temp2 , bldg_1480_y - bldg_1480_w - temp1);
            p4 = new Point(bldg_1490_x + bldg_1490_h +temp2 , bldg_1490_y - bldg_1490_w - temp1);
            temp3 = 15;
            p5.X = p1.X - temp3;
            p5.Y = (p1.Y + p2.Y) / 2;
            p6.X = p3.X + temp3;
            p6.Y = (p3.Y + p4.Y) / 2;
            p7.X = (p2.X + p3.X) / 2;
            p7.Y = p2.Y - temp3;
            //points = new Point[] { p1, p5, p2, p7, p3, p6, p4 };
            //e.Graphics.DrawCurve(streetPen, points);
            points = new Point[] { p1, p5, p2,};
            e.Graphics.DrawCurve(streetPen, points);
            points = new Point[] { p3, p6, p4 };
            e.Graphics.DrawCurve(streetPen, points);
            p8 = new Point(p2.X, p2.Y - 20);
            e.Graphics.DrawLine(streetPen, p2, p8);
            p8 = new Point(p3.X, p3.Y - 20);
            e.Graphics.DrawLine(streetPen, p3, p8);
            
            p1 = new Point(bldg_1490_x + bldg_1490_h + temp2, bldg_1490_y);
            e.Graphics.DrawLine(streetPen, p4, p1);
            p2 = new Point(p1.X + bldg_ParkAveTown_w, p1.Y);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(p2.X, bldg_ParkAveTown_y - bldg_ParkAveTown_h);
            p2 = new Point(bldg_ParkAveTown_x - bldg_ParkAveTown_w, p1.Y);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(p2.X, bldg_123E_y + bldg_123E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2 = new Point(bldg_145E_x + bldg_145E_w, p1.Y);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(p2.X, p2.Y - bldg_145E_h - temp4);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2 = new Point(bldg_173E_x, p1.Y);
            p1 = new Point(bldg_173E_x, bldg_173E_y + bldg_173E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p2 = new Point(bldg_195E_x + bldg_195E_w, p1.Y);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(p2.X, p2.Y - bldg_195E_h - temp4);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(bldg_1584S_x + bldg_1584S_h, p2.Y);
            p2 = new Point(p1.X, bldg_98E_y - bldg_98E_h);
            e.Graphics.DrawLine(streetPen, p1, p2);
            p1 = new Point(bldg_98E_x - bldg_98E_w - 30, p2.Y);
            e.Graphics.DrawLine(streetPen, p1, p2);
            
            //set up e for 1480 Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1480_x, bldg_1480_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1480_h / 2;
            unit_w = bldg_1480_w / 1;
            //draw units
            DrawUnit(0, 0, "1482 S", "1482 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1480 S", "1480 S", "100 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1490 bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1490_x, bldg_1490_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1490_h / 2;
            unit_w = bldg_1490_w / 1;
            //draw units
            DrawUnit(0, 0, "1492 S", "1492 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1490 S", "1490 S", "100 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_1490_w / 2, bldg_1490_h + 20);
            e.Graphics.DrawString("100 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 85 E house
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_85E_x, bldg_85E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_85E_h / 1;
            unit_w = bldg_85E_w / 1;
            //draw units
            DrawUnit(0, 0, "85 E", "85 E", "1500 S", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_85E_w / 2 + 50, bldg_85E_h + 25);
            e.Graphics.DrawString("1500 South", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 98 E house
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_98E_x, bldg_98E_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_98E_h / 1;
            unit_w = bldg_98E_w / 2;
            //draw units
            DrawUnit(0, 0, "98 E", "98 E", "1500 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "96 E", "96 E", "1500 S", unit_w, unit_h, addressFormat, e);

            //set up e for Park Avenue townhomes
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_ParkAveTown_x, bldg_ParkAveTown_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_ParkAveTown_h / 1;
            unit_w = bldg_ParkAveTown_w / 4;
            //draw units
            DrawUnit(0, 0, "120 E", "120 E", "1500 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "116 E", "116 E", "1500 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(2*unit_w, 0, "112 E", "112 E", "1500 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(3*unit_w, 0, "108 E", "108 E", "1500 S", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_ParkAveTown_w / 2, bldg_ParkAveTown_h + 25);
            e.Graphics.DrawString("1500 South", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 1533 S 4-plex
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1533S_x, bldg_1533S_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_1533S_h / 2;
            unit_w = bldg_1533S_w / 2;
            //draw units
            DrawUnit(0, 0, "1533 S", "1533 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1537 S", "1537 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1531 S", "1531 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1535 S", "1535 S", "100 E", unit_w, unit_h, addressFormat, e);
            
            //set up e for 1551 S 4-plex
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1551S_x, bldg_1551S_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_1551S_h / 2;
            unit_w = bldg_1551S_w / 2;
            //draw units
            DrawUnit(0, 0, "1551 S", "1551 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1555 S", "1555 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1549 S", "1549 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1553 S", "1553 S", "100 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1563 S 4-plex
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1563S_x, bldg_1563S_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_1563S_h / 2;
            unit_w = bldg_1563S_w / 2;
            //draw units
            DrawUnit(0, 0, "1563 S", "1563 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1567 S", "1567 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1561 S", "1561 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1565 S", "1565 S", "100 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1575 bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1575S_x, bldg_1575S_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_1575S_h / 2;
            unit_w = bldg_1575S_w / 1;
            //draw units
            DrawUnit(0, 0, "1575 S", "1575 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1573 S", "1573 S", "100 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1585 bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1585S_x, bldg_1585S_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_1585S_h / 1;
            unit_w = bldg_1585S_w / 1;
            //draw units
            DrawUnit(0, 0, "1585 S", "1585 S", "100 E", unit_w, unit_h, addressFormat, e);

            //set up e for 123 E bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_123E_x, bldg_123E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_123E_h / 1;
            unit_w = bldg_123E_w / 1;
            //draw units
            DrawUnit(0, 0, "123 E", "123 E", "1600 S", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_123E_w / 2, bldg_123E_h + 25);
            e.Graphics.DrawString("1600 South", streetFont, Brushes.Black, p1, addressFormat);
         
            //set up e for 137 E bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_137E_x, bldg_137E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_137E_h / 2;
            unit_w = bldg_137E_w / 1;
            //draw units
            DrawUnit(0, 0, "137 E", "137 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "135 E", "135 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //set up e for 145 E bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_145E_x, bldg_145E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_145E_h / 2;
            unit_w = bldg_145E_w / 1;
            //draw units
            DrawUnit(0, 0, "145 E", "145 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "147 E", "147 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //draw street name
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_145E_x, bldg_145E_y);
            e.Graphics.RotateTransform(-90);
            p1 = new Point(-bldg_145E_h / 2, bldg_145E_w+20);
            e.Graphics.DrawString("140 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 173 E 4-plex
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_173E_x, bldg_173E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_173E_h / 2;
            unit_w = bldg_173E_w / 2;
            //draw units
            DrawUnit(0, 0, "173 E", "173 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "177 E", "177 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "171 E", "171 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "175 E", "175 E", "1600 S", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_173E_w / 2, bldg_173E_h + 25);
            e.Graphics.DrawString("1600 South", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 183 E 4-plex
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_183E_x, bldg_183E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_183E_h / 2;
            unit_w = bldg_183E_w / 2;
            //draw units
            DrawUnit(0, 0, "183 E", "183 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "187 E", "187 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "181 E", "181 E", "1600 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "185 E", "185 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //set up e for 195 E 
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_195E_x, bldg_195E_y);
            e.Graphics.RotateTransform(0);
            unit_h = bldg_195E_h / 1;
            unit_w = bldg_195E_w / 1;
            //draw units
            DrawUnit(0, 0, "195 E", "195 E", "1600 S", unit_w, unit_h, addressFormat, e);

            //set up e for 1584 S
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1584S_x, bldg_1584S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1584S_h / 1;
            unit_w = bldg_1584S_w / 1;
            //draw units
            DrawUnit(0, 0, "1584 S", "1584 S", "100 E", unit_w, unit_h, addressFormat, e);
            
            //set up e for 1576 S
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1576S_x, bldg_1576S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1576S_h / 1;
            unit_w = bldg_1576S_w / 1;
            //draw units
            DrawUnit(0, 0, "1576 S", "1576 S", "100 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_1576S_w / 2, bldg_1576S_h + 25);
            e.Graphics.DrawString("100 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 1564 S
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1564S_x, bldg_1564S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1564S_h / 1;
            unit_w = bldg_1564S_w / 1;
            //draw units
            DrawUnit(0, 0, "1564 S", "1564 S", "100 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1546 S
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1546S_x, bldg_1546S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1546S_h / 1;
            unit_w = bldg_1546S_w / 1;
            //draw units
            DrawUnit(0, 0, "1546 S", "1546 S", "100 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_1546S_w / 2 + 50, bldg_1546S_h + 25);
            e.Graphics.DrawString("100 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 1538 S 4-plex
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1538S_x, bldg_1538S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1538S_h / 2;
            unit_w = bldg_1538S_w / 2;
            //draw units
            DrawUnit(0, 0, "1538 S", "1538 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1536 S", "1536 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1542 S", "1542 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1540 S", "1540 S", "100 E", unit_w, unit_h, addressFormat, e);

            //set up e for 1530 S
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1530S_x, bldg_1530S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1530S_h / 1;
            unit_w = bldg_1530S_w / 1;
            //draw units
            DrawUnit(0, 0, "1530 S", "1530 S", "100 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_1530S_w / 2, bldg_1530S_h + 25);
            e.Graphics.DrawString("100 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 1520 S
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1520S_x, bldg_1520S_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_1520S_h / 1;
            unit_w = bldg_1520S_w / 1;
            //draw units
            DrawUnit(0, 0, "1520 S", "1520 S", "100 E", unit_w, unit_h, addressFormat, e);
        }

        private void DrawCondosTextAndStuff(PrintPageEventArgs e)
        {
            Point upperLeftPoint = new Point();
            String textToPrint;
            SizeF tempSize = new SizeF();
            e.Graphics.ResetTransform();
            textToPrint = "Condo Area";
            tempSize = e.Graphics.MeasureString(textToPrint, pageTitleFont);
            //center text
            upperLeftPoint.X = e.PageBounds.Width/2 - (int)tempSize.Width/2;
            upperLeftPoint.X = upperLeftPoint.X + 100; // uncenter text for looks
            upperLeftPoint.Y = 400;
            DrawTitleAndStuff(textToPrint, upperLeftPoint, e);
            //drawCompass
            drawCompass(e, new Point(750, 200));
            drawNotOnWardListLegend(e);
        }

        private void drawNotOnWardListLegend(PrintPageEventArgs e)
        {
            Pen compassPen = new Pen(Color.Black, 1);
            string str1 = "* Not on ward list";
            string str2 = "~ Attending another ward";
            Font fnt = new Font("Ariel", 10, FontStyle.Regular);
            Point p1 = new Point();
            Point p2 = new Point();
            SizeF tempSize1 = new SizeF();
            SizeF tempSize2 = new SizeF();
            const int sideMarginX = 50;
            const int botMarginY = 35;
            tempSize1 = e.Graphics.MeasureString(str1, fnt);
            tempSize2 = e.Graphics.MeasureString(str2, fnt);
            p1.X = 850 - (int)tempSize2.Width - sideMarginX;
            p1.Y = 1100 - (int)tempSize2.Height - (int)tempSize1.Height - botMarginY;
            p2.X = p1.X;
            p2.Y = 1100 - (int)tempSize2.Height - botMarginY;
            e.Graphics.ResetTransform();
            e.Graphics.DrawString(str1, fnt, Brushes.Black, p1);
            e.Graphics.DrawString(str2, fnt, Brushes.Black, p2);

            DateTime objToday = DateTime.Today;
            str1 = "Printed: " + objToday.ToString("dd MMM yyyy");
            tempSize1 = e.Graphics.MeasureString(str1, fnt);
            p1.X =50;
            p1.Y = 1100 - (int)tempSize1.Height - botMarginY;
            e.Graphics.DrawString(str1, fnt, Brushes.Black, p1);
        }

        private void drawCompass(PrintPageEventArgs e, Point p)
        {
            Pen compassPen = new Pen(Color.Black, 3);
            Point p1 = new Point();
            Point p2 = new Point();
            Point p3 = new Point();
            Point p4 = new Point();
            string str = "North";
            Font fnt = new Font("Ariel", 18, FontStyle.Bold);
            e.Graphics.ResetTransform();

            p1 = p;
            p2 = p;
            p2.Y = p2.Y - 100;
            p3 = p2;
            p3.X = p3.X - 20;
            p3.Y = p3.Y + 20;
            p4 = p2;
            p4.X = p4.X + 20;
            p4.Y = p4.Y + 20;

            e.Graphics.DrawLine(compassPen, p1, p2);
            e.Graphics.DrawLine(compassPen, p2, p3);
            e.Graphics.DrawLine(compassPen, p2, p4);
            p2.Y = p2.Y - 20;
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(str, fnt, Brushes.Black, p2, strFormat);
        }

        private void DrawCondos(PrintPageEventArgs e)
        {
            Rectangle propertyRect = new Rectangle();
            Rectangle addressRect = new Rectangle();
            Rectangle nameRect = new Rectangle();
            StringFormat addressFormat = new StringFormat();
            Point p1 = new Point();
            Point p2 = new Point();
            string tempString;
            string[] addressStringsToFind = new string[2];
            string[] stringsToWrite;
            SizeF stringSize = new SizeF();
            int unit_h;
            int unit_w;
            int bldg_125E_NorthBldg_x = 75;
            int bldg_125E_NorthBldg_y = 350;
            int bldg_125E_NorthBldg_w = 297;// helps if divisible by 3
            int bldg_125E_NorthBldg_h = 180; // helps if divisible by 3

            int bldg_125E_MiddleBldg_x = 75;
            int bldg_125E_MiddleBldg_y = 675;
            int bldg_125E_MiddleBldg_w = 297;// helps if divisible by 3
            int bldg_125E_MiddleBldg_h = 180; // helps if divisible by 3

            int bldg_125E_SouthBldg_x = 75;
            int bldg_125E_SouthBldg_y = 1000;
            int bldg_125E_SouthBldg_w = 297;// helps if divisible by 3
            int bldg_125E_SouthBldg_h = 180; // helps if divisible by 3

            int bldg_1575S_x = 574;
            int bldg_1575S_y = 1025;
            int bldg_1575S_w = 297;// helps if divisible by 3
            int bldg_1575S_h = 180; // helps if divisible by 3

            int bldg_175E_x = 775;
            int bldg_175E_y = 703;
            int bldg_175E_w = 297;// helps if divisible by 3
            int bldg_175E_h = 180; // helps if divisible by 3
            int temp1;

            addressFormat.Alignment = StringAlignment.Center;
            addressFormat.LineAlignment = StringAlignment.Center;
           
            //draw roads
            e.Graphics.ResetTransform();
            temp1 = 20;
            p1 = new Point(bldg_125E_NorthBldg_x, bldg_125E_NorthBldg_y - bldg_125E_NorthBldg_w -temp1);
            p2 = new Point(bldg_125E_SouthBldg_x, bldg_1575S_y);
            e.Graphics.DrawLine(streetPen, p1, p2);

            p1 = new Point(bldg_175E_x, bldg_1575S_y);
            e.Graphics.DrawLine(streetPen, p1, p2);

            p2 = new Point(bldg_175E_x, bldg_175E_y - temp1);
            e.Graphics.DrawLine(streetPen, p1, p2);

            //set up e for 125 E NorthBldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_125E_NorthBldg_x, bldg_125E_NorthBldg_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_125E_NorthBldg_h / 3;
            unit_w = bldg_125E_NorthBldg_w / 3;
            //draw condos
            DrawUnit(0, 0, "1521", "1521 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1515", "1515 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2*unit_w, 0, "1509", "1509 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1519", "1519 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1513", "1513 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, unit_h, "1507", "1507 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, 2*unit_h, "1517", "1517 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 2 * unit_h, "1511", "1511 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2*unit_w, 2 * unit_h, "1505", "1505 S", "125 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_125E_NorthBldg_w / 2, -10);
            e.Graphics.DrawString("125 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 125 E MiddleBldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_125E_MiddleBldg_x, bldg_125E_MiddleBldg_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_125E_MiddleBldg_h / 3;
            unit_w = bldg_125E_MiddleBldg_w / 3;
            //draw condos
            DrawUnit(0, 0, "1547", "1547 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1541", "1541 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, 0, "1535", "1535 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1545", "1545 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1539", "1539 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, unit_h, "1533", "1533 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, 2 * unit_h, "1543", "1543 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 2 * unit_h, "1537", "1537 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, 2 * unit_h, "1531", "1531 S", "125 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_125E_MiddleBldg_w / 2, -10);
            e.Graphics.DrawString("125 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 125 E SouthBldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_125E_SouthBldg_x, bldg_125E_SouthBldg_y);
            e.Graphics.RotateTransform(-90);
            unit_h = bldg_125E_SouthBldg_h / 3;
            unit_w = bldg_125E_SouthBldg_w / 3;
            //draw condos
            DrawUnit(0, 0, "1565", "1565 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1559", "1559 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, 0, "1553", "1553 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1563", "1563 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1557", "1557 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, unit_h, "1551", "1551 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, 2 * unit_h, "1561", "1561 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 2 * unit_h, "1555", "1555 S", "125 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, 2 * unit_h, "1549", "1549 S", "125 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_125E_SouthBldg_w / 2, -10);
            e.Graphics.DrawString("125 East", streetFont, Brushes.Black, p1, addressFormat);
            
            //set up e for 1575 S Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_1575S_x, bldg_1575S_y);
            e.Graphics.RotateTransform(180);
            unit_h = bldg_1575S_h / 3;
            unit_w = bldg_1575S_w / 3;
            //draw condos
            DrawUnit(0, 0, "163", "163 E", "1575 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "157", "157 E", "1575 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, 0, "151", "151 E", "1575 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "161", "161 E", "1575 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "155", "155 E", "1575 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, unit_h, "149", "149 E", "1575 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, 2 * unit_h, "159", "159 E", "1575 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 2 * unit_h, "153", "153 E", "1575 S", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, 2 * unit_h, "147", "147 E", "1575 S", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_1575S_w / 2, -30);
            e.Graphics.DrawString("1575 South", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for 175 E Bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(bldg_175E_x, bldg_175E_y);
            e.Graphics.RotateTransform(90);
            unit_h = bldg_175E_h / 3;
            unit_w = bldg_175E_w / 3;
            //draw condos
            DrawUnit(0, 0, "1556", "1556 S", "175 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1562", "1562 S", "175 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, 0, "1568", "1568 S", "175 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1554", "1554 S", "175 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1560", "1560 S", "175 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, unit_h, "1566", "1566 S", "175 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, 2 * unit_h, "1552", "1552 S", "175 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 2 * unit_h, "1558", "1558 S", "175 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(2 * unit_w, 2 * unit_h, "1564", "1564 S", "175 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(bldg_175E_w / 2, -10);
            e.Graphics.DrawString("175 East", streetFont, Brushes.Black, p1, addressFormat);

        }
        /// <summary>
        /// This function will draw the rectangles for each condo at position x,y and rotation rot relative to e.
        /// This function assumes that e has been set up before it is called.
        /// The address string and search strings must be passed in also.
        /// </summary>
        /// <param name="unit_x"></param>
        /// <param name="unit_y"></param>
        /// <param name="addStr"></param>
        /// <param name="addToFind0"></param>
        /// <param name="addToFind1"></param>
        /// <param name="e"></param>
        private void DrawUnit(int unit_x, int unit_y, string addStr,
            string addToFind0, string addToFind1, int unitw, int unith,
            StringFormat addFrmt, PrintPageEventArgs e)
        {
            string[] addStrsToFind = new string[2];
            string[] strsToWrite;
            Rectangle propertyRect = new Rectangle(unit_x, unit_y, unitw, unith);
            Rectangle addressRect = new Rectangle(unit_x, unit_y + unith * 3 / 4, unitw, unith * 1 / 4);
            Rectangle nameRect = new Rectangle(unit_x, unit_y, unitw, unith * 3 / 4);
            e.Graphics.DrawString(addStr, addressFont, Brushes.Black, addressRect, addFrmt);
            //get strings to write
            addStrsToFind[0] = addToFind0;
            addStrsToFind[1] = addToFind1;
            strsToWrite = WardList.matchPropertyToNames(addStrsToFind);
            e.Graphics.DrawRectangle(propertyBoundPen, propertyRect);
            DrawNameInNameBox(strsToWrite, nameRect, nameFont, e);
            if (PRINT_NAME_BOX == true)
            { e.Graphics.DrawRectangle(nameBoundPen, nameRect); }
            if (PRINT_ADDRESS_BOX == true)
            { e.Graphics.DrawRectangle(addressBoundPen, addressRect); }
        }

        private void DrawNameInNameBox(string[] stringsToWrite, Rectangle nameRect, Font nameFont, PrintPageEventArgs e)
        {
            string stringToWrite;
            string[] tempStrs = new string[WardList.MAX_NUM_MATCHED_NAMES_RETURNED];
            int nameBoxMarginH = 2; //horizontal margin for name box on each side
            int nameBoxMarginV = 2; //vertical margin for name box on each side
            int maxStringLength;
            int numNames;
            int i, j, currentNameNum, nameToWriteIdx, tempNameNum;
            float totalHeight;
            int lineSpaceHeight;
            int tempTextHeight;
            int tempSpaceHeight;
            //int breakHere;
            int tempWidth;
            Font[] nameFonts = new Font[WardList.MAX_NUM_MATCHED_NAMES_RETURNED];
            SizeF[] nameSize = new SizeF[WardList.MAX_NUM_MATCHED_NAMES_RETURNED];
            SizeF tempSize = new SizeF();
            lineSpaceHeight = DEFAULT_LINE_SPACE_HEIGHT;//The same for all lines (for now)
            //build string
            maxStringLength = nameRect.Width - 2 * nameBoxMarginH;
            totalHeight = 0;
            numNames = 0;
            while((numNames < WardList.MAX_NUM_MATCHED_NAMES_RETURNED)
                    && stringsToWrite[numNames] != "")
            {
                nameFonts[numNames] = findFontSize(stringsToWrite[numNames], nameFont, maxStringLength, e);
                nameSize[numNames] = e.Graphics.MeasureString(stringsToWrite[numNames], nameFonts[numNames]);
                totalHeight = totalHeight + nameSize[numNames].Height;
                //add in line space height after first line
                if(numNames > 0)
                {
                    totalHeight = totalHeight + lineSpaceHeight;
                }
                numNames++;
            }
            //if too big, concatenate lines
            if (totalHeight >= (float)nameRect.Height)
            {
                //names won't fit vertically into name rectangle
                //concatenate names onto same line
                totalHeight = 0;
                lineSpaceHeight = MIN_LINE_SPACE_HEIGHT;
                for (i = 0; i < stringsToWrite.Length; i++)
                {
                    tempStrs[i] = string.Copy(stringsToWrite[i]);
                }
                //initialize
                nameToWriteIdx = 0;
                nameFonts[nameToWriteIdx] = new Font(nameFonts[nameToWriteIdx].Name, MIN_FONT_SIZE);
                nameSize[nameToWriteIdx] = e.Graphics.MeasureString(stringsToWrite[nameToWriteIdx], nameFonts[nameToWriteIdx]);
                totalHeight = nameSize[nameToWriteIdx].Height;
                stringsToWrite[nameToWriteIdx] = "";
                for (tempNameNum = 0; tempNameNum < numNames; tempNameNum++)
                {
                    if (tempNameNum > 0)
                    {
                        stringsToWrite[nameToWriteIdx]
                            = stringsToWrite[nameToWriteIdx] + "/";
                    }
                    for(j=0; j<tempStrs[tempNameNum].Length; j++)
                    {
                        stringsToWrite[nameToWriteIdx]
                            = stringsToWrite[nameToWriteIdx] + tempStrs[tempNameNum][j];
                        tempSize = e.Graphics.MeasureString(stringsToWrite[nameToWriteIdx], nameFonts[nameToWriteIdx]);
                        if (tempSize.Width >= (nameRect.Width-2*nameBoxMarginH))
                        {
                            //max size of line reached
                            nameToWriteIdx++;
                            nameFonts[nameToWriteIdx] = new Font(nameFonts[nameToWriteIdx].Name, MIN_FONT_SIZE);
                            nameSize[nameToWriteIdx] = e.Graphics.MeasureString(stringsToWrite[nameToWriteIdx], nameFonts[nameToWriteIdx]);
                            stringsToWrite[nameToWriteIdx] = "";

                            totalHeight = totalHeight + nameSize[nameToWriteIdx].Height + lineSpaceHeight;
                           
                        }
                    }
                }
                numNames = nameToWriteIdx + 1;
                for (i = numNames ; i < WardList.MAX_NUM_MATCHED_NAMES_RETURNED; i++)
                {
                    stringsToWrite[i] = "";
                }
            }//end concatenate
            
            //write names
            tempSize = e.Graphics.MeasureString(stringsToWrite[0], nameFonts[0]);
            int totalStringsHeight = (int)(numNames * tempSize.Height) + (numNames - 1) * lineSpaceHeight;
            Point upperLeftPoint = new Point();
            upperLeftPoint.Y = nameRect.Y + nameRect.Height / 2 - (int)(totalHeight / 2.0);
            for (i = 0; i < numNames; i++)
            {
                stringToWrite = stringsToWrite[i];
                if (i > 0)
                {
                    tempTextHeight = (int)tempSize.Height;
                    tempSpaceHeight = lineSpaceHeight;
                }
                else
                {
                    tempTextHeight = 0;
                    tempSpaceHeight = 0;
                }
                tempSize = e.Graphics.MeasureString(stringToWrite, nameFonts[i]);

                upperLeftPoint.X = nameRect.X + nameRect.Width / 2 - (int)(tempSize.Width / 2.0);

//                upperLeftPoint.Y = nameRect.Y + nameRect.Height / 2 - (int)(totalStringsHeight / 2.0)
//                   + i*(int)tempSize.Height + i*(int)lineSpaceHeight;
                upperLeftPoint.Y = upperLeftPoint.Y + (tempTextHeight + tempSpaceHeight);
   
                e.Graphics.DrawString(stringToWrite, nameFonts[i], Brushes.Black, upperLeftPoint);
            }
        }

        private void DrawSandyBrook(PrintPageEventArgs e)
        {
            Rectangle propertyRect = new Rectangle();
            Rectangle addressRect = new Rectangle();
            Rectangle nameRect = new Rectangle();
            StringFormat addressFormat = new StringFormat();
            Point p1 = new Point();
            Point p2 = new Point();
            string tempString;
            string[] addressStringsToFind = new string[2];
            string[] stringsToWrite;
            SizeF stringSize = new SizeF();
            
            int unit_h;
            int unit_w;

            //sb = Sandy Brook
            int sb_townHome_x = 458;
            int sb_townHome_y = 273;
            int sb_townHome_w = 198;// helps if divisible by number of units across
            int sb_townHome_h = 60; // helps if divisible by number of units up and down

            int sb_duplex_x = 320;
            int sb_duplex_y = 300;
            int sb_duplex_w = 198;// helps if divisible by number of units across
            int sb_duplex_h = 60; // helps if divisible by number of units up and down

            int sb_NW_bldg_x = 200;
            int sb_NW_bldg_y = 400;
            int sb_NW_bldg_w = 198;// helps if divisible by number of units across
            int sb_NW_bldg_h = 120; // helps if divisible by number of units up and down

            int sb_NE_bldg_x = 398;
            int sb_NE_bldg_y = 625;
            int sb_NE_bldg_w = 198;// helps if divisible by number of units across
            int sb_NE_bldg_h = 120; // helps if divisible by number of units up and down

            int sb_SW_bldg_x = 200;
            int sb_SW_bldg_y = 650;
            int sb_SW_bldg_w = 198;// helps if divisible by number of units across
            int sb_SW_bldg_h = 120; // helps if divisible by number of units up and down

            int sb_SE_bldg_x = 518;
            int sb_SE_bldg_y = 800;
            int sb_SE_bldg_w = 198;// helps if divisible by number of units across
            int sb_SE_bldg_h = 120; // helps if divisible by number of units up and down
            int temp1;

            addressFormat.Alignment = StringAlignment.Center;
            addressFormat.LineAlignment = StringAlignment.Center;

            //draw roads
            e.Graphics.ResetTransform();
            temp1 = 20;
            p1 = new Point(sb_townHome_x + sb_townHome_h, sb_townHome_y- sb_townHome_w - temp1);
            p2 = new Point(sb_SE_bldg_x, sb_SW_bldg_y + sb_SW_bldg_w + temp1);
            e.Graphics.DrawLine(streetPen, p1, p2);

            p1 = new Point(sb_NW_bldg_x - sb_NW_bldg_h, sb_NW_bldg_y  - temp1);
            p2 = new Point(sb_SW_bldg_x - sb_SW_bldg_h, sb_SW_bldg_y + sb_SW_bldg_w + temp1);
            e.Graphics.DrawLine(streetPen, p1, p2);

            p1 = new Point(sb_SE_bldg_x, sb_SW_bldg_y + sb_SW_bldg_w + temp1);
            e.Graphics.DrawLine(streetPen, p1, p2);

            //set up e for sb_townHome
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(sb_townHome_x, sb_townHome_y);
            e.Graphics.RotateTransform(-90);
            unit_h = sb_townHome_h / 1;
            unit_w = sb_townHome_w / 2;
            //draw town home units
            DrawUnit(0, 0, "1462", "1462 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1460", "1460 S", "100 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(sb_townHome_w / 2, sb_townHome_h + 30);
            e.Graphics.DrawString("100 East", streetFont, Brushes.Black, p1, addressFormat);
 
            //set up e for sb_duplex
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(sb_duplex_x, sb_duplex_y);
            e.Graphics.RotateTransform(0);
            unit_h = sb_duplex_h / 1;
            unit_w = sb_duplex_w / 2;
            //draw town home units
            DrawUnit(0, 0, "1464", "1464 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1466", "1466 S", "100 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            e.Graphics.RotateTransform(-90);
            p1 = new Point(-(sb_duplex_h / 2), sb_duplex_w + 30);
            e.Graphics.DrawString("100 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for sb_NW_bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(sb_NW_bldg_x, sb_NW_bldg_y);
            e.Graphics.RotateTransform(90);
            unit_h = sb_NW_bldg_h / 2;
            unit_w = sb_NW_bldg_w / 2;
            //draw town home units
            DrawUnit(0, 0, "1465", "1465 S", "70 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1469", "1469 S", "70 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1463", "1463 S", "70 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1467", "1467 S", "70 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(sb_NW_bldg_w / 2, sb_NW_bldg_h + 30);
            e.Graphics.DrawString("70 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for sb_NE_bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(sb_NE_bldg_x, sb_NE_bldg_y);
            e.Graphics.RotateTransform(-90);
            unit_h = sb_NE_bldg_h / 2;
            unit_w = sb_NE_bldg_w / 2;
            //draw town home units
            DrawUnit(0, 0, "1474", "1474 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1470", "1470 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1472", "1472 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1468", "1468 S", "100 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(sb_NE_bldg_w / 2, sb_NE_bldg_h + 30);
            e.Graphics.DrawString("100 East", streetFont, Brushes.Black, p1, addressFormat);

            //set up e for sb_SW_bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(sb_SW_bldg_x, sb_SW_bldg_y);
            e.Graphics.RotateTransform(90);
            unit_h = sb_SW_bldg_h / 2;
            unit_w = sb_SW_bldg_w / 2;
            //draw town home units
            DrawUnit(0, 0, "1473", "1473 S", "70 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1477", "1477 S", "70 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1471", "1471 S", "70 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1475", "1475 S", "70 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            p1 = new Point(sb_SW_bldg_w / 2, sb_SW_bldg_h + 30);
            e.Graphics.DrawString("70 East", streetFont, Brushes.Black, p1, addressFormat);
            
            //set up e for sb_SE_bldg
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(sb_SE_bldg_x, sb_SE_bldg_y);
            e.Graphics.RotateTransform(180);
            unit_h = sb_SE_bldg_h / 2;
            unit_w = sb_SE_bldg_w / 2;
            //draw town home units
            DrawUnit(0, 0, "1478", "1478 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, 0, "1481", "1481 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(0, unit_h, "1476", "1476 S", "100 E", unit_w, unit_h, addressFormat, e);
            DrawUnit(unit_w, unit_h, "1479", "1479 S", "100 E", unit_w, unit_h, addressFormat, e);
            //draw street name
            e.Graphics.RotateTransform(90);
            p1 = new Point(sb_SE_bldg_h / 2, 30);
            e.Graphics.DrawString("100 East", streetFont, Brushes.Black, p1, addressFormat);

        }
        private Font findFontSize(string tempString, Font nameFont, int stringLengthMax, PrintPageEventArgs e)
        {
            Font currentNameFont = nameFont;
            SizeF sSize = e.Graphics.MeasureString(tempString, currentNameFont);
            while (sSize.Width > stringLengthMax && stringFont.Size > MIN_FONT_SIZE)
            {
                currentNameFont = new Font(nameFont.FontFamily, currentNameFont.Size - 1);
                sSize = e.Graphics.MeasureString(tempString, currentNameFont);
            }
            return currentNameFont;
        }

        
        private void previewBtn_Click(object sender, EventArgs e)
        {
            filePrintPreviewMenuItem_Click(sender, e);
        }

        private void AddFamilyBtn_Click(object sender, EventArgs e)
        {
            Add_Family addFamilyForm = new Add_Family();
            addFamilyForm.MyParentForm = this;

            if (addFamilyForm.ShowDialog() == DialogResult.OK)
            {
                //add info to table then save file

                //add new row to data table for this family
                DataRow dr = WardList.dtNotOnWardList.NewRow(); //dr gets all columns of dt
                //Last Name
                dr["lastName"] = addFamilyForm.lastName;
                //hoh1
                dr["hoh1"] = addFamilyForm.hoh1;
                //hoh2
                dr["hoh2"] = addFamilyForm.hoh2;
                //address
                dr["address"] = addFamilyForm.address;
                //status
                dr["status"] = addFamilyForm.status;

                //add row to table
                WardList.dtNotOnWardList.Rows.Add(dr);
                string tmpStr = WardList.dtNotOnWardList.Rows.Count.ToString();
                lblNumFamsNotOnList.Text = "Number of Families: " + tmpStr;

                //save file
                WardList.saveNotOnWardListToFile();
            }
        }

        private void wardListBrowseBtn_Click(object sender, EventArgs e)
        {
            string tmpStr;
            OpenFileDialog openFileDlg = new OpenFileDialog();

            openFileDlg.Title = "Select A Ward List";
            int i = WardList.onWardListPath.LastIndexOf("\\");
            if (i >= 0)
            {
                tmpStr = WardList.onWardListPath.Substring(0, i);
            }
            else
            {
                tmpStr = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            openFileDlg.InitialDirectory = tmpStr;
            openFileDlg.Filter = "CSV   *.csv|*.csv|VCF   *.vcf|*.vcf|All Files   *.*|*.*";
            openFileDlg.FilterIndex = 1;
            openFileDlg.FileName = "";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                //wardListPathTextBox.Text = openFileDlg.FileName;
                updateOnWardListPath(openFileDlg.FileName);
                WardList.populateDtOnWardList();
                tmpStr = WardList.dtOnWardList.Rows.Count.ToString();
                lblNumFamsOnList.Text = "Number of Families: " + tmpStr;
                
                //update registry setting
                if (WardList.OnWardListFileType != 0)
                {
                    programRegSettings.SaveWardListFile(openFileDlg.FileName);
                }
            }
        }

        private void notOnWardListBrowseBtn_Click(object sender, EventArgs e)
        {
            string tmpStr;
            OpenFileDialog openFileDlg = new OpenFileDialog();

            openFileDlg.Title = "Select \"Families Not Ward List\" File";
            int i = WardList.notOnWardListPath.LastIndexOf("\\");
            if (i >= 0)
            {
                tmpStr = WardList.notOnWardListPath.Substring(0, i);
            }
            else
            {
                tmpStr = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            openFileDlg.InitialDirectory = tmpStr;
            openFileDlg.Filter = "CSV   *.csv|*.csv|All Files   *.*|*.*";
            openFileDlg.FilterIndex = 1;
            openFileDlg.FileName = "";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                //notOnwardListPathTextBox.Text = openFileDlg.FileName;
                updateNotOnWardListPath(openFileDlg.FileName);
                WardList.populateDtNotOnWardList();
                tmpStr =WardList.dtNotOnWardList.Rows.Count.ToString();
                lblNumFamsNotOnList.Text = "Number of Families: " + tmpStr;

                //update registry setting
                if (WardList.OnWardListFileType != 0)
                {
                    programRegSettings.SaveNotOnWardListFile(openFileDlg.FileName);
                }
            }
        }

        private void DeleteFamilyBtn_Click(object sender, EventArgs e)
        {
            int rowNum = NotOnWardList.CurrentCell.RowIndex;
            DataRow dr = WardList.dtNotOnWardList.Rows[rowNum];
            string tmpStr = dr["hoh1"].ToString() + " " + dr["lastName"].ToString();
            tmpStr = "Are you sure you want to delete " + tmpStr + " from \"Families Not On Ward List\" and save the new list";
            if(MessageBox.Show(tmpStr, "Delete Family", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                dr.Delete();
                WardList.saveNotOnWardListToFile();
                tmpStr = WardList.dtNotOnWardList.Rows.Count.ToString();
                lblNumFamsNotOnList.Text = "Number of Families: " + tmpStr;
            }
        }

        private void EditFamilyBtn_Click(object sender, EventArgs e)
        {
            int rowNum = NotOnWardList.CurrentCell.RowIndex;
            DataRow dr = WardList.dtNotOnWardList.Rows[rowNum];
            
            Add_Family addFamilyForm = new Add_Family(dr);
            addFamilyForm.MyParentForm = this;

            if (addFamilyForm.ShowDialog() == DialogResult.OK)
            {
                //add edited info to table then save file

                if  (WardList.dtNotOnWardList.Rows.Count !=0)
                {
                   WardList.dtNotOnWardList.Rows[rowNum]["lastName"]= addFamilyForm.lastName;
                   WardList.dtNotOnWardList.Rows[rowNum]["hoh1"] = addFamilyForm.hoh1;
                   WardList.dtNotOnWardList.Rows[rowNum]["hoh2"] = addFamilyForm.hoh2;
                   WardList.dtNotOnWardList.Rows[rowNum]["address"] = addFamilyForm.address;
                   WardList.dtNotOnWardList.Rows[rowNum]["status"] = addFamilyForm.status;
                   // Do I need to use an update command to make the table current ???

                   //save file
                   WardList.saveNotOnWardListToFile();
                }
            }
        }

  
        private void OnWardList_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            OnWardList_ColumnWidthChanged();
        }

        private void OnWardList_ColumnWidthChanged()
        {
            int i = 0;
            int sum = 0;
            for (i = 0; i < OnWardList.Columns.Count; i++)
            {
                sum += OnWardList.Columns[i].Width;
            }
            sum += OnWardList.RowHeadersWidth;
            sum += 19;
            Size tableSize = OnWardList.Size;
            i = tableSize.Width - sum;
            if (i > 0)
            {
                //make the last column bigger
                OnWardList.Columns[OnWardList.Columns.Count - 1].Width += i;
            }

        }

        private void newFileNotOnWardListBtn_Click(object sender, EventArgs e)
        {
            string tmpStr;
            SaveFileDialog saveFileDlg = new SaveFileDialog();

            saveFileDlg.Title = "Select \"Families Not Ward List\" File Name To Create";
              tmpStr = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
              saveFileDlg.InitialDirectory = tmpStr;
              saveFileDlg.Filter = "CSV   *.csv|*.csv|All Files   *.*|*.*";
              saveFileDlg.FilterIndex = 1;
              saveFileDlg.FileName = "PeopleNotOnWardList";
              if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(saveFileDlg.FileName, FileMode.Create);
                file.Close();
                updateNotOnWardListPath(saveFileDlg.FileName);
                WardList.populateDtOnWardList();
                tmpStr = WardList.dtOnWardList.Rows.Count.ToString();
                lblNumFamsOnList.Text = "Number of Families: " + tmpStr;

                //update registry setting
                programRegSettings.SaveNotOnWardListFile(saveFileDlg.FileName);
            }
        }

        //-------------- end of event handlers -------------------------------

    }// end - class Form1
}// end namespace