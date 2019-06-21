
using System;
using Xunit;
using Cronos;

namespace CronosTests
{
    public class DateSpanTests
    {
        [Fact]
        public void Constructor_01()
        {
            var now = DateTime.Now;
            var sp = new DateSpan(now, now.AddHours(1));
            Assert.Equal(now, sp.Start);
            Assert.Equal(now.AddHours(1), sp.End);
        }

        [Fact]
        public void Constructor_Throws_01()
        {
            var now = DateTime.Now;
            Assert.Throws<ArgumentOutOfRangeException>(() => new DateSpan(now, now.AddHours(-1)));
        }

        [Fact]
        public void Intersects_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(2));
            var sp2 = new DateSpan(now.AddHours(1), now.AddHours(3));
            Assert.True(sp1.Intersects(sp2));
        }

        [Fact]
        public void Intersects_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(2));
            var sp2 = new DateSpan(now.AddHours(-1), now.AddHours(1));
            Assert.True(sp1.Intersects(sp2));
        }

        [Fact]
        public void Intersects_03()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(2));
            var sp2 = new DateSpan(now.AddHours(-1), now.AddHours(3));
            Assert.True(sp1.Intersects(sp2));
        }

        [Fact]
        public void Intersects_04()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(1), now.AddHours(2));
            Assert.True(sp1.Intersects(sp2));
        }

        [Fact]
        public void Intersects_05()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(5), now.AddHours(7));
            Assert.False(sp1.Intersects(sp2));
        }

        [Fact]
        public void Union_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(1), now.AddHours(2));
            var result = sp1.Union(sp2);
            Assert.Equal(now, result.Start);
            Assert.Equal(now.AddHours(3), result.End);
        }

        [Fact]
        public void Union_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(1), now.AddHours(4));
            var result = sp1.Union(sp2);
            Assert.Equal(now, result.Start);
            Assert.Equal(now.AddHours(4), result.End);
        }

        [Fact]
        public void Union_03()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(-1), now.AddHours(2));
            var result = sp1.Union(sp2);
            Assert.Equal(now.AddHours(-1), result.Start);
            Assert.Equal(now.AddHours(3), result.End);
        }

        [Fact]
        public void Union_04()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(-1), now.AddHours(5));
            var result = sp1.Union(sp2);
            Assert.Equal(now.AddHours(-1), result.Start);
            Assert.Equal(now.AddHours(5), result.End);
        }

        [Fact]
        public void Union_Throws_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(5), now.AddHours(7));
            Assert.Throws<ArgumentOutOfRangeException>(() => sp1.Union(sp2));
        }

        [Fact]
        public void Intersection_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(-1), now.AddHours(2));
            var result = sp1.Intersection(sp2);
            Assert.Equal(now, result.Start);
            Assert.Equal(now.AddHours(2), result.End);
        }

        [Fact]
        public void Intersection_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(1), now.AddHours(5));
            var result = sp1.Intersection(sp2);
            Assert.Equal(now.AddHours(1), result.Start);
            Assert.Equal(now.AddHours(3), result.End);
        }

        [Fact]
        public void Intersection_Throws_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(4), now.AddHours(7));
            Assert.Throws<ArgumentOutOfRangeException>(() => sp1.Intersection(sp2));
        }

        [Fact]
        public void Intersection_Throws_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(-4), now.AddHours(-2));
            Assert.Throws<ArgumentOutOfRangeException>(() => sp1.Intersection(sp2));
        }
    }
}
