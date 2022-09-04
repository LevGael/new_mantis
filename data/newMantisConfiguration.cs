namespace newMantis.Configuration
{
    public class MantisConfigManager : IMantisConfigManager
    {
        private readonly IConfiguration _configuration;
        public MantisConfigManager() {}
        public MantisConfigManager(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
 
        public string Uri 
        {
            get
            {
                return this._configuration["MantisConfig:Uri"];
            }
        }

        public string Authorization 
        {
            get
            {
                return this._configuration["MantisConfig:Authorization"];
            }
        }
    }
}