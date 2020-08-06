namespace PasswordGenerator
{
    public class GeneratorConfig
    {
        public SeedConfig Seed { get; set; }
        public PasswordConfig Password { get; set; }
        public string StreamSource { get; set; }
        public string StreamSourcePath { get; set; }

        public GeneratorConfig()
        {
            Seed = new SeedConfig("");
            Password = new PasswordConfig("");
        }

        public GeneratorConfig(string seedFormula, string suffix, string suffixFormula = null)
        {
            Seed = new SeedConfig(seedFormula);
            Password = new PasswordConfig(suffix, suffixFormula);
        }
    }
}