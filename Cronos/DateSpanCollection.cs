
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Cronos
{
    public class DateSpanCollection : IEnumerable<DateSpan>
    {
        readonly List<DateSpan> _content;

        public DateSpanCollection(IEnumerable<DateSpan> values)
        {
            _content = Normalize(values.ToList());
        }

        public DateSpanCollection(params DateSpan[] values)
        {
            _content = Normalize(values.ToList());
        }

        // Private CTOR for use when list has already been normalized
        DateSpanCollection(List<DateSpan> content)
        {
            _content = content;
        }

        public DateSpan this[int index] => _content[index];

        public int Count => _content.Count;

        public DateSpanCollection Union(DateSpanCollection rhs)
        {
            // Easy method, since normalize basically creates union.
            var list = new List<DateSpan>(_content);
            list.AddRange(rhs);
            return new DateSpanCollection(Normalize(list));
        }

        public DateSpanCollection Intersection(DateSpanCollection rhs)
        {
            var list = new List<DateSpan>(_content);
            list.AddRange(rhs._content);
            return new DateSpanCollection(Intersections(list));
        }

        public DateSpanCollection Inverse()
        {
            var list = new List<DateSpan>();
            if (_content.Count > 0)
            {
                if (_content[0].Start != DateTime.MinValue)
                    list.Add(new DateSpan(DateTime.MinValue, _content[0].Start));
                int next = 1;
                foreach (var idx in _content)
                {
                    if (_content.Count > next)
                    {
                        list.Add(new DateSpan(idx.End, _content[next].Start));
                        next += 1;
                    }
                }
                if (_content.Last().End != DateTime.MaxValue)
                    list.Add(new DateSpan(_content.Last().End, DateTime.MaxValue));
            }
            return new DateSpanCollection(list);
        }

        #region [ -- Interface implementation -- ]

        public IEnumerator<DateSpan> GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        #endregion

        #region [ -- Static private helper methods -- ]

        static void Sort(List<DateSpan> list)
        {
            list.Sort((lhs, rhs) => lhs.CompareTo(rhs));
        }

        static List<DateSpan> Normalize(List<DateSpan> list)
        {
            // Sorting first to simplify logic further down
            Sort(list);

            // Merging intersecting items using OR
            var result = new List<DateSpan>();
            int innerFrom = 0;
            var iterator = list.GetEnumerator();
            while(iterator.MoveNext())
            {
                var toAdd = iterator.Current;
                foreach (var idx in list.Skip(++innerFrom))
                {
                    if (!toAdd.Intersects(idx))
                        break;
                    toAdd = toAdd.Union(idx);
                    innerFrom += 1;
                    iterator.MoveNext();
                }
                result.Add(toAdd);
            }
            return result;
        }

        static List<DateSpan> Intersections(List<DateSpan> list)
        {
            // Sorting first to simplify logic further down
            Sort(list);

            // Merging intersecting items using AND
            var result = new List<DateSpan>();
            int innerFrom = 0;
            foreach (var idx in list)
            {
                foreach (var idxInner in list.Skip(++innerFrom))
                {
                    if (!idxInner.Intersects(idx))
                        break;
                    result.Add(idx.Intersection(idxInner));
                }
            }
            return result;
        }

        #endregion
    }
}
