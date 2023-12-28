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
        private static Ugyfel bejelentkezettUgyfel;
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
                    {
                        bejelentkezettUgyfel = ugyfel;
                        return true;
                    }
                }
            }
            return false;
        }
        public static int ValasztoMenu(List<string> opciok, string cim)
        {
            Console.Clear();
            Console.WriteLine(cim);

            for (int i = 0; i < opciok.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {opciok[i]}");
            }
            Console.Write("Bevitel: ");
            return int.Parse(Console.ReadLine());
        }
        public static string CheckOsszeg(int beker)
        {
            if (!(beker % 1000 == 0))   return "Kérjük ezerrel osztható összeget adjon meg.";
            else if (beker > 150000)   return "Kérjük ne adjon meg 150 ezernél nagyobb összeget.";
            return "";
        }
        public static string KeszpenzCalc(int kertMenny)
        {
            bejelentkezettUgyfel.Egyenleg -= kertMenny;
            FajlIr();
            
            Dictionary<int, int> cimletekDb = new Dictionary<int, int>();
            int[] cimletek = { 20000, 10000, 5000, 2000, 1000 };
            int maradek = kertMenny;
            do
            {
                maradek = HelyesCimlet(maradek, cimletek, cimletekDb);
            } while (maradek > 0);
            return Kiiratas(cimletekDb);
        }
        public static int HelyesCimlet(int maradek, int[] cimletek, Dictionary<int, int> cimletekDb)
        {
            for (int i = 0; i < cimletek.Length; i++)
            {
                if (cimletek[i] <= maradek)
                {
                    maradek -= cimletek[i];
                    try
                    {
                        cimletekDb[cimletek[i]] += 1;
                    }
                    catch (Exception KeyNotFoundException)
                    {
                        cimletekDb[cimletek[i]] = 1;
                    }
                    return maradek;
                }
            }
            return 0;
        }
        public static string ReverseString(string str)
        {
            string output = "";
            for(int i = str.Length - 1; i >= 0; i--)
                output += str[i];
            return output;
        }
        public static void FajlIr()
        {
            string currentDir = Directory.GetCurrentDirectory().ToString();
            string levagando = "";
            for (int i = 0; i < 17; i++)
            {
                levagando += currentDir[currentDir.Length - i - 1];
            }
            string fajlUt = currentDir.Replace(ReverseString(levagando), "") + @"\adatok.txt";
            List<string> sorok = new List<string> { "nev;lejaratidatum;kartyaszam;cvv;egyenleg;pin;kartyatipus" };
            
            for (int i = 0; i < ugyfelek.Count; i++)
            {
                Ugyfel sor = ugyfelek[i];
                sorok.Add($"{sor.Nev};{sor.LejaratiDatum.Year}-{sor.LejaratiDatum.Month}-{sor.LejaratiDatum.Day};{sor.Kartyaszam};{sor.CVV};{sor.Egyenleg};{sor.PIN};{sor.Kartyatipus}");
            }
            File.WriteAllLines(fajlUt, sorok);
        }
        public static string Kiiratas(Dictionary<int, int> cimletekDb)
        {
            string kiiras = "Kérjük vegye el a készpénzt: \n";
            foreach (KeyValuePair<int, int> cimlet in cimletekDb)
                kiiras += $"{cimlet.Value} x {EzresTagolas(cimlet.Key.ToString())} Ft\n";

            return kiiras += $"\nEgyenlege: {EzresTagolas(bejelentkezettUgyfel.Egyenleg.ToString())} Ft";
        }
        public static string EzresTagolas(string tagolando)
        {
            string tagolt = "";
            for (int i = 0; i < tagolando.Length; i++)
            {
                tagolt += tagolando[i];
                if(i + 3 < tagolando.Length && (tagolando.Length - i - 1) % 3 == 0)
                    tagolt += " ";

            }
            return tagolt;
        }
    }
}