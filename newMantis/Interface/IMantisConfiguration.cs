namespace newMantis.Configuration
{
    public interface IMantisConfigManager
    {
        string Uri { get; }

        string Authorization { get; }
    }
}