
using System.Numerics;

int x = 5;

Console.WriteLine(x);

long y = 6;

Console.WriteLine(y);

Use<int>();
Use<long>();

static void Use<T>() where T : INumber<T> {
	Console.WriteLine(T.AdditiveIdentity);
	Console.WriteLine(T.MultiplicativeIdentity);
}