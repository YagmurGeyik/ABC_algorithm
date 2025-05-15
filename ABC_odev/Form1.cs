using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ABC_odev
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Grafik başlatmaları
            chart1.Series.Clear();
            chart1.Series.Add("X1");
            chart1.Series.Add("X2");
            chart1.Series["X1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["X2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            chart2.Series.Clear();
            chart2.Series.Add("Fx");
            chart2.Series["Fx"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            // Çok satırlı textbox ayarı
            textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
        }
        private void GrafikSerileriHazirla()
        {
            chart1.Series.Clear();
            chart1.Series.Add("X1");
            chart1.Series.Add("X2");
            chart1.Series["X1"].ChartType = SeriesChartType.Line;
            chart1.Series["X2"].ChartType = SeriesChartType.Line;

            chart2.Series.Clear();
            chart2.Series.Add("Fx");
            chart2.Series["Fx"].ChartType = SeriesChartType.Line;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            GrafikSerileriHazirla();

            // Giriş parametreleri
            int koloniBoyutu = (int)numericUpDown1.Value;
            int denemeSayisi = (int)numericUpDown2.Value;
            int iterasyon = (int)numericUpDown3.Value;
            double altSinir = (double)numericUpDown4.Value;
            double ustSinir = (double)numericUpDown5.Value;
            int boyut = 2;

            Func<double[], double> fonksiyon = x => x[0] * x[0] + x[1] * x[1] ;
            var abc = new YapayAriKolonisi(koloniBoyutu, iterasyon, altSinir, ustSinir, fonksiyon, boyut, denemeSayisi);
            abc.Calistir();

            if (abc.KararDegerleri.Count == 0 || abc.YakinsamaListesi.Count == 0)
            {
                MessageBox.Show("Veri üretilemedi. Parametreleri kontrol edin.");
                return;
            }

            var enIyi = abc.EnIyiCozum();

            var sb = new StringBuilder();
            sb.AppendLine($"En iyi çözüm:");
            sb.AppendLine($"x1 = {enIyi.X[0]:F4}");
            sb.AppendLine($"x2 = {enIyi.X[1]:F4}");
            sb.AppendLine($"f(x) = {enIyi.Fx:F4}");
            textBox1.Text = sb.ToString();

            chart2.Series["Fx"].Points.Clear();
            for (int i = 0; i < abc.YakinsamaListesi.Count; i++)
                chart2.Series["Fx"].Points.AddXY(i + 1, abc.YakinsamaListesi[i]);

            chart1.Series["X1"].Points.Clear();
            chart1.Series["X2"].Points.Clear();
            for (int i = 0; i < abc.KararDegerleri.Count; i++)
            {
                var x = abc.KararDegerleri[i];
                chart1.Series["X1"].Points.AddXY(i + 1, x[0]);
                chart1.Series["X2"].Points.AddXY(i + 1, x[1]);
            }
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
