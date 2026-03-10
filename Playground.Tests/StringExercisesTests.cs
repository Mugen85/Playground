namespace Playground.Tests;

public class StringExercisesTests
{
    [Fact]
    public void IndexOf_RestituisceZero_SeCarattereEInPrimaPosizione()
    {
        string myString = "C# Time";
        int result = myString.IndexOf('C');
        Assert.Equal(0, result);
    }

    [Fact]
    public void IndexOf_RestituisceMinusUno_SeCarattereNonEsiste()
    {
        string myString = "C# Time";
        int result = myString.IndexOf('Z');
        Assert.Equal(-1, result);
    }

    [Fact]
    public void Substring_EstraeContenutoTraParentesi()
    {
        string message = "Find what is (inside the parentheses)";
        int open = message.IndexOf('(') + 1;
        int length = message.IndexOf(')') - open;
        string result = message.Substring(open, length);
        Assert.Equal("inside the parentheses", result);
    }

    [Fact]
    public void Substring_ConCostanti_NonSiRompeSeTagCambia()
    {
        string message = "Valore: <div>hello</div>";
        const string openTag = "<div>";
        const string closeTag = "</div>";
        int open = message.IndexOf(openTag) + openTag.Length;
        int length = message.IndexOf(closeTag) - open;
        Assert.Equal("hello", message.Substring(open, length));
    }

    [Fact]
    public void LastIndexOf_TrovaUltimaOccorrenza()
    {
        string message = "hello there!";
        Assert.Equal(7, message.LastIndexOf('h'));
    }

    [Fact]
    public void While_EstraeTosteLeParentesi()
    {
        string message = "(alfa) e (beta) e (gamma)";
        var risultati = new List<string>();

        while (true)
        {
            int open = message.IndexOf('(');
            if (open == -1) break;
            open += 1;
            int close = message.IndexOf(')');
            risultati.Add(message.Substring(open, close - open));
            message = message.Substring(close + 1);
        }

        Assert.Equal(new[] { "alfa", "beta", "gamma" }, risultati);
    }

    [Fact]
    public void IndexOfAny_TrovaPrimoSimboloApertura()
    {
        string message = "Testo (con parentesi)";
        char[] simboli = { '[', '{', '(' };
        int pos = message.IndexOfAny(simboli);
        Assert.Equal('(', message[pos]);
    }
}