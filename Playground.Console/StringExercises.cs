namespace Playground.Console;

public static class StringExercises
{
    public static void Run()
    {
        EsercizioIndexOfBase();
        EsercizioSubstringConTag();
        EsercizioSenzaValoriMagici();
        EsercizioPerTrovarePosizioneCaratteri();
        EsercizioLastIndexOf();
        EsercizioUltimaCoppiaDiParentesi();
        EsercizioTutteLeParentesiConWhile();
        EsercizioIndexOfAny();
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

    static void EsercizioPerTrovarePosizioneCaratteri()
    {
        System.Console.WriteLine("=== Trovare la posizione di un carattere ===");

        string message = "C# Time";

        int positionC = message.IndexOf('C');
        int positionZ = message.IndexOf('Z');

        System.Console.WriteLine($"Posizione 'C': {positionC}"); // 0
        System.Console.WriteLine($"Posizione 'Z': {positionZ}"); // -1
    }

    static void EsercizioLastIndexOf()
{
    System.Console.WriteLine("=== LastIndexOf ===");

    string message = "hello there!";

    int first_h = message.IndexOf('h');
    int last_h = message.LastIndexOf('h');

    System.Console.WriteLine($"Prima 'h': posizione {first_h}");   // 0
    System.Console.WriteLine($"Ultima 'h': posizione {last_h}");   // 7
}

static void EsercizioUltimaCoppiaDiParentesi()
{
    System.Console.WriteLine("=== Ultima coppia di parentesi ===");

    string message = "(What if) I am (only interested) in the last (set of parentheses)?";
    int openingPosition = message.LastIndexOf('(');

    openingPosition += 1;
    int closingPosition = message.LastIndexOf(')');
    int length = closingPosition - openingPosition;
    System.Console.WriteLine(message.Substring(openingPosition, length));
    // output: set of parentheses
}

static void EsercizioTutteLeParentesiConWhile()
{
    System.Console.WriteLine("=== Tutte le parentesi con while ===");

    string message = "(What if) there are (more than) one (set of parentheses)?";

    while (true)
    {
        int openingPosition = message.IndexOf('(');
        if (openingPosition == -1) break; // nessuna parentesi rimasta → esci

        openingPosition += 1;
        int closingPosition = message.IndexOf(')');
        int length = closingPosition - openingPosition;
        System.Console.WriteLine(message.Substring(openingPosition, length));

        // taglia la stringa dopo la parentesi chiusa → prossima iterazione
        message = message.Substring(closingPosition + 1);
    }
    // output: What if / more than / set of parentheses
}

static void EsercizioIndexOfAny()
{
    System.Console.WriteLine("=== IndexOfAny con simboli misti ===");

    string message = "(What if) I have [different symbols] but every {open symbol} needs a [matching closing symbol]?";

    char[] openSymbols = { '[', '{', '(' };
    int closingPosition = 0;

    while (true)
    {
        int openingPosition = message.IndexOfAny(openSymbols, closingPosition);
        if (openingPosition == -1) break;

        // capisce quale simbolo ha trovato
        string currentSymbol = message.Substring(openingPosition, 1);

        char matchingSymbol = ' ';
        switch (currentSymbol)
        {
            case "[": matchingSymbol = ']'; break;
            case "{": matchingSymbol = '}'; break;
            case "(": matchingSymbol = ')'; break;
        }

        openingPosition += 1;
        closingPosition = message.IndexOf(matchingSymbol, openingPosition);

        int length = closingPosition - openingPosition;
        System.Console.WriteLine(message.Substring(openingPosition, length));
    }
    // output: What if / different symbols / open symbol / matching closing symbol
}
}