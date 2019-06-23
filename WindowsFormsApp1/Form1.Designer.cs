namespace WindowsFormsApp1
{
    partial class Transform
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChoiceOne = new System.Windows.Forms.GroupBox();
            this.XYZ_GRS = new System.Windows.Forms.RadioButton();
            this.BLH_GRS = new System.Windows.Forms.RadioButton();
            this.XY1992 = new System.Windows.Forms.RadioButton();
            this.UTM = new System.Windows.Forms.RadioButton();
            this.XY2000 = new System.Windows.Forms.RadioButton();
            this.ChoiceTwo = new System.Windows.Forms.GroupBox();
            this.resultXYZ_GRS = new System.Windows.Forms.RadioButton();
            this.resultXY1992 = new System.Windows.Forms.RadioButton();
            this.resultBLH_GRS = new System.Windows.Forms.RadioButton();
            this.resultXY2000 = new System.Windows.Forms.RadioButton();
            this.resultUTM = new System.Windows.Forms.RadioButton();
            this.LongitudeChoice = new System.Windows.Forms.GroupBox();
            this.longitude24 = new System.Windows.Forms.RadioButton();
            this.longitude21 = new System.Windows.Forms.RadioButton();
            this.longitude18 = new System.Windows.Forms.RadioButton();
            this.longitude15 = new System.Windows.Forms.RadioButton();
            this.LongitudeUTM = new System.Windows.Forms.GroupBox();
            this.longitudeUTM21 = new System.Windows.Forms.RadioButton();
            this.longitudeUTM15 = new System.Windows.Forms.RadioButton();
            this.resultLongitudeChoice = new System.Windows.Forms.GroupBox();
            this.resultLongitude24 = new System.Windows.Forms.RadioButton();
            this.resultLongitude21 = new System.Windows.Forms.RadioButton();
            this.resultLongitude18 = new System.Windows.Forms.RadioButton();
            this.resultLongitude15 = new System.Windows.Forms.RadioButton();
            this.resultLongitudeUTM = new System.Windows.Forms.GroupBox();
            this.resultLongitudeUTM21 = new System.Windows.Forms.RadioButton();
            this.resultLongitudeUTM15 = new System.Windows.Forms.RadioButton();
            this.resFormatBLH = new System.Windows.Forms.GroupBox();
            this.resDegMinSecBLH = new System.Windows.Forms.RadioButton();
            this.resDegreesBLH = new System.Windows.Forms.RadioButton();
            this.CountUp = new System.Windows.Forms.Button();
            this.loadFile = new System.Windows.Forms.Button();
            this.filePath = new System.Windows.Forms.TextBox();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ETRF89 = new System.Windows.Forms.RadioButton();
            this.ETRF2000 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.resETRF89 = new System.Windows.Forms.RadioButton();
            this.resETRF2000 = new System.Windows.Forms.RadioButton();
            this.TextBoxTrans2D3D = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.PrecisionGB = new System.Windows.Forms.GroupBox();
            this.LengthPrecisionDUD = new System.Windows.Forms.DomainUpDown();
            this.AnglePrecisionDUD = new System.Windows.Forms.DomainUpDown();
            this.LengthInfo = new System.Windows.Forms.Label();
            this.AngleInfo = new System.Windows.Forms.Label();
            this.TransOption = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.FormatBLH = new System.Windows.Forms.GroupBox();
            this.degMinSecBLH = new System.Windows.Forms.RadioButton();
            this.degreesBLH = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ChoiceOne.SuspendLayout();
            this.ChoiceTwo.SuspendLayout();
            this.LongitudeChoice.SuspendLayout();
            this.LongitudeUTM.SuspendLayout();
            this.resultLongitudeChoice.SuspendLayout();
            this.resultLongitudeUTM.SuspendLayout();
            this.resFormatBLH.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.PrecisionGB.SuspendLayout();
            this.TransOption.SuspendLayout();
            this.FormatBLH.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChoiceOne
            // 
            this.ChoiceOne.BackColor = System.Drawing.Color.DarkGray;
            this.ChoiceOne.Controls.Add(this.XYZ_GRS);
            this.ChoiceOne.Controls.Add(this.BLH_GRS);
            this.ChoiceOne.Controls.Add(this.XY1992);
            this.ChoiceOne.Controls.Add(this.UTM);
            this.ChoiceOne.Controls.Add(this.XY2000);
            this.ChoiceOne.ForeColor = System.Drawing.Color.Black;
            this.ChoiceOne.Location = new System.Drawing.Point(4, 137);
            this.ChoiceOne.Name = "ChoiceOne";
            this.ChoiceOne.Size = new System.Drawing.Size(132, 138);
            this.ChoiceOne.TabIndex = 0;
            this.ChoiceOne.TabStop = false;
            this.ChoiceOne.Text = "GRS80";
            this.ChoiceOne.UseCompatibleTextRendering = true;
            this.ChoiceOne.Visible = false;
            // 
            // XYZ_GRS
            // 
            this.XYZ_GRS.AutoSize = true;
            this.XYZ_GRS.ForeColor = System.Drawing.Color.Maroon;
            this.XYZ_GRS.Location = new System.Drawing.Point(7, 113);
            this.XYZ_GRS.Name = "XYZ_GRS";
            this.XYZ_GRS.Size = new System.Drawing.Size(72, 17);
            this.XYZ_GRS.TabIndex = 9;
            this.XYZ_GRS.Text = "XYZ GRS";
            this.XYZ_GRS.UseVisualStyleBackColor = true;
            this.XYZ_GRS.CheckedChanged += new System.EventHandler(this.XYZ_GRS_CheckedChanged);
            // 
            // BLH_GRS
            // 
            this.BLH_GRS.AutoSize = true;
            this.BLH_GRS.ForeColor = System.Drawing.Color.Maroon;
            this.BLH_GRS.Location = new System.Drawing.Point(7, 90);
            this.BLH_GRS.Name = "BLH_GRS";
            this.BLH_GRS.Size = new System.Drawing.Size(72, 17);
            this.BLH_GRS.TabIndex = 8;
            this.BLH_GRS.Text = "BLH GRS";
            this.BLH_GRS.UseVisualStyleBackColor = true;
            this.BLH_GRS.CheckedChanged += new System.EventHandler(this.BLH_GRS_CheckedChanged);
            // 
            // XY1992
            // 
            this.XY1992.AutoSize = true;
            this.XY1992.ForeColor = System.Drawing.Color.Maroon;
            this.XY1992.Location = new System.Drawing.Point(7, 44);
            this.XY1992.Name = "XY1992";
            this.XY1992.Size = new System.Drawing.Size(69, 17);
            this.XY1992.TabIndex = 1;
            this.XY1992.Text = "X Y 1992";
            this.XY1992.UseVisualStyleBackColor = true;
            this.XY1992.CheckedChanged += new System.EventHandler(this.XY1992_CheckedChanged);
            // 
            // UTM
            // 
            this.UTM.AutoSize = true;
            this.UTM.ForeColor = System.Drawing.Color.Maroon;
            this.UTM.Location = new System.Drawing.Point(7, 67);
            this.UTM.Name = "UTM";
            this.UTM.Size = new System.Drawing.Size(49, 17);
            this.UTM.TabIndex = 7;
            this.UTM.Text = "UTM";
            this.UTM.UseVisualStyleBackColor = true;
            this.UTM.CheckedChanged += new System.EventHandler(this.UTM_CheckedChanged);
            // 
            // XY2000
            // 
            this.XY2000.AutoSize = true;
            this.XY2000.ForeColor = System.Drawing.Color.Maroon;
            this.XY2000.Location = new System.Drawing.Point(7, 20);
            this.XY2000.Name = "XY2000";
            this.XY2000.Size = new System.Drawing.Size(69, 17);
            this.XY2000.TabIndex = 0;
            this.XY2000.Text = "X Y 2000";
            this.XY2000.UseVisualStyleBackColor = true;
            this.XY2000.CheckedChanged += new System.EventHandler(this.XY2000_CheckedChanged);
            // 
            // ChoiceTwo
            // 
            this.ChoiceTwo.BackColor = System.Drawing.Color.DarkGray;
            this.ChoiceTwo.Controls.Add(this.resultXYZ_GRS);
            this.ChoiceTwo.Controls.Add(this.resultXY1992);
            this.ChoiceTwo.Controls.Add(this.resultBLH_GRS);
            this.ChoiceTwo.Controls.Add(this.resultXY2000);
            this.ChoiceTwo.Controls.Add(this.resultUTM);
            this.ChoiceTwo.ForeColor = System.Drawing.Color.Black;
            this.ChoiceTwo.Location = new System.Drawing.Point(259, 137);
            this.ChoiceTwo.Name = "ChoiceTwo";
            this.ChoiceTwo.Size = new System.Drawing.Size(132, 138);
            this.ChoiceTwo.TabIndex = 1;
            this.ChoiceTwo.TabStop = false;
            this.ChoiceTwo.Text = "GRS80";
            this.ChoiceTwo.UseCompatibleTextRendering = true;
            this.ChoiceTwo.Visible = false;
            // 
            // resultXYZ_GRS
            // 
            this.resultXYZ_GRS.AutoSize = true;
            this.resultXYZ_GRS.ForeColor = System.Drawing.Color.Maroon;
            this.resultXYZ_GRS.Location = new System.Drawing.Point(7, 113);
            this.resultXYZ_GRS.Name = "resultXYZ_GRS";
            this.resultXYZ_GRS.Size = new System.Drawing.Size(72, 17);
            this.resultXYZ_GRS.TabIndex = 12;
            this.resultXYZ_GRS.Text = "XYZ GRS";
            this.resultXYZ_GRS.UseVisualStyleBackColor = true;
            this.resultXYZ_GRS.CheckedChanged += new System.EventHandler(this.ResultXYZ_GRS_CheckedChanged);
            // 
            // resultXY1992
            // 
            this.resultXY1992.AutoSize = true;
            this.resultXY1992.ForeColor = System.Drawing.Color.Maroon;
            this.resultXY1992.Location = new System.Drawing.Point(7, 44);
            this.resultXY1992.Name = "resultXY1992";
            this.resultXY1992.Size = new System.Drawing.Size(69, 17);
            this.resultXY1992.TabIndex = 1;
            this.resultXY1992.TabStop = true;
            this.resultXY1992.Text = "X Y 1992";
            this.resultXY1992.UseVisualStyleBackColor = true;
            this.resultXY1992.CheckedChanged += new System.EventHandler(this.ResultXY1992_CheckedChanged);
            // 
            // resultBLH_GRS
            // 
            this.resultBLH_GRS.AutoSize = true;
            this.resultBLH_GRS.ForeColor = System.Drawing.Color.Maroon;
            this.resultBLH_GRS.Location = new System.Drawing.Point(7, 90);
            this.resultBLH_GRS.Name = "resultBLH_GRS";
            this.resultBLH_GRS.Size = new System.Drawing.Size(72, 17);
            this.resultBLH_GRS.TabIndex = 11;
            this.resultBLH_GRS.Text = "BLH GRS";
            this.resultBLH_GRS.UseVisualStyleBackColor = true;
            this.resultBLH_GRS.CheckedChanged += new System.EventHandler(this.ResultBLH_GRS_CheckedChanged);
            // 
            // resultXY2000
            // 
            this.resultXY2000.AutoSize = true;
            this.resultXY2000.ForeColor = System.Drawing.Color.Maroon;
            this.resultXY2000.Location = new System.Drawing.Point(7, 20);
            this.resultXY2000.Name = "resultXY2000";
            this.resultXY2000.Size = new System.Drawing.Size(69, 17);
            this.resultXY2000.TabIndex = 0;
            this.resultXY2000.TabStop = true;
            this.resultXY2000.Text = "X Y 2000";
            this.resultXY2000.UseVisualStyleBackColor = true;
            this.resultXY2000.CheckedChanged += new System.EventHandler(this.ResultXY2000_CheckedChanged);
            // 
            // resultUTM
            // 
            this.resultUTM.AutoSize = true;
            this.resultUTM.ForeColor = System.Drawing.Color.Maroon;
            this.resultUTM.Location = new System.Drawing.Point(7, 67);
            this.resultUTM.Name = "resultUTM";
            this.resultUTM.Size = new System.Drawing.Size(49, 17);
            this.resultUTM.TabIndex = 10;
            this.resultUTM.Text = "UTM";
            this.resultUTM.UseVisualStyleBackColor = true;
            this.resultUTM.CheckedChanged += new System.EventHandler(this.ResultUTM_CheckedChanged);
            // 
            // LongitudeChoice
            // 
            this.LongitudeChoice.BackColor = System.Drawing.Color.DarkGray;
            this.LongitudeChoice.Controls.Add(this.longitude24);
            this.LongitudeChoice.Controls.Add(this.longitude21);
            this.LongitudeChoice.Controls.Add(this.longitude18);
            this.LongitudeChoice.Controls.Add(this.longitude15);
            this.LongitudeChoice.ForeColor = System.Drawing.Color.Black;
            this.LongitudeChoice.Location = new System.Drawing.Point(143, 162);
            this.LongitudeChoice.Name = "LongitudeChoice";
            this.LongitudeChoice.Size = new System.Drawing.Size(111, 113);
            this.LongitudeChoice.TabIndex = 2;
            this.LongitudeChoice.TabStop = false;
            this.LongitudeChoice.Text = "Pasy południkowe";
            this.LongitudeChoice.UseCompatibleTextRendering = true;
            this.LongitudeChoice.Visible = false;
            // 
            // longitude24
            // 
            this.longitude24.AutoSize = true;
            this.longitude24.ForeColor = System.Drawing.Color.Maroon;
            this.longitude24.Location = new System.Drawing.Point(8, 90);
            this.longitude24.Name = "longitude24";
            this.longitude24.Size = new System.Drawing.Size(44, 17);
            this.longitude24.TabIndex = 3;
            this.longitude24.TabStop = true;
            this.longitude24.Text = "24 °";
            this.longitude24.UseVisualStyleBackColor = true;
            // 
            // longitude21
            // 
            this.longitude21.AutoSize = true;
            this.longitude21.ForeColor = System.Drawing.Color.Maroon;
            this.longitude21.Location = new System.Drawing.Point(8, 67);
            this.longitude21.Name = "longitude21";
            this.longitude21.Size = new System.Drawing.Size(44, 17);
            this.longitude21.TabIndex = 2;
            this.longitude21.TabStop = true;
            this.longitude21.Text = "21 °";
            this.longitude21.UseVisualStyleBackColor = true;
            // 
            // longitude18
            // 
            this.longitude18.AutoSize = true;
            this.longitude18.ForeColor = System.Drawing.Color.Maroon;
            this.longitude18.Location = new System.Drawing.Point(7, 44);
            this.longitude18.Name = "longitude18";
            this.longitude18.Size = new System.Drawing.Size(44, 17);
            this.longitude18.TabIndex = 1;
            this.longitude18.TabStop = true;
            this.longitude18.Text = "18 °";
            this.longitude18.UseVisualStyleBackColor = true;
            // 
            // longitude15
            // 
            this.longitude15.AutoSize = true;
            this.longitude15.ForeColor = System.Drawing.Color.Maroon;
            this.longitude15.Location = new System.Drawing.Point(7, 20);
            this.longitude15.Name = "longitude15";
            this.longitude15.Size = new System.Drawing.Size(44, 17);
            this.longitude15.TabIndex = 0;
            this.longitude15.TabStop = true;
            this.longitude15.Text = "15 °";
            this.longitude15.UseVisualStyleBackColor = true;
            // 
            // LongitudeUTM
            // 
            this.LongitudeUTM.Controls.Add(this.longitudeUTM21);
            this.LongitudeUTM.Controls.Add(this.longitudeUTM15);
            this.LongitudeUTM.Location = new System.Drawing.Point(143, 161);
            this.LongitudeUTM.Name = "LongitudeUTM";
            this.LongitudeUTM.Size = new System.Drawing.Size(111, 66);
            this.LongitudeUTM.TabIndex = 7;
            this.LongitudeUTM.TabStop = false;
            this.LongitudeUTM.Text = "Pasy południkowe";
            this.LongitudeUTM.UseCompatibleTextRendering = true;
            this.LongitudeUTM.Visible = false;
            // 
            // longitudeUTM21
            // 
            this.longitudeUTM21.AutoSize = true;
            this.longitudeUTM21.Location = new System.Drawing.Point(8, 43);
            this.longitudeUTM21.Name = "longitudeUTM21";
            this.longitudeUTM21.Size = new System.Drawing.Size(44, 17);
            this.longitudeUTM21.TabIndex = 3;
            this.longitudeUTM21.TabStop = true;
            this.longitudeUTM21.Text = "21 °";
            this.longitudeUTM21.UseVisualStyleBackColor = true;
            // 
            // longitudeUTM15
            // 
            this.longitudeUTM15.AutoSize = true;
            this.longitudeUTM15.Location = new System.Drawing.Point(8, 19);
            this.longitudeUTM15.Name = "longitudeUTM15";
            this.longitudeUTM15.Size = new System.Drawing.Size(44, 17);
            this.longitudeUTM15.TabIndex = 2;
            this.longitudeUTM15.TabStop = true;
            this.longitudeUTM15.Text = "15 °";
            this.longitudeUTM15.UseVisualStyleBackColor = true;
            // 
            // resultLongitudeChoice
            // 
            this.resultLongitudeChoice.BackColor = System.Drawing.Color.DarkGray;
            this.resultLongitudeChoice.Controls.Add(this.resultLongitude24);
            this.resultLongitudeChoice.Controls.Add(this.resultLongitude21);
            this.resultLongitudeChoice.Controls.Add(this.resultLongitude18);
            this.resultLongitudeChoice.Controls.Add(this.resultLongitude15);
            this.resultLongitudeChoice.ForeColor = System.Drawing.Color.Black;
            this.resultLongitudeChoice.Location = new System.Drawing.Point(397, 162);
            this.resultLongitudeChoice.Name = "resultLongitudeChoice";
            this.resultLongitudeChoice.Size = new System.Drawing.Size(112, 113);
            this.resultLongitudeChoice.TabIndex = 3;
            this.resultLongitudeChoice.TabStop = false;
            this.resultLongitudeChoice.Text = "Pasy południkowe";
            this.resultLongitudeChoice.UseCompatibleTextRendering = true;
            this.resultLongitudeChoice.Visible = false;
            // 
            // resultLongitude24
            // 
            this.resultLongitude24.AutoSize = true;
            this.resultLongitude24.ForeColor = System.Drawing.Color.Maroon;
            this.resultLongitude24.Location = new System.Drawing.Point(8, 90);
            this.resultLongitude24.Name = "resultLongitude24";
            this.resultLongitude24.Size = new System.Drawing.Size(44, 17);
            this.resultLongitude24.TabIndex = 3;
            this.resultLongitude24.TabStop = true;
            this.resultLongitude24.Text = "24 °";
            this.resultLongitude24.UseVisualStyleBackColor = true;
            // 
            // resultLongitude21
            // 
            this.resultLongitude21.AutoSize = true;
            this.resultLongitude21.ForeColor = System.Drawing.Color.Maroon;
            this.resultLongitude21.Location = new System.Drawing.Point(8, 67);
            this.resultLongitude21.Name = "resultLongitude21";
            this.resultLongitude21.Size = new System.Drawing.Size(44, 17);
            this.resultLongitude21.TabIndex = 2;
            this.resultLongitude21.TabStop = true;
            this.resultLongitude21.Text = "21 °";
            this.resultLongitude21.UseVisualStyleBackColor = true;
            // 
            // resultLongitude18
            // 
            this.resultLongitude18.AutoSize = true;
            this.resultLongitude18.ForeColor = System.Drawing.Color.Maroon;
            this.resultLongitude18.Location = new System.Drawing.Point(7, 44);
            this.resultLongitude18.Name = "resultLongitude18";
            this.resultLongitude18.Size = new System.Drawing.Size(44, 17);
            this.resultLongitude18.TabIndex = 1;
            this.resultLongitude18.TabStop = true;
            this.resultLongitude18.Text = "18 °";
            this.resultLongitude18.UseVisualStyleBackColor = true;
            // 
            // resultLongitude15
            // 
            this.resultLongitude15.AutoSize = true;
            this.resultLongitude15.ForeColor = System.Drawing.Color.Maroon;
            this.resultLongitude15.Location = new System.Drawing.Point(7, 20);
            this.resultLongitude15.Name = "resultLongitude15";
            this.resultLongitude15.Size = new System.Drawing.Size(44, 17);
            this.resultLongitude15.TabIndex = 0;
            this.resultLongitude15.TabStop = true;
            this.resultLongitude15.Text = "15 °";
            this.resultLongitude15.UseVisualStyleBackColor = true;
            // 
            // resultLongitudeUTM
            // 
            this.resultLongitudeUTM.BackColor = System.Drawing.Color.DarkGray;
            this.resultLongitudeUTM.Controls.Add(this.resultLongitudeUTM21);
            this.resultLongitudeUTM.Controls.Add(this.resultLongitudeUTM15);
            this.resultLongitudeUTM.ForeColor = System.Drawing.Color.Black;
            this.resultLongitudeUTM.Location = new System.Drawing.Point(397, 161);
            this.resultLongitudeUTM.Name = "resultLongitudeUTM";
            this.resultLongitudeUTM.Size = new System.Drawing.Size(112, 66);
            this.resultLongitudeUTM.TabIndex = 8;
            this.resultLongitudeUTM.TabStop = false;
            this.resultLongitudeUTM.Text = "Pasy południkowe";
            this.resultLongitudeUTM.UseCompatibleTextRendering = true;
            this.resultLongitudeUTM.Visible = false;
            // 
            // resultLongitudeUTM21
            // 
            this.resultLongitudeUTM21.AutoSize = true;
            this.resultLongitudeUTM21.ForeColor = System.Drawing.Color.Maroon;
            this.resultLongitudeUTM21.Location = new System.Drawing.Point(8, 43);
            this.resultLongitudeUTM21.Name = "resultLongitudeUTM21";
            this.resultLongitudeUTM21.Size = new System.Drawing.Size(44, 17);
            this.resultLongitudeUTM21.TabIndex = 3;
            this.resultLongitudeUTM21.TabStop = true;
            this.resultLongitudeUTM21.Text = "21 °";
            this.resultLongitudeUTM21.UseVisualStyleBackColor = true;
            // 
            // resultLongitudeUTM15
            // 
            this.resultLongitudeUTM15.AutoSize = true;
            this.resultLongitudeUTM15.ForeColor = System.Drawing.Color.Maroon;
            this.resultLongitudeUTM15.Location = new System.Drawing.Point(8, 19);
            this.resultLongitudeUTM15.Name = "resultLongitudeUTM15";
            this.resultLongitudeUTM15.Size = new System.Drawing.Size(44, 17);
            this.resultLongitudeUTM15.TabIndex = 2;
            this.resultLongitudeUTM15.TabStop = true;
            this.resultLongitudeUTM15.Text = "15 °";
            this.resultLongitudeUTM15.UseVisualStyleBackColor = true;
            // 
            // resFormatBLH
            // 
            this.resFormatBLH.Controls.Add(this.resDegMinSecBLH);
            this.resFormatBLH.Controls.Add(this.resDegreesBLH);
            this.resFormatBLH.Location = new System.Drawing.Point(259, 281);
            this.resFormatBLH.Name = "resFormatBLH";
            this.resFormatBLH.Size = new System.Drawing.Size(250, 33);
            this.resFormatBLH.TabIndex = 13;
            this.resFormatBLH.TabStop = false;
            this.resFormatBLH.Text = "Format danych wyjściowych [BLH]";
            // 
            // resDegMinSecBLH
            // 
            this.resDegMinSecBLH.AutoSize = true;
            this.resDegMinSecBLH.ForeColor = System.Drawing.Color.Maroon;
            this.resDegMinSecBLH.Location = new System.Drawing.Point(81, 13);
            this.resDegMinSecBLH.Name = "resDegMinSecBLH";
            this.resDegMinSecBLH.Size = new System.Drawing.Size(167, 17);
            this.resDegMinSecBLH.TabIndex = 5;
            this.resDegMinSecBLH.Text = "stopnie, minuty, sekundy [° \' \"]";
            this.resDegMinSecBLH.UseVisualStyleBackColor = true;
            this.resDegMinSecBLH.CheckedChanged += new System.EventHandler(this.ResDegMinSecBLH_CheckedChanged);
            // 
            // resDegreesBLH
            // 
            this.resDegreesBLH.AutoSize = true;
            this.resDegreesBLH.Checked = true;
            this.resDegreesBLH.ForeColor = System.Drawing.Color.Maroon;
            this.resDegreesBLH.Location = new System.Drawing.Point(7, 13);
            this.resDegreesBLH.Name = "resDegreesBLH";
            this.resDegreesBLH.Size = new System.Drawing.Size(72, 17);
            this.resDegreesBLH.TabIndex = 4;
            this.resDegreesBLH.TabStop = true;
            this.resDegreesBLH.Text = "stopnie [°]";
            this.resDegreesBLH.UseVisualStyleBackColor = true;
            this.resDegreesBLH.CheckedChanged += new System.EventHandler(this.ResDegreesBLH_CheckedChanged);
            // 
            // CountUp
            // 
            this.CountUp.Location = new System.Drawing.Point(629, 3);
            this.CountUp.Name = "CountUp";
            this.CountUp.Size = new System.Drawing.Size(110, 23);
            this.CountUp.TabIndex = 4;
            this.CountUp.Text = "Wykonaj obliczenia";
            this.CountUp.UseVisualStyleBackColor = true;
            this.CountUp.Click += new System.EventHandler(this.CountUp_Click);
            // 
            // loadFile
            // 
            this.loadFile.Enabled = false;
            this.loadFile.Location = new System.Drawing.Point(3, 333);
            this.loadFile.Name = "loadFile";
            this.loadFile.Size = new System.Drawing.Size(110, 23);
            this.loadFile.TabIndex = 5;
            this.loadFile.Text = "Wczytaj plik";
            this.loadFile.UseVisualStyleBackColor = true;
            this.loadFile.Click += new System.EventHandler(this.LoadFile_Click_1);
            // 
            // filePath
            // 
            this.filePath.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.filePath.Location = new System.Drawing.Point(4, 362);
            this.filePath.Name = "filePath";
            this.filePath.ReadOnly = true;
            this.filePath.Size = new System.Drawing.Size(505, 20);
            this.filePath.TabIndex = 6;
            // 
            // OFD
            // 
            this.OFD.Filter = "Text files (*.txt)|*.txt";
            this.OFD.InitialDirectory = "c:\\\\";
            this.OFD.RestoreDirectory = true;
            this.OFD.ShowReadOnly = true;
            this.OFD.Title = "Wybierz plik tekstowy:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ETRF89);
            this.groupBox1.Controls.Add(this.ETRF2000);
            this.groupBox1.Location = new System.Drawing.Point(4, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 100);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "UKŁAD WEJŚCIOWY";
            // 
            // ETRF89
            // 
            this.ETRF89.AutoSize = true;
            this.ETRF89.Location = new System.Drawing.Point(10, 55);
            this.ETRF89.Name = "ETRF89";
            this.ETRF89.Size = new System.Drawing.Size(214, 17);
            this.ETRF89.TabIndex = 3;
            this.ETRF89.Text = "PL-ETRF89 (POLREF) elipsoida GRS80";
            this.ETRF89.UseVisualStyleBackColor = true;
            this.ETRF89.CheckedChanged += new System.EventHandler(this.ETRF89_CheckedChanged);
            // 
            // ETRF2000
            // 
            this.ETRF2000.AutoSize = true;
            this.ETRF2000.Location = new System.Drawing.Point(10, 31);
            this.ETRF2000.Name = "ETRF2000";
            this.ETRF2000.Size = new System.Drawing.Size(219, 17);
            this.ETRF2000.TabIndex = 2;
            this.ETRF2000.Text = "PL-ETRF2000/ep.2011 elipsoida GRS80";
            this.ETRF2000.UseVisualStyleBackColor = true;
            this.ETRF2000.CheckedChanged += new System.EventHandler(this.ETRF2000_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.resETRF89);
            this.groupBox2.Controls.Add(this.resETRF2000);
            this.groupBox2.Location = new System.Drawing.Point(259, 31);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 100);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "UKŁAD WYJŚCIOWY";
            // 
            // resETRF89
            // 
            this.resETRF89.AutoSize = true;
            this.resETRF89.Location = new System.Drawing.Point(7, 55);
            this.resETRF89.Name = "resETRF89";
            this.resETRF89.Size = new System.Drawing.Size(214, 17);
            this.resETRF89.TabIndex = 5;
            this.resETRF89.Text = "PL-ETRF89 (POLREF) elipsoida GRS80";
            this.resETRF89.UseVisualStyleBackColor = true;
            this.resETRF89.CheckedChanged += new System.EventHandler(this.ResETRF89_CheckedChanged);
            // 
            // resETRF2000
            // 
            this.resETRF2000.AutoSize = true;
            this.resETRF2000.Location = new System.Drawing.Point(7, 31);
            this.resETRF2000.Name = "resETRF2000";
            this.resETRF2000.Size = new System.Drawing.Size(219, 17);
            this.resETRF2000.TabIndex = 4;
            this.resETRF2000.Text = "PL-ETRF2000/ep.2011 elipsoida GRS80";
            this.resETRF2000.UseVisualStyleBackColor = true;
            this.resETRF2000.CheckedChanged += new System.EventHandler(this.ResETRF2000_CheckedChanged);
            // 
            // TextBoxTrans2D3D
            // 
            this.TextBoxTrans2D3D.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.TextBoxTrans2D3D.Enabled = false;
            this.TextBoxTrans2D3D.ForeColor = System.Drawing.Color.Maroon;
            this.TextBoxTrans2D3D.Location = new System.Drawing.Point(4, 5);
            this.TextBoxTrans2D3D.Name = "TextBoxTrans2D3D";
            this.TextBoxTrans2D3D.ReadOnly = true;
            this.TextBoxTrans2D3D.Size = new System.Drawing.Size(505, 20);
            this.TextBoxTrans2D3D.TabIndex = 11;
            this.TextBoxTrans2D3D.Text = "TRANSFORMACJE PŁASKIE I TRÓJWYMIAROWE";
            this.TextBoxTrans2D3D.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(753, 414);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.DarkGray;
            this.tabPage1.Controls.Add(this.PrecisionGB);
            this.tabPage1.Controls.Add(this.TransOption);
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Controls.Add(this.resFormatBLH);
            this.tabPage1.Controls.Add(this.FormatBLH);
            this.tabPage1.Controls.Add(this.TextBoxTrans2D3D);
            this.tabPage1.Controls.Add(this.CountUp);
            this.tabPage1.Controls.Add(this.LongitudeUTM);
            this.tabPage1.Controls.Add(this.resultLongitudeUTM);
            this.tabPage1.Controls.Add(this.ChoiceOne);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.ChoiceTwo);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.LongitudeChoice);
            this.tabPage1.Controls.Add(this.resultLongitudeChoice);
            this.tabPage1.Controls.Add(this.filePath);
            this.tabPage1.Controls.Add(this.loadFile);
            this.tabPage1.ForeColor = System.Drawing.Color.Maroon;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(745, 388);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "TRANS 2D/3D";
            this.tabPage1.Click += new System.EventHandler(this.TabPage1_Click);
            // 
            // PrecisionGB
            // 
            this.PrecisionGB.Controls.Add(this.LengthPrecisionDUD);
            this.PrecisionGB.Controls.Add(this.AnglePrecisionDUD);
            this.PrecisionGB.Controls.Add(this.LengthInfo);
            this.PrecisionGB.Controls.Add(this.AngleInfo);
            this.PrecisionGB.Location = new System.Drawing.Point(515, 323);
            this.PrecisionGB.Name = "PrecisionGB";
            this.PrecisionGB.Size = new System.Drawing.Size(224, 59);
            this.PrecisionGB.TabIndex = 18;
            this.PrecisionGB.TabStop = false;
            // 
            // LengthPrecisionDUD
            // 
            this.LengthPrecisionDUD.Location = new System.Drawing.Point(157, 37);
            this.LengthPrecisionDUD.Name = "LengthPrecisionDUD";
            this.LengthPrecisionDUD.Size = new System.Drawing.Size(60, 20);
            this.LengthPrecisionDUD.TabIndex = 22;
            this.LengthPrecisionDUD.Text = "0.0001";
            // 
            // AnglePrecisionDUD
            // 
            this.AnglePrecisionDUD.Location = new System.Drawing.Point(157, 10);
            this.AnglePrecisionDUD.Name = "AnglePrecisionDUD";
            this.AnglePrecisionDUD.Size = new System.Drawing.Size(61, 20);
            this.AnglePrecisionDUD.TabIndex = 21;
            this.AnglePrecisionDUD.Text = "0.0001";
            // 
            // LengthInfo
            // 
            this.LengthInfo.AutoSize = true;
            this.LengthInfo.Location = new System.Drawing.Point(6, 39);
            this.LengthInfo.Name = "LengthInfo";
            this.LengthInfo.Size = new System.Drawing.Size(146, 13);
            this.LengthInfo.TabIndex = 20;
            this.LengthInfo.Text = "Dokładność zapisu XY(Z) [m]";
            // 
            // AngleInfo
            // 
            this.AngleInfo.AutoSize = true;
            this.AngleInfo.Location = new System.Drawing.Point(6, 13);
            this.AngleInfo.Name = "AngleInfo";
            this.AngleInfo.Size = new System.Drawing.Size(145, 13);
            this.AngleInfo.TabIndex = 17;
            this.AngleInfo.Text = "Dokładność zapisu kątów [\"]";
            // 
            // TransOption
            // 
            this.TransOption.Controls.Add(this.radioButton1);
            this.TransOption.Controls.Add(this.radioButton2);
            this.TransOption.Location = new System.Drawing.Point(259, 323);
            this.TransOption.Name = "TransOption";
            this.TransOption.Size = new System.Drawing.Size(250, 33);
            this.TransOption.TabIndex = 15;
            this.TransOption.TabStop = false;
            this.TransOption.Text = "Opcja Transformacji";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.ForeColor = System.Drawing.Color.Maroon;
            this.radioButton1.Location = new System.Drawing.Point(145, 13);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(79, 17);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.Text = "Empiryczna";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.ForeColor = System.Drawing.Color.Maroon;
            this.radioButton2.Location = new System.Drawing.Point(7, 13);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(84, 17);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Teoretyczna";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Aquamarine;
            this.richTextBox1.Location = new System.Drawing.Point(515, 31);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(224, 283);
            this.richTextBox1.TabIndex = 14;
            this.richTextBox1.Text = "MONITOR:";
            // 
            // FormatBLH
            // 
            this.FormatBLH.Controls.Add(this.degMinSecBLH);
            this.FormatBLH.Controls.Add(this.degreesBLH);
            this.FormatBLH.Location = new System.Drawing.Point(4, 281);
            this.FormatBLH.Name = "FormatBLH";
            this.FormatBLH.Size = new System.Drawing.Size(250, 33);
            this.FormatBLH.TabIndex = 12;
            this.FormatBLH.TabStop = false;
            this.FormatBLH.Text = "Format danych wejściowych [BLH]";
            // 
            // degMinSecBLH
            // 
            this.degMinSecBLH.AutoSize = true;
            this.degMinSecBLH.ForeColor = System.Drawing.Color.Maroon;
            this.degMinSecBLH.Location = new System.Drawing.Point(82, 13);
            this.degMinSecBLH.Name = "degMinSecBLH";
            this.degMinSecBLH.Size = new System.Drawing.Size(167, 17);
            this.degMinSecBLH.TabIndex = 5;
            this.degMinSecBLH.Text = "stopnie, minuty, sekundy [° \' \"]";
            this.degMinSecBLH.UseVisualStyleBackColor = true;
            this.degMinSecBLH.CheckedChanged += new System.EventHandler(this.DegMinSecBLH_CheckedChanged);
            // 
            // degreesBLH
            // 
            this.degreesBLH.AutoSize = true;
            this.degreesBLH.Checked = true;
            this.degreesBLH.ForeColor = System.Drawing.Color.Maroon;
            this.degreesBLH.Location = new System.Drawing.Point(7, 13);
            this.degreesBLH.Name = "degreesBLH";
            this.degreesBLH.Size = new System.Drawing.Size(72, 17);
            this.degreesBLH.TabIndex = 4;
            this.degreesBLH.TabStop = true;
            this.degreesBLH.Text = "stopnie [°]";
            this.degreesBLH.UseVisualStyleBackColor = true;
            this.degreesBLH.CheckedChanged += new System.EventHandler(this.DegreesBLH_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.DarkGray;
            this.tabPage2.ForeColor = System.Drawing.Color.Maroon;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(745, 388);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "TRANS-H";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.DarkGray;
            this.tabPage3.ForeColor = System.Drawing.Color.Maroon;
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(745, 388);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "PL-geoid-2011";
            // 
            // Transform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(777, 438);
            this.Controls.Add(this.tabControl1);
            this.Name = "Transform";
            this.Text = "TransPol w.2.06";
            this.ChoiceOne.ResumeLayout(false);
            this.ChoiceOne.PerformLayout();
            this.ChoiceTwo.ResumeLayout(false);
            this.ChoiceTwo.PerformLayout();
            this.LongitudeChoice.ResumeLayout(false);
            this.LongitudeChoice.PerformLayout();
            this.LongitudeUTM.ResumeLayout(false);
            this.LongitudeUTM.PerformLayout();
            this.resultLongitudeChoice.ResumeLayout(false);
            this.resultLongitudeChoice.PerformLayout();
            this.resultLongitudeUTM.ResumeLayout(false);
            this.resultLongitudeUTM.PerformLayout();
            this.resFormatBLH.ResumeLayout(false);
            this.resFormatBLH.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.PrecisionGB.ResumeLayout(false);
            this.PrecisionGB.PerformLayout();
            this.TransOption.ResumeLayout(false);
            this.TransOption.PerformLayout();
            this.FormatBLH.ResumeLayout(false);
            this.FormatBLH.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ChoiceOne;
        private System.Windows.Forms.RadioButton XY1992;
        private System.Windows.Forms.RadioButton XY2000;
        private System.Windows.Forms.GroupBox ChoiceTwo;
        private System.Windows.Forms.RadioButton resultXY1992;
        private System.Windows.Forms.RadioButton resultXY2000;
        private System.Windows.Forms.GroupBox LongitudeChoice;
        private System.Windows.Forms.RadioButton longitude24;
        private System.Windows.Forms.RadioButton longitude21;
        private System.Windows.Forms.RadioButton longitude18;
        private System.Windows.Forms.RadioButton longitude15;
        private System.Windows.Forms.GroupBox resultLongitudeChoice;
        private System.Windows.Forms.RadioButton resultLongitude24;
        private System.Windows.Forms.RadioButton resultLongitude21;
        private System.Windows.Forms.RadioButton resultLongitude18;
        private System.Windows.Forms.RadioButton resultLongitude15;
        private System.Windows.Forms.Button CountUp;
        private System.Windows.Forms.Button loadFile;
        private System.Windows.Forms.TextBox filePath;
        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.RadioButton XYZ_GRS;
        private System.Windows.Forms.RadioButton BLH_GRS;
        private System.Windows.Forms.RadioButton UTM;
        private System.Windows.Forms.RadioButton resultXYZ_GRS;
        private System.Windows.Forms.RadioButton resultBLH_GRS;
        private System.Windows.Forms.RadioButton resultUTM;
        private System.Windows.Forms.GroupBox LongitudeUTM;
        private System.Windows.Forms.RadioButton longitudeUTM21;
        private System.Windows.Forms.RadioButton longitudeUTM15;
        private System.Windows.Forms.GroupBox resultLongitudeUTM;
        private System.Windows.Forms.RadioButton resultLongitudeUTM21;
        private System.Windows.Forms.RadioButton resultLongitudeUTM15;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton ETRF89;
        private System.Windows.Forms.RadioButton ETRF2000;
        private System.Windows.Forms.RadioButton resETRF89;
        private System.Windows.Forms.RadioButton resETRF2000;
        private System.Windows.Forms.TextBox TextBoxTrans2D3D;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox resFormatBLH;
        private System.Windows.Forms.RadioButton resDegMinSecBLH;
        private System.Windows.Forms.RadioButton resDegreesBLH;
        private System.Windows.Forms.GroupBox FormatBLH;
        private System.Windows.Forms.RadioButton degMinSecBLH;
        private System.Windows.Forms.RadioButton degreesBLH;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox TransOption;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label AngleInfo;
        private System.Windows.Forms.GroupBox PrecisionGB;
        private System.Windows.Forms.Label LengthInfo;
        private System.Windows.Forms.DomainUpDown LengthPrecisionDUD;
        private System.Windows.Forms.DomainUpDown AnglePrecisionDUD;
    }
}

