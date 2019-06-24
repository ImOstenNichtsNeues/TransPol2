using System;
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
namespace WindowsFormsApp1
{

    public partial class Transform : Form
    {
        
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
        byte longitude=0;
        // canIStartCounting sprawdza, czy dane wejściowe zostały wprowadzone prawidłowo oraz czy układ wyjściowy został wybrany.
        bool canIStartCounting = false;
        //TransformateOption określa czy wybrano rozwiązanie teoretyczne czy empiryczne [grid]. True - teoretyczna, false - empiryczna.
        bool transformateOption = true;
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
                                //MessageBox.Show(data[1] + " " + data[2]);
                            }
                            catch (Exception ex) //SPRAWDZENIE CZY FORMAT PLIKÓW JEST DOPUSZCZALNY
                            {
                                if (ex is FormatException || ex is ArgumentException)
                                {
                                    problems++;
                                    this.MonitorRichTextBox.Text+=("Punkt " + data[0] + ": Niedopuszczalna wartość parametrów.\n");
                                }
                                else { MessageBox.Show("Nastąpił błąd wczytania pliku. Sprawdź format danych wejściowych."); problems++; break; }
                            }
                        }
                        else { this.MonitorRichTextBox.Text += ("Punkt " + data[0] + ": Nieodpowiednia liczba wejściowych parametrów(" + data.Length + ").\n"); problems++; }
                    }
                }
            }
            else { MessageBox.Show("Plik \"" + Path.GetFileName(filePath.Text) + "\" jest pusty."); problems++; }
            clock.Stop();
            if (problems == 0) { this.canIStartCounting = true; 
            this.MonitorRichTextBox.Text += "Czas wczytywania danych startowych: " + Convert.ToString(clock.Elapsed)+ ".\n";
            }
            //MessageBox.Show(clock.Elapsed+" ");
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
                                //MessageBox.Show(data[1] + " " + data[2]);
                            }
                            catch (Exception ex) //SPRAWDZENIE CZY FORMAT PLIKÓW JEST DOPUSZCZALNY
                            {
                                if (ex is FormatException || ex is ArgumentException)
                                {
                                    problems = true; 
                                    this.MonitorRichTextBox.Text += ("Punkt " + data[0] + ": Niedopuszczalna wartość parametrów.\n");
                                }
                                else { MessageBox.Show("Nastąpił błąd wczytania pliku. Sprawdź format danych wejściowych.");
                                    problems = true;  break; }
                            }
                        }
                        else {
                            this.MonitorRichTextBox.Text += ("Punkt " + data[0] + ": Nieodpowiednia liczba wejściowych parametrów(" + data.Length + ").\n");
                            problems =true; }
                    }
                }
            }
            else { MessageBox.Show("Plik \"" + Path.GetFileName(filePath.Text) + "\" jest pusty.");
                problems = true; }  
            clock.Stop();
            if (!problems)
            {
                this.canIStartCounting = true;
                this.MonitorRichTextBox.Text += "Czas wczytywania danych startowych: " + Convert.ToString(clock.Elapsed)+ ".\n";
            }
            //MessageBox.Show(clock.Elapsed + " ");
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
                                            PointsBLH.Last().isCorrectlyFormated();
                                            anyFalseFormattedPoint = false;
                                        }
                                        //MessageBox.Show(data[1] + " " + data[2]);
                                    }
                                    catch (Exception ex) //SPRAWDZENIE CZY FORMAT PLIKÓW JEST DOPUSZCZALNY
                                    {
                                        if (ex is FormatException || ex is ArgumentException)
                                        {
                                            problems=true;
                                            this.MonitorRichTextBox.Text += ("Punkt " + data[0] + ": Niedopuszczalna wartość parametrów."); 
                                        }
                                        else { MessageBox.Show("Nastąpił błąd wczytania pliku. Sprawdź format danych wejściowych.");
                                            problems=true; goto gameOver; }
                                    }
                                } 
                                else {
                                    this.MonitorRichTextBox.Text += ("Punkt " + data[0] + ": Nieodpowiednia liczba wejściowych parametrów(" + data.Length + ")");
                                    problems=true;  }
                                break;
                            case 8:
                                if (data.Length == format)
                                {
                                    try
                                    {
                                        this.PointsBLH.Add(new PointBLH(data[0], Convert.ToDouble(data[1]), Convert.ToDouble(data[2]), Convert.ToDouble(data[3]), Convert.ToDouble(data[4]), Convert.ToDouble(data[5]), Convert.ToDouble(data[6]), Convert.ToDouble(data[7])));  
                                        if (!PointsBLH.Last().isCorrectlyFormated())
                                        {
                                            PointsBLH.Last().isCorrectlyFormated();
                                            anyFalseFormattedPoint = false;
                                        }
                                        //MessageBox.Show(data[1] + " " + data[4]);
                                    }
                                    catch (Exception ex) //SPRAWDZENIE CZY FORMAT PLIKÓW JEST DOPUSZCZALNY
                                    {
                                        if (ex is FormatException || ex is ArgumentException)
                                        {
                                            problems=true;
                                            this.MonitorRichTextBox.Text += ("Punkt " + data[0] + ": Niedopuszczalna wartość parametrów.");
                                        }
                                        else { MessageBox.Show("Nastąpił błąd wczytania pliku. Sprawdź format danych wejściowych.");
                                            problems =true; goto gameOver; }
                                    }
                                }
                                else {
                                    this.MonitorRichTextBox.Text += ("Punkt " + data[0] + ": Nieodpowiednia liczba wejściowych parametrów(" + data.Length + ")");
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
                this.MonitorRichTextBox.Text += "\n Czas wczytywania danych startowych: " + Convert.ToString(clock.Elapsed);
            } 
            //MessageBox.Show(clock.Elapsed + " ");
        }
        public void getPointsData() 
        {
            if (!filePath.Text.Equals(""))
            {
                if (this.start.ToString().Equals("Układ 2000"))
                {
                    this.longitude = setLongitude(longitude15, longitude18, longitude21, longitude24);
                    if (this.longitude != 0)
                    {
                        loadPoints2D();
                    }
                    else
                    {
                        MessageBox.Show("Nie określono południka osiowego."); 
                    }
                }
                else if (this.start.ToString().Equals("Układ 1992"))
                {
                    this.longitude = 19;
                    loadPoints2D();
                }
                else if (this.start.ToString().Equals("UTM"))
                {
                        this.longitude = setLongitude(longitudeUTM15, longitudeUTM21);
                        if (this.longitude != 0)
                        {
                            loadPoints2D();
                        }
                        else
                        {
                            MessageBox.Show("Nie określono południka osiowego.");
                        }
                    }
                else if (this.start.ToString().Equals("XYZ GRS80"))
                {
                    loadPoints3D();
                }
                else if (this.start.ToString().Equals("BLH GRS80"))
                {
                    loadPointsBLH();
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
            
            for(int i=0; i< list.Length; i++)
            {
                if (list[i].Checked)
                {
                    this.longitude = Convert.ToByte(list[i].Text.Substring(0, 2)); break;
                }
            }
            return this.longitude;
        }
        private void CountUp_Click(object sender, EventArgs e)
        {
            this.MonitorRichTextBox.Clear(); this.MonitorRichTextBox.Text = "MONITOR: \n";
            //List<WebGrid> greedy = webGrids();
            //MessageBox.Show(this.degreeForm + " " + this.resultDegreeForm);
            if (!filePath.Text.Equals(""))
            {
                getPointsData();         
                if (this.canIStartCounting)
                {
                    this.PointsBLH.ForEach(p => p.convertToDegrees());
                    setGridDeltas(this.PointsBLH);
                    double anglePrecision = Convert.ToDouble(this.AnglePrecisionDUD.Text)/3600 * Math.PI / 180;
                    double lengthPrecision = Convert.ToDouble(this.LengthPrecisionDUD.Text) * Math.PI / 180;
                    this.Points3D.Clear();
                    this.Points.Clear();
                    this.PointsBLH.Clear();
                }      
            }
            else
            {
                MessageBox.Show("Nie wybrano zbioru punktów.");
            }
            this.longitude = 0; this.canIStartCounting = false;
        }
        //WCZYTANIE SIATKI GRID DO METODY EMPIRYCZNEJ oraz INTERPOLACJA DWULINIOWA
        private List<WebGrid> webGrids()
        {
            List<WebGrid> result = new List<WebGrid>();
            string [] text = File.ReadAllLines("gridETRF.txt");
            foreach(string line in text)
            {
                string[] point = Regex.Split(line.Trim(), @"\s+");
                result.Add(new WebGrid(Convert.ToDouble(point[0]), Convert.ToDouble(point[1]), Convert.ToDouble(point[2]), Convert.ToDouble(point[3]), Convert.ToDouble(point[4])));
            }
            return result;
        }
        private List<PointBLH> setGridDeltas(List<PointBLH> points)
        {
            Stopwatch clock = new Stopwatch(); clock.Start();
            List<WebGrid> grid = webGrids();
            clock.Stop();
            this.MonitorRichTextBox.Text += "\n Wczytywanie siatki grid: " + Convert.ToString(clock.Elapsed);
            List<PointBLH> result = new List<PointBLH>();
            points.ForEach(p =>
            {
                double B = p.fi(); double Bdown = Math.Floor(B * 100) / 100; double Bup = Math.Ceiling(B * 100) / 100;
                double L = p.lambda(); double Ldown = Math.Floor(L * 100) / 100; double Lup = Math.Ceiling(L * 100) / 100;
                WebGrid grid11 = grid.Find(q => q.fi().Equals(Bdown) && q.lambda().Equals(Ldown));
                //MessageBox.Show(grid11.fi() + " " + grid11.lambda());
                WebGrid grid12 = grid.Find(q => q.fi().Equals(Bup) && q.lambda().Equals(Ldown));
                WebGrid grid21 = grid.Find(q => q.fi().Equals(Bdown) && q.lambda().Equals(Lup));
                WebGrid grid22 = grid.Find(q => q.fi().Equals(Bup) && q.lambda().Equals(Lup));
                BilinearInterpolation(B, L, grid11, grid12, grid21, grid22);
            });
            return result;
        }
        /* KONIECZNIE ŁADOWAĆ PUNKTY SIATKI W ODPOWIEDNIEJ KOLEJNOŚCI!! */
        private void BilinearInterpolation(double B, double L, WebGrid grid11, WebGrid grid12, WebGrid grid21, WebGrid grid22)
        {
            //MessageBox.Show(grid11.lambda() + "");
            double df1 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid11.deltaFi() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid21.deltaFi();
            double df2 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid12.deltaFi() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid22.deltaFi();
            MessageBox.Show(df2+"");
            double dfi = (grid21.fi() - B) / (grid21.fi() - grid11.fi()) * df1 + (B - grid11.fi()) / (grid21.fi() - grid11.fi()) * df2;
            double dl1 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid11.deltaLambda() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid21.deltaLambda();
            double dl2 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid12.deltaLambda() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid22.deltaLambda();
            double dl = (grid21.fi() - B) / (grid21.fi() - grid11.fi()) * dl1 + (B - grid11.fi()) / (grid21.fi() - grid11.fi()) *dl2;
            double dh1 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid11.deltaH() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid21.deltaH();
            double dh2 = (grid21.lambda() - L) / (grid21.lambda() - grid11.lambda()) * grid12.deltaH() + (L - grid11.lambda()) / (grid21.lambda() - grid11.lambda()) * grid22.deltaH();
            double dh = (grid21.fi() - B) / (grid21.fi() - grid11.fi()) * dh1 + (B - grid11.fi()) / (grid21.fi() - grid11.fi()) * dh2;
            MessageBox.Show(dfi+"");
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
        //WYBÓR UKŁADU ODNIESIENIA PLIKÓW: WEJŚCIOWEGO I WYJŚCIOWEGO
        private void ETRF2000_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceOne.Visible = true;
            this.startETRF.Clear();
            this.startETRF.Append("ETRF2000");
        }

        private void ETRF89_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceOne.Visible = true;
            this.startETRF.Clear();
            this.startETRF.Append("ETRF89");
        }

        private void ResETRF2000_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceTwo.Visible = true;
            this.endETRF.Clear();
            this.endETRF.Append("ETRF2000");
        }

        private void ResETRF89_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceTwo.Visible = true;
            this.endETRF.Clear();
            this.endETRF.Append("ETRF89");
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
            });
            return result;
        }
        public List<Point> U1992ToGK(List<Point> Points)
        {
            List<Point> result = new List<Point>();
            Points.ForEach(p =>
            {
                double xGK = (p.x() + 5300000) / 0.9993;
                double yGK = (p.y() - 500000) / 0.9993;
                result.Add(new Point(p.Name(), xGK, yGK));
            });
            return result;
        }
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
        }
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
        }
        public List<Point3D> BLH2XYZ(List<PointBLH> PointsBLH)
        {
            double a = 6378137;
            double e2 = 0.006694380022903;
            List<Point3D> result = new List<Point3D>();
            PointsBLH.ForEach(x =>
            {
                if (!x.Format()) { x.convertToDegrees(); }
                string name = x.Name();
                double N = a / Math.Sqrt(1 - e2 * Math.Sin(x.fi()*Math.PI/180) * Math.Sin(x.fi() * Math.PI / 180));
                double X = (N + x.height()) * Math.Cos(x.fi() * Math.PI / 180) * Math.Cos(x.lambda() * Math.PI / 180);
                double Y = (N + x.height()) * Math.Cos(x.fi() * Math.PI / 180) * Math.Sin(x.lambda() * Math.PI / 180);
                double Z = (N + x.height()) * Math.Sin(x.fi() * Math.PI / 180) - e2 * N * Math.Sin(x.fi() * Math.PI / 180);
                result.Add(new Point3D(name, X, Y, Z));
            });
            return result;
        }
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
                    H = Math.Sqrt(Math.Pow(point.x(), 2) + Math.Pow(point.y(), 2)) / Math.Cos(B) - N;
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
        }
        public List<Point> BLH2XYGK(List<PointBLH> PointsBLH, byte longitude)
        {
            List<Point> result = new List<Point>();
            double a = 6378137;
            double e2 = 0.006694380022903;
            double e12 = 0.00673949677548;
            double longitude0 = longitude * Math.PI / 180;
            double A0 = 1 - (e2 / 4) - (3 * Math.Pow(e2,2) / 64) - (5 * Math.Pow(e2,3) / 256);
            double A2 = 3 / 8 * (e2 + (Math.Pow(e2,2) / 4) + 15 * Math.Pow(e2,3) / 128);
            double A4 = 15 / 256 * (Math.Pow(e2,2) + 3 * Math.Pow(e2,3) / 4);
            double A6 = 35 * Math.Pow(e2,3) / 3072;
            PointsBLH.ForEach(p =>
            {
                if (!p.Format()) { p.convertToDegrees(); }
                double fi = p.fi() * Math.PI / 180; double lambda = p.lambda() * Math.PI / 180;
                double sigma = a * (A0 * fi - A2 * Math.Sin(2 * fi) + A4 * Math.Sin(4 * fi) - A6 * Math.Sin(6 * fi));
                double l = lambda - longitude0;
                double t = Math.Tan(fi);
                double eta2 = (e12) * (Math.Cos(fi)) * Math.Cos(fi);
                double N = a / Math.Sqrt(1 - e2 * Math.Sin(fi) * Math.Sin(fi));
  double xGK = sigma + (Math.Pow(l,2) / 2) * N * Math.Sin(fi) * Math.Cos(fi) * (1 + (Math.Pow(l, 2) / 12) * (Math.Pow(Math.Cos(fi),2)) * (5 - Math.Pow(t,2) + 9 * (eta2) + 4 * Math.Pow(eta2,2)) + (Math.Pow(l,4) / 360) * Math.Pow(Math.Cos(fi),4) * (61 - 58 * Math.Pow(t,2) + Math.Pow(t,4) + 270 * (eta2) - 330 * (eta2) * Math.Pow(t,2)));
  double yGK = l * N * Math.Cos(fi) * (1 + (Math.Pow(l,2) / 6) * Math.Pow(Math.Cos(fi),2) * (1 - Math.Pow(t,2) + eta2) + (Math.Pow(l,4) / 120) * Math.Pow(Math.Cos(fi),4) * (5 - 18 * Math.Pow(t,2) + Math.Pow(t,4) + 14 * (eta2) - 58 * (eta2) * Math.Pow(t,2)));
                result.Add(new Point(p.Name(), xGK, yGK));
            });
            return result;
        }
        public List<PointBLH> XYGK2BLH(List<Point> Points, byte longitude, double precision)
        {
            List<PointBLH> result = new List<PointBLH>();
            double a = 6378137;
            double e2 = 0.006694380022903;
            double e12 = 0.00673949677548;
            double longitude0 = longitude * Math.PI / 180;
            double A0 = 1 - (e2 / 4) - (3 * Math.Pow(e2, 2) / 64) - (5 * Math.Pow(e2, 3) / 256);
            double A2 = 3 / 8 * (e2 + (Math.Pow(e2, 2) / 4) + 15 * Math.Pow(e2, 3) / 128);
            double A4 = 15 / 256 * (Math.Pow(e2, 2) + 3 * Math.Pow(e2, 3) / 4);
            double A6 = 35 * Math.Pow(e2, 3) / 3072;
            Points.ForEach(p =>
            {
                double epsilon = 1;
                double sigma = p.x();
                double fi0 = sigma / a * A0;
                while (epsilon > precision)
                {
                    double fi1 = ((sigma / a) + A2 * Math.Sin(2 * fi0) - A4 * Math.Sin(4 * fi0) + A6 * Math.Sin(6 * fi0)) / A0;
                    epsilon = Math.Abs(fi1 - fi0);
                    fi0 = fi1;
                }
                double N = a / Math.Sqrt(1 - e2 * Math.Sin(fi0 * Math.PI / 180) * Math.Sin(fi0 * Math.PI / 180));
                double M = a * (1 - e2) / Math.Pow(Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(fi0), 2)), 3);
                double t = Math.Tan(fi0);
                double eta2 = (e12) * Math.Cos(fi0) * Math.Cos(fi0);
                double L = longitude+ p.y() / (N * (Math.Cos(fi0))) * (1 - (Math.Pow(p.y(),2) / (6 * Math.Pow(N,2))) * (1 + 2 * Math.Pow(t,2) + (eta2)) + (Math.Pow(p.y(),4) / (120 * Math.Pow(N,4))) * (5 + 28 * Math.Pow(t,2) + 24 * Math.Pow(t,4) + 6 * (eta2) + 8 * (eta2) * Math.Pow(t,2)));
                double B = fi0 - ((Math.Pow(p.y(),2) * t) / (2 * M * N)) * (1 - (Math.Pow(p.y(),2) / (12 * Math.Pow(N,2))) * (5 + 3 * Math.Pow(t,2) + (eta2) - 9 * eta2 * Math.Pow(t,2) - 4 * Math.Pow(eta2,2)) + Math.Pow(p.y(),4) / (360 * Math.Pow(N,4)) * (61 + 90 * Math.Pow(t,2) + 45 * Math.Pow(t,4)));
                result.Add(new PointBLH(p.Name(), B * 180 / Math.PI, L*180/Math.PI, 0 ));
            });
            return result;
        }
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
            double A2 = 3 / 8 * (e2 + (Math.Pow(e2, 2) / 4) + 15 * Math.Pow(e2, 3) / 128);
            double A4 = 15 / 256 * (Math.Pow(e2, 2) + 3 * Math.Pow(e2, 3) / 4);
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
                yUTM += N * l7 / 5040 * cos7fi * (61 - 479 * t2 + 179 * t4 - t6);
                yUTM *= m0;
                result.Add(new Point(p.Name(), xUTM, yUTM));
            });
            return result;
        }
        public List<PointBLH> UTM2BLH(List<Point> Points, byte longitude, double precision)
        {
            List<PointBLH> result = new List<PointBLH>();
            double m0 = 0.9996;
            double longitude0 = longitude * Math.PI / 180;
            double e2 = 0.00669438;
            double a = 6378137;
            double b = 6356752.3142;
            e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);
            double A0 = 1 - (e2 / 4) - (3 * Math.Pow(e2, 2) / 64) - (5 * Math.Pow(e2, 3) / 256);
            double A2 = 3 / 8 * (e2 + (Math.Pow(e2, 2) / 4) + 15 * Math.Pow(e2, 3) / 128);
            double A4 = 15 / 256 * (Math.Pow(e2, 2) + 3 * Math.Pow(e2, 3) / 4);
            double A6 = 35 * Math.Pow(e2, 3) / 3072;
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
                double N = a / Math.Sqrt(1 - e2 * Math.Sin(fi0 * Math.PI / 180) * Math.Sin(fi0 * Math.PI / 180));
                double M = a * (1 - e2) / Math.Pow(Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(fi0), 2)), 3);
                double psi = N / M; double psi2 = Math.Pow(psi, 2); double psi4 = Math.Pow(psi, 4); double psi3 = Math.Pow(psi, 3);
                double t = Math.Tan(fi0); double t2 = Math.Pow(t, 2); double t4 = Math.Pow(t, 4); double t6 = Math.Pow(t, 6);
                double v = N; double v3 = Math.Pow(v, 3); double v5 = Math.Pow(v, 5); double v7 = Math.Pow(v, 7);
                double E = p.y(); double E2 = Math.Pow(E, 2); double E4 = Math.Pow(E, 4); double E6 = Math.Pow(E, 6); double E8 = Math.Pow(E, 8);
                double E3 = Math.Pow(E, 3); double E5 = Math.Pow(E, 5); double E7 = Math.Pow(E, 7);
                double m03 = Math.Pow(m0, 3); double m05 = Math.Pow(m0, 5); double m07 = Math.Pow(m0, 7);
                double element = E2 / (2 * m0 * v) - E4 / (24 * m03 * v3) * (-4 * psi2 + 9 * psi * (1 - t2) + 12 * t2);
                element += E6 / (720 * m05 * v5) * (8 * psi4 * (11 - 24 * t2) + 12 * psi3 * (21 - 71 * t2) + 15 * psi2 * (15 - 98 * t2 + 15 * t4) + 180 * psi * (5 * t2 - 3 * t4) + 360 * t4);
                element += E8 / (40320 * m07 * v7) * (1385 + 3633 * t2 + 4095 * t4 + 1575 * t6);
                element *= t / (m0 * M);
                double B = fi0 - element;
                double L = E / (m0 * v) - E3 / (6 * m03 * v3) * (psi + 2 * t2);
                L += E5 / (120 * m05 * v5) * (-4 * psi3 * (1 - 6 * t2) + psi2 * (9 - 68 * t2) + 72 * psi * t2 + 24 * t4);
                L += E7 / (5040 * m07 * v7) * (61 + 662 * t2 + 1320 * t4 + 720 * t6);
                L *= 1 / Math.Cos(fi0);
                result.Add(new PointBLH(p.Name(), B * 180 / Math.PI, L * 180 / Math.PI, 0));
            });
            return result;
        }
        //SCENARIUSZE TRANSFORMACYJNE: POPRAWIĆ I DODAĆ UTM!!!
        public List<Point> U2000To1992(byte longitude, double precision, List<Point> Points)
        {
            List<Point> result = new List<Point>();
            List<PointBLH> bottom = XYGK2BLH(U2000ToGK(Points, longitude), longitude, precision);
            if (this.startETRF.Equals(this.endETRF))
            {
                List<Point> result2 = GKToU1992(BLH2XYGK(bottom, longitude));
            }
            else if (this.startETRF.Equals("ETRF89") && this.endETRF.Equals(ETRF2000))
            {
                if (this.transformateOption)
                {
                    List<Point3D> helper = BLH2XYZ(bottom);
                    helper.ForEach(p => {
                        p.ETRF89TO2000();
                    });

                }
            }
            ////List<Point> result = GKToU1992(BLH2XYGK(XYGK2BLH(U2000ToGK(Points, longitude), longitude, precision), longitude));
            return result;
        }
        public List<Point> U1992To2000(byte longitude, double precision, List<Point> Points)
        {
            List<Point> result = GKToU2000(BLH2XYGK(XYGK2BLH(U1992ToGK(Points), longitude, precision), longitude), longitude);
                return result;
        }
        //wszystkie wartości longitude odnoszą się do południka osiowego układu 2000.
        public List<Point3D> U2000ToXYZ(byte longitude, double precision, List<Point> Points)
        {
            List<Point3D> result = BLH2XYZ(XYGK2BLH(U2000ToGK(Points, longitude), longitude, precision));
            return result;
        }
        public List<Point3D> U1992ToXYZ(byte longitude, double precision, List<Point> Points)
        {
            List<Point3D> result = BLH2XYZ(XYGK2BLH(U1992ToGK(Points), longitude, precision));
            return result;
        }
        public List<Point> XYZ2U2000(byte longitude, double precision, List<Point3D> Points)
        {
            List<Point> result = GKToU2000(BLH2XYGK(XYZ2BLH(Points,precision),longitude), longitude);
            return result;
        }
        public List<Point> XYZ2U1992(byte longitude, double precision, List<Point3D> Points)
        {
            List<Point> result = GKToU1992(BLH2XYGK(XYZ2BLH(Points, precision), longitude));
            return result;
        }
        public List<PointBLH> U2000ToBLH(byte longitude, double precision, List<Point> Points)
        {
            List<PointBLH> result = XYGK2BLH(U2000ToGK(Points, longitude), longitude, precision);
                return result;
        }
        public List<PointBLH> U1992ToBLH(byte longitude, double precision, List<Point> Points)
        {
            List<PointBLH> result = XYGK2BLH(U1992ToGK(Points), longitude, precision);
            return result;
        }
        public List<Point> BLH2U2000(byte longitude, double precision, List<PointBLH> Points)
        {
            List<Point> result = GKToU2000(BLH2XYGK(Points,longitude), longitude);
            return result;
        }
        public List<Point> BLH2U1992(byte longitude, double precision, List<PointBLH> Points)
        {
            List<Point> result = GKToU1992(BLH2XYGK(Points, longitude));
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
            this.MonitorRichTextBox.Clear(); this.MonitorRichTextBox.Text = "MONITOR: \n";
        }

        private void TeoreticOptionRB_CheckedChanged(object sender, EventArgs e)
        {
            this.transformateOption = true;
        }

        private void GridOptionRB_CheckedChanged(object sender, EventArgs e)
        {
            this.transformateOption = false;
        }
        //NADPISYWANIE RICHTEXTBOXA - FUNCKJA DLA POZOSTAŁYCH KLAS CZĘŚCIOWYCH
        public static void Display2TextBox( string text)
        {          
             box.Text += "\n " + text;
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
               
                Transform.Display2TextBox("Punkt \"" + this.name + "\": Nieprawidłowy format zapisu stopni (Wartość B poza [0,90] \\ wartość L poza [0,180)).");
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
                    WindowsFormsApp1.Transform.Display2TextBox("Punkt \"" + this.name + "\": Nieprawidłowy format zapisu stopni (Wymagana wartość całkowita).");
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
                    WindowsFormsApp1.Transform.Display2TextBox("Punkt \"" + this.name + "\": Nieprawidłowy format zapisu minut kątowych(Wymagana wartość całkowita).");
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
                    WindowsFormsApp1.Transform.Display2TextBox("Punkt \"" + this.name + "\": Nieprawidłowy format zapisu minut kątowych. Wartość poza [0,60)");
                }
                //SPRAWDZA CZY SEKUNDY KĄTOWE NALEŻĄ DO PRZEDZIAŁU [0,60)
                if ((this.Bsec >= 0 && this.Bsec < 60) && (this.Lsec >= 0 && this.Lsec < 60))
                {
                    problems--;
                }
                else
                {
                    WindowsFormsApp1.Transform.Display2TextBox("Punkt \"" + this.name + "\": Nieprawidłowy format zapisu sekund kątowych. Wartość poza [0,60)");
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
