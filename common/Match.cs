namespace common
{
    public class Match
    {
        public static bool CaseInsensitive(string a, string b)
        {
            return a.Equals(b, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool CaseSensitive(string a, string b)
        {
            return a.Equals(b, StringComparison.CurrentCulture);
        }
    }
}
