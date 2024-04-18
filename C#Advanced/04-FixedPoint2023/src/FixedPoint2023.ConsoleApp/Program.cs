using Cuni.Arithmetics.FixedPoint;

// IMPORTANT NOTE: You should NOT change anything in this Main method as part of your solution! Put you implementation in other methods and types. Introduce new projects into solution if beneficial.

{
    Console.WriteLine(">>> Fixed<byte, Dot3> >>>");
	Fixed<byte, Dot3> x1 = new(1.25);
	Console.WriteLine(x1.ToDouble());

	Fixed<byte, Dot3> x2 = new(3.5);
	Console.WriteLine(x2);

	x1 += x2;
	Console.WriteLine(x1.ToDouble());

	x2 *= new Fixed<byte, Dot3>(2);
	Console.WriteLine(x2.ToDouble());

	x1 *= new Fixed<byte, Dot3>(2);
	Console.WriteLine(x1.ToDouble());

	x1 = new(16);
	x1 *= new Fixed<byte, Dot3>(64);
	Console.WriteLine(x1.ToDouble());

	x1 = new(5);
	x1 *= new Fixed<byte, Dot3>(31);
	Console.WriteLine(x1.ToDouble());

	x1 = new(1 << 20);
	x1 *= x1;
	Console.WriteLine(x1.ToDouble());

	x1 = new(7);
	x1 /= new Fixed<byte, Dot3>(2);
	Console.WriteLine(x1.ToDouble());

	x1 = new(7);
	x1 /= new Fixed<byte, Dot3>(8);
	Console.WriteLine(x1.ToDouble());

	x1 = new(7);
	x1 /= new Fixed<byte, Dot3>(16);
	Console.WriteLine(x1.ToDouble());

	x1 = new(7);
	x1 /= new Fixed<byte, Dot3>(0.5);
	Console.WriteLine(x1.ToDouble());

	x1 = new(7);
	x1 /= new Fixed<byte, Dot3>(1.5);
	Console.WriteLine(x1.ToDouble());
}

Console.WriteLine("---");

{
	Console.WriteLine(">>> Fixed<int, Dot3> >>>");
	Fixed<int, Dot3> x1 = new(1.25);
	Console.WriteLine(x1.ToDouble());

	Fixed<int, Dot3> x2 = new(3.5);
	Console.WriteLine(x2);

	x1 += x2;
	Console.WriteLine(x1);

	x2 *= new Fixed<int, Dot3>(2);
	Console.WriteLine(x2);

	x1 *= new Fixed<int, Dot3>(2);
	Console.WriteLine(x1);

	x1 = new(16);
	x1 *= new Fixed<int, Dot3>(64);
	Console.WriteLine(x1);

	x1 = new(5);
	x1 *= new Fixed<int, Dot3>(31);
	Console.WriteLine(x1);

	x1 = new(1 << 20);
	x1 *= x1;
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<int, Dot3>(2);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<int, Dot3>(8);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<int, Dot3>(16);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<int, Dot3>(0.5);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<int, Dot3>(1.5);
	Console.WriteLine(x1);
}

Console.WriteLine("---");

{
	Console.WriteLine(">>> Fixed<int, Dot8> >>>");
	Fixed<int, Dot8> x1 = new(1.25);
	Console.WriteLine(x1.ToDouble());

	Fixed<int, Dot8> x2 = new(3.5);
	Console.WriteLine(x2);

	x1 += x2;
	Console.WriteLine(x1);

	x2 *= new Fixed<int, Dot8>(2);
	Console.WriteLine(x2);

	x1 *= new Fixed<int, Dot8>(2);
	Console.WriteLine(x1);

	x1 = new(16);
	x1 *= new Fixed<int, Dot8>(64);
	Console.WriteLine(x1);

	x1 = new(5);
	x1 *= new Fixed<int, Dot8>(31);
	Console.WriteLine(x1);

	x1 = new(1 << 20);
	x1 *= x1;
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<int, Dot8>(2);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<int, Dot8>(8);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<int, Dot8>(16);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<int, Dot8>(0.5);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<int, Dot8>(1.5);
	Console.WriteLine(x1);
}

Console.WriteLine("---");

{
	Console.WriteLine(">>> Fixed<long, Dot8> >>>");
	Fixed<long, Dot8> x1 = new(1.25);
	Console.WriteLine(x1.ToDouble());

	Fixed<long, Dot8> x2 = new(3.5);
	Console.WriteLine(x2);

	x1 += x2;
	Console.WriteLine(x1);

	x2 *= new Fixed<long, Dot8>(2);
	Console.WriteLine(x2);

	x1 *= new Fixed<long, Dot8>(2);
	Console.WriteLine(x1);

	x1 = new(16);
	x1 *= new Fixed<long, Dot8>(64);
	Console.WriteLine(x1);

	x1 = new(5);
	x1 *= new Fixed<long, Dot8>(31);
	Console.WriteLine(x1);

	x1 = new(1 << 20);
	x1 *= x1;
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<long, Dot8>(2);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<long, Dot8>(8);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<long, Dot8>(16);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<long, Dot8>(0.5);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<long, Dot8>(1.5);
	Console.WriteLine(x1);
}

Console.WriteLine("---");

{
	Console.WriteLine(">>> Fixed<long, Dot16> >>>");
	Fixed<long, Dot16> x1 = new(1.25);
	Console.WriteLine(x1.ToDouble());

	Fixed<long, Dot16> x2 = new(3.5);
	Console.WriteLine(x2);

	x1 += x2;
	Console.WriteLine(x1);
    var x3 = new Fixed<long, Dot16>();
	x3 += x2;
    x2 *= new Fixed<long, Dot16>(2);
	Console.WriteLine(x2);

	x1 *= new Fixed<long, Dot16>(2);
	Console.WriteLine(x1);

	x1 = new(16);
	x1 *= new Fixed<long, Dot16>(64);
	Console.WriteLine(x1);

	x1 = new(5);
	x1 *= new Fixed<long, Dot16>(31);
	Console.WriteLine(x1);

	x1 = new(1 << 20);
	x1 *= x1;
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<long, Dot16>(2);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<long, Dot16>(8);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<long, Dot16>(16);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<long, Dot16>(0.5);
	Console.WriteLine(x1);

	x1 = new(7);
	x1 /= new Fixed<long, Dot16>(1.5);
	Console.WriteLine(x1);
}

Console.WriteLine("======");

{
	Fixed<int, Dot3> x1;
	Fixed<int, Dot8> x2 = new(3.5);

	// Error: Cannot implicitly convert type 'Cuni.Arithmetics.FixedPoint.Fixed<int, Cuni.Arithmetics.FixedPoint.Dot8>' to 'Cuni.Arithmetics.FixedPoint.Fixed<int, Cuni.Arithmetics.FixedPoint.Dot3>'
	 //x1 = x2;

	//Fixed<long, Dot8> x3;
	// Error: Cannot implicitly convert type 'Cuni.Arithmetics.FixedPoint.Fixed<int, Cuni.Arithmetics.FixedPoint.Dot8>' to 'Cuni.Arithmetics.FixedPoint.Fixed<long, Cuni.Arithmetics.FixedPoint.Dot8>'
	 //x3 = x2;

	x1 = x2.To<Dot3>();
	Console.WriteLine(x1);

	x2 = new(2.5625);
	Console.WriteLine(x2);

	x1 = x2.To<Dot3>();
	Console.WriteLine(x1);

	x1 = new(1.25);
	x2 = x1.To<Dot8>();
	Console.WriteLine(x2);
}

Console.WriteLine("++++++");

{
	var iList = new List<int> { 1, 2, 3, 4 };
	var iSum = iList.SumAll();
	Console.WriteLine(iSum);

	var dList = new List<double> { 1.1, 2.2, 3.3, 4.4, 0.125 };
	var dSum = dList.SumAll();
	Console.WriteLine(dSum);

	Console.WriteLine(">>> Fixed >>>");

	var f1List = new List<Fixed<int, Dot8>> { new(1), new(2), new(3), new(4) };
	var f1Sum = f1List.SumAll();
	Console.WriteLine(f1Sum);

	var f2List = new List<Fixed<int, Dot4>> { new(1.1), new(2.2), new(3.3), new(4.4), new(0.125) };
	var f2Sum = f2List.SumAll();
	Console.WriteLine(f2Sum);

	var f3List = new List<Fixed<int, Dot8>> { new(1.1), new(2.2), new(3.3), new(4.4), new(0.125) };
	var f3Sum = f3List.SumAll();
	Console.WriteLine(f3Sum);

	var f4List = new List<Fixed<int, Dot16>> { new(1.1), new(2.2), new(3.3), new(4.4), new(0.125) };
	var f4Sum = f4List.SumAll();
	Console.WriteLine(f4Sum);
}