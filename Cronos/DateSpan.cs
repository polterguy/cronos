
using System;

namespace Cronos
{
    public struct DateSpan
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

        public DateSpan Union(DateSpan rhs)
        {
            if (!Intersects(rhs))
                throw new ArgumentOutOfRangeException($"{nameof(rhs)} must intersect with date");

            return new DateSpan(Start < rhs.Start ? Start : rhs.Start, End > rhs.End ? End : rhs.End);
        }

        public DateSpan Intersection(DateSpan rhs)
        {
            if (!Intersects(rhs))
                throw new ArgumentOutOfRangeException($"{nameof(rhs)} must intersect with date");

            return new DateSpan(Start > rhs.Start ? Start : rhs.Start, End < rhs.End ? End : rhs.End);
        }
    }
}
