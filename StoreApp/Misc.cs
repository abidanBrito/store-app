using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp
{
    public static class Misc
    {
        public static bool validateDiscount(string input)
        {  
            if (int.TryParse(input, out int parsedValue) &&
                parsedValue > 0 && parsedValue < 100)
            {
                return true;
            }

            return false;
        }

        public static string normalize(string input)
        {
            // pasar todo a minúscula y hacer un substring de la primera letra 
            // y ponerla a mayúscula
            string first = char.ToUpper(input[0]).ToString();
            input.Remove(0, 1);
            input = first + input;

            return input;
        }

    }
}
