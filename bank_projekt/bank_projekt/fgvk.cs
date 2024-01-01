using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ugyfelek
{
    public static class Fgvk
    {
        private static List<Ugyfel> ugyfelek = new List<Ugyfel>();
        public static Ugyfel bejelentkezettUgyfel;
        private static int[] cimletek = { 20000, 10000, 5000, 2000, 1000 };
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
        public static List<Ugyfel> GetUgyfelek()
        {
            return ugyfelek;
        }
        public static int ValasztoMenu(List<string> opciok, string cim)
        {
            ShowOpciok(opciok, cim);
            Console.Write("Bevitel: ");
            return int.Parse(Console.ReadLine());
        }
        public static void ShowOpciok(List<string> opciok, string cim)
        {
            Console.Clear();
            Console.WriteLine(cim);

            for (int i = 0; i < opciok.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {opciok[i]}");
            }
        }
        public static string CheckOsszeg(int beker)
        {
            if (!(beker % 1000 == 0))   return "Kérjük ezerrel osztható összeget adjon meg.";
            else if (beker > 150000)   return "Kérjük ne adjon meg 150 ezer forintnál nagyobb összeget.";
            return "";
        }
        public static string KeszpenzCalc(int kertMenny)
        {
            bejelentkezettUgyfel.Egyenleg -= kertMenny;
            FajlIr();
            
            Dictionary<int, int> cimletekDb = new Dictionary<int, int>();
            int maradek = kertMenny;
            do
            {
                maradek = HelyesCimlet(maradek, cimletekDb);
            } while (maradek > 0);
            return Kiiratas(cimletekDb);
        }
        public static int HelyesCimlet(int maradek, Dictionary<int, int> cimletekDb)
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
        public static Dictionary<int, int> CimletDbDict()
        {
            Dictionary<int, int> cimletekDb = new Dictionary<int, int>();
            foreach (var cimlet in cimletek)
            {
                cimletekDb.Add(cimlet, 0);
            }
            return cimletekDb;
        }
        public static int[] GetCimletek()
        {
            return cimletek;
        }
        public static List<string> CreateMenuLista(Dictionary<int, int> cimletekDb)
        {
            List<string> opcMenu = new List<string>();
            foreach (KeyValuePair<int, int> cimlet in cimletekDb){
               opcMenu.Add($"{EzresTagolas(cimlet.Key.ToString())} Ft x {cimlet.Value.ToString()}");
            }
            opcMenu.Add("Kész");
            return opcMenu;
        }
        public static string FrissitEgyenleg(int egyenleg, Ugyfel celUgyfel)
        {
            Ugyfel celSzamla;
            if (celUgyfel == null)
                celSzamla = bejelentkezettUgyfel;
            else
                celSzamla = celUgyfel;
            celSzamla.Egyenleg += egyenleg;

            FajlIr();
            return $"Egyenleg: {EzresTagolas(celSzamla.Egyenleg.ToString())} Ft";
        }
        public static void AdatKilistazas(string valasztas)
        {
            if (valasztas == "1")
                NevModosit();
            if (valasztas == "6")
                PINModosit();
            if (valasztas == "7")
                KartyatipusModosit();
        }
        public static void NevModosit()
        {
            string ujNev = "a";
            string ujNevIsm = "a";
            do
            {
                Console.Clear();
                if (ujNev != ujNevIsm)
                    Console.WriteLine("A megadott nevek nem egyeznek");
                if (!NevCheck(ujNev))
                    Console.WriteLine("A névnek tartalmaznia kell min. 2 nagybetűt és min. 1 szóközt");
                Console.Write("Adja meg a kívánt nevet: ");
                ujNev = Console.ReadLine();
                Console.Write("Adja meg a nevet ismét: ");
                ujNevIsm = Console.ReadLine();
            } while (ujNev != ujNevIsm || !NevCheck(ujNev));

            bejelentkezettUgyfel.Nev = ujNev;
            FajlIr();

            Console.WriteLine("A név módosítása sikeres");
        }
        public static void PINModosit()
        {
            string ujPIN = "1";
            string ujPINIsm = "1";
            bool elso = true;
            do
            {
                Console.Clear();
                if (!elso)
                {
                    if (ujPIN != ujPINIsm)
                        Console.WriteLine("A megadott PIN-kódok nem egyeznek");
                    if (ujPIN.Length != 4 || !LetezoPINCheck(ujPIN))
                        Console.WriteLine("A megadott PIN-kód nem megfelelő");
                }
                Console.Write("Adja meg a kívánt PIN-kódot: ");
                ujPIN = Console.ReadLine();
                Console.Write("Adja meg a PIN-kódot ismét: ");
                ujPINIsm = Console.ReadLine();
                elso = false;
            } while (ujPIN != ujPINIsm || ujPIN.Length != 4 || !LetezoPINCheck(ujPIN));

            bejelentkezettUgyfel.PIN = ujPIN;
            FajlIr();

            Console.WriteLine("A PIN-kód módosítása sikeres");
        }
        public static void KartyatipusModosit()
        {
            string ujTipus = "a";
            string ujTipusIsm = "a";
            bool elso = true;
            do
            {
                Console.Clear();
                if (!elso)
                {
                    if (ujTipus != ujTipusIsm)
                        Console.WriteLine("A megadott bankok nem egyeznek");
                    if (!LetezoBankCheck(ujTipus))
                        Console.WriteLine("A megadott bank nem létezik");
                }
                Console.Write("Adja meg a kívánt bankot: ");
                ujTipus = Console.ReadLine();
                Console.Write("Adja meg a bankot ismét: ");
                ujTipusIsm = Console.ReadLine();
                elso = false;
            } while (ujTipus != ujTipusIsm || !LetezoBankCheck(ujTipus));

            bejelentkezettUgyfel.Kartyatipus = ujTipus;
            FajlIr();

            Console.WriteLine("A kártyatípus módosítása sikeres");
        }
        public static bool LetezoPINCheck(string megadottPIN)
        {
            foreach (var ugyfel in ugyfelek)
            {
                if (ugyfel.PIN == megadottPIN)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool LetezoBankCheck(string megadottBank)
        {
            string[] bankok = { "Erste", "Raiffeisen", "Takarékbank", "MKB", "Cofidis", "OTP", "CiBBank" };
            foreach (var bank in bankok)
            {
                if (bank == megadottBank)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool NevCheck(string megadottNev)
        {
            int db = 0;
            if (megadottNev.Contains(' '))
            {
                string[] reszek = megadottNev.Split(' ');
                foreach (string resz in reszek)
                {
                    if (Char.IsUpper(resz, 0))
                        db++;
                }
                if (db >= 2)
                    return true;
            }
            return false;
        }

    }
}

    