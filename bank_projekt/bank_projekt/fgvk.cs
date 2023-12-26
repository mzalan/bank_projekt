using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ugyfelek
{
    public static class Fgvk
    {
        private static List<Ugyfel> ugyfelek = new List<Ugyfel>();
        public static void FajlOlvas()
        {
            StreamReader sr = new StreamReader("adatok.txt");
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string sor = sr.ReadLine();
                ugyfelek.Add(new Ugyfel(sor));
            }
            sr.Close();
        }
        public static bool CheckPin(string beirtPin, int probalkozasok)
        {
            if (beirtPin.Length == 4)
            {
                foreach (var ugyfel in ugyfelek)
                {
                    if (ugyfel.PIN == beirtPin)
                        return true;
                }
            }
            return false;
        }
    }
}