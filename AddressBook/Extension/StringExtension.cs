using System.Linq;

namespace AddressBook.Extension
{
    public static class StringExtension
    {
        public static bool HasOnlyWithespaces(this string str)
        {
            return str.All(char.IsWhiteSpace);
        }
    }
}
