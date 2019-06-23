
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Cronos
{
    public class DateRangeCollection : IEnumerable<DateRange>
    {
        readonly List<DateRange> _content;

        public DateRangeCollection(IEnumerable<DateRange> values)
            : this (Normalize(values.ToList()))
        { }

        public DateRangeCollection(params DateRange[] values)
            : this (Normalize(values.ToList()))
        { }

        DateRangeCollection(List<DateRange> content)
        {
            _content = content;
        }

        public DateRange this[int index] => _content[index];

        public int Count => _content.Count;

        public TimeSpan Size => new TimeSpan(_content.Sum(x => (x.End - x.Start).Ticks));

        #region [ -- Algebraic helper methods -- ]

        public DateRangeCollection Union(DateRangeCollection rhs)
        {
            /*
             * Simple case, since Normalize basically creates a union.
             * Hence, we don't really need any implementation, beyond
             * whatever logic the CTOR of the class contains, besides
             * from making sure we pass in both sides' content to the CTOR.
             */
            var list = new List<DateRange>(_content);
            list.AddRange(rhs);
            return new DateRangeCollection(Normalize(list));
        }

        public DateRangeCollection Intersection(DateRangeCollection rhs)
        {
            // Buffer used to hold our result.
            var list = new List<DateRange>();

            /*
             * The iterator for the this instance, which is used to enumerate
             * all the instances in the this object.
             */
            var selfIterator = GetEnumerator();

            /*
             * The iterator for the rhs (argument) collection.
             * Notice, these iterators are incremented independently,
             * in such a way that each relevant instance in each
            * collection is compared for intersection towards the other
            * party's relevant instance(s).
             *
             * This algorithm assumes that both sides are sorted lists,
             * and normalized, which shouldn't be a problem, since creating
             * an instance of a DateTimeCollection is impossible without
             * also making sure it's normalized.
             */
            var otherIterator = rhs.GetEnumerator();

            /*
             * Verifying both sides have instances, since if not, the
             * obvious result would be an empty collection
             */
            if (selfIterator.MoveNext() && otherIterator.MoveNext())
            {
                /*
                 * The body of our algorithm, which increments the correct
                 * iterator, to make sure we only compare relevant instances.
                 */
                while (true)
                {
                    var selfCurrent = selfIterator.Current;
                    var otherCurrent = otherIterator.Current;

                    // Checking for intersection.
                    if (selfCurrent.Intersects(otherCurrent))
                        list.Add(selfCurrent.Intersection(otherCurrent));

                    /*
                     * Which iterator we advance depends upon which item
                     * has the largest End date, to make sure consecutive relevant
                     * instances are compared towards all relevant instances
                     * in the other collection.
                     *
                     * If any of the iterators yields no more results, we are
                     * done, and no more additions are possible.
                     */
                    if (selfCurrent.End > otherCurrent.End)
                    {
                        if (!otherIterator.MoveNext())
                            break;
                    }
                    else
                    {
                        if(!selfIterator.MoveNext())
                            break;
                    }
                }
            }

            /*
             * Our end result, passed into our private CTOR, which assumes
             * the items are already normalized.
             */
            return new DateRangeCollection(list);
        }

        public DateRangeCollection Inverse(bool edgeValues = true)
        {
            // Buffer to hold our result.
            var list = new List<DateRange>();

            // Making sure we actually have anything to inverse here.
            if (_content.Count > 0)
            {
                // Adding "start edge value" if we should
                if (edgeValues && _content[0].Start != DateTime.MinValue)
                    list.Add(new DateRange(DateTime.MinValue, _content[0].Start));

                // Used to figure out the next item in our list.
                int next = 0;
                foreach (var idx in _content)
                {
                    if (_content.Count > ++next)
                        list.Add(new DateRange(idx.End, _content[next].Start));
                }

                // Adding "end edge value" if we should
                if (_content[_content.Count - 1].End != DateTime.MaxValue)
                    list.Add(new DateRange(_content[_content.Count - 1].End, DateTime.MaxValue));
            }
            else
            {
                // Nothing to reverse, hence creating a datespan ranging all possible dates.
                list.Add(new DateRange(DateTime.MinValue, DateTime.MaxValue));
            }
            return new DateRangeCollection(list);
        }

        #endregion

        #region [ -- Overloaded operators -- ]

        public static DateRangeCollection operator | (DateRangeCollection lhs, DateRangeCollection rhs)
        {
            return lhs.Union(rhs);
        }

        public static DateRangeCollection operator & (DateRangeCollection lhs, DateRangeCollection rhs)
        {
            return lhs.Intersection(rhs);
        }

        public static DateRangeCollection operator ! (DateRangeCollection self)
        {
            return self.Inverse();
        }

        #endregion

        #region [ -- Interface implementation -- ]

        public IEnumerator<DateRange> GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        #endregion

        #region [ -- Private helper methods -- ]

        static void Sort(List<DateRange> list)
        {
            list.Sort((lhs, rhs) => lhs.CompareTo(rhs));
        }

        static List<DateRange> Normalize(List<DateRange> list)
        {
            /*
             * Sorting first to simplify logic further down.
             * Notice, without sorting the specified list first,
             * the big O cost of this algorithm would be significantly
             * more expensive. By sorting the list first, we can get away
             * with a big O cost of O(n).
             */
            Sort(list);

            // Return value.
            var result = new List<DateRange>();

            /*
             * Inner iterator, the one we're comparing the outer with.
             * Notice, we have to advance it initially to avoid comparing
             * the same instances to each other.
             */
            var innerIterator = list.GetEnumerator();
            innerIterator.MoveNext();

            /*
             * Our main iterator, the one we're comparing our inner iterator's
             * values with. The inner iterator loops through all values in our
             * specified list.
             */
            var outerIterator = list.GetEnumerator();
            while(outerIterator.MoveNext())
            {
                /*
                 * The item we should add, which might change depending upon
                 * whether or not the inner iterator's value intersects with it.
                 */
                var current = outerIterator.Current;
                while (innerIterator.MoveNext())
                {
                    var inner = innerIterator.Current;
                    if (!current.Intersects(inner))
                        break; // No intersection.

                    /*
                     * Items are intersecting.
                     * Notice, since we have handled the item in front of
                     * the outer iterator's current position, we must also advance
                     * the outer iterator's current position, to make sure the outer
                     * iterator is always exactly one step behind the inner iterator.
                     */
                    current = current.Union(inner);
                    outerIterator.MoveNext();
                }
                result.Add(current);
            }
            return result;
        }

        #endregion
    }
}
