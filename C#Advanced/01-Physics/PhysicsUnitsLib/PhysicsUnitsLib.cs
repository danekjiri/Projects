using System.Security.Cryptography;

namespace PhysicsUnitsLib
{
    public static class MetersExtension
    {
        public static Distance Meters(this double d) => new Distance(d);
        public static Distance Meters(this int d) => new Distance(d);
        public static Distance Meters(this long d) => new Distance(d);
    }

    public static class SecondsExtension
    {
        public static Time Seconds(this double d) => new Time(d);
        public static Time Seconds(this int d) => new Time(d);
        public static Time Seconds(this long d) => new Time(d);
    }

    public static class MeterPerSecondsExtension
    {
        public static Speed MeterPerSeconds(this double d) => new Speed(d);
        public static Speed MeterPerSeconds(this int d) => new Speed(d);
        public static Speed MeterPerSeconds(this long d) => new Speed(d);
    }

    public interface IPhysicValue
    {
        public double Value { get; }
    }

    public record struct Distance : IPhysicValue
    {
        public double Value { get; init; }

        public Distance(double d)
        {
            Value = d;
        }

        public static Distance operator +(Distance d1, Distance d2) => new Distance(d1.Value + d2.Value);
        public static Distance operator -(Distance d1, Distance d2) => new Distance(d1.Value - d2.Value);
        public static Distance operator *(Distance d1, Distance d2) => new Distance(d1.Value * d2.Value);
        public static Distance operator *(Distance d1, double d2) => new Distance(d1.Value * d2);
        public static Distance operator *(double d1, Distance d2) => new Distance(d1 * d2.Value);
        public static Distance operator /(Distance d1, Distance d2) => new Distance(d1.Value / d2.Value);

        public static implicit operator Distance(double d) => new Distance(d);

        public static explicit operator double(Distance d) => d.Value;
    }
    public record struct Time : IPhysicValue
    {
        public double Value { get; init; }

        public Time(double d)
        {
            Value = d;
        }

        public static Time operator +(Time t1, Time t2) => new Time(t1.Value + t2.Value);
        public static Time operator -(Time t1, Time t2) => new Time(t1.Value - t2.Value);
        public static Time operator *(Time t1, Time t2) => new Time(t1.Value * t2.Value);
        public static Time operator *(Time t1, double d2) => new Time(t1.Value * d2);
        public static Time operator *(double d1, Time t2) => new Time(d1 * t2.Value);
        public static Time operator /(Time t1, Time t2) => new Time(t1.Value / t2.Value);
        public static Speed operator /(Distance d1, Time t2) => new Speed(d1.Value / t2.Value); // formula for speed given by distance/time

        public static implicit operator Time(double d) => new Time(d);

        public static explicit operator double(Time t) => t.Value;
    }
    public record struct Speed : IPhysicValue
    {
        public double Value { get; init; }

        public Speed(double d)
        {
            Value = d;
        }
        public static Speed operator +(Speed s1, Speed s2) => new Speed(s1.Value + s2.Value);
        public static Speed operator -(Speed s1, Speed s2) => new Speed(s1.Value - s2.Value);
        public static Speed operator *(Speed s1, Speed s2) => new Speed(s1.Value * s2.Value);
        public static Speed operator *(Speed s1, double d2) => new Speed(s1.Value * d2);
        public static Speed operator *(double d1, Speed s2) => new Speed(d1 * s2.Value);
        public static Distance operator *(Speed s1, Time t2) => new Distance(s1.Value * t2.Value); // formula for distance given by speed*time
        public static Distance operator *(Time t1, Speed s2) => new Distance(t1.Value * s2.Value); // formula for distance given by time*speed
        public static Speed operator /(Speed s1, Speed s2) => new Speed(s1.Value * s2.Value);

        public static implicit operator Speed(double d) => new Speed(d);

        public static explicit operator double(Speed s) => s.Value;
    }

}
