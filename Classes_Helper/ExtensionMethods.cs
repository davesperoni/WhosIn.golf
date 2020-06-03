using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhosIn
{
    public static class ExtensionMethods
    {
        public static string UppercaseFirstLetter(this string value)
        {
            // Uppercase the first letter in the string.
            if (value.Length > 0)
            {
                char[] array = value.ToCharArray();
                array[0] = char.ToUpper(array[0]);
                return new string(array);
            }
            return value;

        }

        public static string Substring2(this string tempString, int startIndex, int intLength)
        {

            if (string.IsNullOrEmpty(tempString))
            {
                return "";
            }

            if (startIndex < 0)
            {
                return "";
            }

            if ((startIndex + 1) > tempString.Length)
            {
                return "";
            }

            if ((startIndex + intLength) > tempString.Length)
            {
                return tempString.Substring(startIndex);
            }

            return tempString.Substring(startIndex, intLength);
        }

        public static string Substring2(this string tempString, int startIndex)
        {
            if (startIndex < 0)
            {
                return "";
            }

            if ((startIndex + 1) > tempString.Length)
            {
                return "";
            }

            return tempString.Substring(startIndex);
        }

        public static void NoNull(this string[,] array1)
        {
            for (int row = array1.GetLowerBound(0); row <= array1.GetUpperBound(0); row++)
                for (int col = array1.GetLowerBound(1); col <= array1.GetUpperBound(1); col++)
                    if (array1[row, col] == null)
                    {
                        array1[row, col] = "";
                    }

        }

        public static void NoNull(this string[] array1)
        {
            for (int row = array1.GetLowerBound(0); row <= array1.GetUpperBound(0); row++)
            {
                if (array1[row] == null)
                {
                    array1[row] = "";
                }
            }



        }

        public static DateTime? MakeNullIfNoDate(this DateTime? value)
        {
            if (value == Convert.ToDateTime(c.NODATE_STRING))
            {
                return null;
            }
            else
            {
                return value;
            }
        }

    }
}



