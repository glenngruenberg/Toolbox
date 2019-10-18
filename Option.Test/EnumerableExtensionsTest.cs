using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Option.Extensions;
using Option.Test.Helpers;
using Xunit;

namespace Option.Test
{
    public class EnumerableExtensionsTest
    {
        private const string Expected = "foo";

        [Fact]
        public void FirstOrNoneWithFirst_ReturnsTypedSome()
        {
            var list = new List<string> { Expected };

            var result = list.FirstOrNone();

            result.Should().BeOfType<Some<string>>();
        }

        [Fact]
        public void FirstOrNoneWithFirst_ContainsValue()
        {
            var list = new List<string> { Expected };

            var result = list.FirstOrNone()
                .ResultOr(() => throw new Exception("Test failed because FirstOrNone returned None"));

            result.Should().Be(Expected);
        }

        [Fact]
        public void FirstOrNoneOnEmpty_ReturnsNone()
        {
            var result = new List<string>().FirstOrNone();

            result.Should().BeOfType<None<string>>();
        }

        [Fact]
        public void FirstOrNoneWithFirst_WhenFirstIsNull_ReturnsNone()
        {
            var list = new List<string> { null };

            var result = list.FirstOrNone();

            result.Should().BeOfType<None<string>>();
        }

        [Fact]
        public void FirstOrNonePredicateWithFirst_ReturnsTypedSome()
        {
            var notThisOne = new TestObject { Option = null };
            var thatOne = new TestObject();
            var list = new List<TestObject> { notThisOne, thatOne };

            var result = list.FirstOrNone(x => x.Option is Some<TestProperty>);

            result.Should().BeOfType<Some<TestObject>>();
        }

        [Fact]
        public void FirstOrNonePredicateWithFirst_ContainsCorrectValue()
        {
            TestProperty nullProperty = null;
            var notThisOne = new TestObject { Option = nullProperty };
            var thatOne = new TestObject();
            var list = new List<TestObject> { notThisOne, thatOne };

            var result = list.FirstOrNone(x => x.Option is Some<TestProperty>)
                .ResultOr(() => throw new Exception("Test failed because FirstOrNone returned None"));

            result.Should().Be(thatOne);
        }

        [Fact]
        public void FirstOrNonePredicateOnEmpty_ReturnsNone()
        {
            TestProperty nullProperty = null;
            var notThisOne = new TestObject { Option = nullProperty };
            var list = new List<TestObject> { notThisOne };

            var result = list.FirstOrNone(x => x.Option is Some<TestProperty>);

            result.Should().BeOfType<None<TestObject>>();
        }

        [Fact]
        public void FirstOrNonePredicateWithFirst_WhenContainsNull_ContainsCorrectValue()
        {
            var testObject = new TestObject();
            var list = new List<TestObject> { null, testObject };

            var result = list.FirstOrNone(x => x.Option is Some<TestProperty>)
                .ResultOr(() => throw new Exception("Test failed because FirstOrNone returned None"));

            result.Should().Be(testObject);
        }

        [Fact]
        public void SelectSome_ReturnsCorrectType()
        {
            var list = new List<TestObject> { new TestObject() };

            var result = list.SelectSome(x => x.Option).First();

            result.Should().BeOfType<TestProperty>();
        }

        [Fact]
        public void SelectSome_ReturnsCorrectValue()
        {
            var testProperty = new TestProperty();
            var list = new List<TestObject> { new TestObject { Option = testProperty } };

            var result = list.SelectSome(x => x.Option).First();

            result.Should().Be(testProperty);
        }

        [Fact]
        public void SelectSome_FiltersOutNones()
        {
            TestProperty nullProperty = null;
            var notThisOne = new TestObject { Option = nullProperty };
            var list = new List<TestObject> { notThisOne, new TestObject() };

            var result = list.SelectSome(x => x.Option);

            result.Should().ContainSingle();
        }
    }
}