
using System;
using System.Linq;
using Xunit;
using Cronos;

namespace CronosTests
{
    public class DateSpanTestsCollection
    {
        [Fact]
        public void Constructor_01()
        {
            var now = DateTime.Now;
            var c = new DateSpanCollection(
                new DateSpan(now.AddHours(1), now.AddHours(2)),
                new DateSpan(now.AddHours(3), now.AddHours(4)));
            Assert.Equal(2, c.Count);
            Assert.Equal(now.AddHours(1), c.First().Start);
            Assert.Equal(now.AddHours(2), c.First().End);
            Assert.Equal(now.AddHours(3), c.Last().Start);
            Assert.Equal(now.AddHours(4), c.Last().End);
        }

        [Fact]
        public void Constructor_02()
        {
            var now = DateTime.Now;
            var c = new DateSpanCollection(
                new DateSpan(now.AddHours(3), now.AddHours(5)),
                new DateSpan(now.AddHours(4), now.AddHours(7)));
            Assert.Single(c);
            Assert.Equal(now.AddHours(3), c.First().Start);
            Assert.Equal(now.AddHours(7), c.First().End);
        }

        [Fact]
        public void Constructor_03()
        {
            var now = DateTime.Now;
            var c = new DateSpanCollection(
                new DateSpan(now.AddHours(3), now.AddHours(5)),
                new DateSpan(now.AddHours(4), now.AddHours(7)),
                new DateSpan(now.AddHours(10), now.AddHours(12)),
                new DateSpan(now.AddHours(11), now.AddHours(14)));
            Assert.Equal(2, c.Count);
            Assert.Equal(now.AddHours(3), c.First().Start);
            Assert.Equal(now.AddHours(7), c.First().End);
            Assert.Equal(now.AddHours(10), c.Last().Start);
            Assert.Equal(now.AddHours(14), c.Last().End);
        }

        [Fact]
        public void Constructor_04()
        {
            var now = DateTime.Now;
            var c = new DateSpanCollection(new DateSpan(now.AddHours(3), now.AddHours(5)));
            Assert.Single(c);
        }

        [Fact]
        public void Sum_01()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddHours(1), now.AddHours(3)),
                new DateSpan(now.AddHours(5), now.AddHours(7)));
            Assert.Equal(new TimeSpan(4,0,0), c1.Size);
        }

        [Fact]
        public void Union_01()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddHours(1), now.AddHours(3)),
                new DateSpan(now.AddHours(5), now.AddHours(7)));
            var c2 = new DateSpanCollection(
                new DateSpan(now.AddHours(9), now.AddHours(11)),
                new DateSpan(now.AddHours(13), now.AddHours(15)));
            var result = c1.Union(c2);
            Assert.Equal(4, result.Count);
            Assert.Equal(now.AddHours(1), result.First().Start);
            Assert.Equal(now.AddHours(3), result.First().End);
            Assert.Equal(now.AddHours(5), result[1].Start);
            Assert.Equal(now.AddHours(7), result[1].End);
            Assert.Equal(now.AddHours(9), result[2].Start);
            Assert.Equal(now.AddHours(11), result[2].End);
            Assert.Equal(now.AddHours(13), result[3].Start);
            Assert.Equal(now.AddHours(15), result[3].End);
        }

        [Fact]
        public void Union_02()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddHours(1), now.AddHours(6)),
                new DateSpan(now.AddHours(5), now.AddHours(7)));
            var c2 = new DateSpanCollection(
                new DateSpan(now.AddHours(9), now.AddHours(11)),
                new DateSpan(now.AddHours(13), now.AddHours(15)));
            var result = c1.Union(c2);
            Assert.Equal(3, result.Count);
            Assert.Equal(now.AddHours(1), result.First().Start);
            Assert.Equal(now.AddHours(7), result.First().End);
            Assert.Equal(now.AddHours(9), result[1].Start);
            Assert.Equal(now.AddHours(11), result[1].End);
            Assert.Equal(now.AddHours(13), result[2].Start);
            Assert.Equal(now.AddHours(15), result[2].End);
        }

        [Fact]
        public void Union_03()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddHours(1), now.AddHours(3)),
                new DateSpan(now.AddHours(5), now.AddHours(7)));
            var c2 = new DateSpanCollection(
                new DateSpan(now.AddHours(6), now.AddHours(11)),
                new DateSpan(now.AddHours(13), now.AddHours(15)));
            var result = c1.Union(c2);
            Assert.Equal(3, result.Count);
            Assert.Equal(now.AddHours(1), result.First().Start);
            Assert.Equal(now.AddHours(3), result.First().End);
            Assert.Equal(now.AddHours(5), result[1].Start);
            Assert.Equal(now.AddHours(11), result[1].End);
            Assert.Equal(now.AddHours(13), result[2].Start);
            Assert.Equal(now.AddHours(15), result[2].End);
        }

        [Fact]
        public void Union_04()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddHours(1), now.AddHours(40)));
            var c2 = new DateSpanCollection(
                new DateSpan(now.AddHours(2), now.AddHours(4)),
                new DateSpan(now.AddHours(5), now.AddHours(6)),
                new DateSpan(now.AddHours(8), now.AddHours(10)));
            var result = c1.Union(c2);
            Assert.Single(result);
            Assert.Equal(now.AddHours(1), result.First().Start);
            Assert.Equal(now.AddHours(40), result.First().End);
        }

        [Fact]
        public void Intersection_01()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddHours(1), now.AddHours(3)),
                new DateSpan(now.AddHours(5), now.AddHours(7)));
            var c2 = new DateSpanCollection(
                new DateSpan(now.AddHours(6), now.AddHours(11)),
                new DateSpan(now.AddHours(13), now.AddHours(15)));
            var result = c1.Intersection(c2);
            Assert.Single(result);
            Assert.Equal(now.AddHours(6), result.First().Start);
            Assert.Equal(now.AddHours(7), result.First().End);
        }

        [Fact]
        public void Intersection_02()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddHours(1), now.AddHours(4)),
                new DateSpan(now.AddHours(5), now.AddHours(8)));
            var c2 = new DateSpanCollection(
                new DateSpan(now.AddHours(2), now.AddHours(3)),
                new DateSpan(now.AddHours(6), now.AddHours(7)));
            var result = c1.Intersection(c2);
            Assert.Equal(2, result.Count);
            Assert.Equal(now.AddHours(2), result[0].Start);
            Assert.Equal(now.AddHours(3), result[0].End);
            Assert.Equal(now.AddHours(6), result[1].Start);
            Assert.Equal(now.AddHours(7), result[1].End);
        }

        [Fact]
        public void Intersection_03()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddMinutes(1), now.AddMinutes(10)));
            var c2 = new DateSpanCollection(
                new DateSpan(now.AddMinutes(2), now.AddMinutes(3)),
                new DateSpan(now.AddMinutes(6), now.AddMinutes(7)));
            var result = c1.Intersection(c2);
            Assert.Equal(2, result.Count);
            Assert.Equal(now.AddMinutes(2), result[0].Start);
            Assert.Equal(now.AddMinutes(3), result[0].End);
            Assert.Equal(now.AddMinutes(6), result[1].Start);
            Assert.Equal(now.AddMinutes(7), result[1].End);
        }

        [Fact]
        public void Intersection_04()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddMinutes(1), now.AddMinutes(10)));
            var c2 = new DateSpanCollection(
                new DateSpan(now.AddMinutes(2), now.AddMinutes(3)),
                new DateSpan(now.AddMinutes(4), now.AddMinutes(5)),
                new DateSpan(now.AddMinutes(6), now.AddMinutes(7)));
            var result = c1.Intersection(c2);
            Assert.Equal(3, result.Count);
            Assert.Equal(now.AddMinutes(2), result[0].Start);
            Assert.Equal(now.AddMinutes(3), result[0].End);
            Assert.Equal(now.AddMinutes(4), result[1].Start);
            Assert.Equal(now.AddMinutes(5), result[1].End);
            Assert.Equal(now.AddMinutes(6), result[2].Start);
            Assert.Equal(now.AddMinutes(7), result[2].End);
        }

        [Fact]
        public void Intersection_05()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddMinutes(5), now.AddMinutes(50)));
            var c2 = new DateSpanCollection(
                new DateSpan(now.AddMinutes(2), now.AddMinutes(10)),
                new DateSpan(now.AddMinutes(12), now.AddMinutes(20)));
            var result = c1.Intersection(c2);
            Assert.Equal(2, result.Count);
            Assert.Equal(now.AddMinutes(5), result[0].Start);
            Assert.Equal(now.AddMinutes(10), result[0].End);
            Assert.Equal(now.AddMinutes(12), result[1].Start);
            Assert.Equal(now.AddMinutes(20), result[1].End);
        }

        [Fact]
        public void Intersection_06()
        {
            var now = DateTime.Now;
            var c1 = new DateSpanCollection(
                new DateSpan(now.AddMinutes(5), now.AddMinutes(10)));
            var c2 = new DateSpanCollection(
                new DateSpan(now.AddMinutes(12), now.AddMinutes(20)),
                new DateSpan(now.AddMinutes(22), now.AddMinutes(30)));
            var result = c1.Intersection(c2);
            Assert.Empty(result);
        }

        [Fact]
        public void Inverse_02()
        {
            var now = DateTime.Now;
            var c = new DateSpanCollection(
                new DateSpan(now.AddMinutes(12), now.AddMinutes(20)),
                new DateSpan(now.AddMinutes(22), now.AddMinutes(30)));
            var result = c.Inverse();
            Assert.Equal(3, result.Count);
            Assert.Equal(now.AddMinutes(20), result[1].Start);
            Assert.Equal(now.AddMinutes(22), result[1].End);
            var result2 = result.Inverse();
            Assert.Equal(2, result2.Count);
            Assert.Equal(now.AddMinutes(12), result2[0].Start);
            Assert.Equal(now.AddMinutes(20), result2[0].End);
            Assert.Equal(now.AddMinutes(22), result2[1].Start);
            Assert.Equal(now.AddMinutes(30), result2[1].End);
        }
    }
}
