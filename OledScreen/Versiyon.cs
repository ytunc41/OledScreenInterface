using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OledScreen
{
    public static class Versiyon
    {
        private static string _versiyon;
        public static string getVS { get { return _versiyon; } private set { _versiyon = value; } }

        static Versiyon()
        {
            getVS = "v1.0.0";
        }

        /* Versiyon: 1.0.0
         * Tarih: 14.09.2020
         * 
         * - Helper nesnesi eklenmistir.
         * - Helper nesnesi icerisine resim boyut degistirme ve seriPort algilama gibi yardimci olacak yardimci metotlar eklenmistir.
         * - Resimler yuksek kalitede resize edilebilmektedir.
         * - ResizeImage sinifi eklenerek yeniden boyutlandirilan resmin bilgileri bu sinifta saklanmistir.
         * - CommPro ve altinda bulunan diger paket gonderme nesneleri eklenerek haberlesme siniflari olusturulmustur.
         * - MainForm'a paket gonderme ve paket alma ile iliskili metotlar eklenmistir.
         * - Baglanti yapilirken belirli bir timeout suresi belirlenmistir ve bu sure icerisinde baglanti paketi gelmez ise baglanti sonlandilirmaktadir.
         */
    }
}
