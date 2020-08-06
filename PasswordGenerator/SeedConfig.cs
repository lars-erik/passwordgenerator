namespace PasswordGenerator
{
    public class SeedConfig
    {
        public string Formula { get; set; }

        public SeedConfig()
        {
            
        }

        public SeedConfig(string formula)
        {
            Formula = formula;
        }
    }
}