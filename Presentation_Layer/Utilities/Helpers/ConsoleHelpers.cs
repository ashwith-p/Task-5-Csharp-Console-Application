namespace EmployeeDirectory.Utilities.Helpers
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

        public static int ConsoleIntegerInput()
        {
            string input = Console.ReadLine() ?? "-1";
            int result;
            if (!int.TryParse(input, out var x) || input.Length > 1)
            {
                result = -1;
            }
            else
            {
                result = x;
            }
            return result;
        }
    }
}
