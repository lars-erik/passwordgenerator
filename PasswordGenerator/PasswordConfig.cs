namespace PasswordGenerator
{
    public class PasswordConfig
    {
        public string Suffix { get; set; }
        public string SuffixFormula { get; set; }

        public PasswordConfig()
        {
            
        }

        public PasswordConfig(string suffix, string suffixFormula = null)
        {
            Suffix = suffix ?? "";
            SuffixFormula = suffixFormula;
        }
    }
}