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
        Remove();
        Replace();
        EsercizioChallengeHtml();
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
    static void Remove()
    {
        System.Console.WriteLine("=== Remove — posizione fissa ===");

        // Caso d'uso reale: dati con struttura fissa (es. file legacy)
        // 5 cifre ID | 20 caratteri nome | 6 importo | 3 articoli
        string data = "12345John Smith          5000  3  ";
        string updatedData = data.Remove(5, 20); // rimuovo il nome
        System.Console.WriteLine(updatedData);   // 123455000  3

        System.Console.WriteLine("=== Remove — posizione dinamica ===");

        // Variante: posizione calcolata con IndexOf
        string message = "What if I want to remove (this part) from the string?";
        int openingPosition = message.IndexOf('(');
        int closingPosition = message.IndexOf(')') + 1;
        string result = message.Remove(openingPosition, closingPosition - openingPosition);
        System.Console.WriteLine(result);
    }
    static void Replace()
    {
        System.Console.WriteLine("=== Replace — uso diretto ===");

        // Caso d'uso principale: pulizia di dati con pattern ripetuto
        string message = "This--is--ex-amp-le--da-ta";
        message = message.Replace("--", " ");  // doppio trattino → spazio
        message = message.Replace("-", "");    // trattino singolo → niente
        System.Console.WriteLine(message);     // This is example data

        System.Console.WriteLine("=== Replace — con posizione calcolata ===");

        // Versione con IndexOf: salva le posizioni in variabili (non ripetere IndexOf)
        string testo = "What if I want to replace (this part) with something else?";
        int open = testo.IndexOf('(');
        int close = testo.IndexOf(')') + 1;        // salvo subito il risultato
        string toReplace = testo.Substring(open, close - open);
        string result = testo.Replace(toReplace, "[REDACTED]");
        System.Console.WriteLine(result);
    }
    static void EsercizioChallengeHtml()
    {
        System.Console.WriteLine("=== Challenge: estrarre testo da HTML ===");

        const string input = "<div><h2>Widgets &trade;</h2><span>5000</span></div>";
        const string openSpan  = "<span>";
        const string closeSpan = "</span>";

        // Estrai quantity tra <span> e </span>
        int openingPosition = input.IndexOf(openSpan) + openSpan.Length;
        int closingPosition = input.IndexOf(closeSpan); 
        string quantity = input.Substring(openingPosition, closingPosition - openingPosition);

       // Rimuovi <div> all'inizio
        string output = input.Remove(0, "<div>".Length);
       // Rimuovi </div> alla fine — LastIndexOf trova la posizione, Length evita il conteggio
        int lastDivPos = output.LastIndexOf("</div>");
        output = output.Remove(lastDivPos, "</div>".Length);

        output = output.Replace("&trade;", "&reg;");

        System.Console.WriteLine($"Quantity: {quantity}"); // <span>5000
        System.Console.WriteLine($"Output: {output}"); // <h2>Widgets &reg;</h2><span>5000</span>
    }
    
}