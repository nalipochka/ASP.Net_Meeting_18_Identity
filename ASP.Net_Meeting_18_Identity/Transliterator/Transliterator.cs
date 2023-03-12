using System.Text;

namespace ASP.Net_Meeting_18_Identity.Transliterator
{
    public static class Transliterator
    {
        private static readonly string[] RussianChars = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я", "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
        private static readonly string[] LatinChars = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "h", "ts", "ch", "sh", "shch", "", "y", "", "e", "yu", "ya", "A", "B", "V", "G", "D", "E", "YO", "ZH", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "H", "TS", "CH", "SH", "SHCH", "", "Y", "", "E", "YU", "YA" };

        public static string ConvertToTranslit(string? input)
        {
            

            if (input != null)
            {
                StringBuilder result = new StringBuilder();

                for (int i = 0; i < input.Length; i++)
                {
                    string currentChar = input[i].ToString();
                    int charIndex = Array.IndexOf(RussianChars, currentChar);

                    if (charIndex >= 0)
                    {
                        result.Append(LatinChars[charIndex]);
                    }
                    else
                    {
                        result.Append(currentChar);
                    }
                }
                return result.ToString().Replace(" ","_");
            }
            else
            {
                return input!;
            }
        }
    }
}
