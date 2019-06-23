
using System;

namespace Cronos
{
    public class DateSpan : IComparable<DateSpan>
    {
        public DateSpan(DateTime start, DateTime end)
        {
            if (end <= start)
                throw new ArgumentOutOfRangeException($"{nameof(start)} must be less than {nameof(end)}");

            Start = start;
            End = end;
        }

        public DateTime Start { get; }

        public DateTime End { get; }

        public TimeSpan Size { get { return End - Start; } }

        public bool Intersects(DateSpan rhs)
        {
            return rhs.Start < End && rhs.End > Start;
        }

        public bool Adjacent(DateSpan rhs)
        {
            return rhs.Start == End || rhs.End == Start;
        }

        public DateSpan Union(DateSpan rhs)
        {
            if (!Intersects(rhs) && !Adjacent(rhs))
                throw new ArgumentOutOfRangeException($"{nameof(rhs)} must be adjacent or intersect with the current instance");

            return new DateSpan(Start < rhs.Start ? Start : rhs.Start, End > rhs.End ? End : rhs.End);
        }

        public DateSpan Intersection(DateSpan rhs)
        {
            if (!Intersects(rhs))
                throw new ArgumentOutOfRangeException($"{nameof(rhs)} must intersect with date");

            return new DateSpan(Start < rhs.Start ? rhs.Start : Start, End > rhs.End ? rhs.End : End);
        }

        #region [ -- Overloaded operators -- ]

        public static bool operator < (DateSpan lhs, DateSpan rhs)
        {
            return lhs.CompareTo(rhs) == -1;
        }

        public static bool operator > (DateSpan lhs, DateSpan rhs)
        {
            return lhs.CompareTo(rhs) == 1;
        }

        public static bool operator == (DateSpan lhs, DateSpan rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator != (DateSpan lhs, DateSpan rhs)
        {
            return !lhs.Equals(rhs);
        }

        #endregion

        #region [ -- Interface implementations -- ]

        public int CompareTo(DateSpan other)
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
            return obj is DateSpan other && CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() * 17 + End.GetHashCode();
        }

        #endregion
    }
}
