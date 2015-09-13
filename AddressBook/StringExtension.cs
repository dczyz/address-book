namespace AddressBook
{
    public static class StringExtension
    {
        public static bool HasOnlyWithespaces(this string str)
        {
            return str.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "").Length == 0;
        }
    }
}
