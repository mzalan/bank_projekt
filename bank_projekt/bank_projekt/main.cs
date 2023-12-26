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
        Console.Clear();
        Console.WriteLine("Kérjük válasszon az alábbi menüpontok közül:");
        Console.WriteLine("1. Adatok kilistázása és módosítása");
        Console.WriteLine("2. Készpénz felvétel");
        Console.WriteLine("3. Készpénz befizetés");
        Console.WriteLine("4. Utalás\n");
        Console.Write("Menüpont: ");
        Console.ReadLine();
    }
}

if (valasztas == "2")
{

}




