using System;
using FluentAssertions;
using Option.Extensions;
using Xunit;
// ReSharper disable ExpressionIsAlwaysNull

namespace Option.Test
{
    public class ObjectExtensionTest
    {
        private const string Expected = "foo";

        [Fact]
        public void WhenGivenTrue_ReturnsTypedSome()
        {
            var result = Expected.Given(true);

            result.Should().BeOfType<Some<string>>();
        }

        [Fact]
        public void WhenGivenTrue_ContainsValue()
        {
            var result = Expected.Given(true)
                .ResultOr(() => throw new Exception("Test failed because Given returned None"));

            result.Should().Be(Expected);
        }

        [Fact]
        public void WhenGivenTrue_ButObjectIsNull_ReturnsTypedNone()
        {
            string testObject = null;

            var result = testObject.Given(true);

            result.Should().BeOfType<None<string>>();
        }

        [Fact]
        public void WhenGivenFalse_ReturnsTypedNone()
        {
            var result = Expected.Given(false);

            result.Should().BeOfType<None<string>>();
        }

        [Fact]
        public void WhenGivenTruthyPredicate_ReturnsTypedSome()
        {
            var result = Expected.Given(obj => true);

            result.Should().BeOfType<Some<string>>();
        }

        [Fact]
        public void WhenGivenTruthyPredicate_ContainsValue()
        {
            var result = Expected.Given(obj => true)
                .ResultOr(() => throw new Exception("Test failed because Given returned None"));

            result.Should().Be(Expected);
        }

        [Fact]
        public void WhenGivenTruthyPredicate_ButObjectIsNull_ReturnsTypedNone()
        {
            string testObject = null;

            var result = testObject.Given(obj => true);

            result.Should().BeOfType<None<string>>();
        }

        [Fact]
        public void WhenGivenFalsyPredicate_ReturnsTypedNone()
        {
            var result = Expected.Given(obj => false);

            result.Should().BeOfType<None<string>>();
        }
    }
}