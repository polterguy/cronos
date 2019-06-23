
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
            Assert.Throws<ArgumentOutOfRangeException>(() => new DateSpan(now, now));
        }

        [Fact]
        public void Constructor_Throws_02()
        {
            var now = DateTime.Now;
            Assert.Throws<ArgumentOutOfRangeException>(() => new DateSpan(now, now.AddHours(-1)));
        }

        [Fact]
        public void Size()
        {
            var now = DateTime.Now;
            var sp = new DateSpan(now.AddHours(2), now.AddHours(5));
            Assert.Equal(new TimeSpan(3,0,0), sp.Size);
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
        public void Intersects_06()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(-5), now.AddHours(-1));
            Assert.False(sp1.Intersects(sp2));
        }

        [Fact]
        public void Intersects_07()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(3));
            Assert.True(sp1.Intersects(sp2));
        }

        [Fact]
        public void Adjacent_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(3), now.AddHours(5));
            Assert.True(sp1.Adjacent(sp2));
        }

        [Fact]
        public void Adjacent_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(-3), now);
            Assert.True(sp1.Adjacent(sp2));
        }

        [Fact]
        public void Adjacent_03()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(3));
            Assert.False(sp1.Adjacent(sp2));
        }

        [Fact]
        public void Adjacent_04()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(4), now.AddHours(7));
            Assert.False(sp1.Adjacent(sp2));
        }

        [Fact]
        public void Adjacent_05()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(2), now.AddHours(5));
            Assert.False(sp1.Adjacent(sp2));
        }

        [Fact]
        public void Adjacent_06()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(-5), now.AddHours(-1));
            Assert.False(sp1.Adjacent(sp2));
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
        public void Union_05()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(3), now.AddHours(5));
            var result = sp1.Union(sp2);
            Assert.Equal(now, result.Start);
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
        public void Union_Throws_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(-5), now.AddHours(-1));
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

        [Fact]
        public void Operator_eq_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(5));
            var sp2 = new DateSpan(now, now.AddHours(5));
            Assert.True(sp1 == sp2);
        }

        [Fact]
        public void Operator_eq_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(5));
            var sp2 = new DateSpan(now, now.AddHours(3));
            Assert.False(sp1 == sp2);
        }

        [Fact]
        public void Operator_eq_03()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(5));
            Assert.False(sp1 == sp2);
        }

        [Fact]
        public void Operator_eq_04()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now.AddHours(1), now.AddHours(5));
            var sp2 = new DateSpan(now.AddHours(2), now.AddHours(5));
            Assert.False(sp1 == sp2);
        }

        [Fact]
        public void Operator_eq_05()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now.AddHours(2), now.AddHours(5));
            var sp2 = new DateSpan(now.AddHours(1), now.AddHours(5));
            Assert.False(sp1 == sp2);
        }

        [Fact]
        public void Operator_neq_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(5));
            var sp2 = new DateSpan(now, now.AddHours(5));
            Assert.False(sp1 != sp2);
        }

        [Fact]
        public void Operator_neq_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(5));
            var sp2 = new DateSpan(now, now.AddHours(3));
            Assert.True(sp1 != sp2);
        }

        [Fact]
        public void Operator_neq_03()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(5));
            Assert.True(sp1 != sp2);
        }

        [Fact]
        public void Operator_neq_04()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now.AddHours(1), now.AddHours(5));
            var sp2 = new DateSpan(now.AddHours(2), now.AddHours(5));
            Assert.True(sp1 != sp2);
        }

        [Fact]
        public void Operator_neq_05()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now.AddHours(2), now.AddHours(5));
            var sp2 = new DateSpan(now.AddHours(1), now.AddHours(5));
            Assert.True(sp1 != sp2);
        }

        [Fact]
        public void Operator_lt_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(5), now.AddHours(7));
            Assert.True(sp1 < sp2);
            Assert.False(sp2 < sp1);
        }

        [Fact]
        public void Operator_lt_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(-5), now.AddHours(-2));
            Assert.False(sp1 < sp2);
            Assert.True(sp2 < sp1);
        }

        [Fact]
        public void Operator_lt_03()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(3));
            Assert.False(sp1 < sp2);
        }

        [Fact]
        public void Operator_lt_04()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(2));
            Assert.False(sp1 < sp2);
            Assert.True(sp2 < sp1);
        }

        [Fact]
        public void Operator_lt_05()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(5));
            Assert.True(sp1 < sp2);
            Assert.False(sp2 < sp1);
        }

        [Fact]
        public void Operator_gt_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now.AddHours(3), now.AddHours(5));
            var sp2 = new DateSpan(now.AddHours(1), now.AddHours(2));
            Assert.True(sp1 > sp2);
            Assert.False(sp2 > sp1);
        }

        [Fact]
        public void Operator_gt_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now.AddHours(2), now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(4), now.AddHours(6));
            Assert.False(sp1 > sp2);
            Assert.True(sp2 > sp1);
        }

        [Fact]
        public void Operator_gt_03()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(3));
            Assert.False(sp1 > sp2);
        }

        [Fact]
        public void Operator_gt_04()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(2));
            var sp2 = new DateSpan(now, now.AddHours(3));
            Assert.False(sp1 > sp2);
            Assert.True(sp2 > sp1);
        }

        [Fact]
        public void Operator_gt_05()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(5));
            var sp2 = new DateSpan(now, now.AddHours(3));
            Assert.True(sp1 > sp2);
            Assert.False(sp2 > sp1);
        }

        [Fact]
        public void Operator_lt_eq_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(5), now.AddHours(7));
            Assert.True(sp1 <= sp2);
            Assert.False(sp2 <= sp1);
        }

        [Fact]
        public void Operator_lt_eq_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(3));
            Assert.True(sp1 <= sp2);
        }

        [Fact]
        public void Operator_lt_eq_03()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(2));
            Assert.False(sp1 <= sp2);
            Assert.True(sp2 <= sp1);
        }

        [Fact]
        public void Operator_lt_eq_04()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(5));
            Assert.True(sp1 <= sp2);
            Assert.False(sp2 <= sp1);
        }

        [Fact]
        public void Operator_mt_eq_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now.AddHours(5), now.AddHours(7));
            Assert.False(sp1 >= sp2);
            Assert.True(sp2 >= sp1);
        }

        [Fact]
        public void Operator_mt_eq_02()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(3));
            Assert.True(sp1 >= sp2);
        }

        [Fact]
        public void Operator_mt_eq_03()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(2));
            Assert.True(sp1 >= sp2);
            Assert.False(sp2 >= sp1);
        }

        [Fact]
        public void Operator_mt_eq_04()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now, now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(5));
            Assert.False(sp1 >= sp2);
            Assert.True(sp2 >= sp1);
        }

        [Fact]
        public void Operator_OR_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now.AddHours(1), now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(5));
            Assert.Equal(sp1.Union(sp2), sp1 | sp2);
        }

        [Fact]
        public void Operator_AND_01()
        {
            var now = DateTime.Now;
            var sp1 = new DateSpan(now.AddHours(1), now.AddHours(3));
            var sp2 = new DateSpan(now, now.AddHours(5));
            Assert.Equal(sp1.Intersection(sp2), sp1 & sp2);
        }
    }
}
