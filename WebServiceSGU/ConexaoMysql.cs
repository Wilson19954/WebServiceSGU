namespace WebServiceSGU
{
    public class ConexaoMysql
    {
        public static string conexaoString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration.GetConnectionString("ConexaoLocal");
        }
    }
}
