
using System;

namespace Cronos
{
    public class DateRange : IComparable<DateRange>
    {
        public DateRange(DateTime start, DateTime end)
        {
            if (end <= start)
                throw new ArgumentOutOfRangeException($"{nameof(start)} must be less than {nameof(end)}");

            Start = start;
            End = end;
        }

        public DateTime Start { get; }

        public DateTime End { get; }

        public TimeSpan Size { get { return End - Start; } }

        #region [ -- Algebraic helper methods -- ]

        public bool Intersects(DateRange rhs)
        {
            return rhs.Start < End && rhs.End > Start;
        }

        public bool Adjacent(DateRange rhs)
        {
            return rhs.Start == End || rhs.End == Start;
        }

        public DateRange Union(DateRange rhs)
        {
            if (!Intersects(rhs) && !Adjacent(rhs))
                throw new ArgumentOutOfRangeException($"{nameof(rhs)} must be adjacent or intersect with the current instance");

            return new DateRange(Start < rhs.Start ? Start : rhs.Start, End > rhs.End ? End : rhs.End);
        }

        public DateRange Intersection(DateRange rhs)
        {
            if (!Intersects(rhs))
                throw new ArgumentOutOfRangeException($"{nameof(rhs)} must intersect with date");

            return new DateRange(Start < rhs.Start ? rhs.Start : Start, End > rhs.End ? rhs.End : End);
        }

        #endregion

        #region [ -- Overloaded operators -- ]

        public static bool operator == (DateRange lhs, DateRange rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator != (DateRange lhs, DateRange rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static bool operator < (DateRange lhs, DateRange rhs)
        {
            return lhs.CompareTo(rhs) == -1;
        }

        public static bool operator > (DateRange lhs, DateRange rhs)
        {
            return lhs.CompareTo(rhs) == 1;
        }

        public static bool operator <= (DateRange lhs, DateRange rhs)
        {
            return lhs.CompareTo(rhs) != 1;
        }

        public static bool operator >= (DateRange lhs, DateRange rhs)
        {
            return lhs.CompareTo(rhs) != -1;
        }

        public static DateRange operator | (DateRange lhs, DateRange rhs)
        {
            return lhs.Union(rhs);
        }

        public static DateRange operator & (DateRange lhs, DateRange rhs)
        {
            return lhs.Intersection(rhs);
        }

        #endregion

        #region [ -- Interface implementations -- ]

        public int CompareTo(DateRange other)
        {
            var result = Start.CompareTo(other.Start);
            if (result == 0)
                result = End.CompareTo(other.End);
            return result;
        }

        #endregion

        #region [ -- Overrides -- ]

        public override string ToString()
        {
            return Start.ToString() + " - " + End.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is DateRange other && CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() * 17 + End.GetHashCode();
        }

        #endregion
    }
}
