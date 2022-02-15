using NUnit.Framework;
using API.Model;
using System.Collections.Generic;


[TestFixture]
public class SpelRepositoryTest
{
    [Test]
    public void AddSpel_Spel_SpelAdded()
    {
        Spel spel = new Spel();
        SpelRepository spelRepository = new SpelRepository();
        spelRepository.AddSpel(spel);
        Assert.Contains(spel, spelRepository.GetSpellen());
    }

    [Test]
    public void GetSpellen_Spellen_List()
    {
        Spel spel0 = new Spel();
        Spel spel1 = new Spel();
        Spel spel2 = new Spel();
        SpelRepository spelRepository = new SpelRepository();
        spelRepository.AddSpel(spel0);
        spelRepository.AddSpel(spel1);
        spelRepository.AddSpel(spel2);
        Assert.Contains(spel0, spelRepository.GetSpellen());
        Assert.Contains(spel1, spelRepository.GetSpellen());
        Assert.Contains(spel2, spelRepository.GetSpellen());
    }

    [Test]
    public void GetSpellen_noSpellen_EmptyList()
    {
        SpelRepository spelRepository = new SpelRepository();
        Assert.AreEqual(new List<Spel>(), spelRepository.GetSpellen());
    }

    [Test]
    public void GetSpel_Spel_Spel()
    {
        Spel spel = new Spel();
        SpelRepository spelRepository = new SpelRepository();
        spelRepository.AddSpel(spel);
        Assert.AreEqual(spel, spelRepository.GetSpel(spel.Token));
    }

    [Test]
    public void GetSpel_EmptyString_Null()
    {
        SpelRepository spelRepository = new SpelRepository();
        Assert.AreEqual(null, spelRepository.GetSpel(""));
    }
}
