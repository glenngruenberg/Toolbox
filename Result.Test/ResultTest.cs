using System;
using FluentAssertions;
using Xunit;

namespace Result.Test
{
    public class ResultTest
    {
        private const int ExpectedInt = 15;
        private const string ExpectedString = "This is an expected value";

        private const string Error = "error";
        private const string Success = "success";

        private static string ShouldBe(string expected) => $"Result should be {expected}";
        private static string IgnoresOn(string expected) => $"Function should not trigger on {expected}";
        
        [Fact]
        public void Merge_WhenSuccessful_ReturnsSuccessFunctionResult()
        {
            var testee = new Result<int, IError>(ExpectedInt);

            var result = testee.Merge(
                success => success,
                _ => throw new Exception(ShouldBe(Success)));

            result.Should().Be(ExpectedInt);
        }

        [Fact]
        public void Merge_WhenError_ReturnsErrorFunctionResult()
        {
            var testee = new Result<ISuccess, int>(ExpectedInt);

            var result = testee.Merge(
                success => throw new Exception(ShouldBe(Error)),
                error => error);

            result.Should().Be(ExpectedInt);
        }

        [Fact]
        public void OnSuccess_WhenSuccessful_AppliesFunction()
        {
            var testee = new Result<int, IError>(ExpectedInt);
            
            var result = testee.OnSuccess<string>(_ => ExpectedString)
                .Merge(
                    success => success,
                    _ => throw new Exception(ShouldBe(Success)));

            result.Should().Be(ExpectedString);
        }

        [Fact]
        public void OnSuccess_WhenError_DoesNothing()
        {
            var testee = new Result<ISuccess, int>(ExpectedInt);

            var result = testee.OnSuccess<ISuccess>(_ => throw new Exception(IgnoresOn(Error)))
                .Merge(
                    _ => throw new Exception(ShouldBe(Error)),
                    error => error);

            result.Should().Be(ExpectedInt);
        }

        [Fact]
        public void OnSuccess_IsChainable()
        {
            var testee = new Result<int, IError>(ExpectedInt);

            var result = testee.OnSuccess<string>(_ => ExpectedString)
                .OnSuccess<int>(_ => ExpectedInt)
                .Merge(
                    success => success,
                    _ => throw new Exception("Result should be success"));

            result.Should().Be(ExpectedInt);
        }

        [Fact]
        public void OnError_WhenError_AppliesFunction()
        {
            var testee = new Result<ISuccess, int>(ExpectedInt);

            var result = testee.OnError<string>(_ => ExpectedString)
                .Merge(
                    _ => throw new Exception(ShouldBe(Error)),
                    error => error);

            result.Should().Be(ExpectedString);
        }

        [Fact]
        public void OnError_WhenSuccess_DoesNothing()
        {
            var testee = new Result<int, string>(ExpectedInt);

            var result = testee.OnError<int>(_ => throw new Exception(IgnoresOn(Success)))
                .Merge(
                    success => success,
                    _ => throw new Exception(ShouldBe(Success)));

            result.Should().Be(ExpectedInt);
        }

        [Fact]
        public void OnError_IsChainable()
        {
            var testee = new Result<ISuccess, int>(ExpectedInt);

            var result = testee.OnError<string>(_ => ExpectedString)
                .OnError<int>(_ => ExpectedInt)
                .Merge(
                    _ => throw new Exception(ShouldBe(Error)),
                    error => error);

            result.Should().Be(ExpectedInt);
        }

        [Fact]
        public void OnSuccessAndOnError_WhenSuccessful_AreComposable()
        {
            var testee = new Result<int, IError>(ExpectedInt);

            var result = testee.OnSuccess<string>(_ => ExpectedString)
                .OnError<IError>(_ => throw new Exception(IgnoresOn(Success)))
                .OnSuccess<int>(_ => ExpectedInt)
                .Merge(
                    success => success,
                    _ => throw new Exception(ShouldBe(Success)));

            result.Should().Be(ExpectedInt);
        }

        [Fact]
        public void OnSuccessAndOnError_WhenError_AreComposable()
        {
            var testee = new Result<ISuccess, int>(ExpectedInt);

            var result = testee.OnError<string>(_ => ExpectedString)
                .OnSuccess<ISuccess>(_ => throw new Exception(IgnoresOn(Error)))
                .OnError<int>(_ => ExpectedInt)
                .Merge(
                    _ => throw new Exception(ShouldBe(Error)),
                    error => error);

            result.Should().Be(ExpectedInt);
        }

        private interface IError { }

        private interface ISuccess { }
    }
}
