namespace ZopaLoans.Sys.IO
{
    public class Console : IConsole
    {
        public void WriteLine(string value)
        {
            System.Console.WriteLine(value);
        }

        public string ReadLine()
        {
            return System.Console.ReadLine();
        }
    }
}