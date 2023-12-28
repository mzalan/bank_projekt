using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ugyfelek
{
    public class Ugyfel
    {
        public string Nev { get; set; }
        public DateTime LejaratiDatum { get; set; }
        public string Kartyaszam { get; set; }
        public string CVV { get; set; }
        public int Egyenleg { get; set; }
        public string PIN { get; set; }
        public string Kartyatipus { get; set; }

        public Ugyfel(string sor)
        {
            string[] adatok = sor.Split(';');
            Nev = adatok[0];
            LejaratiDatum = DateTime.Parse(adatok[1]);
            Kartyaszam = adatok[2];
            CVV = adatok[3];
            Egyenleg = int.Parse(adatok[4]);
            PIN = adatok[5];
            Kartyatipus = adatok[6];
        }
    }
}