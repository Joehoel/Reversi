using System.Collections.Generic;
using API.Model;
public class SpelRepository : ISpelRepository
{
    // Lijst met tijdelijke spellen
    public List<Spel> Spellen { get; set; }

    public SpelRepository()
    {
        Spel spel1 = new Spel();
        Spel spel2 = new Spel();
        Spel spel3 = new Spel();

        spel1.Player1Token = "abcdef";
        spel1.Description = "Potje snel reversi, dus niet lang nadenken";
        spel2.Player1Token = "ghijkl";
        spel2.Player2Token = "mnopqr";
        spel2.Description = "Ik zoek een gevorderde tegenspeler!";
        spel3.Player1Token = "stuvwx";
        spel3.Description = "Na dit spel wil ik er nog een paar spelen tegen zelfde tegenstander";


        Spellen = new List<Spel> { spel1, spel2, spel3 };
    }

    public void AddSpel(Spel spel)
    {
        Spellen.Add(spel);
    }

    public List<Spel> GetSpellen()
    {
        return Spellen;
    }

    public Spel GetSpel(string spelToken)
    {
        return Spellen.Find(spel => spel.Token == spelToken);
    }


    // ...

}
