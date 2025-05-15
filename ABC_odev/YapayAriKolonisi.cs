using System;
using System.Collections.Generic;
using System.Linq;

namespace ABC_odev
{
    internal class YapayAriKolonisi
    {
        private int koloniBoyutu, boyut, maxIterasyon, limit, denemeLimiti;
        private double altSinir, ustSinir;
        private Func<double[], double> fonksiyon;
        private BesinKaynakYöneticisi kaynakYonetici;
        private Random rnd = new Random();

        public List<double> YakinsamaListesi { get; private set; }
        public List<double[]> KararDegerleri { get; private set; }

        public YapayAriKolonisi(int koloniBoyutu, int maxIterasyon, double altSinir, double ustSinir, Func<double[], double> fonksiyon, int boyut = 2, int denemeLimiti = -1)
        {
            this.koloniBoyutu = koloniBoyutu;
            this.maxIterasyon = maxIterasyon;
            this.altSinir = altSinir;
            this.ustSinir = ustSinir;
            this.fonksiyon = fonksiyon;
            this.boyut = boyut;

            // Eğer dışarıdan deneme limiti verilmişse kullan, yoksa otomatik hesapla
            this.denemeLimiti = denemeLimiti > 0 ? denemeLimiti : (koloniBoyutu / 2) * boyut / 2;

            kaynakYonetici = new BesinKaynakYöneticisi(koloniBoyutu / 2, boyut, altSinir, ustSinir, fonksiyon);
            YakinsamaListesi = new List<double>();
            KararDegerleri = new List<double[]>();
        }

        public void Calistir()
        {
            for (int iter = 0; iter < maxIterasyon; iter++)
            {
                IsciAriFazi();
                GozcuAriFazi();
                KasifAriFazi();

                // En iyi çözümün f(x) ve X değerlerini kaydet
                var enIyi = kaynakYonetici.EnIyiKaynak();
                YakinsamaListesi.Add(enIyi.Fx);
                KararDegerleri.Add((double[])enIyi.X.Clone());
            }
        }

        private void IsciAriFazi()
        {
            foreach (var kaynak in kaynakYonetici.Kaynaklar)
            {
                double[] yeniX = (double[])kaynak.X.Clone();
                int j = rnd.Next(boyut);
                int k;
                do { k = rnd.Next(kaynakYonetici.Kaynaklar.Count); } while (kaynak == kaynakYonetici.Kaynaklar[k]);

                double phi = rnd.NextDouble() * 2 - 1; // -1 ile 1 arası
                yeniX[j] = kaynak.X[j] + phi * (kaynak.X[j] - kaynakYonetici.Kaynaklar[k].X[j]);
                yeniX[j] = Math.Max(altSinir, Math.Min(ustSinir, yeniX[j]));

                double fx = fonksiyon(yeniX);
                double fitness = fx >= 0 ? 1 / (1 + fx) : 1 + Math.Abs(fx);

                if (fitness > kaynak.Fitness)
                {
                    kaynak.X = yeniX;
                    kaynak.DegerleriHesapla(fonksiyon);
                    kaynak.DenemeSayisi = 0;
                }
                else
                {
                    kaynak.DenemeSayisi++;
                }
            }
        }

        private void GozcuAriFazi()
        {
            double toplamFitness = kaynakYonetici.Kaynaklar.Sum(k => k.Fitness);
            foreach (var kaynak in kaynakYonetici.Kaynaklar)
            {
                double olasilik = kaynak.Fitness / toplamFitness;
                if (rnd.NextDouble() < olasilik)
                {
                    double[] yeniX = (double[])kaynak.X.Clone();
                    int j = rnd.Next(boyut);
                    int k;
                    do { k = rnd.Next(kaynakYonetici.Kaynaklar.Count); } while (kaynak == kaynakYonetici.Kaynaklar[k]);

                    double phi = rnd.NextDouble() * 2 - 1;
                    yeniX[j] = kaynak.X[j] + phi * (kaynak.X[j] - kaynakYonetici.Kaynaklar[k].X[j]);
                    yeniX[j] = Math.Max(altSinir, Math.Min(ustSinir, yeniX[j]));

                    double fx = fonksiyon(yeniX);
                    double fitness = fx >= 0 ? 1 / (1 + fx) : 1 + Math.Abs(fx);

                    if (fitness > kaynak.Fitness)
                    {
                        kaynak.X = yeniX;
                        kaynak.DegerleriHesapla(fonksiyon);
                        kaynak.DenemeSayisi = 0;
                    }
                    else
                    {
                        kaynak.DenemeSayisi++;
                    }
                }
            }
        }

        private void KasifAriFazi()
        {
            foreach (var kaynak in kaynakYonetici.Kaynaklar)
            {
                if (kaynak.DenemeSayisi > denemeLimiti)
                {
                    for (int i = 0; i < boyut; i++)
                        kaynak.X[i] = rnd.NextDouble() * (ustSinir - altSinir) + altSinir;

                    kaynak.DegerleriHesapla(fonksiyon);
                    kaynak.DenemeSayisi = 0;
                }
            }
        }

        public BesinKaynak EnIyiCozum()
        {
            return kaynakYonetici.EnIyiKaynak();
        }
    }
}
