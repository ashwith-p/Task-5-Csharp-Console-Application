using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Directory_Console_app.Utilities.Helpers
{
    public class ConsoleHelpers
    {
        public static void ConsoleOutput(string message, bool isNewLine = true)
        {
            if (isNewLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
        }

        public static string ConsoleIntegerInput() //change name,done
        {
            string input = Console.ReadLine() ?? "-1";
            if (!int.TryParse(input, out var _) || input.Length > 1)
            {
                input = "-1";
            }
            return input;
        }
    }
}
