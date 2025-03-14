namespace src.Framework;

public class Log
{
    public static void Normal(string message) => WriteLog("MSG", message, ConsoleColor.White);
    public static void Info(string message) => WriteLog("INFO", message, ConsoleColor.Cyan);
    public static void Warning(string message) => WriteLog("WARNING", message, ConsoleColor.Yellow);
    public static void Error(string message) => WriteLog("ERROR", message, ConsoleColor.Red);
    
    
    private static readonly Lock Lock = new();
    private static void WriteLog(string level, string message, ConsoleColor color)
    {
        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        lock (Lock)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{timeStamp}] [{level}] - [{message}]");
            Console.ResetColor();
        }
    }
}