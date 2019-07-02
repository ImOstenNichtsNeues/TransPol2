﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
namespace WindowsFormsApp1
{

    public partial class Transform : Form
    {
        StringBuilder comunicator = new StringBuilder("");
        StringBuilder errorCatcher = new StringBuilder("");
        StringBuilder FileName =new StringBuilder("");
        StringBuilder start = new StringBuilder("");
        StringBuilder end = new StringBuilder("");
        StringBuilder startETRF = new StringBuilder("");
        StringBuilder endETRF = new StringBuilder("");
        List<Point> Points = new List<Point>();
        List<Point3D> Points3D = new List<Point3D>();
        List<PointBLH> PointsBLH = new List<PointBLH>();
        string[] data;        
        /* Zmienne bool degreeForm i degreeForm odpowiadają za format danych kątowych wejściowych i wyjściowym. true oznacza format kątowy.
         False oznacza format kąt-min-sec. Nie ma opcji wprowadzania danych w gradach!*/
        bool degreeForm = true; bool resultDegreeForm = true;
        /*parametr longitude określa wartość południka osiowego, jeśli takowy występuje.*/
        byte longitude=0; byte resLongitude = 0; double longitude65 = 0; double resLongitude65 = 0;
        //Promienie krzywizn, potrzebne do wyznaczenia współrzędnych w odwzorowaniu stereograficznym Roussihle'a elipsoidy Krasowkiego.
        //To samo tyczy się współrzędnych punktów głównych
        double R0 = 0; double resR0 = 0; double x0; double y0; double resx0; double resy0;
        //Rzędna punktu głównego strefy w odwzorowaniu GK - potrzebna do wyznaczenia wsp w odwzorowaniu Roussihle'a elipsoidy Krasowskiego.
        double xGK0=0; double resXGK0=0;
        //Określnie strefy układu 1965:
        byte strefa = 0;
        // canIStartCounting sprawdza, czy dane wejściowe zostały wprowadzone prawidłowo oraz czy układ wyjściowy został wybrany.
        bool canIStartCounting = false;
        //TransformateOption określa czy wybrano rozwiązanie teoretyczne czy empiryczne [grid]. True - teoretyczna, false - empiryczna.
        bool transformateOption = true;
        //StripesSize określa, czy dla układu 1942 wybrano pasy trzystopniowe(true) czy sześciostopniowe(false).
        public static RichTextBox box = new RichTextBox();
        public void setFalseGroupBoxVisibility(GroupBox first, GroupBox second)
        {
            first.Visible = false;
            second.Visible = false;
        }
        public void switchGroupBoxVisibility(GroupBox visible, GroupBox invisible)
        {
            visible.Visible = true;
            invisible.Visible = false;
        }
        public void clearRadioButtonsCheck(params RadioButton[] buttons)
        {
            foreach(RadioButton buton in buttons)
            {
                buton.Checked = false;
            }
        }
        public void loadPoints2D()
        {
            Stopwatch clock = new Stopwatch();
            clock.Start();
            StreamReader stream = new StreamReader(filePath.Text);
            int problems = 0;      //SPRAWDZA CZY WCZYTYWANY PLIK MOŻNA DOPUŚCIĆ DO ETAPU OBLICZENIOWEGO
            long size = new FileInfo(filePath.Text).Length;
            string line;
            if (size != 0 && !File.ReadAllText(filePath.Text).All(char.IsWhiteSpace)) //SPRAWDZA CZY WCZYTANY PLIK JEST PUSTY
            {
                while (stream.Peek() != -1)
                {
                    line = stream.ReadLine().Trim();
                    if (!line.All(char.IsWhiteSpace))
                    {
                        this.data = Regex.Split(line.Trim(), @"\s+");
                        if (data.Length == 3)
                        {
                            try
                            {
                                this.Points.Add(new Point(data[0], Convert.ToDouble(data[1]), Convert.ToDouble(data[2])));         
                            }
                            catch (Exception ex) //SPRAWDZENIE CZY FORMAT PLIKÓW JEST DOPUSZCZALNY
                            {
                                if (ex is FormatException || ex is ArgumentException)
                                {
                                    problems++;
                                    Thread.Sleep(100);
                                    this.errorCatcher.Clear();
                                    this.errorCatcher.Append("Punkt " + this.data[0] + ": Niedopuszczalna wartość parametrów.\n");
                                    TransformerBW.ReportProgress(0);   
                                }
                                else { MessageBox.Show("Nastąpił błąd wczytania pliku. Sprawdź format danych wejściowych."); problems++; break; }
                            }
                        }
                        else {
                            Thread.Sleep(100);
                            this.errorCatcher.Clear();
                            this.errorCatcher.Append("Punkt " + this.data[0] + ": Nieodpowiednia liczba wejściowych parametrów(" + this.data.Length + ").\n");
                            TransformerBW.ReportProgress(0); problems++;
                        }
                    }
                }
            }
            else { MessageBox.Show("Plik \"" + Path.GetFileName(filePath.Text) + "\" jest pusty."); problems++; }
            clock.Stop();
            if (problems == 0) { this.canIStartCounting = true;
                Thread.Sleep(500);
                this.comunicator.Clear();
                this.comunicator.Append("Czas wczytywania danych startowych: " + Convert.ToString(clock.Elapsed)+ ".\n");
                TransformerBW.ReportProgress(1);
            }
             }
        public void loadPoints3D()
        {
            Stopwatch clock = new Stopwatch();
            clock.Start();
            StreamReader stream = new StreamReader(filePath.Text);
            bool problems = false;       //SPRAWDZA CZY WCZYTYWANY PLIK MOŻNA DOPUŚCIĆ DO ETAPU OBLICZENIOWEGO
            long size = new FileInfo(filePath.Text).Length;
            string line;
            if (size != 0 && !File.ReadAllText(filePath.Text).All(char.IsWhiteSpace)) //SPRAWDZA CZY WCZYTANY PLIK JEST PUSTY
            {
                while (stream.Peek() != -1)
                {
                    line = stream.ReadLine().Trim();
                    if (!line.All(char.IsWhiteSpace))
                    {
                        this.data = Regex.Split(line.Trim(), @"\s+");
                        if (data.Length == 4)
                        {
                            try
                            {
                                this.Points3D.Add(new Point3D(data[0], Convert.ToDouble(data[1]), Convert.ToDouble(data[2]), Convert.ToDouble(data[3])));
                              
                            }
                            catch (Exception ex) //SPRAWDZENIE CZY FORMAT PLIKÓW JEST DOPUSZCZALNY
                            {
                                if (ex is FormatException || ex is ArgumentException)
                                {
                                    problems = true;
                                    Thread.Sleep(100);
                                    this.errorCatcher.Clear();
                                    this.errorCatcher.Append("Punkt " + this.data[0] + ": Niedopuszczalna wartość parametrów.\n");
                                    TransformerBW.ReportProgress(1);
                                }
                                else { MessageBox.Show("Nastąpił błąd wczytania pliku. Sprawdź format danych wejściowych.");
                                    problems = true;  break; }
                            }
                        }
                        else {
                            Thread.Sleep(100);
                            this.errorCatcher.Clear();
                            this.errorCatcher.Append("Punkt " + this.data[0] + ": Nieodpowiednia liczba wejściowych parametrów(" + this.data.Length + ").\n");
                            problems =true; }
                    }
                }
            }
            else { MessageBox.Show("Plik \"" + Path.GetFileName(filePath.Text) + "\" jest pusty.");
                problems = true; }  
            clock.Stop();
            if (!problems)
            {
                Thread.Sleep(500);
                this.canIStartCounting = true;
                this.comunicator.Clear();
                this.comunicator.Append("Czas wczytywania danych startowych: " + Convert.ToString(clock.Elapsed) + ".\n");
                TransformerBW.ReportProgress(1);
            }
        
        }
        public void loadPointsBLH()
        {
            Stopwatch clock = new Stopwatch();
            clock.Start();
            StreamReader stream = new StreamReader(filePath.Text);
            bool problems = false;      //SPRAWDZA CZY WCZYTYWANY PLIK MOŻNA DOPUŚCIĆ DO ETAPU OBLICZENIOWEGO
            bool anyFalseFormattedPoint = true;
            long size = new FileInfo(filePath.Text).Length;
            string line;
            if (size != 0 && !File.ReadAllText(filePath.Text).All(char.IsWhiteSpace)) //SPRAWDZA CZY WCZYTANY PLIK JEST PUSTY
            {
                while (stream.Peek() != -1)
                {
                    line = stream.ReadLine().Trim();
                    if (!line.All(char.IsWhiteSpace))
                    {
                        this.data = Regex.Split(line.Trim(), @"\s+");
                        int format = this.degreeForm ? 4 : 8;
                        switch (format)
                        {
                            case 4:
                                if (data.Length == format)
                                {
                                    try
                                    {
                                        this.PointsBLH.Add(new PointBLH(data[0], Convert.ToDouble(data[1]), Convert.ToDouble(data[2]), Convert.ToDouble(data[3])));
                                        if (!PointsBLH.Last().isCorrectlyFormated())
                                        {
                                            isCorrectlyFormated(PointsBLH.Last());
                                            anyFalseFormattedPoint = false;
                                        }
                                    }
                                    catch (Exception ex) //SPRAWDZENIE CZY FORMAT PLIKÓW JEST DOPUSZCZALNY
                                    {
                                        if (ex is FormatException || ex is ArgumentException)
                                        {
                                            problems=true;
                                            Thread.Sleep(100);
                                            this.errorCatcher.Clear();
                                            this.errorCatcher.Append("Punkt " + this.data[0] + ": Niedopuszczalna wartość parametrów.\n");
                                           TransformerBW.ReportProgress(0);
                                        }
                                        else { MessageBox.Show("Nastąpił błąd wczytania pliku. Sprawdź format danych wejściowych.");
                                            problems=true; goto gameOver; }
                                    }
                                } 
                                else {
                                    Thread.Sleep(100);
                                    this.errorCatcher.Clear();
                                    this.errorCatcher.Append("Punkt " + this.data[0] + ": Nieodpowiednia liczba wejściowych parametrów(" + data.Length + ").\n");
                                    this.TransformerBW.ReportProgress(0);
                                    problems =true;  }
                                break;
                            case 8:
                                if (data.Length == format)
                                {
                                    try
                                    {
                                        this.PointsBLH.Add(new PointBLH(data[0], Convert.ToDouble(data[1]), Convert.ToDouble(data[2]), Convert.ToDouble(data[3]), Convert.ToDouble(data[4]), Convert.ToDouble(data[5]), Convert.ToDouble(data[6]), Convert.ToDouble(data[7])));  
                                        if (!PointsBLH.Last().isCorrectlyFormated())
                                        {
                                            isCorrectlyFormated(PointsBLH.Last());
                                            anyFalseFormattedPoint = false;
                                        }
                                    }
                                    catch (Exception ex) //SPRAWDZENIE CZY FORMAT PLIKÓW JEST DOPUSZCZALNY
                                    {
                                        if (ex is FormatException || ex is ArgumentException)
                                        {
                                            problems=true; Thread.Sleep(100);
                                            this.errorCatcher.Clear();
                                            this.errorCatcher.Append("Punkt " + this.data[0] + ": Niedopuszczalna wartość parametrów.\n");
                                            this.TransformerBW.ReportProgress(0);
                                        }
                                        else { MessageBox.Show("Nastąpił błąd wczytania pliku. Sprawdź format danych wejściowych.");
                                            problems =true; goto gameOver; }
                                    }
                                }
                                else {
                                    Thread.Sleep(100);
                                    this.errorCatcher.Clear();
                                    this.errorCatcher.Append("Punkt " + this.data[0] + ": Nieodpowiednia liczba wejściowych parametrów(" + data.Length + ").\n");
                                    this.TransformerBW.ReportProgress(0);
                                    problems =true;  }
                                break;
                        }
                    }
                }
            }
            else { MessageBox.Show("Plik \"" + Path.GetFileName(filePath.Text) + "\" jest pusty."); problems=true; }
            gameOver:
            clock.Stop();
            if ((!problems) && anyFalseFormattedPoint) { this.canIStartCounting = true;
                Thread.Sleep(500);
                this.comunicator.Clear();
                this.comunicator.Append("Czas wczytywania danych startowych: " + Convert.ToString(clock.Elapsed) + ".\n");
                this.TransformerBW.ReportProgress(1);
            } 
        }
        public void getPointsData() 
        {
            if (!filePath.Text.Equals(""))
            {
                if (this.start.ToString().Equals("Układ 2000"))
                {
                    this.longitude = setLongitude(longitude15, longitude18, longitude21, longitude24) ;
                    if (this.longitude != 0)
                    {
                        loadPoints2D(); this.Points3D.Clear(); this.PointsBLH.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Nie określono południka osiowego."); 
                    }
                }
                else if (this.start.ToString().Equals("Układ 1992"))
                {
                    this.longitude = 19;
                    loadPoints2D(); this.Points3D.Clear(); this.PointsBLH.Clear();
                }
                else if (this.start.ToString().Equals("UTM"))
                {
                        this.longitude = setLongitude(longitudeUTM15, longitudeUTM21);
                        if (this.longitude != 0)
                        {
                            loadPoints2D(); this.Points3D.Clear(); this.PointsBLH.Clear();
                    }
                        else
                        {
                            MessageBox.Show("Nie określono południka osiowego.");
                        }
                    }
                else if (this.start.ToString().Equals("XYZ GRS80"))
                {
                    loadPoints3D(); this.Points.Clear(); this.PointsBLH.Clear();
                }
                else if (this.start.ToString().Equals("BLH GRS80"))
                {
                    loadPointsBLH(); this.Points3D.Clear(); this.Points.Clear();
                }
                else
                {
                    MessageBox.Show("Jak mam to niby zrobić???");
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano zbioru punktów.");
            }
        }
        
        public Transform()
        {
             InitializeComponent();
        }

         byte setLongitude(params RadioButton[] list)
        {
            byte longitude = 0;
            for(int i=0; i< list.Length; i++)
            {
                if (list[i].Checked)
                {
                    longitude = Convert.ToByte(list[i].Text.Substring(0, 2)); break;
                }
            }
            return longitude;
        }

        private void CountUp_Click(object sender, EventArgs e)
        {
            this.FileName.Clear();
            this.Points3D.Clear();
            this.Points.Clear();
            this.PointsBLH.Clear();
            this.FileOpenerButton.Visible = false; this.FileOpenerButton.Text = "";
            this.MonitorRichTextBox.Clear(); this.MonitorRichTextBox.Text = "MONITOR: \n";
            if (!filePath.Text.Equals(""))
            {
                TransformerBW.RunWorkerAsync();

            }
            else
            {
                MessageBox.Show("Nie wybrano zbioru punktów.");
            }
            //koniecPsot:
            //this.longitude = 0; this.canIStartCounting = false; this.resLongitude = 0;
        }
        //WCZYTANIE SIATKI GRID DO METODY EMPIRYCZNEJ oraz INTERPOLACJA DWULINIOWA
        private List<WebGrid> webGrids()
        {
            List<WebGrid> result = new List<WebGrid>();
            string[] text= Regex.Split(Properties.Resources.gridETRF, "\n");
            foreach (string line in text)
            {
                string[] point = Regex.Split(line.Trim(), @"\s+");
                result.Add(new WebGrid(Convert.ToDouble(point[0]), Convert.ToDouble(point[1]), Convert.ToDouble(point[2]), Convert.ToDouble(point[3]), Convert.ToDouble(point[4])));
            }
            return result;
        }
        private List<PointBLH> setGridDeltasNchangeETRF(List<PointBLH> points, bool ETRF2000change89)
        {
            Stopwatch clock = new Stopwatch(); clock.Start();
            List<WebGrid> grid = webGrids();
            clock.Stop();
            this.comunicator.Clear();
            this.comunicator.Append( "Wczytywanie siatki grid: " + Convert.ToString(clock.Elapsed)+ ".\n");
            this.TransformerBW.ReportProgress(2);
            List<PointBLH> result = new List<PointBLH>();
            points.ForEach(p =>
            {
                if (!p.Format()) { p.convertToDegrees(); }
                double B = p.fi(); double Bdown = Math.Floor(B * 100) / 100; double Bup = Math.Ceiling(B * 100) / 100;
                double L = p.lambda(); double Ldown = Math.Floor(L * 100) / 100; double Lup = Math.Ceiling(L * 100) / 100;
                WebGrid grid11 = grid.Find(q => q.fi().Equals(Bdown) && q.lambda().Equals(Ldown));
                //MessageBox.Show(grid11.fi() + " " + grid11.lambda());
                WebGrid grid12 = grid.Find(q => q.fi().Equals(Bup) && q.lambda().Equals(Ldown));
                //MessageBox.Show(grid12.fi() + " " + grid12.lambda());
                WebGrid grid21 = grid.Find(q => q.fi().Equals(Bdown) && q.lambda().Equals(Lup));
                //MessageBox.Show(grid21.fi() + " " + grid21.lambda());
                WebGrid grid22 = grid.Find(q => q.fi().Equals(Bup) && q.lambda().Equals(Lup));
                //MessageBox.Show(grid22.fi() + " " + grid22.lambda());
                WebGrid deltas = BilinearInterpolation(B, L, grid11, grid12, grid21, grid22);
                if (ETRF2000change89)
                {
                    result.Add(new PointBLH(p.Name(), B - deltas.deltaFi(), L - deltas.deltaLambda(), p.height() - deltas.deltaH()));
                }
                else if (!ETRF2000change89)
                {
                    result.Add(new PointBLH(p.Name(), B + deltas.deltaFi(), L + deltas.deltaLambda(), p.height() + deltas.deltaH()));
                }
            });
            return result;
        } //działa

        /* KONIECZNIE ŁADOWAĆ PUNKTY SIATKI W ODPOWIEDNIEJ KOLEJNOŚCI!! */
        private WebGrid BilinearInterpolation(double B, double L, WebGrid grid11, WebGrid grid12, WebGrid grid21, WebGrid grid22)
        {
            double dfi, dl, dh;

                if (grid11.Equals(grid22))
                {
                    dfi = grid11.deltaFi(); dl = grid11.deltaLambda(); dh = grid11.deltaH();
                }
            else
            {
                double df1 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid11.deltaFi() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid21.deltaFi();
                double df2 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid12.deltaFi() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid22.deltaFi();
                dfi = (grid12.fi() - B) / (grid12.fi() - grid11.fi()) * df1 + (B - grid11.fi()) / (grid12.fi() - grid11.fi()) * df2;
                double dl1 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid11.deltaLambda() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid21.deltaLambda();
                double dl2 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid12.deltaLambda() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid22.deltaLambda();
                dl = (grid12.fi() - B) / (grid12.fi() - grid11.fi()) * dl1 + (B - grid11.fi()) / (grid12.fi() - grid11.fi()) * dl2;
                double dh1 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid11.deltaH() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid21.deltaH();
                double dh2 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid12.deltaH() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid22.deltaH();
                dh = (grid12.fi() - B) / (grid12.fi() - grid11.fi()) * dh1 + (B - grid11.fi()) / (grid12.fi() - grid11.fi()) * dh2;
                //MessageBox.Show(dfi+" "+dl+" "+dh);
            }
            //MessageBox.Show(grid11.lambda() + "");
            WebGrid result = new WebGrid(B, L, dfi, dl, dh);
            return result;
        }

        //WYBÓR UKŁADU WSPÓŁRZĘDNYCH PLIKÓW: WEJŚĆIOWEGO I WYJŚCIOWEGO
        private void XY2000_CheckedChanged(object sender, EventArgs e)
        {
            switchGroupBoxVisibility(LongitudeChoice,LongitudeUTM);
            clearRadioButtonsCheck(longitudeUTM15, longitudeUTM21);
            this.start.Clear(); this.start.Append("Układ 2000"); loadFile.Enabled = true;
        }
        private void ResultXY2000_CheckedChanged(object sender, EventArgs e)
        {
            switchGroupBoxVisibility(resultLongitudeChoice, resultLongitudeUTM);
            clearRadioButtonsCheck(resultLongitudeUTM15,resultLongitudeUTM21);
            this.end.Clear(); this.end.Append("Układ 2000"); 
        }
        private void ResultXY1992_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(resultLongitudeUTM,resultLongitudeChoice);
            clearRadioButtonsCheck(resultLongitude15, resultLongitude18, resultLongitude21, resultLongitude24, resultLongitudeUTM15, resultLongitudeUTM21);
            this.end.Clear(); this.end.Append("Układ 1992"); 
        }
        private void XY1992_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(LongitudeUTM, LongitudeChoice);
            clearRadioButtonsCheck(longitude15, longitude18, longitude21, longitude24, longitudeUTM15, longitudeUTM21);
            this.start.Clear(); this.start.Append("Układ 1992"); loadFile.Enabled = true;
        }
        private void UTM_CheckedChanged(object sender, EventArgs e)
        {
            switchGroupBoxVisibility(LongitudeUTM,LongitudeChoice); 
            clearRadioButtonsCheck(longitude15, longitude18, longitude21, longitude24);
            this.start.Clear();
            this.start.Append("UTM"); loadFile.Enabled = true;
        }
        private void ResultUTM_CheckedChanged(object sender, EventArgs e)
        {
            switchGroupBoxVisibility(resultLongitudeUTM, resultLongitudeChoice);
            clearRadioButtonsCheck(resultLongitude15, resultLongitude18, resultLongitude21, resultLongitude24);
            this.end.Clear(); this.end.Append("UTM");
        }
        private void BLH_GRS_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(LongitudeUTM, LongitudeChoice);
            clearRadioButtonsCheck(longitude15, longitude18, longitude21, longitude24, longitudeUTM15, longitudeUTM21);
            this.start.Clear(); this.start.Append("BLH GRS80"); loadFile.Enabled = true;
        }
        private void XYZ_GRS_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(LongitudeUTM, LongitudeChoice);
            clearRadioButtonsCheck(longitude15, longitude18, longitude21, longitude24, longitudeUTM15, longitudeUTM21);
            this.start.Clear(); this.start.Append("XYZ GRS80"); loadFile.Enabled = true;
        }
        private void ResultBLH_GRS_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(resultLongitudeUTM, resultLongitudeChoice);
            clearRadioButtonsCheck(resultLongitude15, resultLongitude18, resultLongitude21, resultLongitude24, resultLongitudeUTM15, resultLongitudeUTM21);
            this.end.Clear(); this.end.Append("BLH GRS80");
        }
        private void ResultXYZ_GRS_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(resultLongitudeUTM, resultLongitudeChoice);
            clearRadioButtonsCheck(resultLongitude15, resultLongitude18, resultLongitude21, resultLongitude24, resultLongitudeUTM15, resultLongitudeUTM21);
            this.end.Clear(); this.end.Append("XYZ GRS80");
        }
        //WYBÓR UKŁADU WSPÓŁRZĘDNYCH: KRASOWSKI
        private void XY65RB_CheckedChanged(object sender, EventArgs e)
        {
            switchGroupBoxVisibility(XY65StrefaGB, XY42GB);
            clearRadioButtonsCheck(longitudeK15, longitudeK18, longitudeK21, longitudeK24, xy42width3, xy42width6);
            this.start.Clear(); this.start.Append("Układ 1965"); loadFile.Enabled = true;
        }
        private void XY42RB_CheckedChanged(object sender, EventArgs e)
        {
            switchGroupBoxVisibility(XY42GB, XY65StrefaGB);
            clearRadioButtonsCheck(xy65s1,xy65s2,xy65s3,xy65s4,xy65s5);
            this.start.Clear(); this.start.Append("Układ 1942"); loadFile.Enabled = true;
        }
        private void XYGUGIK80RB_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(XY65StrefaGB, XY42GB);
            clearRadioButtonsCheck(longitudeK15, longitudeK18, longitudeK21, longitudeK24, xy42width3, xy42width6);
            clearRadioButtonsCheck(xy65s1, xy65s2, xy65s3, xy65s4, xy65s5);
            this.start.Clear(); this.start.Append("Układ 1942"); loadFile.Enabled = true;
        }
        private void BLHKrasowskiRB_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(XY65StrefaGB, XY42GB);
            clearRadioButtonsCheck(longitudeK15, longitudeK18, longitudeK21, longitudeK24, xy42width3, xy42width6);
            clearRadioButtonsCheck(xy65s1, xy65s2, xy65s3, xy65s4, xy65s5);
            this.start.Clear(); this.start.Append("BLH Krasowski"); loadFile.Enabled = true;
        }
        private void XYZKrasowskiRB_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(XY65StrefaGB, XY42GB);
            clearRadioButtonsCheck(longitudeK15, longitudeK18, longitudeK21, longitudeK24, xy42width3, xy42width6);
            clearRadioButtonsCheck(xy65s1, xy65s2, xy65s3, xy65s4, xy65s5);
            this.start.Clear(); this.start.Append("XYZ Krasowski"); loadFile.Enabled = true;
        }
        private void ResXY65RB_CheckedChanged(object sender, EventArgs e)
        {
            switchGroupBoxVisibility(resXY65StrefaGB, resXY42GB);
            clearRadioButtonsCheck(reslongitudeK15, reslongitudeK18, reslongitudeK21, reslongitudeK24, resxy42width3, resxy42width6);
            this.end.Clear(); this.end.Append("Układ 1965");
        }
        private void ResXY42RB_CheckedChanged(object sender, EventArgs e)
        {
            switchGroupBoxVisibility(resXY42GB, resXY65StrefaGB);
            clearRadioButtonsCheck(resxy65s1, resxy65s2, resxy65s3, resxy65s4, resxy65s5);
            this.end.Clear(); this.end.Append("Układ 1942");
        }
        private void ResXYGUGIK80RB_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(resXY65StrefaGB, resXY42GB);
            clearRadioButtonsCheck(reslongitudeK15, reslongitudeK18, reslongitudeK21, reslongitudeK24, resxy42width3, resxy42width6);
            clearRadioButtonsCheck(resxy65s1, resxy65s2, resxy65s3, resxy65s4, resxy65s5);
            this.end.Clear(); this.end.Append("Układ 1942"); 
        }
        private void ResBLHKrasowskiRB_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(resXY65StrefaGB, resXY42GB);
            clearRadioButtonsCheck(reslongitudeK15, reslongitudeK18, reslongitudeK21, reslongitudeK24, resxy42width3, resxy42width6);
            clearRadioButtonsCheck(resxy65s1, resxy65s2, resxy65s3, resxy65s4, resxy65s5);
            this.end.Clear(); this.end.Append("BLH Krasowski");
        }
        private void ResXYZKrasowskiRB_CheckedChanged(object sender, EventArgs e)
        {
            setFalseGroupBoxVisibility(resXY65StrefaGB, resXY42GB);
            clearRadioButtonsCheck(reslongitudeK15, reslongitudeK18, reslongitudeK21, reslongitudeK24, resxy42width3, resxy42width6);
            clearRadioButtonsCheck(resxy65s1, resxy65s2, resxy65s3, resxy65s4, resxy65s5);
            this.end.Clear(); this.end.Append("XYZ Krasowski");
        }
        //USTAWIENIA WYBORU PASÓW UKŁADU PUŁKOWO 42'
        private void Xy42width3_CheckedChanged(object sender, EventArgs e)
        {
            XY42LongitudeGB.Visible = true;
            clearRadioButtonsCheck(longitudeK18, longitudeK24);
            longitudeK18.Visible = false; longitudeK24.Visible = false;
        }
        private void Xy42width6_CheckedChanged(object sender, EventArgs e)
        {
            XY42LongitudeGB.Visible = true;
            longitudeK18.Visible = true; longitudeK24.Visible = true;
        }
        //WCZYTYWANIE PLIKU WEJŚCIOWEGO
        [STAThread]
        private void LoadFile_Click_1(object sender, EventArgs e)
        {
            using (OFD)
            {
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    filePath.Text = OFD.FileName;
                }
            }
        }
        //USTAWIENIA PARAMETRÓ DLA STREF UKŁADU 1965
        private void Xy65s1_CheckedChanged(object sender, EventArgs e)
        {
            this.longitude65 = 21 + 5 / 60; this.x0 = 5467000.00; this.y0 = 4637000.00;
            this.R0 = 6382390.164984; this.xGK0 = 5610467.577042;
        }
        private void Resxy65s1_CheckedChanged(object sender, EventArgs e)
        {
            this.resLongitude65 = 21 + 5 / 60; this.resx0 = 5467000.00; this.resy0 = 4637000.00;
            this.resR0 = 6382390.164984; this.resXGK0 = 5610467.577042;
        }
        private void Xy65s2_CheckedChanged(object sender, EventArgs e)
        {
            this.longitude65 = 21 + 30 / 60 + 10 / 3600; this.x0 = 5806000.00; this.y0 = 4603000.00;
            this.R0 = 6384119.427305; this.xGK0 = 5874939.874115;
        }
        private void Resxy65s2_CheckedChanged(object sender, EventArgs e)
        {
            this.resLongitude65 = 21 + 30 / 60 + 10 / 3600; this.resx0 = 5806000.00; this.resy0 = 4603000.00;
            this.resR0 = 6384119.427305; this.resXGK0 = 5874939.874115;
        }
        private void Xy65s3_CheckedChanged(object sender, EventArgs e)
        {
            this.longitude65 = 17 + 30 / 3600; this.x0 = 5999000.00; this.y0 = 3501000.00;
            this.R0 = 6384536.793566; this.xGK0 = 5939644.770112;
        }
        private void Resxy65s3_CheckedChanged(object sender, EventArgs e)
        {
            this.resLongitude65 = 17 + 30 / 3600; this.resx0 = 5999000.00; this.resy0 = 3501000.00;
            this.resR0 = 6384536.793566; this.resXGK0 = 5939644.770112;
        }
        private void Xy65s4_CheckedChanged(object sender, EventArgs e)
        {
            this.longitude65 = 16 + 40 / 60 + 20 / 3600; this.x0 = 5627000.00; this.y0 = 3703000.00;
            this.R0 = 6383155.165130; this.xGK0 = 5726819.667829;
        }
        private void Resxy65s4_CheckedChanged(object sender, EventArgs e)
        {
            this.resLongitude65 = 16 + 40 / 60 + 20 / 3600; this.resx0 = 5627000.00; this.resy0 = 3703000.00;
            this.resR0 = 6383155.165130; this.resXGK0 = 5726819.667829;
        }
        private void Xy65s5_CheckedChanged(object sender, EventArgs e)
        {
            this.longitude65 = 18 + 57 / 60 + 30 / 3600; this.x0 = -4700000.00; this.y0 = 237000.00;
            this.R0 = 0; this.xGK0 = 0;
        }
        //WYBÓR UKŁADU ODNIESIENIA PLIKÓW: WEJŚCIOWEGO I WYJŚCIOWEGO // WYBÓR ELIPSOIDY ODNIESIENIA
        private void ETRF2000_CheckedChanged(object sender, EventArgs e)
        {
            clearRadioButtonsCheck(XY65RB, XY42RB, XYGUGIK80RB, BLHKrasowskiRB, XYZKrasowskiRB);
            switchGroupBoxVisibility(ChoiceOne,KrasowskiGB);
            clearRadioButtonsCheck(xy65s1,xy65s2,xy65s3,xy65s4,xy65s5,xy42width3,xy42width6);
            clearRadioButtonsCheck(longitudeK15, longitudeK18, longitudeK21, longitudeK24);
            setFalseGroupBoxVisibility(XY65StrefaGB, XY42GB);
            this.startETRF.Clear();
            this.startETRF.Append("ETRF2000");
        }
        private void ETRF89_CheckedChanged(object sender, EventArgs e)
        {
            clearRadioButtonsCheck(XY65RB, XY42RB, XYGUGIK80RB, BLHKrasowskiRB, XYZKrasowskiRB);
            switchGroupBoxVisibility(ChoiceOne, KrasowskiGB);
            clearRadioButtonsCheck(xy65s1, xy65s2, xy65s3, xy65s4, xy65s5, xy42width3, xy42width6);
            clearRadioButtonsCheck(longitudeK15, longitudeK18, longitudeK21, longitudeK24);
            setFalseGroupBoxVisibility(XY65StrefaGB, XY42GB);
            this.startETRF.Clear();
            this.startETRF.Append("ETRF89");
        }
        private void ResETRF2000_CheckedChanged(object sender, EventArgs e)
        {
            clearRadioButtonsCheck(resXY65RB, resXY42RB, resXYGUGIK80RB, resBLHKrasowskiRB, resXYZKrasowskiRB);
            switchGroupBoxVisibility(ChoiceTwo, resKrasowskiGB);
            clearRadioButtonsCheck(resxy65s1, resxy65s2, resxy65s3, resxy65s4, resxy65s5, resxy42width3, resxy42width6);
            clearRadioButtonsCheck(reslongitudeK15, reslongitudeK18, reslongitudeK21, reslongitudeK24);
            setFalseGroupBoxVisibility(resXY65StrefaGB, resXY42GB);
            this.endETRF.Clear();
            this.endETRF.Append("ETRF2000");
        }
        private void ResETRF89_CheckedChanged(object sender, EventArgs e)
        {
            clearRadioButtonsCheck(resXY65RB, resXY42RB, resXYGUGIK80RB, resBLHKrasowskiRB, resXYZKrasowskiRB);
            switchGroupBoxVisibility(ChoiceTwo, resKrasowskiGB);
            clearRadioButtonsCheck(resxy65s1, resxy65s2, resxy65s3, resxy65s4, resxy65s5, resxy42width3, resxy42width6);
            clearRadioButtonsCheck(reslongitudeK15, reslongitudeK18, reslongitudeK21, reslongitudeK24);
            setFalseGroupBoxVisibility(resXY65StrefaGB, resXY42GB);
            this.endETRF.Clear();
            this.endETRF.Append("ETRF89");
        }
        private void KrasowskiRB_CheckedChanged(object sender, EventArgs e)
        {
            clearRadioButtonsCheck(XY2000, XY1992, UTM, XYZ_GRS, BLH_GRS);
            switchGroupBoxVisibility(KrasowskiGB, ChoiceOne);
            clearRadioButtonsCheck(longitude15, longitude18, longitude21, longitude24, longitudeUTM15, longitudeUTM21);
            setFalseGroupBoxVisibility(LongitudeChoice, LongitudeUTM);
            this.startETRF.Clear();
            this.startETRF.Append("Krasowski");
        }
        private void ResKrasowskiRB_CheckedChanged(object sender, EventArgs e)
        {
            clearRadioButtonsCheck(resultXY2000, resultXY1992, resultUTM, resultXYZ_GRS, resultBLH_GRS);
            switchGroupBoxVisibility(resKrasowskiGB, ChoiceTwo);
            clearRadioButtonsCheck(resultLongitude15, resultLongitude18, resultLongitude21, resultLongitude24, resultLongitudeUTM15, resultLongitudeUTM21);
            setFalseGroupBoxVisibility(resultLongitudeChoice, resultLongitudeUTM);
            this.endETRF.Clear();
            this.endETRF.Append("Krasowski");
        }

        //WYBÓR FORMATU BLH PLIKÓW: WEJŚCIOWEGO I WYJŚCIOWEGO
        private void DegreesBLH_CheckedChanged(object sender, EventArgs e)
        {
            this.degreeForm = true;
        }
        private void DegMinSecBLH_CheckedChanged(object sender, EventArgs e)
        {
            this.degreeForm = false;
        }
        private void ResDegreesBLH_CheckedChanged(object sender, EventArgs e)
        {
            this.resultDegreeForm = true;
        }
        private void ResDegMinSecBLH_CheckedChanged(object sender, EventArgs e)
        {
            this.resultDegreeForm = false;
        }
        //TRANSFORMACJE WSPÓŁRZĘDNYCH DLA TEGO SAMEGO UKŁADU ODNIESIENIA
        public List<Point> U2000ToGK(List<Point> Points, byte longitude)
        {
            List<Point> result = new List<Point>();
            Points.ForEach(p =>
            {
                double xGK = p.x() / 0.999923;
                double yGK = (p.y() - 500000 - 1000000 * longitude / 3) / 0.999923;

                result.Add(new Point(p.Name(), xGK, yGK));
                //MessageBox.Show(xGK + " " + yGK);
            });
            return result;
        } //działa
        public List<Point> U1992ToGK(List<Point> Points)
        {
            List<Point> result = new List<Point>();
            Points.ForEach(p =>
            {
                double xGK = (p.x() + 5300000) / 0.9993;
                double yGK = (p.y() - 500000) / 0.9993;
                result.Add(new Point(p.Name(), xGK, yGK));
                MessageBox.Show(xGK + " " + yGK);
            });
            return result;
        } //działa
        public List<Point> GKToU2000(List<Point> Points, byte longitude)
        {
            List<Point> result = new List<Point>();
            Points.ForEach(p =>
            {
                double x = p.x() * 0.999923;
                double y = p.y()* 0.999923 + 500000 + 1000000 * longitude / 3;
            result.Add(new Point(p.Name(), x, y));
            });
            return result;
        } //działa
        public List<Point> GKToU1992(List<Point> Points)
        {
            List<Point> result = new List<Point>();
            Points.ForEach(p =>
            {
                double x = p.x() * 0.9993 - 5300000;
                double y = p.y() *0.9993 + 500000;
                result.Add(new Point(p.Name(), x, y));
            });
            return result;
        } //działa
        public List<Point3D> BLH2XYZ(List<PointBLH> PointsBLH)
        {
            double a = 6378137;
            double e2 = 0.006694380022903;
            List<Point3D> result = new List<Point3D>();
            PointsBLH.ForEach(x =>
            {
                if (!x.Format()) { x.convertToDegrees(); }               
                string name = x.Name();
                double N = a / (Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(x.fi() * Math.PI / 180), 2)));
                //MessageBox.Show("N: " + N);
                double X = (N + x.height()) * Math.Cos(x.fi() * Math.PI / 180) * Math.Cos(x.lambda() * Math.PI / 180);
                double Y = (N + x.height()) * Math.Cos(x.fi() * Math.PI / 180) * Math.Sin(x.lambda() * Math.PI / 180);
                double Z = (N + x.height()) * Math.Sin(x.fi() * Math.PI / 180) - e2 * N * Math.Sin(x.fi() * Math.PI / 180);
                result.Add(new Point3D(name, X, Y, Z));
            });
            return result;
        } //działa
        public List<PointBLH> XYZ2BLH(List<Point3D> Points3D, double precision)
        {
            List<PointBLH> result = new List<PointBLH>();
            Points3D.ForEach(point =>
            {
                int iterator = 1;
                double B = 0; double H = 0;
                double a = 6378137;
                double e2 = 0.006694380022903;
                double L = Math.Atan(point.y() / point.x())*180/Math.PI;
                double dif = 1;
                double tangensB = point.z() / Math.Sqrt(point.x() * point.x() + point.y() * point.y()) * (1 / (1 - e2));
                while (dif > precision)
                {
                    B = Math.Atan(tangensB);
                    double N = a / Math.Sqrt(1 - e2 * Math.Sin(B) * Math.Sin(B));
                    H = (Math.Sqrt(Math.Pow(point.x(), 2) + Math.Pow(point.y(), 2)) / Math.Cos(B)) - N;                    
                    double tangensB1 = point.z() / Math.Sqrt(Math.Pow(point.x(), 2) + Math.Pow(point.y(), 2)) * 1 / (1 - e2 * (N / (N + H)));
                    double B1 = Math.Atan(tangensB1);
                    dif = Math.Abs(B1 - B);
                    tangensB = tangensB1; iterator++;
                    B = B1;
                }
                //MessageBox.Show(iterator.ToString());
                result.Add(new PointBLH(point.Name(), B*180/Math.PI, L, H));
            });
            return result;
        } //działa
        public List<Point> BLH2XYGK(List<PointBLH> PointsBLH, byte longitude)
        {
            List<Point> result = new List<Point>();
            double a = 6378137;
            double e2 = 0.006694380022903;
            double e12 = 0.00673949677548;
            double longitude0 = longitude * Math.PI / 180;
            double A0 = 1 - (e2 / 4) - (3 * Math.Pow(e2,2) / 64) - (5 * Math.Pow(e2,3) / 256);
            double A2 = ((e2 + (Math.Pow(e2,2) / 4) + (15 * Math.Pow(e2,3)) / 128));  A2 *= 0.375;
            double A4 = ((Math.Pow(e2,2) + 3 * Math.Pow(e2,3) / 4)); A4 *= 0.05859375;
            double A6 = 35 * Math.Pow(e2,3) / 3072;

            PointsBLH.ForEach(p =>
            {
                if (!p.Format()) { p.convertToDegrees(); }
                double fi = p.fi() * Math.PI / 180; double lambda = p.lambda() * Math.PI / 180;
                double sigma = a * (A0 * fi - A2 * Math.Sin(2 * fi) + A4 * Math.Sin(4 * fi) - A6 * Math.Sin(6 * fi));
                double l = lambda - longitude0;
                double t = Math.Tan(fi);
                double eta2 = (e12) * Math.Pow(Math.Cos(fi),2);
                double N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(fi), 2));
  double xGK = sigma + (Math.Pow(l,2) / 2) * N * Math.Sin(fi) * Math.Cos(fi) * (1 + (Math.Pow(l, 2) / 12) * (Math.Pow(Math.Cos(fi),2)) * (5 - Math.Pow(t,2) + 9 * (eta2) + 4 * Math.Pow(eta2,2)) + (Math.Pow(l,4) / 360) * Math.Pow(Math.Cos(fi),4) * (61 - 58 * Math.Pow(t,2) + Math.Pow(t,4) + 270 * (eta2) - 330 * (eta2) * Math.Pow(t,2)));
  double yGK = l * N * Math.Cos(fi) * (1 + (Math.Pow(l,2) / 6) * Math.Pow(Math.Cos(fi),2) * (1 - Math.Pow(t,2) + eta2) + (Math.Pow(l,4) / 120) * Math.Pow(Math.Cos(fi),4) * (5 - 18 * Math.Pow(t,2) + Math.Pow(t,4) + 14 * (eta2) - 58 * (eta2) * Math.Pow(t,2)));
                result.Add(new Point(p.Name(), xGK, yGK));
            });
            return result; 
        } //działa
        public List<PointBLH> XYGK2BLH(List<Point> Points, byte longitude, double precision)
        {
            List<PointBLH> result = new List<PointBLH>();
            double a = 6378137;
            double e2 = 0.006694380022903;
            double e12 = 0.00673949677548;
            double longitude0 = longitude * Math.PI / 180;
            double A0 = 1 - (e2 / 4) - (3 * Math.Pow(e2, 2) / 64) - (5 * Math.Pow(e2, 3) / 256);
            double A2 =(e2 + (Math.Pow(e2, 2) / 4) + 15 * Math.Pow(e2, 3) / 128); A2 *= 0.375;
            double A4 = (Math.Pow(e2, 2) + 3 * Math.Pow(e2, 3) / 4); A4 *= 0.05859375;
            double A6 = 35 * Math.Pow(e2, 3) / 3072;
            Points.ForEach(p =>
            {
                double epsilon = 1;
                double sigma = p.x();
                //MessageBox.Show(sigma.ToString());
                double fi0 = sigma / a * A0;
                while (epsilon > precision)
                {
                    double fi1 = ((sigma / a) + A2 * Math.Sin(2 * fi0) - A4 * Math.Sin(4 * fi0) + A6 * Math.Sin(6 * fi0)) / A0;
                    epsilon = Math.Abs(fi1 - fi0);
                    fi0 = fi1;
                    //MessageBox.Show(fi0.ToString());
                }
                double N = a / Math.Sqrt(1 - e2 * Math.Sin(fi0) * Math.Sin(fi0));
                double M = a * (1 - e2) / Math.Pow(Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(fi0), 2)), 3);
                double t = Math.Tan(fi0);
                double eta2 = (e12) * Math.Cos(fi0) * Math.Cos(fi0);
                //MessageBox.Show(eta2.ToString());
                double L = longitude*Math.PI/180 + p.y() / (N * (Math.Cos(fi0)))
                * (1 - (Math.Pow(p.y(),2) / (6 * Math.Pow(N,2))) * (1 + 2 * Math.Pow(t,2) + (eta2))
                + (Math.Pow(p.y(),4) / (120 * Math.Pow(N,4)))
                * (5 + 28 * Math.Pow(t,2) + 24 * Math.Pow(t,4) + 6 * (eta2) + 8 * (eta2) * Math.Pow(t,2)));
                double B = fi0 - ((Math.Pow(p.y(),2) * t) / (2 * M * N)) * (1 - (Math.Pow(p.y(),2) / (12 * Math.Pow(N,2))) * (5 + 3 * Math.Pow(t,2) + (eta2) - 9 * eta2 * Math.Pow(t,2) - 4 * Math.Pow(eta2,2)) + Math.Pow(p.y(),4) / (360 * Math.Pow(N,4)) * (61 + 90 * Math.Pow(t,2) + 45 * Math.Pow(t,4)));
                result.Add(new PointBLH(p.Name(), B * 180 / Math.PI, L*180/Math.PI, 0 ));
                
            });
            return result;
        } //działa
        public List<Point> BLH2UTM(List<PointBLH> PointsBLH, byte longitude)
        {
            List<Point> result = new List<Point>();
            double m0 = 0.9996;
            double longitude0 = longitude * Math.PI / 180;
            double e2 = 0.00669438;
            double a = 6378137;
            double b = 6356752.3142;
            e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);
            double A0 = 1 - (e2 / 4) - (3 * Math.Pow(e2, 2) / 64) - (5 * Math.Pow(e2, 3) / 256);
            double A2 = (e2 + (Math.Pow(e2, 2) / 4) + 15 * Math.Pow(e2, 3) / 128); A2 *= 0.375;
            double A4 = (Math.Pow(e2, 2) + 3 * Math.Pow(e2, 3) / 4); A4 *= 0.05859375;
            double A6 = 35 * Math.Pow(e2, 3) / 3072;
            PointsBLH.ForEach(p =>
            {
                if (!p.Format()) { p.convertToDegrees(); }
                double fi = p.fi() * Math.PI / 180; double lambda = p.lambda() * Math.PI / 180;
                double l = lambda - longitude0;
                double t = Math.Tan(fi);
                double sigma = a * (A0 * fi - A2 * Math.Sin(2 * fi) + A4 * Math.Sin(4 * fi) - A6 * Math.Sin(6 * fi));
                double N = a / Math.Sqrt(1 - e2 * Math.Sin(fi) * Math.Sin(fi));
                double M = a * (1 - e2) / Math.Pow(Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(fi), 2)), 3);
                double psi = N / M;
                double xUTM = sigma + N * Math.Pow(l, 2) / 2 * Math.Sin(fi) * Math.Cos(fi) + N * Math.Pow(l, 4) / 24 * Math.Sin(fi) * Math.Pow(Math.Cos(fi), 3) * (psi + 4 * Math.Pow(psi, 2) - Math.Pow(t, 2));
                xUTM += N * Math.Pow(l, 6) / 720 * Math.Sin(fi) * Math.Pow(Math.Cos(fi), 5) * (8 * Math.Pow(psi, 4) * (11-24*Math.Pow(t,2)) - 28*Math.Pow(psi,3)*(1- 6*Math.Pow(t,2)) + Math.Pow(psi,2)*(1-32*Math.Pow(t,2)) - 2*psi*Math.Pow(t,2) + Math.Pow(t,4));
                xUTM += N * Math.Pow(l, 8) / 40320 * Math.Sin(fi) * Math.Pow(Math.Cos(fi), 7) * (1385 - 3111 * Math.Pow(t, 2) + 543 * Math.Pow(t, 4) - Math.Pow(t, 6));
                xUTM *= m0;
                double t2 = Math.Pow(t, 2); double t4 = Math.Pow(t, 4); double t6 = Math.Pow(t, 6);
                double l3 = Math.Pow(l, 3); double l5 = Math.Pow(l, 5); double l7 = Math.Pow(l, 7);
                double cos3fi = Math.Pow(Math.Cos(fi), 3); double cos5fi = Math.Pow(Math.Cos(fi), 5); double cos7fi = Math.Pow(Math.Cos(fi), 7);
                double psi2 = Math.Pow(psi, 2); double psi3 = Math.Pow(psi, 3);
                double yUTM = N * l * Math.Cos(fi) + N * l3 / 6 * cos3fi * (psi - t2) + N * l5 / 120 * cos5fi * (4 * psi3 * (1 - 6 * t2) + psi2 * (1 + 8 * t2) -2*psi*t2 + t4);
                yUTM += N * l7 / 5040 * cos7fi * (61 - 479 * t2 + 179 * t4 - t6); yUTM *= m0;
                //MessageBox.Show(yUTM.ToString());
                yUTM +=500000;
                int n = longitude.Equals(15) ? 3 : 4; yUTM += n * 1000000;               
                result.Add(new Point(p.Name(), xUTM, yUTM));
            });
            return result;
        } //nie działa
        public List<PointBLH> UTM2BLH(List<Point> Points, byte longitude, double precision)
        {
            List<PointBLH> result = new List<PointBLH>();
            double m0 = 0.9996;
            double longitude0 = longitude * Math.PI / 180;
            double e2 = 0.00669438;
            double a = 6378137;
            double b = 6356752.3142;
            e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);
            //MessageBox.Show(e2.ToString());
            double A0 = 1 - (e2 / 4) - (3 * Math.Pow(e2, 2) / 64) - (5 * Math.Pow(e2, 3) / 256);
            double A2 = (e2 + (Math.Pow(e2, 2) / 4) + 15 * Math.Pow(e2, 3) / 128); A2 *= 0.375;
            double A4 = (Math.Pow(e2, 2) + 3 * Math.Pow(e2, 3) / 4); A4 *= 0.05859375;
            double A6 = 35 * Math.Pow(e2, 3) / 3072;
            //MessageBox.Show(A0 + " " + A2 + " " + A4 + " " + A6);
            Points.ForEach(p =>
            {
                double epsilon = 1;
                double sigma = p.x()/m0;
                double fi0 = sigma / a * A0;
                while (epsilon > precision)
                {
                    double fi1 = ((sigma / a) + A2 * Math.Sin(2 * fi0) - A4 * Math.Sin(4 * fi0) + A6 * Math.Sin(6 * fi0)) / A0;
                    epsilon = Math.Abs(fi1 - fi0);
                    fi0 = fi1;
                }
                
                double N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(fi0), 2));
                double M = a * (1 - e2) / Math.Pow(Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(fi0), 2)), 3); MessageBox.Show(M.ToString());
                double psi = N / M; double psi2 = Math.Pow(psi, 2); double psi4 = Math.Pow(psi, 4); double psi3 = Math.Pow(psi, 3);
                double t = Math.Tan(fi0); double t2 = Math.Pow(t, 2); double t4 = Math.Pow(t, 4); double t6 = Math.Pow(t, 6);
                double v = N; double v3 = Math.Pow(v, 3); double v5 = Math.Pow(v, 5); double v7 = Math.Pow(v, 7);
                double E = p.y(); int n = longitude.Equals(15) ? 3 : 4; E -= 500000; E -= (n * 1000000);
                double E2 = Math.Pow(E, 2); double E4 = Math.Pow(E, 4); double E6 = Math.Pow(E, 6); double E8 = Math.Pow(E, 8);
                double E3 = Math.Pow(E, 3); double E5 = Math.Pow(E, 5); double E7 = Math.Pow(E, 7);
                double m03 = Math.Pow(m0, 3); double m05 = Math.Pow(m0, 5); double m07 = Math.Pow(m0, 7);
                double element = E2 / (2 * m0 * v) - E4 / (24 * m03 * v3) * (-4 * psi2 + 9 * psi * (1 - t2) + 12 * t2);
                element += (E6 / (720 * m05 * v5)) * (8 * psi4 * (11 - 24 * t2) - 12 * psi3 * (21 - 71 * t2) 
                + 15 * psi2 * (15 - 98 * t2 + 15 * t4) + 180 * psi * (5 * t2 - 3 * t4) + 360 * t4);
                element -= (E8 / (40320 * m07 * v7) * (1385 + 3633 * t2 + 4095 * t4 + 1575 * t6));
                element *= (t / (m0 * M));
                double B = fi0 - element;
                double L = E / (m0 * v) - (E3 / (6 * m03 * v3)) * (psi + 2 * t2);
                L += E5 / (120 * m05 * v5) * (-4 * psi3 * (1 - 6 * t2) + psi2 * (9 - 68 * t2) + 72 * psi * t2 + 24 * t4);
                L -= E7 / (5040 * m07 * v7) * (61 + 662 * t2 + 1320 * t4 + 720 * t6);
                L *= (1 / Math.Cos(fi0));
                result.Add(new PointBLH(p.Name(), B * 180 / Math.PI, L * 180 / Math.PI + longitude, 0));
            });
            return result;
        } //nie działa
        //Transformacje współrzędnych dla elipsoidy Krasowskiego
        public List<Point> Krasowski2XY65(List<PointBLH> Points, double x0, double y0, double R0, byte longitude, double xGK0)
        {
            /*xGK0 będzie równe zeru przy wyborze strefy 5. Niezależnie nie wpłynie na obliczenia.*/
            List<Point> result = new List<Point>();
            List<Point> helper = Krasowski2XYGK(Points, longitude);
            if(this.strefa>0 && this.strefa < 5)
            {
                helper.ForEach(p =>
                {
                    double m0 = 0.9998;
                    double xR = (2*R0*Math.Sin((p.x()-xGK0)/R0))/(Math.Cos((p.x() - xGK0) / R0)+Math.Cosh(p.y()/R0));
                    double yR = (2 * R0 * Math.Sinh(p.y() / R0)) / (Math.Cos((p.x() - xGK0) / R0) + Math.Cosh(p.y() / R0));
                    double x65 = m0 * xR + x0;
                    double y65 = m0 * yR + y0;
                    result.Add(new Point(p.Name(), x65, y65));
                });
            }
            else if(this.strefa.Equals(5))
            {
                double m0 = 0.999983;
                helper.ForEach(p =>
                {
                    double x65 = m0 * p.x() + x0; double y65 = m0 * p.y() + y0;
                    result.Add(new Point(p.Name(), x65, y65));
                });
            }
            return result;
        }
        public List<PointBLH> XY65ToKrasowski(List<Point> Points, double x0, double y0, double R0, byte longitude, double xGK0, double precision)
        {
            List<PointBLH> result = new List<PointBLH>();
            List<Point> helper = new List<Point>();
            Points.ForEach(p=> {
                double m0 = this.strefa.Equals(5) ? 0.999983 : 0.9998;
                double xR = (p.x() - x0) / m0; double yR = (p.y() - y0) / m0;
                helper.Add(new Point(p.Name(), xR, yR));
            });
            if (this.strefa.Equals(5))
            {
                result = XYGK2Krasowski(helper, longitude, precision);
            }
            else if(this.strefa>0 && this.strefa < 5)
            {
                List<Point> helper2 = new List<Point>();
                helper.ForEach(p =>
                {
                    double xR = p.x(); double xR2 = Math.Pow(xR, 2); double xR3 = Math.Pow(xR, 3); double xR4 = Math.Pow(xR, 4);
                    double xR5 = Math.Pow(xR, 5); double xR6 = Math.Pow(xR, 6); double xR7 = Math.Pow(xR, 7);
                    double yR = p.y(); double yR2 = Math.Pow(yR, 2); double yR3 = Math.Pow(yR, 3); double yR4 = Math.Pow(yR, 4);
                    double yR5 = Math.Pow(yR, 5); double yR6 = Math.Pow(yR, 6); double yR7 = Math.Pow(yR, 7);
                    double R02 = Math.Pow(R0, 2); double R04 = Math.Pow(R0, 4); double R06 = Math.Pow(R0, 6);
                    double xGK = xGK0 + xR - xR3 / (12 * R02) + xR * yR2 / (4 * R02) + xR5 / (80 * R04) - xR3 * yR2 / (8 * R04);
                    xGK += xR * yR4 / (16 * R04) - xR7 / (448 * R06) + 3 * xR5 * yR2 / (64 * R06) - 5 * xR3 * yR4 / (64 * R06) + xR * yR6 / (64 * R06);
                    double yGK = yR - xR2 * yR / (4 * R02) + yR3 / (12 * R02) + xR4 * yR / (16 * R04) - xR2 * yR3 / (8 * R06) + yR5 / (80 * R04) - xR6 * yR / (64 * R06);
                    yGK += 5 * xR4 * yR3 / (64 * R06) - 3 * xR2 * yR5 / (64 * R06) + yR7 / (448 * R06);
                    helper2.Add(new Point(p.Name(), xGK, yGK));
                });
                result = XYGK2Krasowski(helper2, longitude, precision);
            }
            return result;
        }
        public List<Point> Krasowski2XYGK(List<PointBLH> Points, byte longitude)
        {
            List<Point> result = new List<Point>();
            double a = 6378245.000;
            double b = 6356863.019;
            double e2 = 0.00669342;
            e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);
            double e12;
            ////e12 = (Math.Pow(b, 2) - Math.Pow(a, 2)) / Math.Pow(b, 2);
            e12 = e2 / (1 - e2);
            double longitude0 = longitude * Math.PI / 180;
            double A0 = 1 - (e2 / 4) - (3 * Math.Pow(e2, 2) / 64) - (5 * Math.Pow(e2, 3) / 256);
            double A2 = ((e2 + (Math.Pow(e2, 2) / 4) + (15 * Math.Pow(e2, 3)) / 128)); A2 *= 0.375;
            double A4 = ((Math.Pow(e2, 2) + 3 * Math.Pow(e2, 3) / 4)); A4 *= 0.05859375;
            double A6 = 35 * Math.Pow(e2, 3) / 3072;

            PointsBLH.ForEach(p =>
            {
                if (!p.Format()) { p.convertToDegrees(); }
                double fi = p.fi() * Math.PI / 180; double lambda = p.lambda() * Math.PI / 180;
                double sigma = a * (A0 * fi - A2 * Math.Sin(2 * fi) + A4 * Math.Sin(4 * fi) - A6 * Math.Sin(6 * fi));
                double l = lambda - longitude0;
                double t = Math.Tan(fi);
                double eta2 = (e12) * Math.Pow(Math.Cos(fi), 2);
                double N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(fi), 2));
                double xGK = sigma + (Math.Pow(l, 2) / 2) * N * Math.Sin(fi) * Math.Cos(fi) * (1 + (Math.Pow(l, 2) / 12) * (Math.Pow(Math.Cos(fi), 2)) * (5 - Math.Pow(t, 2) + 9 * (eta2) + 4 * Math.Pow(eta2, 2)) + (Math.Pow(l, 4) / 360) * Math.Pow(Math.Cos(fi), 4) * (61 - 58 * Math.Pow(t, 2) + Math.Pow(t, 4) + 270 * (eta2) - 330 * (eta2) * Math.Pow(t, 2)));
                double yGK = l * N * Math.Cos(fi) * (1 + (Math.Pow(l, 2) / 6) * Math.Pow(Math.Cos(fi), 2) * (1 - Math.Pow(t, 2) + eta2) + (Math.Pow(l, 4) / 120) * Math.Pow(Math.Cos(fi), 4) * (5 - 18 * Math.Pow(t, 2) + Math.Pow(t, 4) + 14 * (eta2) - 58 * (eta2) * Math.Pow(t, 2)));
                result.Add(new Point(p.Name(), xGK, yGK));
            });
            return result;
        }
        public List<PointBLH> XYGK2Krasowski(List<Point> Points, double longitude, double precision)
        {
            List<PointBLH> result = new List<PointBLH>();
            double a = 6378245.000;
            double b = 6356863.019;
            double e2 = 0.00669342;
            e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);
            double e12 = e2 / (1 - e2);
            double longitude0 = longitude * Math.PI / 180;
            double A0 = 1 - (e2 / 4) - (3 * Math.Pow(e2, 2) / 64) - (5 * Math.Pow(e2, 3) / 256);
            double A2 = (e2 + (Math.Pow(e2, 2) / 4) + 15 * Math.Pow(e2, 3) / 128); A2 *= 0.375;
            double A4 = (Math.Pow(e2, 2) + 3 * Math.Pow(e2, 3) / 4); A4 *= 0.05859375;
            double A6 = 35 * Math.Pow(e2, 3) / 3072;
            Points.ForEach(p =>
            {
                double epsilon = 1;
                double sigma = p.x();
                //MessageBox.Show(sigma.ToString());
                double fi0 = sigma / a * A0;
                while (epsilon > precision)
                {
                    double fi1 = ((sigma / a) + A2 * Math.Sin(2 * fi0) - A4 * Math.Sin(4 * fi0) + A6 * Math.Sin(6 * fi0)) / A0;
                    epsilon = Math.Abs(fi1 - fi0);
                    fi0 = fi1;
                    //MessageBox.Show(fi0.ToString());
                }
                double N = a / Math.Sqrt(1 - e2 * Math.Sin(fi0) * Math.Sin(fi0));
                double M = a * (1 - e2) / Math.Pow(Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(fi0), 2)), 3);
                double t = Math.Tan(fi0);
                double eta2 = (e12) * Math.Cos(fi0) * Math.Cos(fi0);
                //MessageBox.Show(eta2.ToString());
                double L = longitude * Math.PI / 180 + p.y() / (N * (Math.Cos(fi0)))
                * (1 - (Math.Pow(p.y(), 2) / (6 * Math.Pow(N, 2))) * (1 + 2 * Math.Pow(t, 2) + (eta2))
                + (Math.Pow(p.y(), 4) / (120 * Math.Pow(N, 4)))
                * (5 + 28 * Math.Pow(t, 2) + 24 * Math.Pow(t, 4) + 6 * (eta2) + 8 * (eta2) * Math.Pow(t, 2)));
                double B = fi0 - ((Math.Pow(p.y(), 2) * t) / (2 * M * N)) * (1 - (Math.Pow(p.y(), 2) / (12 * Math.Pow(N, 2))) * (5 + 3 * Math.Pow(t, 2) + (eta2) - 9 * eta2 * Math.Pow(t, 2) - 4 * Math.Pow(eta2, 2)) + Math.Pow(p.y(), 4) / (360 * Math.Pow(N, 4)) * (61 + 90 * Math.Pow(t, 2) + 45 * Math.Pow(t, 4)));
                result.Add(new PointBLH(p.Name(), B * 180 / Math.PI, L * 180 / Math.PI, 0));

            });
            return result;
        }
        public List<Point> Krasowski2XY42(List<PointBLH> Points, byte longitude, double precision, bool stripesSize)
        {
            //Parametr stripesSize określa czy wybrano pasy trzystopniowe(true) czy sześciostopniowe(false) dla układu Pułkowo 42'.
            List<Point> result = new List<Point>();
            List<Point> helper = Krasowski2XYGK(Points, longitude);
            if (stripesSize)
            {
                helper.ForEach(p =>
                {
                    double X42 = p.x();
                    double Y42 = p.y() + 500000 + longitude / 3 * 1000000;
                    result.Add(new Point(p.Name(), X42, Y42));
                });
            }
            else
            {
                helper.ForEach(p =>
                {
                    double X42 = p.x();
                    double Y42 = p.y() + 500000 + (longitude + 3 )/ 6 * 1000000;
                    result.Add(new Point(p.Name(), X42, Y42));
                });
            }
            return result;
        }
        public List<PointBLH> XY42ToKrasowski(List<Point> Points, byte longitude, double precision, bool stripesSize)
        {
            //Parametr stripesSize określa czy wybrano pasy trzystopniowe(true) czy sześciostopniowe(false) dla układu Pułkowo 42'.
            List<PointBLH> result = new List<PointBLH>();
            List<Point> helper = new List<Point>();
            if (stripesSize)
            {
                Points.ForEach(p => {
                    double xGK = p.x(); double yGK = p.y() - 500000 - longitude / 3 * 1000000;
                    helper.Add(new Point(p.Name(), xGK, yGK));
                });
            }
            else
            {
                Points.ForEach(p => {
                    double xGK = p.x(); double yGK = p.y() - 500000 - (longitude + 3)/ 6 * 1000000;
                    helper.Add(new Point(p.Name(), xGK, yGK));
                });
            }
            result = XYGK2Krasowski(helper, longitude, precision);
            return result;
        }
        public List<Point> Krasowski2GUGIK80(List<PointBLH> Points, double xGK0)
        {
            List<Point> result = new List<Point>();
            double x0 = 500000; double y0 = 500000; double m0 = 0.999714;
            double longitude = 19 + 10 / 60; double R0 = 215000;
            return result;
        }
        //SCENARIUSZE TRANSFORMACYJNE: 20 GŁÓWNYCH PERMUTACJI
        //PRECISION ZAWSZE ODNOSI SIĘ DO DOKŁADNOŚCI KĄTOWEJ. DOKŁADNOŚĆ LINIOWA DOTYCZY WYŁĄCZNIE KOŃCOWYCH WYNIKÓW.
        //Wszystkie wartości longitude odnoszą się do południka osiowego układu 2000 lub południka osiowego UTM.
        //20 permutacji daje łącznie 280/395 scenariuszy.
        public List<Point> U2000To1992(byte longitude, double precision, List<Point> Points) 
            /*logitude to południk osiowy układu 2000 */
        {
            List<Point> result = new List<Point>();
            //JAK POZYSKAĆ FAKTYCZNĄ WARTOŚĆ WYSOKOŚCI???
            List<PointBLH> bottom = XYGK2BLH(U2000ToGK(Points, longitude), longitude, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = GKToU1992(BLH2XYGK(bottom, 19));
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = GKToU1992(BLH2XYGK(XYZ2BLH(helper,precision), 19));
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = GKToU1992(BLH2XYGK(helper, 19));
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = GKToU1992(BLH2XYGK(XYZ2BLH(helper, precision),19 ));
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = GKToU1992(BLH2XYGK(helper, 19));
                }
            }
            ////List<Point> result = GKToU1992(BLH2XYGK(XYGK2BLH(U2000ToGK(Points, longitude), longitude, precision), longitude));
            return result;
        }
        public List<Point> U1992To2000(byte longitude, double precision, List<Point> Points)
        {
            List<Point> result = new List<Point>();
            List<PointBLH> bottom = XYGK2BLH(U1992ToGK(Points), 19, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = GKToU2000(BLH2XYGK(bottom, longitude),longitude);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = GKToU2000(BLH2XYGK(XYZ2BLH(helper, precision), longitude),longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = GKToU2000(BLH2XYGK(helper, longitude),longitude);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = GKToU2000(BLH2XYGK(XYZ2BLH(helper, precision), longitude),longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = GKToU2000(BLH2XYGK(helper, longitude),longitude);
                }
            }
            return result;
        }
        public List<Point3D> U2000ToXYZ(byte longitude, double precision, List<Point> Points)
        {
            List<Point3D> result = new List<Point3D>();
            List<PointBLH> bottom = XYGK2BLH(U2000ToGK(Points, longitude), longitude, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = BLH2XYZ(bottom);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = helper;
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = BLH2XYZ(helper);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = helper;
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = BLH2XYZ(helper);
                }
            }
            return result;
        }
        public List<Point3D> U1992ToXYZ(double precision, List<Point> Points)
        {
            List<Point3D> result = new List<Point3D>();
            List<PointBLH> bottom = XYGK2BLH(U1992ToGK(Points),19,precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = BLH2XYZ(bottom);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = helper;
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = BLH2XYZ(helper);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = helper;
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = BLH2XYZ(helper);
                }
            }
            return result;
        }
        public List<Point> XYZ2U2000(byte longitude, double precision, List<Point3D> Points)
        {
            List<Point> result = new List<Point>();
            List<PointBLH> bottom = XYZ2BLH(Points,precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result =GKToU2000(BLH2XYGK(bottom,longitude),longitude);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(Points);
                    result = GKToU2000(BLH2XYGK(XYZ2BLH(helper,precision),longitude),longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = GKToU2000(BLH2XYGK(helper,longitude),longitude);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(Points);
                    result = GKToU2000(BLH2XYGK(XYZ2BLH(helper, precision), longitude), longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = GKToU2000(BLH2XYGK(helper, longitude), longitude);
                }
            }
            
            return result;
        }
        public List<Point> XYZ2U1992( double precision, List<Point3D> Points)
        {
            List<Point> result = new List<Point>();
            List<PointBLH> bottom = XYZ2BLH(Points, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = GKToU1992(BLH2XYGK(bottom,19));
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(Points);
                    result = GKToU1992(BLH2XYGK(XYZ2BLH(helper, precision), 19));
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = GKToU1992(BLH2XYGK(helper, 19));
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(Points);
                    result = GKToU1992(BLH2XYGK(XYZ2BLH(helper, precision), 19));
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = GKToU1992(BLH2XYGK(helper, 19));
                }
            }

            return result;
        }
        public List<PointBLH> U2000ToBLH(byte longitude, double precision, List<Point> Points)
        {
            List<PointBLH> result = new List<PointBLH>();
            List<PointBLH> bottom = XYGK2BLH(U2000ToGK(Points, longitude), longitude, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = bottom;
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = XYZ2BLH(helper,precision);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = helper;
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = XYZ2BLH(helper, precision);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = helper;
                }
            }
            return result;
        }
        public List<PointBLH> U1992ToBLH(double precision, List<Point> Points)
        {
            List<PointBLH> result = new List<PointBLH>();
            List<PointBLH> bottom = XYGK2BLH(U1992ToGK(Points), 19, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = bottom;
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = XYZ2BLH(helper, precision);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = helper;
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = XYZ2BLH(helper, precision);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = helper;
                }
            }
            return result;
        }
        public List<Point> BLH2U2000(byte longitude, double precision, List<PointBLH> Points)
        {
            List<Point> result = new List<Point>();
            if (this.startETRF.Equals(this.endETRF))
            {
                result = GKToU2000(BLH2XYGK(Points, longitude), longitude);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(Points));
                    result = GKToU2000(BLH2XYGK(XYZ2BLH(helper, precision),longitude),longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(Points, false);
                    result = GKToU2000(BLH2XYGK(helper, longitude), longitude);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(Points));
                    result = GKToU2000(BLH2XYGK(XYZ2BLH(helper, precision), longitude), longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(Points, true);
                    result = GKToU2000(BLH2XYGK(helper, longitude), longitude);
                }
            }
            return result;
        }
        public List<Point> BLH2U1992( double precision, List<PointBLH> Points)
        {
            List<Point> result = new List<Point>();
            if (this.startETRF.Equals(this.endETRF))
            {
                result = GKToU1992(BLH2XYGK(Points, 19));
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(Points));
                    result = GKToU1992(BLH2XYGK(XYZ2BLH(helper, precision), 19));
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(Points, false);
                    result = GKToU1992(BLH2XYGK(helper, 19));
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(Points));
                    result = GKToU1992(BLH2XYGK(XYZ2BLH(helper, precision), 19));
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(Points, true);
                    result = GKToU1992(BLH2XYGK(helper, 19));
                }
            }
            return result;
        }
        public List<Point> UTM2U1992(byte longitude, double precision, List<Point> Points)
        {
            /*logitude to południk osiowy układu UTM */  
               List<Point> result = new List<Point>();
                //JAK POZYSKAĆ FAKTYCZNĄ WARTOŚĆ WYSOKOŚCI???
                List<PointBLH> bottom = UTM2BLH(Points, longitude, precision);
                if (this.startETRF.Equals(this.endETRF))
                {
                    result = GKToU1992(BLH2XYGK(bottom, 19));
                }
                else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
                {
                    if (this.transformateOption)
                    {
                        List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                        result = GKToU1992(BLH2XYGK(XYZ2BLH(helper, precision), 19));
                    }
                    else if (!this.transformateOption)
                    {
                        List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                        result = GKToU1992(BLH2XYGK(helper, 19));
                    }
                }
                else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
                {
                    if (this.transformateOption)
                    {
                        List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                        result = GKToU1992(BLH2XYGK(XYZ2BLH(helper, precision), 19));
                    }
                    else if (!this.transformateOption)
                    {
                        List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                        result = GKToU1992(BLH2XYGK(helper, 19));
                    }
                }
                return result;       
        }
        public List<Point> UTM2U2000(byte longitudeUTM, byte longitude2000, double precision, List<Point> Points)
        {
            /*logitude to południk osiowy układu UTM */
            List<Point> result = new List<Point>();
            //JAK POZYSKAĆ FAKTYCZNĄ WARTOŚĆ WYSOKOŚCI???
            List<PointBLH> bottom = UTM2BLH(Points, longitudeUTM, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = GKToU2000(BLH2XYGK(bottom, longitude2000),longitude2000);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = GKToU2000(BLH2XYGK(XYZ2BLH(helper, precision), longitude2000),longitude2000);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = GKToU2000(BLH2XYGK(helper, longitude2000),longitude2000);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = GKToU2000(BLH2XYGK(XYZ2BLH(helper, precision), longitude2000), longitude2000);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = GKToU2000(BLH2XYGK(helper, longitude2000), longitude2000);
                }
            }
            return result;
        }
        public List<Point> U1992ToUTM(byte longitude, double precision, List<Point> Points)
        {
            List<Point> result = new List<Point>();
            List<PointBLH> bottom = XYGK2BLH(U1992ToGK(Points), 19, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = BLH2UTM(bottom, longitude);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = BLH2UTM(XYZ2BLH(helper, precision), longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = BLH2UTM(helper, longitude);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = BLH2UTM(XYZ2BLH(helper, precision), longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = BLH2UTM(helper, longitude);
                }
            }
            return result;
        }
        public List<Point> U2000ToUTM(byte longitudeUTM, byte longitude2000, double precision, List<Point> Points)
        {
            List<Point> result = new List<Point>();
            List<PointBLH> bottom = XYGK2BLH(U2000ToGK(Points,longitude2000), longitude2000, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = BLH2UTM(bottom, longitudeUTM);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = BLH2UTM(XYZ2BLH(helper, precision), longitudeUTM);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = BLH2UTM(helper, longitudeUTM);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = BLH2UTM(XYZ2BLH(helper, precision), longitudeUTM);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = BLH2UTM(helper, longitudeUTM);
                }
            }
            return result;
        }
        public List<Point> XYZ2UTM(byte longitude, double precision, List<Point3D> Points)
        {
            List<Point> result = new List<Point>();
            List<PointBLH> bottom = XYZ2BLH(Points, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = BLH2UTM(bottom, longitude);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(Points);
                    result = BLH2UTM(XYZ2BLH(helper, precision), longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = BLH2UTM(helper, longitude);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(Points);
                    result = BLH2UTM(XYZ2BLH(helper, precision), longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = BLH2UTM(helper, longitude);
                }
            }

            return result;
        }
        public List<Point> BLH2UTM(byte longitude, double precision, List<PointBLH> Points)
        {
            List<Point> result = new List<Point>();
            if (this.startETRF.Equals(this.endETRF))
            {
                result = BLH2UTM(Points, longitude);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(Points));
                    result = BLH2UTM(XYZ2BLH(helper, precision), longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(Points, false);
                    result = BLH2UTM(helper, longitude);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(Points));
                    result = BLH2UTM(XYZ2BLH(helper, precision), longitude);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(Points, true);
                    result = BLH2UTM(helper, longitude);
                }
            }
            return result;
        }
        public List<PointBLH> UTMtoBLH(byte longitude, double precision, List<Point> Points)
        {
            List<PointBLH> result = new List<PointBLH>();
            List<PointBLH> bottom = UTM2BLH(Points, longitude, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = bottom;
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = XYZ2BLH(helper, precision);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = helper;
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = XYZ2BLH(helper, precision);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = helper;
                }
            }
            return result;
        }
        public List<Point3D> UTMtoXYZ(byte longitude, double precision, List<Point> Points)
        {
            List<Point3D> result = new List<Point3D>();
            List<PointBLH> bottom = UTM2BLH(Points, longitude, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = BLH2XYZ(bottom);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = helper;
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = BLH2XYZ(helper);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = helper;
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = BLH2XYZ(helper);
                }
            }
            return result;
        }
        public List<PointBLH> XYZ2BLHFull(List<Point3D> Points3D, double precision)
        {
            List<PointBLH> result = new List<PointBLH>();
            List<PointBLH> bottom = XYZ2BLH(Points3D,precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = bottom;
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(Points3D);
                    result = XYZ2BLH(helper, precision);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = helper;
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(Points3D);
                    result = XYZ2BLH(helper, precision);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = helper;
                }
            }
            return result;
        }
        public List<Point3D> BLH2XYZFull(List<PointBLH> Points)
        {
            List<Point3D> result = new List<Point3D>();
            
            if (this.startETRF.Equals(this.endETRF))
            {
                result = BLH2XYZ(Points);
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(Points));
                    result = helper;
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(Points, false);
                    result = BLH2XYZ(helper);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(Points));
                    result = helper;
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(Points, true);
                    result = BLH2XYZ(helper);
                }
            }
            return result;
        }
        //SCENARIUSZE TRANSFORMUJĄCE DO TEGO SAMEGO UKŁADU WSPÓŁRZĘDNYCH:
        public List<Point> U2000ToU2000(byte longitudeS, byte longitudeE, double precision, List<Point> Points)
        {
            List<Point> result = new List<Point>();
            List<PointBLH> bottom = XYGK2BLH(U2000ToGK(Points,longitudeS), longitudeS, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                if (longitudeE.Equals(longitudeS))
                {
                    result = Points;
                }
                else
                {
                    result = GKToU2000(BLH2XYGK(bottom, longitudeE), longitudeE);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = GKToU2000(BLH2XYGK(XYZ2BLH(helper, precision), longitudeE), longitudeE);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = GKToU2000(BLH2XYGK(helper, longitudeE), longitudeE);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = GKToU2000(BLH2XYGK(XYZ2BLH(helper, precision), longitudeE), longitudeE);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = GKToU2000(BLH2XYGK(helper, longitudeE), longitudeE);
                }
            }
            return result;
        }
        public List<Point> U1992ToU1992(double precision, List<Point> Points)
        {
                List<Point> result = new List<Point>();
                //JAK POZYSKAĆ FAKTYCZNĄ WARTOŚĆ WYSOKOŚCI???
                List<PointBLH> bottom = XYGK2BLH(U1992ToGK(Points), 19, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                result = Points;
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = GKToU1992(BLH2XYGK(XYZ2BLH(helper, precision), 19));
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = GKToU1992(BLH2XYGK(helper, 19));
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = GKToU1992(BLH2XYGK(XYZ2BLH(helper, precision), 19));
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = GKToU1992(BLH2XYGK(helper, 19));
                }
            }
                ////List<Point> result = GKToU1992(BLH2XYGK(XYGK2BLH(U2000ToGK(Points, longitude), longitude, precision), longitude));
                return result;
            }
        public List<Point> UTM2UTM(byte longitudeS, byte longitudeE, double precision, List<Point> Points)
        {
            List<Point> result = new List<Point>();
            List<PointBLH> bottom = UTM2BLH(Points, longitudeS, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                if (longitudeS.Equals(longitudeE))
                {
                    result = Points;
                }
                else
                {
                    result = BLH2UTM(bottom, longitudeE);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = BLH2UTM(XYZ2BLH(helper, precision), longitudeE);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = BLH2UTM(helper, longitudeE);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = BLH2UTM(XYZ2BLH(helper, precision), longitudeE);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = BLH2UTM(helper, longitudeE);
                }
            }
            return result;
        }
        public List<Point3D> XYZ2XYZ(double precision, List<Point3D> Points)
        {
            List<Point3D> result = new List<Point3D>();
            
            if (this.startETRF.Equals(this.endETRF))
            {
                result = Points;
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(Points);
                    result = helper;
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(XYZ2BLH(Points,precision), false);
                    result = BLH2XYZ(helper);
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(Points);
                    result = helper;
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(XYZ2BLH(Points, precision), true);
                    result = BLH2XYZ(helper);
                }
            }
            return result;
        }
        public List<PointBLH> BLH2BLH(double precision, List<PointBLH> Points)
        {
            List<PointBLH> result = new List<PointBLH>();
            Points.ForEach(p => { if (!p.Format()) { p.convertToDegrees(); } });
            List<PointBLH> bottom = Points;
            if (this.startETRF.Equals(this.endETRF))
            {
                result = bottom;
            }
            else if (this.startETRF.ToString().Equals("ETRF89") && this.endETRF.ToString().Equals("ETRF2000"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF89TOETRF2000(BLH2XYZ(bottom));
                    result = XYZ2BLH(helper, precision);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, false);
                    result = helper;
                }
            }
            else if (this.startETRF.ToString().Equals("ETRF2000") && this.endETRF.ToString().Equals("ETRF89"))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = ETRF2000TO89(BLH2XYZ(bottom));
                    result = XYZ2BLH(helper, precision);
                }
                else if (!this.transformateOption)
                {
                    List<PointBLH> helper = setGridDeltasNchangeETRF(bottom, true);
                    result = helper;
                }
            }
            return result;
        }
        private void TabPage1_Click(object sender, EventArgs e)
        {
            
        }
        //USTALENIE MOŻLIWEJ DOKŁADNOŚCI KĄTOWEJ I LINIOWEJ oraz WYBÓR METODY OBLICZENIOWEJ
        private void Transform_Load(object sender, EventArgs e)
        {            
            DomainUpDown.DomainUpDownItemCollection collection = this.AnglePrecisionDUD.Items;
            collection.Add("0,001");
            collection.Add("0,0001");
            collection.Add("0,00001");
            this.AnglePrecisionDUD.Text = "0,0001";
            DomainUpDown.DomainUpDownItemCollection collection2 = this.LengthPrecisionDUD.Items;
            collection2.Add("0,001");
            collection2.Add("0,0001");
            collection2.Add("0,00001");
            this.LengthPrecisionDUD.Text = "0,0001";
            this.MonitorRichTextBox.Clear(); this.MonitorRichTextBox.Text = "MONITOR: ";
        }

        private void TeoreticOptionRB_CheckedChanged(object sender, EventArgs e)
        {
            this.transformateOption = true;
        }

        private void GridOptionRB_CheckedChanged(object sender, EventArgs e)
        {
            this.transformateOption = false;
        }
        //PRÓBA NAGRANIA SPRAWDZANIA FORMATU BLH
        public bool isCorrectlyFormated(PointBLH p)
        {
            bool Btrue = false; byte problems = 5; bool problemsVol2 = false;
            //SPRAWDZA CZY STOPNIE NALEŻĄ DO WARTOŚCI [0,90] I [0,180]
            if ((p.fi() >= 0 && p.fi() <= 90) && (p.lambda() >= 0 && p.lambda() <= 180))
            {
                problems--; problemsVol2 = true;
            }
            else
            {
                Thread.Sleep(100);
                this.errorCatcher.Clear();
                this.errorCatcher.Append("Punkt \"" + p.Name() + "\": Nieprawidłowy format zapisu stopni (Wartość B poza [0,90] \\ wartość L poza [0,180)). \n");
                this.TransformerBW.ReportProgress(0);
                //this.MonitorRichTextBox.Text+=("Punkt \"" + p.Name() + "\": Nieprawidłowy format zapisu stopni (Wartość B poza [0,90] \\ wartość L poza [0,180)). \n");
            }
            if (!p.Format())
            {
                //SPRAWDZA CZY WARTOŚCI STOPNIOWE SĄ LICZBAMI CAŁKOWITYMI
                bool isIntegerB = Math.Floor(p.fi()).Equals(p.fi());
                bool isIntegerL = Math.Floor(p.lambda()).Equals(p.lambda());
                if (isIntegerB && isIntegerL)
                {
                    problems--;
                }
                else
                {
                    Thread.Sleep(100);
                    this.errorCatcher.Clear();
                    this.errorCatcher.Append("Punkt \"" + p.Name() + "\": Nieprawidłowy format zapisu stopni (Wymagana wartość całkowita).\n");
                    this.TransformerBW.ReportProgress(0);
                    //this.MonitorRichTextBox.Text += ("Punkt \"" + p.Name() + "\": Nieprawidłowy format zapisu stopni (Wymagana wartość całkowita).\n");
                }
                //SPRAWDZA CZY MINUTY KĄTOWE SĄ WARTOŚCIAMI CAŁKOWITYMI
                bool minutesAreInteger = Math.Floor(p.bmin()).Equals(p.bmin()) && Math.Floor(p.lmin()).Equals(p.lmin());
                if (minutesAreInteger)
                {
                    problems--;
                }
                else
                {
                    Thread.Sleep(100);
                    this.errorCatcher.Clear();
                    this.errorCatcher.Append("Punkt \"" + p.Name() + "\": Nieprawidłowy format zapisu minut kątowych(Wymagana wartość całkowita).\n");
                    this.TransformerBW.ReportProgress(0);
                    //this.MonitorRichTextBox.Text += ("Punkt \"" + p.Name() + "\": Nieprawidłowy format zapisu minut kątowych(Wymagana wartość całkowita).\n");
                }
                // SPRAWDZA CZY MINUTY KĄTOWE NALEŻĄ DO PRZEDZIAŁU [0,60)
                bool isbetweenB = (p.bmin() >= 0 && p.bmin() < 60);
                bool isBetweenL = (p.lmin() >= 0 && p.lmin() < 60);
                if (isbetweenB && isBetweenL)
                {
                    problems--;
                }
                else
                {
                    Thread.Sleep(100);
                    this.errorCatcher.Clear();
                    this.errorCatcher.Append("Punkt \"" + p.Name() + "\": Nieprawidłowy format zapisu minut kątowych. Wartość poza [0,60).\n");
                    this.TransformerBW.ReportProgress(0);
                    //this.MonitorRichTextBox.Text += ("Punkt \"" + p.Name() + "\": Nieprawidłowy format zapisu minut kątowych. Wartość poza [0,60).\n");
                }
                //SPRAWDZA CZY SEKUNDY KĄTOWE NALEŻĄ DO PRZEDZIAŁU [0,60)
                if ((p.bsec() >= 0 && p.bsec() < 60) && (p.lsec() >= 0 && p.lsec() < 60))
                {
                    problems--;
                }
                else
                {
                    Thread.Sleep(100);
                    this.errorCatcher.Clear();
                    this.errorCatcher.Append("Punkt \"" + p.Name() + "\": Nieprawidłowy format zapisu sekund kątowych. Wartość poza [0,60).\n");
                    this.TransformerBW.ReportProgress(0);
                }
                return Btrue = problems == 0 ? true : false;
            }
            else
            {
                return problemsVol2;
            }
        }
        //TRANSFORMACJA KONFOREMNA 7-STOPNIOWA HELMERTA DLA XYZ MIĘDZY UKŁADAMI ETRF89-ETRF2000
        public List<Point3D> ETRF89TOETRF2000(List<Point3D> points)
        {
            List<Point3D> result = new List<Point3D>();
            points.ForEach(p =>
            {
                double Xs = 3696570.6591; double Ys = 1297521.5905; double Zs = 5011111.1273;
                double dX = p.x() - Xs; double dY = p.y() - Ys; double dZ = p.z() - Zs;
                double Xfin = p.x() + (-0.0322) + (-0.00000005102) * dX + (-0.00000000746) * dY + (0.00000004804) * dZ;
                double Yfin = p.y() + (-0.0347) + (0.00000000746) * dX + (-0.00000005102) * dY + (0.00000006152) * dZ;
                double Zfin = p.z() + (-0.0507) + (-0.00000004804) * dX + (-0.00000006152) * dY + (-0.00000005102) * dZ;
                result.Add(new Point3D(p.Name(), Xfin, Yfin, Zfin));
            });
            return result;
        }
        public List<Point3D> ETRF2000TO89(List<Point3D> points)
        {
            List<Point3D> result = new List<Point3D>();
            points.ForEach(p =>
            {
                double Xs = 3696570.6268; double Ys = 1297521.5559; double Zs = 5011111.0767;
                double dX = p.x() - Xs; double dY = p.y() - Ys; double dZ = p.z() - Zs;
                double Xfin = p.x() + (0.0322) + (0.00000005102) * dX + (0.00000000746) * dY + (-0.00000004804) * dZ;
                double Yfin = p.y() + (0.0347) + (-0.00000000746) * dX + (0.00000005102) * dY + (-0.00000006152) * dZ;
                double Zfin = p.z() + (0.0507) + (0.00000004804) * dX + (0.00000006152) * dY + (0.00000005102) * dZ;
                result.Add(new Point3D(p.Name(), Xfin, Yfin, Zfin));
            });
            return result; 
        }
        //ODCZYT PLIKU TXT W NOTATNIKU ORAZ ZAPIS WYNIKÓW:
        private void FileOpenerButton_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", this.FileName.ToString());
        }
        public void PrintFile(List<PointBLH> Points, string filename,int precisionA,int precisionL, bool degreeForm, string transformType)
        {
            StreamWriter file = new StreamWriter(filename);
            StringBuilder firstLine = degreeForm ? new StringBuilder("P B L H ") : new StringBuilder("P B B' B\" L L' L\" H ");
            file.WriteLine(firstLine.ToString());
            int precisiondF = precisionA + 4;
            Points.ForEach(p =>
            {
                StringBuilder line = new StringBuilder();
                if (degreeForm)
                {
                    if (!p.Format()) { p.convertToDegrees(); }
                    double fi = Math.Round(p.fi(), precisiondF);
                    double lambda = Math.Round(p.lambda(), precisiondF);
                    double H = Math.Round(p.height(), precisionL);
                    line.Append(p.Name()).Append(" ").Append(fi).Append(" ").Append(lambda).Append(" ").Append(H)
                    .Append(" ").Append(transformType);
                    file.WriteLine(line.ToString());
                }
                else if (!degreeForm)
                {
                    p.convertToDegreesMinsNSecs();
                    double bsec = Math.Round(p.bsec(), precisionA); double lsec = Math.Round(p.lsec(),precisionA);
                    double H = Math.Round(p.height(), precisionL);
                    line.Append(p.Name()).Append(" ").Append(p.fi()).Append(" ").Append(p.bmin()).Append(" ").Append(bsec).Append(" ")
                    .Append(p.lambda()).Append(" ").Append(p.lmin()).Append(" ").Append(lsec).Append(" ")
                    .Append(H).Append(" ").Append(transformType);
                    file.WriteLine(line);
                }
            });
            file.Close();
        }
        public void PrintFile(List<Point3D> Points, string filename, int precision, string transformType)
        {
            StreamWriter file = new StreamWriter(filename);
            StringBuilder firstLine = new StringBuilder("P X Y Z ");
            file.WriteLine(firstLine);
            Points.ForEach(p =>
            {
                double X = Math.Round(p.x(), precision); double Y = Math.Round(p.y(), precision); double Z = Math.Round(p.z(), precision);
                file.WriteLine(p.Name() + " " + X.ToString() + " " + Y.ToString() + " " + Z + " " + transformType);
            });
            file.Close();
        }
        public void PrintFile(List<Point> Points, string filename, int precision, string transformType)
        {
            StreamWriter file = new StreamWriter(filename);
            StringBuilder firstLine = new StringBuilder("P X Y ");
            file.WriteLine(firstLine);
            Points.ForEach(p =>
            {
                double X = Math.Round(p.x(), precision); double Y = Math.Round(p.y(), precision);
                file.WriteLine(p.Name()+ " " + X.ToString() + " " + Y.ToString() + " " + transformType);
            });
            file.Close();
        }
        //FUNKCJA WYKONAWCZA DLA WSZYSTKICH WARIANTÓW PROGRAMU:
        public void GottaTransformThemAll()
        {
            this.resLongitude = 0;
            double anglePrecision = Convert.ToDouble(this.AnglePrecisionDUD.Text) / 3600 * Math.PI / 180;
            int printPrecisionL = this.LengthPrecisionDUD.Text.Length -2;
            int printPrecisionA = this.AnglePrecisionDUD.Text.Length -2;
            if (start.ToString().Equals("Układ 2000"))
            {
                if (end.ToString().Equals("Układ 2000"))
                {
                    this.resLongitude = 0;
                    this.resLongitude = setLongitude(resultLongitude15, resultLongitude18, resultLongitude21, resultLongitude24);
                    if (this.resLongitude != 0)
                    {
                        List<Point> result = U2000ToU2000(longitude, resLongitude, anglePrecision, this.Points);
                        TransformerBW.ReportProgress(10);
                        //FileOpenerButton.Visible = true; FileOpenerButton.Text = "U2000";
                        this.FileName.Append("U2000.txt");
                        PrintFile(result, this.FileName.ToString(), printPrecisionL, ("Układ 2000 do Układu 2000, L0=" + resLongitude.ToString()));
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano południka osiowego funkcji wyjściowej.");
                        goto koniecPsot;
                    }
                }
                else if(end.ToString().Equals("Układ 1992"))
                {
                    List<Point> result = U2000To1992(this.longitude, anglePrecision, this.Points);
                    TransformerBW.ReportProgress(20);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "U1992";
                    this.FileName.Append("U1992.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionL, ("Układ 2000 do Układu 1992, L0=" + resLongitude.ToString()));
                }
                else if (end.ToString().Equals("UTM"))
                {
                    this.resLongitude = setLongitude(resultLongitudeUTM15, resultLongitudeUTM21);
                    if (this.resLongitude!=0){
                        List<Point> result = U2000ToUTM(this.resLongitude, this.longitude, anglePrecision, this.Points);
                        TransformerBW.ReportProgress(30);
                        //FileOpenerButton.Visible = true; FileOpenerButton.Text = "UTM";
                        this.FileName.Append("UTM.txt");
                        PrintFile(result, this.FileName.ToString(), printPrecisionL, ("Układ 2000 do Układu UTM, L0=" + resLongitude.ToString()));
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano południka osiowego funkcji wyjściowej.");
                        goto koniecPsot;
                    }
                }
                else if(end.ToString().Equals("BLH GRS80"))
                {
                    List<PointBLH> result = U2000ToBLH(this.longitude, anglePrecision, this.Points);
                    TransformerBW.ReportProgress(40);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "BLH";
                    this.FileName.Append("BLH.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionA,printPrecisionL,this.resultDegreeForm, "Układ 2000 do BLH");
                }
                else if(end.ToString().Equals("XYZ GRS80"))
                {
                    List<Point3D> result = U2000ToXYZ(this.longitude, anglePrecision, this.Points);
                    TransformerBW.ReportProgress(50);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "XYZ";
                    this.FileName.Append("XYZ.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionL, "Układ 2000 do XYZ");
                }
            }
            else if(start.ToString().Equals("Układ 1992"))
            {
                if (end.ToString().Equals("Układ 2000"))
                {
                    this.resLongitude = setLongitude(resultLongitude15, resultLongitude18, resultLongitude21, resultLongitude24);
                    if (this.resLongitude != 0)
                    {
                        List<Point> result = U1992To2000(this.resLongitude, anglePrecision, this.Points);
                        TransformerBW.ReportProgress(10);
                        //FileOpenerButton.Visible = true; FileOpenerButton.Text = "U2000";
                        this.FileName.Append("U2000.txt");
                        PrintFile(result, this.FileName.ToString(), printPrecisionL, ("Układ 1992 do Układu 2000, L0=" + resLongitude.ToString()));
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano południka osiowego funkcji wyjściowej.");
                        goto koniecPsot;
                    }
                }
                else if (end.ToString().Equals("Układ 1992"))
                {
                    List<Point> result = U1992ToU1992(anglePrecision, this.Points);
                    TransformerBW.ReportProgress(20);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "U1992";
                    this.FileName.Append("U1992.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionL, ("Układ 1992 do Układu 1992, L0=" + resLongitude.ToString()));
                }
                else if (end.ToString().Equals("UTM"))
                {
                    this.resLongitude = setLongitude(resultLongitudeUTM15, resultLongitudeUTM21);
                    if (this.resLongitude != 0)
                    {
                        List<Point> result = U1992ToUTM(this.resLongitude, anglePrecision, this.Points);
                        TransformerBW.ReportProgress(30);
                        //FileOpenerButton.Visible = true; FileOpenerButton.Text = "UTM";
                        this.FileName.Append("UTM.txt");
                        PrintFile(result, this.FileName.ToString(), printPrecisionL, ("Układ 1992 do Układu UTM, L0=" + resLongitude.ToString()));
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano południka osiowego funkcji wyjściowej.");
                        goto koniecPsot;
                    }
                }
                else if (end.ToString().Equals("BLH GRS80"))
                {
                    List<PointBLH> result = U1992ToBLH(anglePrecision, this.Points);
                    TransformerBW.ReportProgress(40);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "BLH";
                    this.FileName.Append("BLH.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionA, printPrecisionL, this.resultDegreeForm, "Układ 1992 do BLH");
                }
                else if (end.ToString().Equals("XYZ GRS80"))
                {
                    List<Point3D> result = U1992ToXYZ(anglePrecision, this.Points);
                    TransformerBW.ReportProgress(50);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "XYZ";
                    this.FileName.Append("XYZ.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionL, "Układ 1992 do XYZ");
                }
            }
            else if (start.ToString().Equals("UTM"))
            {
                if (end.ToString().Equals("Układ 2000"))
                {
                    this.resLongitude = setLongitude(resultLongitude15, resultLongitude18, resultLongitude21, resultLongitude24);
                    if (this.resLongitude != 0)
                    {
                        List<Point> result = UTM2U2000(this.longitude, this.resLongitude, anglePrecision, this.Points);
                        TransformerBW.ReportProgress(10);
                        //FileOpenerButton.Visible = true; FileOpenerButton.Text = "U2000";
                        this.FileName.Append("U2000.txt");
                        PrintFile(result, this.FileName.ToString(), printPrecisionL, ("Układ UTM do Układu 2000, L0=" + resLongitude.ToString()));
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano południka osiowego funkcji wyjściowej.");
                        goto koniecPsot;
                    }
                }
                else if (end.ToString().Equals("Układ 1992"))
                {
                    List<Point> result = UTM2U1992(this.longitude, anglePrecision, this.Points);
                    TransformerBW.ReportProgress(20);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "U1992";
                    this.FileName.Append("U1992.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionL, ("Układ UTM do Układu 1992, L0=" + resLongitude.ToString()));
                }
                else if (end.ToString().Equals("UTM"))
                {
                    this.resLongitude = setLongitude(resultLongitudeUTM15, resultLongitudeUTM21);
                    if (this.resLongitude != 0)
                    {
                        List<Point> result = UTM2UTM(this.longitude, this.resLongitude, anglePrecision, this.Points);
                        TransformerBW.ReportProgress(30);
                        //FileOpenerButton.Visible = true; FileOpenerButton.Text = "UTM";
                        this.FileName.Append("UTM.txt");
                        PrintFile(result, this.FileName.ToString(), printPrecisionL, ("Układ UTM do Układu UTM, L0=" + resLongitude.ToString()));
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano południka osiowego funkcji wyjściowej.");
                        goto koniecPsot;
                    }
                }
                else if (end.ToString().Equals("BLH GRS80"))
                {
                    List<PointBLH> result = UTM2BLH(this.Points, this.longitude, anglePrecision);
                    TransformerBW.ReportProgress(40);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "BLH";
                    this.FileName.Append("BLH.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionA, printPrecisionL, this.resultDegreeForm, "Układ UTM do BLH");
                }
                else if (end.ToString().Equals("XYZ GRS80"))
                {
                    List<Point3D> result = UTMtoXYZ(this.longitude, anglePrecision, this.Points);
                    TransformerBW.ReportProgress(50);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "XYZ";
                    this.FileName.Append("XYZ.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionL, "Układ UTM do XYZ");
                }
            }
            else if (start.ToString().Equals("BLH GRS80"))
            {
                if (end.ToString().Equals("Układ 2000"))
                {
                    this.resLongitude = setLongitude(resultLongitude15, resultLongitude18, resultLongitude21, resultLongitude24);
                    if (this.resLongitude != 0)
                    {
                        List<Point> result = BLH2U2000(this.resLongitude, anglePrecision, this.PointsBLH);
                        TransformerBW.ReportProgress(10);
                        //FileOpenerButton.Visible = true; FileOpenerButton.Text = "U2000";
                        this.FileName.Append("U2000.txt");
                        PrintFile(result, this.FileName.ToString(), printPrecisionL, ("BLH do Układu 2000, L0=" + resLongitude.ToString()));
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano południka osiowego funkcji wyjściowej.");
                        goto koniecPsot;
                    }
                }
                else if (end.ToString().Equals("Układ 1992"))
                {
                    List<Point> result = BLH2U1992(anglePrecision, this.PointsBLH);
                    TransformerBW.ReportProgress(20);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "U1992";
                    this.FileName.Append("U1992.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionL, ("BLH do Układu 1992, L0=" + resLongitude.ToString()));
                }
                else if (end.ToString().Equals("UTM"))
                {
                    this.resLongitude = setLongitude(resultLongitudeUTM15, resultLongitudeUTM21);
                    if (this.resLongitude != 0)
                    {
                        List<Point> result = BLH2UTM(this.PointsBLH, this.resLongitude);
                        TransformerBW.ReportProgress(30);
                        //FileOpenerButton.Visible = true; FileOpenerButton.Text = "UTM";
                        this.FileName.Append("UTM.txt");
                        PrintFile(result, this.FileName.ToString(), printPrecisionL, ("BLH do Układu UTM, L0=" + resLongitude.ToString()));
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano południka osiowego funkcji wyjściowej.");
                        goto koniecPsot;
                    }
                }
                else if (end.ToString().Equals("BLH GRS80"))
                {
                    List<PointBLH> result = BLH2BLH(anglePrecision, this.PointsBLH);
                    TransformerBW.ReportProgress(40);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "BLH";
                    this.FileName.Append("BLH.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionA, printPrecisionL, this.resultDegreeForm, "BLH do BLH.");
                }
                else if(end.ToString().Equals("XYZ GRS80"))
                {
                    List<Point3D> result = BLH2XYZFull(this.PointsBLH);
                    TransformerBW.ReportProgress(50);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "XYZ";
                    this.FileName.Append("XYZ.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionL, "BLH do XYZ.");
                }
            }
            else if (start.ToString().Equals("XYZ GRS80"))
            {
                if (end.ToString().Equals("Układ 2000"))
                {
                    this.resLongitude = setLongitude(resultLongitude15, resultLongitude18, resultLongitude21, resultLongitude24);
                    if (this.resLongitude != 0)
                    {
                        List<Point> result = XYZ2U2000(this.resLongitude, anglePrecision, this.Points3D);
                        TransformerBW.ReportProgress(10);
                        //FileOpenerButton.Visible = true; FileOpenerButton.Text = "U2000";
                        this.FileName.Append("U2000.txt");
                        PrintFile(result, this.FileName.ToString(), printPrecisionL, ("XYZ do Układu 2000, L0=" + resLongitude.ToString()));
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano południka osiowego funkcji wyjściowej.");
                        goto koniecPsot;
                    }
                }
                else if (end.ToString().Equals("Układ 1992"))
                {
                    List<Point> result = XYZ2U1992(anglePrecision, this.Points3D);
                    TransformerBW.ReportProgress(20);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "U1992";
                    this.FileName.Append("U1992.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionL, ("XYZ do Układu 1992, L0=" + resLongitude.ToString()));
                }
                else if (end.ToString().Equals("UTM"))
                {
                    this.resLongitude = setLongitude(resultLongitudeUTM15, resultLongitudeUTM21);
                    if (this.resLongitude != 0)
                    {
                        List<Point> result = XYZ2UTM(this.resLongitude, anglePrecision, this.Points3D);
                        TransformerBW.ReportProgress(30);
                        //FileOpenerButton.Visible = true; FileOpenerButton.Text = "UTM";
                        this.FileName.Append("UTM.txt");
                        PrintFile(result, this.FileName.ToString(), printPrecisionL, ("XYZ do Układu UTM, L0=" + resLongitude.ToString()));
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano południka osiowego funkcji wyjściowej.");
                        goto koniecPsot;
                    }
                }
                else if (end.ToString().Equals("BLH GRS80"))
                {
                    List<PointBLH> result = XYZ2BLHFull(this.Points3D, anglePrecision);
                    TransformerBW.ReportProgress(40);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "BLH";
                    this.FileName.Append("BLH.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionA, printPrecisionL, this.resultDegreeForm, "XYZ do BLH");
                }
                else if (end.ToString().Equals("XYZ GRS80"))
                {
                    List<Point3D> result = XYZ2XYZ(anglePrecision, this.Points3D);
                    TransformerBW.ReportProgress(50);
                    //FileOpenerButton.Visible = true; FileOpenerButton.Text = "XYZ";
                    this.FileName.Append("XYZ.txt");
                    PrintFile(result, this.FileName.ToString(), printPrecisionL, "XYZ do XYZ.");
                }
            }
            koniecPsot:
            this.longitude = 0; this.canIStartCounting = false;
        }
        private void TransformerBW_DoWork(object sender, DoWorkEventArgs e)
        {
            try { getPointsData();
                if (this.canIStartCounting)
                {                  
                    if (!end.Equals(""))
                    {
                        GottaTransformThemAll();
                        Thread.Sleep(100);
                        TransformerBW.ReportProgress(99);
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano układu wyjściowego.");
                    }
                    this.canIStartCounting = false;
                }
               
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

        private void TransformerBW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 0:
                    this.MonitorRichTextBox.Text += this.errorCatcher; break;
                case 1:
                    this.MonitorRichTextBox.Text += this.comunicator; break;
                case 2:
                    this.MonitorRichTextBox.Text += this.comunicator; break;
                case 10:
                    FileOpenerButton.Visible = true; FileOpenerButton.Text = "U2000"; break;
                case 20:
                    FileOpenerButton.Visible = true; FileOpenerButton.Text = "U1992"; break;
                case 30:
                    FileOpenerButton.Visible = true; FileOpenerButton.Text = "UTM"; break;
                case 40:
                    FileOpenerButton.Visible = true; FileOpenerButton.Text = "BLH"; break;
                case 50: FileOpenerButton.Visible = true; FileOpenerButton.Text = "XYZ"; break;
                case 99: this.MonitorRichTextBox.Text += "Koniec obliczeń."; break;
                default: break;
            }
        }

        private void TransformerBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

 
    }


    public partial class Point
    {
        string name;
        double X, Y;
        public Point(string name, double X, double Y)
        {
            this.name = name; this.X = X; this.Y = Y;
        }
        public double x()
        {
            return this.X;
        }
        public double y()
        {
            return this.Y;
        }
        public string Name()
        {
            return this.name;
        }

    }
    public partial class Point3D
    {
        string name;
        double X, Y; double Z;
        public Point3D(string name, double X, double Y, double Z)
        {
            this.name = name; this.X = X; this.Y = Y; this.Z = Z;
        }
        public string Name()
        {
            return this.name;
        }
        public double x()
        {
            return this.X;
        }
        public double y()
        {
            return this.Y;
        }
        public double z()
        {
            return this.Z;
        }

        public void ETRF89TO2000()
        {
            double Xs = 3696570.6591; double Ys = 1297521.5905; double Zs = 5011111.1273;
            double dX = this.X - Xs; double dY = this.Y - Ys; double dZ = this.Z - Zs;
            double Xfin= this.X + (-0.0322) + (-0.00000005102) * dX + (-0.00000000746) * dY + (0.00000004804) * dZ;
            double Yfin= this.Y + (-0.0347) + (0.00000000746) * dX + (-0.00000005102) * dY + (0.00000006152) * dZ;
            double Zfin= this.Z + (-0.0507) + (-0.00000004804) * dX + (-0.00000006152) * dY + (-0.00000005102) * dZ;
            this.X = Xfin; this.Y = Yfin; this.Z = Zfin; 
            //CZY NIE LEPIEJ STWORZYĆ NOWY PUNKT???           
        }
        public void ETRF2000TO89()
        {
            double Xs = 3696570.6268; double Ys = 1297521.5559; double Zs = 5011111.0767;
            double dX = this.X - Xs; double dY = this.Y - Ys; double dZ = this.Z - Zs;
            double Xfin = this.X + (0.0322) + (0.00000005102) * dX + (0.00000000746) * dY + (-0.00000004804) *dZ;
            double Yfin = this.Y + (0.0347) + (-0.00000000746) * dX + (0.00000005102) * dY + (-0.00000006152) * dZ;
            double Zfin = this.Z + (0.0507) + (0.00000004804) * dX + (0.00000006152) * dY + (0.00000005102) * dZ;
            this.X = Xfin; this.Y = Yfin; this.Z = Zfin;
        }
        public void Display()
        {
            MessageBox.Show(this.X + " " + this.Y + " " + this.Z);
        }

    }
    public partial class PointBLH
    {
        string name;
        double B = -1, Bmin = -1, Bsec = -1, L = -1, Lmin = -1, Lsec = -1;
        double H;
        //format: true - ułamki kątowe; false - kąt,min,sec;
        bool format;
        public PointBLH(string name, double B, double L, double H)
        {
            this.name = name; this.B = B; this.L = L; this.H = H; this.format = true;
        }
        public PointBLH(string name, double B, double Bmin, double Bsec, double L, double Lmin, double Lsec, double H)
        {
            this.name = name; this.B = B; this.Bmin = Bmin; this.Bsec = Bsec; this.L = L; this.Lmin = Lmin; this.Lsec = Lsec;
            this.H = H; this.format = false; 
        }
        public string Name()
        {
            return this.name;
        }
        public double fi()
        {
            return this.B;
        }
        public double lambda()
        {
            return this.L;
        }
        public double height()
        {
            return this.H;
        }
        public bool Format()
        {
            return this.format;
        }
        public double bmin()
        {
            return this.Bmin;
        }
        public double bsec()
        {
            return this.Bsec;
        }
        public double lmin()
        {
            return this.Lmin;
        }
        public double lsec()
        {
            return this.Lsec;
        }

        public void convertToDegrees()
        {
            this.B = this.B + this.Bmin / 60 + this.Bsec / 3600;
            this.L = this.L + this.Lmin / 60 + this.Lsec / 3600;
            this.Bmin = -1; this.Bsec = -1; this.Lmin = -1; this.Lsec = -1;
            this.format = true;
        }
        public void convertToDegreesMinsNSecs()
        {
            this.Bmin = Math.Floor((this.B-Math.Floor(this.B))*60);
            this.Lmin = Math.Floor((this.L - Math.Floor(this.L)) * 60);
            this.Bsec = ((this.B - Math.Floor(this.B)) * 60-this.Bmin) * 60;
            this.Lsec = ((this.L - Math.Floor(this.L)) * 60 - this.Lmin) * 60;
            this.B = Math.Floor(this.B);
            this.L = Math.Floor(this.L);
            this.format = false;
        }
        public void Display()
        {
            if (this.format)
            {
                MessageBox.Show(Math.Round(this.B*10000)/10000 + " " + Math.Round(this.L*10000)/10000);
            }
            else
            {
         MessageBox.Show(this.B + " " + this.Bmin + " " + this.Bsec + "\n" + this.L + " " + this.Lmin + " " + this.Lsec);
            }
           
        }
        public bool isCorrectlyFormated()
        { bool Btrue = false; byte problems = 5; bool problemsVol2 = false;
            //SPRAWDZA CZY STOPNIE NALEŻĄ DO WARTOŚCI [0,90] I [0,180]
            if ((this.B >= 0 && this.B <= 90) && (this.L >= 0 && this.L <= 180))
            {
                problems--; problemsVol2 = true;
            }
            else
            {
                //MessageBox.Show("Punkt \"" + this.name + "\": Nieprawidłowy format zapisu stopni (Wartość B poza [0,90] \\ wartość L poza [0,180)).");
            }
            if (!this.format)
            {
                //SPRAWDZA CZY WARTOŚCI STOPNIOWE SĄ LICZBAMI CAŁKOWITYMI
                bool isIntegerB = Math.Floor(this.B).Equals(this.B);
                bool isIntegerL = Math.Floor(this.L).Equals(this.L);
                if (isIntegerB && isIntegerL)
                {
                    problems--;
                }
                else
                {
                    //MessageBox.Show("Punkt \"" + this.name + "\": Nieprawidłowy format zapisu stopni (Wymagana wartość całkowita).");
                }
                //SPRAWDZA CZY MINUTY KĄTOWE SĄ WARTOŚCIAMI CAŁKOWITYMI
                bool minutesAreInteger = Math.Floor(this.Bmin).Equals(this.Bmin) && Math.Floor(this.Lmin).Equals(this.Lmin);
                if (minutesAreInteger)
                {
                    problems--;
                }
                else
                {
                    //MessageBox.Show("Punkt \"" + this.name + "\": Nieprawidłowy format zapisu minut kątowych(Wymagana wartość całkowita).");
                }
                // SPRAWDZA CZY MINUTY KĄTOWE NALEŻĄ DO PRZEDZIAŁU [0,60)
                bool isbetweenB = (this.Bmin >= 0 && this.Bmin < 60);
                bool isBetweenL = (this.Lmin >= 0 && this.Lmin < 60);
                if (isbetweenB && isBetweenL)
                {
                    problems--;
                }
                else
                {
                    //MessageBox.Show("Punkt \"" + this.name + "\": Nieprawidłowy format zapisu minut kątowych. Wartość poza [0,60)");
                }
                //SPRAWDZA CZY SEKUNDY KĄTOWE NALEŻĄ DO PRZEDZIAŁU [0,60)
                if ((this.Bsec >= 0 && this.Bsec < 60) && (this.Lsec >= 0 && this.Lsec < 60))
                {
                    problems--;
                }
                else
                {
                    //MessageBox.Show("Punkt \"" + this.name + "\": Nieprawidłowy format zapisu sekund kątowych. Wartość poza [0,60)");
                }
                return Btrue = problems == 0 ? true : false;
            }
            else {
                return problemsVol2;
            } 
        }
    
    }
    
    public partial class WebGrid
    {
        double B, L, dB, dL, dH;
        public WebGrid(double B, double L, double dB, double dL, double dH)
        {
            this.B = B; this.L = L; this.dB = dB; this.dL = dL; this.dH = dH;
        }
        public double fi()
        {
            return this.B;
        }
        public double lambda()
        {
            return this.L;
        }
        public double deltaFi()
        {
            return this.dB;
        }
        public double deltaLambda()
        {
            return this.dL;
        }
        public double deltaH()
        {
            return this.dH;
        }
    }
   
}
