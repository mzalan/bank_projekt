using Ugyfelek;

Fgvk.FajlOlvas();

string valasztas;
do
{
    Console.Clear();
    Console.WriteLine("Kérjük válasszon az alábbi opciók közül:\n");
    Console.WriteLine("1. Belépés már létező fiókba");
    Console.WriteLine("2. Új fiók nyitása\n");
    Console.Write("Menüpont: ");
    valasztas = Console.ReadLine();
}while(valasztas.Length != 1 && (valasztas != "1" || valasztas != "2" ));


if (valasztas == "1")
{
    Console.Clear();
    Console.WriteLine("Üdvözöljük!\nKérjük adja meg a PIN-kódját.");

    string beirtPin;
    int probalkozasok = 3;
    bool helyes = false;
    do
    {
        Console.Write("PIN-kód: ");
        beirtPin = Console.ReadLine();
        if (!Fgvk.CheckPin(beirtPin, probalkozasok))
            probalkozasok--;
            Console.WriteLine($"Helytelen PIN-kód! {probalkozasok} lehetősége maradt.");
    } while (!Fgvk.CheckPin(beirtPin, probalkozasok) && probalkozasok != 0);

    if (probalkozasok > 0)
    {
        Fomenu();
        
    }
    void Fomenu()
    {
        string valasztas2;
        do
        {

            Console.Clear();
            Console.WriteLine("Kérjük válasszon az alábbi menüpontok közül:");
            Console.WriteLine("1. Adatok kilistázása és módosítása");
            Console.WriteLine("2. Készpénz felvétel");
            Console.WriteLine("3. Készpénz befizetés");
            Console.WriteLine("4. Utalás");
            Console.WriteLine("5. Kijelentkezés\n");
            Console.Write("Menüpont: ");
            valasztas2 = Console.ReadLine();

            if (valasztas2 == "1")
            {

            }

            else if (valasztas2 == "2")
            {
                Penzfelvetel();
                valasztas2 = BackPrompt();
            }

            else if (valasztas2 == "3")
            {
                Penzbefizetes();
                valasztas2 = BackPrompt();
            }

            else if (valasztas2 == "4")
            {
                Utalas();
                valasztas2 = BackPrompt();
            }

            else if (valasztas2 == "5")
            {
                
            }
        }
        while (valasztas2 != "");
    }
}
    string BackPrompt()
    {
        Console.WriteLine("\nHagyja üresen a beviteli mezőt a kilépéshez..");
        return Console.ReadLine();
}
    void Penzfelvetel()
    {
        string kiiras = "";
        List<string> osszegMenu = new List<string> {
                    "5 000",
                    "10 000",
                    "19 000",
                    "25 000",
                    "50 000",
                    "100 000",
                    "150 000",
                    "Más összeg"};
        int beker;
        do
        {
            beker = Fgvk.ValasztoMenu(osszegMenu, "Kérjük válassza ki felvenni kívánt összeget (Ft): ");
            if (beker >= 1 && beker <= 7)
            {
                kiiras = "\n" + Fgvk.KeszpenzCalc(int.Parse(osszegMenu[beker - 1].Replace(" ", "")));
            }
            else if (beker == 8)
            {
                Console.Clear();
                Console.WriteLine("Az automata a következő bankjegyeket bocsátja ki: 1 000 Ft, 2 000 Ft, 5 000 Ft, 10 000 Ft, 20 000 Ft");
                Console.WriteLine("Maximum felvehető összeg: 150 000 Ft");
                string hibauzenet;
                int bekerOssz;
                do
                {
                    Console.Write("Adja meg az összeget: ");
                    bekerOssz = int.Parse(Console.ReadLine());
                    hibauzenet = Fgvk.CheckOsszeg(bekerOssz);
                    Console.WriteLine(hibauzenet);
                } while (hibauzenet != "");
                kiiras = Fgvk.KeszpenzCalc(bekerOssz);
            }
            Console.WriteLine(kiiras);
        } while (beker > 8 || beker < 1);
    }
    void Penzbefizetes()
        {
            BankjegyBeadas("", null);
        }
    
     Ugyfel KartyaAdatok()
    {
        Console.Clear();
        Console.WriteLine("Kérjük adja meg a szükséges adatokat.");
        while (true)
        {
            Console.Write("Kártyaszám: ");
            string kartyaszam = Console.ReadLine();
            Console.Write("Lejárati dátum (év-hó-nap): ");
            DateTime datum = DateTime.Parse(Console.ReadLine());
            Console.Write("Biztonsági kód (CVV): ");
            string cvv = Console.ReadLine();
            foreach (var ugyfel in Fgvk.GetUgyfelek())
            {
                if (ugyfel.Kartyaszam == kartyaszam && ugyfel.LejaratiDatum == datum && ugyfel.CVV == cvv)
                {
                    return ugyfel;
                }
            }
            Console.Clear();
            Console.WriteLine("Nem egyeznek az adatok. Próbálja újra.");
        }
    }
void BankjegyBeadas(string message, Ugyfel celUgyfel)
{
    Dictionary<int, int> cimletekDb = Fgvk.CimletDbDict();
    int opcBeker;
    int osszesen = 0;
    int bankjegyCount = 0;
    do
    {
        opcBeker = Fgvk.ValasztoMenu(Fgvk.CreateMenuLista(cimletekDb), $"{message}Helyezze be a bankjegyeket (max. 200 db)");
        if (opcBeker >= 1 && opcBeker <= 5)
        {
            int[] cimletek = Fgvk.GetCimletek();
            osszesen += cimletek[opcBeker - 1];
            bankjegyCount++;

            try
            {
                cimletekDb[cimletek[opcBeker - 1]] += 1;
            }
            catch (Exception KeyNotFoundException)
            {
                cimletekDb[cimletek[opcBeker - 1]] = 1;
            }
            if (bankjegyCount == 200)
                Fgvk.ShowOpciok(Fgvk.CreateMenuLista(cimletekDb), $"{message}Helyezze be a bankjegyeket (max. 200 db)");
        }
    } while (opcBeker != 6 && bankjegyCount != 200);
    Console.WriteLine($"\nFeltöltött pénzösszeg: {Fgvk.EzresTagolas(osszesen.ToString())} Ft");
    Console.WriteLine(Fgvk.FrissitEgyenleg(osszesen, celUgyfel));
}

void Utalas()
    {
    Ugyfel celUgyfel = KartyaAdatok();
    BankjegyBeadas("Adatok elfogadva!\n", celUgyfel);
}


if (valasztas == "2")
{
    Console.Clear();

    string ujnev;

    Console.Write("Üdvözöljük! Kérjük adja meg a nevét: ");
    ujnev = Console.ReadLine();
}