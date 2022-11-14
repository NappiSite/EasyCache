namespace NappiSite.EasyCache.Tests;

[Serializable]
public record DummyObject(string Name)
{
    public override string ToString()
    {
        return $"{{ Name = {Name} }}";
    }
}