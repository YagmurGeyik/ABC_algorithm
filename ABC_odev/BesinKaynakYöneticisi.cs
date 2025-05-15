using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_odev
{
    internal class BesinKaynakYöneticisi
    {
        public List<BesinKaynak> Kaynaklar { get; private set; }

        public BesinKaynakYöneticisi(int kaynakSayisi, int boyut, double altSinir, double ustSinir, Func<double[], double> fonksiyon)
        {
            Kaynaklar = new List<BesinKaynak>();
            var rand = new Random();

            for (int i = 0; i < kaynakSayisi; i++)
            {
                var kaynak = new BesinKaynak(boyut);
                for (int j = 0; j < boyut; j++)
                    kaynak.X[j] = rand.NextDouble() * (ustSinir - altSinir) + altSinir;

                kaynak.DegerleriHesapla(fonksiyon);
                Kaynaklar.Add(kaynak);
            }
        }

        public BesinKaynak EnIyiKaynak()
        {
            return Kaynaklar.OrderByDescending(k => k.Fitness).First();
        }
    }

}

