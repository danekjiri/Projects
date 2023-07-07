using System.Numerics;
namespace BigInDevision;
class Program
{
    static void Main(string[] args)
    {
        Devision d = new Devision();
        d.DoDevision();
        Console.ReadLine();
    }
}


/*
 //FACTORIAL FUNC
  BigInteger x = 10000;

        BigInteger factorial(BigInteger num)
        {
            if (num == 0)
                return 1;
            else
                return num * factorial(num - 1);
        }
   Console.WriteLine(factorial(x));
 */
