using System.Collections.Generic;
using FluentAssertions;
using Option.Extensions;
using Xunit;

namespace Option.Test
{
    public class DictionaryExtensionsTest
    {
        private const int First = 1;
        private const string Expected = "foo";
        private const string Fallback = "bar";

        [Fact]
        public void TryGetValue_WhenKeyInDict_ReturnsTypedSome()
        {
            var dict = new Dictionary<int, string> { { First, Expected } };

            var result = dict.TryGetValue(First);

            result.Should().BeOfType<Some<string>>();
        }

        [Fact]
        public void TryGetValue_WhenKeyInDict_ReturnedSomeContainsValue()
        {
            var dict = new Dictionary<int, string> { { First, Expected } };

            var result = dict.TryGetValue(First).ResultOr(Fallback);

            result.Should().Be(Expected);
        }

        [Fact]
        public void TryGetValue_WhenValueIsNull_ReturnsTypedNone()
        {
            var dict = new Dictionary<int, string> { { First, null } };

            var result = dict.TryGetValue(First);

            result.Should().BeOfType<None<string>>();
        }

        [Fact]
        public void TryGetValue_WhenKeyNotInDict_ReturnsTypedNone()
        {
            var dict = new Dictionary<int, string>();

            var result = dict.TryGetValue(First);

            result.Should().BeOfType<None<string>>();
        }
    }
}