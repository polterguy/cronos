
using System;

namespace Cronos
{
    public class DateSpan : IComparable
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

        public bool Intersects(DateSpan rhs)
        {
            return rhs.Start < End && rhs.End > Start;
        }

        public bool Adjacent(DateSpan rhs)
        {
            return !Intersects(rhs) && (rhs.Start == End || rhs.End == Start);
        }

        public DateSpan Union(DateSpan rhs)
        {
            if (!Intersects(rhs) && !Adjacent(rhs))
                throw new ArgumentOutOfRangeException($"{nameof(rhs)} must intersect with date");

            return new DateSpan(Start < rhs.Start ? Start : rhs.Start, End > rhs.End ? End : rhs.End);
        }

        public DateSpan Intersection(DateSpan rhs)
        {
            if (!Intersects(rhs))
                throw new ArgumentOutOfRangeException($"{nameof(rhs)} must intersect with date");

            return new DateSpan(Start < rhs.Start ? rhs.Start : Start, End > rhs.End ? rhs.End : End);
        }

        public static bool operator < (DateSpan lhs, DateSpan rhs)
        {
            return lhs.CompareTo(rhs) == -1;
        }

        public static bool operator > (DateSpan lhs, DateSpan rhs)
        {
            return lhs.CompareTo(rhs) == 1;
        }

        #region [ -- Interface implementations -- ]

        public int CompareTo(object obj)
        {
            if (!(obj is DateSpan rhs))
                throw new ArgumentException($"Cannot compare a DateSpan to '{obj}'");

            var result = Start.CompareTo(rhs.Start);
            if (result == 0)
                result = End.CompareTo(rhs.End);
            return result;
        }

        #endregion
    }
}
