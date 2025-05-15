using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_odev
{
    internal class BesinKaynak
    {
            public double[] X { get; set; }
            public double Fx { get; set; }
            public double Fitness { get; set; }
            public int DenemeSayisi { get; set; }

            public BesinKaynak(int boyut)
            {
                X = new double[boyut];
            }

            public void DegerleriHesapla(Func<double[], double> fonksiyon)
            {
                Fx = fonksiyon(X);
                Fitness = Fx >= 0 ? 1 / (1 + Fx) : 1 + Math.Abs(Fx);
            }
        

    }
}
