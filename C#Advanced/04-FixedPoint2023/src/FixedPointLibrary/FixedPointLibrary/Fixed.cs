using System.Numerics;

namespace Cuni
{
    namespace Arithmetics
    {
        /*
         * Library implementing basic Arithmetics operations 
         *  and other methods such as: ToString(), ToDouble(), To() 
         *  and Extension method on list SumAll()
         */
        namespace FixedPoint
        {
            /*
             * DotPrecision (fraction lenght) of given Fixed type
             */
            public enum FixedDotPrecision
            {
                Dot3 = 3,
                Dot4 = 4,
                Dot8 = 8,
                Dot16 = 16,
                Dot24 = 24
            }

            /*
             * Common interface for created fraction part types returning its precision
             */
            public interface IDotPrecision
            {
                public static abstract FixedDotPrecision DotPrecision { get; }
            }

            /*
             * Set of defined precision types follows: Dot3, Dot4, Dot8, Dot16, Dot24
             */
            public sealed class Dot3 : IDotPrecision
            {
                private Dot3()
                {
                    // the instance cannnot be created.
                }
                public static FixedDotPrecision DotPrecision => FixedDotPrecision.Dot3;
            }

            public sealed class Dot4 : IDotPrecision
            {
                private Dot4()
                {
                    // the instance cannnot be created.
                }
                public static FixedDotPrecision DotPrecision => FixedDotPrecision.Dot4;
            }

            public sealed class Dot8 : IDotPrecision
            {
                private Dot8()
                {
                    // the instance cannnot be created.
                }
                public static FixedDotPrecision DotPrecision => FixedDotPrecision.Dot8;
            }

            public sealed class Dot16 : IDotPrecision
            {
                private Dot16()
                {
                    // the instance cannnot be created.
                }
                public static FixedDotPrecision DotPrecision => FixedDotPrecision.Dot16;
            }

            public sealed class Dot24 : IDotPrecision
            {
                private Dot24()
                {
                    // the instance cannnot be created.
                }
                public static FixedDotPrecision DotPrecision => FixedDotPrecision.Dot24;
            }

            /*
             * generic interface defining required methods for FixedPoint generic type
             */
            public interface IFixedMethods<TSize, TFixedDotPrecision>
                where TSize : IBinaryInteger<TSize>, IBitwiseOperators<TSize, TSize, TSize>, IConvertible
                where TFixedDotPrecision : IDotPrecision
            {
                // Print out the FixedPoint number representation in human readable format
                public abstract string ToString();

                // Conversion from FixedPoint representation to .NET floating point representation type (double)
                public double ToDouble();

                // Fraction precision conversion between same Size of FixedPoint type
                public Fixed<TSize, TNewFixedDotPrecision> To<TNewFixedDotPrecision>()
                    where TNewFixedDotPrecision : IDotPrecision;
            }

            /*
             * Generic data type for representation of FixedPoint<Size, DotPrecision> numbers that consists of:
             *   - Size (binary integer - byte, short, int, long)ma
             *   - Dot Precision (defined type - Dot3, Dot4, Dot8, Dot16, Dot24)
             * And required methods by interface: IFixedMethods<TSize, TFixedDotPrecision>
             */
            public readonly record struct Fixed<TSize, TFixedDotPrecision> :
                IFixedMethods<TSize, TFixedDotPrecision>,
                IAdditionOperators<Fixed<TSize, TFixedDotPrecision>, Fixed<TSize, TFixedDotPrecision>, Fixed<TSize, TFixedDotPrecision>>,
                ISubtractionOperators<Fixed<TSize, TFixedDotPrecision>, Fixed<TSize, TFixedDotPrecision>, Fixed<TSize, TFixedDotPrecision>>,
                IMultiplyOperators<Fixed<TSize, TFixedDotPrecision>, Fixed<TSize, TFixedDotPrecision>, Fixed<TSize, TFixedDotPrecision>>,
                IDivisionOperators<Fixed<TSize, TFixedDotPrecision>, Fixed<TSize, TFixedDotPrecision>, Fixed<TSize, TFixedDotPrecision>>,
                IAdditiveIdentity<Fixed<TSize, TFixedDotPrecision>, Fixed<TSize, TFixedDotPrecision>>,
                IMultiplicativeIdentity<Fixed<TSize, TFixedDotPrecision>, Fixed<TSize, TFixedDotPrecision>>
                where TSize : IBinaryInteger<TSize>, IBitwiseOperators<TSize, TSize, TSize>, IConvertible
                where TFixedDotPrecision : IDotPrecision
            {
                public readonly TSize Value { get; init; }
                public static Fixed<TSize, TFixedDotPrecision> AdditiveIdentity =>  new Fixed<TSize, TFixedDotPrecision>(TSize.Zero);
                public static Fixed<TSize, TFixedDotPrecision> MultiplicativeIdentity => new Fixed<TSize, TFixedDotPrecision>(TSize.One);

                // Main constructor for init FixedPoint from double number type
                public Fixed(double doubleConst)
                {
                    this.Value = ConvertDoubleToFixed(doubleConst);
                }

                // Helper constructor for FixedPoint arithmetic
                private Fixed(TSize value)
                {
                    this.Value = value;
                }
                
                // Convert double number type to Given FixedPoint<Size, DotPrecision> - helper method for main constructor
                private TSize ConvertDoubleToFixed(double doubleConst)
                {
                    // the whole part is separated then converted to ulong (because TSize.CreateTruncating(double) not working properly)
                    TSize uintBitValue = TSize.CreateTruncating(Convert.ToUInt64(Math.Truncate(doubleConst)));
                    TSize fractionBitValue = CreateFractionPartFromDouble(doubleConst, (int)TFixedDotPrecision.DotPrecision);

                    return MergeIntAndFractionPart(uintBitValue, fractionBitValue, (int)TFixedDotPrecision.DotPrecision);
                }

                // Merge the whole part of the number and the fraction part together to proper DotPrecision - helper method in ConvertDoubleToFixed()
                private TSize MergeIntAndFractionPart(TSize intPart, TSize fractionPart, int dotPrecision)
                {
                    intPart <<= dotPrecision;
                    TSize result = intPart | fractionPart;
                    return result;
                }

                // Convert fraction part of double number type to FixedPoint fraction part - helper method in ConvertDoubleToFixed()
                private TSize CreateFractionPartFromDouble(double doubleConst, int dotPrecision)
                {
                    double fractionPartDouble = doubleConst - Math.Truncate(doubleConst);
                    double negativePower = 0.5;
                    int fractionPartBits = 0;

                    while (dotPrecision > 0)
                    {
                        fractionPartBits <<= 1;

                        if (fractionPartDouble >= negativePower)
                        {
                            fractionPartDouble -= negativePower;
                            fractionPartBits++;
                        }
                        negativePower /= 2;
                        dotPrecision--;
                    }
                    return TSize.CreateTruncating(fractionPartBits);
                }

                // Convert current FixedPoint<Size, DotPrecison> to corresponding .NET double number data type
                public double ToDouble()
                {
                    TSize uintDoubleValue = this.Value >> (int)TFixedDotPrecision.DotPrecision;
                    double fractionDoubleValue = CreateFractionPartFromFixedPoint(this.Value, (int)TFixedDotPrecision.DotPrecision);

                    return double.CreateTruncating(uintDoubleValue) + fractionDoubleValue;
                }

                // Convert fraction part of current FixedPoint<Size, DotPrecision> to double eg: 11.110 -> 0.75
                private double CreateFractionPartFromFixedPoint(TSize fixedPoint, int dotPrecision)
                {
                    // the conversion is done from LSB (the smallest precision fraction bit)
                    double negativePower = Math.Pow(2, -dotPrecision);
                    double fractionValue = 0;
                    int LSbMask = 1;

                    while (dotPrecision > 0)
                    {
                        // get the last bit and if set, the fraction is greatened by the bit fixed point value
                        fractionValue += negativePower * (int.CreateTruncating(fixedPoint) & LSbMask);

                        fixedPoint >>= 1;
                        negativePower *= 2;
                        dotPrecision--;
                    }
                    return fractionValue;
                }

                // New implementation of ToString() to print out current FixedPoint<Size, DotPrecison> to human readable format using ToDouble()
                public override string ToString()
                {
                    return ((double)this.ToDouble()).ToString();
                }

                // Convert current FixedPoint<Size, DotPrecision> to FixedPoint<Size, NEWDotPrecision> 
                public Fixed<TSize, TNewFixedDotPrecision> To<TNewFixedDotPrecision>()
                    where TNewFixedDotPrecision : IDotPrecision
                {
                    int dotPrecisionDifference = TFixedDotPrecision.DotPrecision - TNewFixedDotPrecision.DotPrecision;
                    TSize helper = this.Value;

                    if (dotPrecisionDifference > 0)
                    {
                        helper >>= dotPrecisionDifference;
                    }
                    else if (dotPrecisionDifference < 0)
                    {
                        helper <<= (-dotPrecisionDifference);
                    }

                    return new Fixed<TSize, TNewFixedDotPrecision>(helper);
                }

                // Implemetation of Addition operation with FixedPoint numbers
                public static Fixed<TSize, TFixedDotPrecision> operator +(Fixed<TSize, TFixedDotPrecision> leftOperand, Fixed<TSize, TFixedDotPrecision> rightOperand)
                {
                    return new Fixed<TSize, TFixedDotPrecision>(leftOperand.Value + rightOperand.Value);
                }

                // Implemetation of Subtraction operation with FixedPoint numbers
                public static Fixed<TSize, TFixedDotPrecision> operator -(Fixed<TSize, TFixedDotPrecision> leftOperand, Fixed<TSize, TFixedDotPrecision> rightOperand)
                {
                    return new Fixed<TSize, TFixedDotPrecision>(leftOperand.Value - rightOperand.Value);
                }

                // Implemetation of Multiplication operation with FixedPoint numbers
                // BAD IMPLEMENTATNION SHOLD BE LIKE:
                //  var res = ((IConvertible) left.Value).ToInt64(null) * ((IConvertible) right.Value).ToInt64(null) && shift by dotPrecision
                //  -> have to add TSize : ..., IConvertible
                public static Fixed<TSize, TFixedDotPrecision> operator *(Fixed<TSize, TFixedDotPrecision> leftOperand, Fixed<TSize, TFixedDotPrecision> rightOperand)
                {
                    TSize FractionMask = CreateLSBMask((int)TFixedDotPrecision.DotPrecision);
                    TSize leftIntPart = leftOperand.Value >> (int)TFixedDotPrecision.DotPrecision;
                    TSize rightIntPart = rightOperand.Value >> (int)TFixedDotPrecision.DotPrecision;

                    TSize leftFractionPart = leftOperand.Value & FractionMask;
                    TSize rightFractionPart = rightOperand.Value & FractionMask;

                    TSize res = TSize.Zero;
                    res += (leftIntPart * rightIntPart) << (int)TFixedDotPrecision.DotPrecision;
                    res += (leftIntPart * rightFractionPart);
                    res += (leftFractionPart * rightIntPart);
                    res += ((leftFractionPart * rightFractionPart) >> (int)TFixedDotPrecision.DotPrecision) & FractionMask;
                    return new Fixed<TSize, TFixedDotPrecision>(res);
                }

                // Implemetation of Division operation with FixedPoint numbers
                public static Fixed<TSize, TFixedDotPrecision> operator /(Fixed<TSize, TFixedDotPrecision> leftOperand, Fixed<TSize, TFixedDotPrecision> rightOperand)
                {
                    ulong helper = Convert.ToUInt64(leftOperand.Value);
                    helper <<= (int)TFixedDotPrecision.DotPrecision;
                    helper = helper / Convert.ToUInt64(rightOperand.Value);
                    return new Fixed<TSize, TFixedDotPrecision>(TSize.CreateTruncating(helper));
                }
                
                // Return binary number of current FixedPoint size with given n bits from LSB - helper method in multiplication operation 
                private static TSize CreateLSBMask(int bitCount)
                {
                    TSize mask = TSize.Zero;
                    while (bitCount > 0)
                    {
                        mask <<= 1;
                        mask++;
                        bitCount--;
                    }
                    return mask;
                }
            }

            /*
             *  Static class that expands behaviour of List<T>
             */
            public static class FixedListExtensions
            {
                // Sum all members of List<T> where T has only additive members - can work with FixedPoint<Size, DotPrecision>
                public static TAdditive SumAll<TAdditive>(this List<TAdditive> listOfAdditives)
                    where TAdditive : IAdditionOperators<TAdditive, TAdditive, TAdditive>, IAdditiveIdentity<TAdditive, TAdditive>
                {
                    TAdditive sum = TAdditive.AdditiveIdentity;
                    
                    foreach (TAdditive additive in listOfAdditives)
                    {
                        sum = sum + additive;
                    }
                    return sum;
                }
            }
        }
    }
}