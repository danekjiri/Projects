namespace DayDifference;
class Program
{
    static void Main(string[] args)
    {
        DayDifference dd = new DayDifference();
        dd.GetDates();
        int diff = dd.CountDifference();
        Console.WriteLine(diff);
        Console.Read();
    }
}

