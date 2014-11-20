namespace ReproTestIssue
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class IntTests : GenericBase<int>
    {
        [Test]
        public void IntTest()
        {
            Assert.Pass();
        }
    }

    [TestFixture]
    public class DoubleTests : GenericBase<double>
    {
        [Test]
        public void DoubleTest()
        {
            Assert.Pass();
        }
    }

    [TestFixture]
    public abstract class GenericBase<T> : Base
        where T : IComparable<T>
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TestCase(1, 2, -1)]
        public void Compare(T first, T second, T expected)
        {
            Assert.AreEqual(expected, first.CompareTo(second));
        }
    }

    public abstract class Base
    {
        [Test]
        public void Meh()
        {
            Assert.Pass();
        }
    }
}
