namespace be_atoutmajeur.Commons;

public class OrderRefGenerator
{
   
    public static string GenerateOrderRefPattern()
    {
        var patterns = new[]
        {
            () => $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
            () => $"AM-{DateTime.UtcNow:yyMMdd}-{Random.Shared.Next(100, 999)}",
            () => $"REF{DateTime.UtcNow:yyMMdd}{Random.Shared.Next(10000, 99999)}",
            () => $"ORD-{DateTime.UtcNow:yyMMdd}-{GenerateRandomLetters(3)}{Random.Shared.Next(100, 999)}"
        };
        var selectedPattern = patterns[Random.Shared.Next(patterns.Length)];
        return selectedPattern();
    }
    protected static string GenerateRandomLetters(int length)
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var result = new char[length];
        
        for (int i = 0; i < length; i++)
        {
            result[i] = letters[Random.Shared.Next(letters.Length)];
        }
        
        return new string(result);
    }
}