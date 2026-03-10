namespace Playground.Console;

public static class StringExercises
{
    public static void Run()
    {
        EsercizioIndexOfBase();
        EsercizioSubstringConTag();
        EsercizioSenzaValoriMagici();
    }

   static void EsercizioIndexOfBase()
{
    System.Console.WriteLine("=== IndexOf base ===");

    string message = "Find what is (inside the parentheses)";

    int openingPosition = message.IndexOf('(');
    int closingPosition = message.IndexOf(')');

    // Step 1: vedo le posizioni grezze
    System.Console.WriteLine($"Posizione '(': {openingPosition}"); // 13
    System.Console.WriteLine($"Posizione ')': {closingPosition}"); // 36

    // Step 2: estraggo il contenuto escludendo la parentesi
    openingPosition += 1;
    int length = closingPosition - openingPosition;
    System.Console.WriteLine(message.Substring(openingPosition, length));
}

    static void EsercizioSubstringConTag()
    {
        System.Console.WriteLine("=== Substring con tag HTML ===");

        string message = "What is the value <span>between the tags</span>?";

        int openingPosition = message.IndexOf("<span>");
        int closingPosition = message.IndexOf("</span>");

        openingPosition += 6; // magic number — vedremo perché è sbagliato
        int length = closingPosition - openingPosition;
        System.Console.WriteLine(message.Substring(openingPosition, length));
    }

    static void EsercizioSenzaValoriMagici()
    {
        System.Console.WriteLine("=== Senza magic values (versione corretta) ===");

        string message = "What is the value <span>between the tags</span>?";

        const string openSpan = "<span>";
        const string closeSpan = "</span>";

        int openingPosition = message.IndexOf(openSpan);
        int closingPosition = message.IndexOf(closeSpan);

        openingPosition += openSpan.Length; // ora se cambio il tag non si rompe nulla
        int length = closingPosition - openingPosition;
        System.Console.WriteLine(message.Substring(openingPosition, length));
    }
}