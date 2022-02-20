using System.Collections.Generic;
using API.Model;

public interface ISpelRepository
{
    void AddSpel(Spel spel);

    List<Spel> GetSpellen();

    Spel GetSpel(string spelToken);
}
