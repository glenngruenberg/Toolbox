using System;
using FluentAssertions;
using Option.Test.Helpers;
using Xunit;
// ReSharper disable ExpressionIsAlwaysNull

namespace Option.Test
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class OptionTest
    {
        private const string Expected = "foo";
        private const string Fallback = "bar";

        public class InitializationTest
        {
            [Fact]
            public void WhenInitializingOption_WithValue_IsSome()
            {
                Option<string> some = Expected;

                some.Should().BeOfType<Some<string>>();
            }

            [Fact]
            public void WhenInitializingOption_WithValue_ContainsValue()
            {
                Option<string> some = Expected;

                ((Some<string>)some).Content.Should().Be(Expected);
            }

            [Fact]
            public void WhenInitializingOption_WithValue_AndUsingImplicitCast_ContainsValue()
            {
                Option<string> some = Expected;
                string result = some as Some<string>;

                result.Should().Be(Expected);
            }

            [Fact]
            public void WhenInitializingOption_WithNone_IsTypedNone()
            {
                Option<string> none = None.Value;

                none.Should().BeOfType<None<string>>();
            }

            [Fact]
            public void WhenInitializingOption_WithNullReference_IsTypedNone()
            {
                TestObject nullObject = null;

                Option<TestObject> none = nullObject;

                none.Should().BeOfType<None<TestObject>>();
            }
        }

        public class ApplyTest
        {
            [Fact]
            public void Apply_OnSome_ReturnsResultTypeSome()
            {
                Option<TestObject> some = new TestObject();

                var result = some.Apply(obj => obj.Property);

                result.Should().BeOfType<Some<TestProperty>>();
            }

            [Fact]
            public void Apply_OnSome_ResultContainsValue()
            {
                var property = new TestProperty();
                var testObject = new TestObject { Property = property };
                Option<TestObject> some = testObject;

                var result = some.Apply(obj => obj.Property);

                ((Some<TestProperty>)result).Content.Should().Be(property);
            }

            [Fact]
            public void Apply_OnSome_WhenUsingImplicitCast_ContainsValue()
            {
                var property = new TestProperty();
                var testObject = new TestObject { Property = property };
                Option<TestObject> some = testObject;

                TestProperty result =
                    some.Apply(obj => obj.Property) as Some<TestProperty>;

                result.Should().Be(property);
            }

            [Fact]
            public void Apply_OnSome_WhenResultIsNullReference_IsTypedNone()
            {
                Option<TestObject> some = new TestObject();

                var result = some.Apply<TestObject>(_ => null);

                result.Should().BeOfType<None<TestObject>>();
            }

            [Fact]
            public void Apply_OnSome_WhenResultIsNoneValue_IsTypedNone()
            {
                Option<TestObject> some = new TestObject();

                var result = some.Apply<TestObject>(_ => None.Value);

                result.Should().BeOfType<None<TestObject>>();
            }

            [Fact]
            public void Apply_OnNone_ReturnsResultTypeNone()
            {
                Option<TestObject> none = None.Value;

                var result = none.Apply(_ => new TestObject());

                result.Should().BeOfType<None<TestObject>>();
            }

            [Fact]
            public void ApplyOption_OnSome_ReturnsResultTypeSome_AndDoesNotNest()
            {
                Option<TestObject> some = new TestObject();

                var result = some.Apply(obj => obj.Option);

                result.Should().BeOfType<Some<TestProperty>>();
            }

            [Fact]
            public void ApplyOption_OnSome_ResultContainsValue()
            {
                var property = new TestProperty();
                var testObject = new TestObject { Option = property };
                Option<TestObject> some = testObject;

                var result = some.Apply(obj => obj.Option);

                ((Some<TestProperty>)result).Content.Should().Be(property);
            }

            [Fact]
            public void ApplyOption_OnSome_WhenUsingImplicitCast_ContainsValue()
            {
                var property = new TestProperty();
                var testObject = new TestObject { Option = property };
                Option<TestObject> some = testObject;

                TestProperty result =
                    some.Apply(obj => obj.Option) as Some<TestProperty>;

                result.Should().Be(property);
            }

            [Fact]
            public void ApplyOption_OnSome_WhenResultIsNullReference_IsTypedNone()
            {
                Option<TestObject> some = new TestObject();

                var result = some.Apply<TestObject>(_ => null);

                result.Should().BeOfType<None<TestObject>>();
            }

            [Fact]
            public void ApplyOption_OnNone_ReturnsResultTypeNone()
            {
                Option<TestObject> none = None.Value;

                var result = none.Apply(_ => (Option<TestObject>) new TestObject());

                result.Should().BeOfType<None<TestObject>>();
            }
        }

        public class ResultOrTest
        {
            [Fact]
            public void ResultOrFallback_OnSome_ReturnsResult()
            {
                Option<string> some = Expected;

                var result = some.ResultOr(Fallback);

                result.Should().Be(Expected);
            }

            [Fact]
            public void ResultOrFunction_OnSome_ReturnsResult()
            {
                const string someWasNone =
                    "The test failed because expected Some was actually None";
                Option<string> some = Expected;

                var result = some.ResultOr(() => throw new Exception(someWasNone));

                result.Should().Be(Expected);
            }

            [Fact]
            public void ResultOrFallback_OnNone_ReturnsFallback()
            {
                Option<string> none = None.Value;

                var result = none.ResultOr(Fallback);

                result.Should().Be(Fallback);
            }

            [Fact]
            public void ResultOrFunction_OnNone_ExecutesFunction()
            {
                Option<string> none = None.Value;

                var result = none.ResultOr(() => Fallback);

                result.Should().Be(Fallback);
            }
        }

        public class OfTypeTest
        {
            [Fact]
            public void OfType_WhenAssignable_ReturnsSomeOfAssignedType()
            {
                Option<DerivedType> derived = new DerivedType();

                var result = derived.OfType<SuperType>();

                result.Should().BeOfType<Some<SuperType>>();
            }

            [Fact]
            public void OfType_WhenUnassignable_ReturnsNoneOfAssignedType()
            {
                Option<SuperType> super = new SuperType();

                var result = super.OfType<DerivedType>();

                result.Should().BeOfType<None<DerivedType>>();
            }

            [Fact]
            public void OfType_WhenNone_ReturnsNoneOfAssignedType()
            {
                Option<DerivedType> derived = None.Value;

                var result = derived.OfType<SuperType>();

                result.Should().BeOfType<None<SuperType>>();
            }
        }

        public class GetHashCodeTest
        {
            [Fact]
            public void GetHashCode_UsesContentHashCode()
            {
                Option<string> some = Expected;
                var expectedHash = Expected.GetHashCode();

                some.GetHashCode().Should().Be(expectedHash);
            }

            [Fact]
            public void GetHashCode_ReturnsNoneHashValue()
            {
                Option<object> none = None.Value;

                none.GetHashCode().Should().Be(None.Value.GetHashCode());
            }
        }

        public class EqualsTest
        {
            [Fact]
            public void Equals_SomesWithSameContent_AreEqual()
            {
                Option<string> some = Expected;
                Option<string> otherSome = Expected;

                some.Should().BeEquivalentTo(otherSome);
            }

            [Fact]
            public void Equals_SomeToContent_AreUnequal()
            {
                Option<string> some = Expected;

                some.Should().NotBeEquivalentTo(Expected);
            }

            [Fact]
            public void Equals_SomesWithDifferentContent_AreUnequal()
            {
                Option<string> some = Expected;
                Option<string> otherSome = Fallback;

                some.Should().NotBeEquivalentTo(otherSome);
            }

            [Fact]
            public void Equals_SomesWithDifferentTypes_AreUnequal()
            {
                var derived = new DerivedType();
                Option<SuperType> superSome = derived;
                Option<DerivedType> derivedSome = derived;

                superSome.Should().NotBeEquivalentTo(derivedSome);
                derivedSome.Should().NotBeEquivalentTo(superSome);
            }

            [Fact]
            public void Equals_SomeToNullReference_AreUnEqual()
            {
                Option<string> some = Expected;
                Option<string> nullSome = null;

                some.Should().NotBeEquivalentTo(nullSome);
            }

            [Fact]
            public void Equals_SomeToNone_AreUnEqual()
            {
                Option<string> some = Expected;
                Option<string> none = None.Value;

                some.Should().NotBeEquivalentTo(none);
            }

            [Fact]
            public void Equals_SameTypedNones_AreEqual()
            {
                Option<object> none = None.Value;
                Option<object> otherNone = None.Value;

                none.Should().BeEquivalentTo(otherNone);
            }

            [Fact]
            public void Equals_TypedNoneToNoneVal_AreEqual()
            {
                Option<object> none = None.Value;

                none.Should().BeEquivalentTo(None.Value);
            }

            [Fact]
            public void Equals_NoneValToTypedNone_AreEqual()
            {
                Option<object> none = None.Value;

                None.Value.Should().BeEquivalentTo(none);
            }

            [Fact]
            public void Equals_DifferentlyTypedNones_AreUnequal()
            {
                Option<string> none = None.Value;
                Option<int> otherNone = None.Value;

                none.Should().NotBeEquivalentTo(otherNone);
            }

            [Fact]
            public void Equals_TypedNoneToNullReference_AreUnequal()
            {
                Option<string> none = None.Value;
                Option<string> nullNone = null;

                none.Should().NotBeEquivalentTo(nullNone);
            }

            [Fact]
            public void Equals_NoneValToNullReference_AreUnequal()
            {
                Option<string> nullNone = null;

                None.Value.Should().NotBeEquivalentTo(nullNone);
            }

            [Fact]
            public void Equals_NoneValToNoneVal_AreEqual()
            {
                var none = None.Value;
                var otherNone = None.Value;

                none.Should().BeEquivalentTo(otherNone);
            }
        }

        public class OperatorTest
        {
            [Fact]
            public void EqualsOperator_SomesWithSameContent_AreEqual()
            {
                Option<string> some = Expected;
                Option<string> otherSome = Expected;

                var result = some == otherSome;

                result.Should().BeTrue();
            }

            [Fact]
            public void EqualsOperator_SomesWithDifferentContent_AreUnequal()
            {
                Option<string> some = Expected;
                Option<string> otherSome = Fallback;

                var result = some == otherSome;

                result.Should().BeFalse();
            }

            [Fact]
            public void EqualsOperator_SomeWithNullReference_AreUnequal()
            {
                Option<string> some = Expected;
                Option<string> nullSome = null;

                var result = some == nullSome;

                result.Should().BeFalse();
            }

            [Fact]
            public void EqualsOperator_SomeWithNone_AreUnequal()
            {
                Option<string> some = Expected;
                Option<string> none = None.Value;

                var result = some == none;

                result.Should().BeFalse();
            }

            [Fact]
            public void EqualsOperator_NoneWithNone_AreEqual()
            {
                Option<string> none = None.Value;
                Option<string> otherNone = None.Value;

                var result = none == otherNone;

                result.Should().BeTrue();
            }

            [Fact]
            public void EqualsOperator_NoneWithSome_AreUnequal()
            {
                Option<string> none = None.Value;
                Option<string> some = Expected;

                var result = none == some;

                result.Should().BeFalse();
            }

            [Fact]
            public void EqualsOperator_NoneWithNull_AreUnequal()
            {
                Option<string> none = None.Value;
                Option<string> nullOption = null;

                var result = none == nullOption;

                result.Should().BeFalse();
            }

            [Fact]
            public void UnequalsOperator_SomesWithSameContent_AreEqual()
            {
                Option<string> some = Expected;
                Option<string> otherSome = Expected;

                var result = some != otherSome;

                result.Should().BeFalse();
            }

            [Fact]
            public void UnequalsOperator_SomesWithDifferentContent_AreUnequal()
            {
                Option<string> some = Expected;
                Option<string> otherSome = Fallback;

                var result = some != otherSome;

                result.Should().BeTrue();
            }

            [Fact]
            public void UnequalsOperator_SomeWithNullReference_AreUnequal()
            {
                Option<string> some = Expected;
                Option<string> nullSome = null;

                var result = some != nullSome;

                result.Should().BeTrue();
            }

            [Fact]
            public void UnequalsOperator_SomeWithNone_AreUnequal()
            {
                Option<string> some = Expected;
                Option<string> none = None.Value;

                var result = some != none;

                result.Should().BeTrue();
            }

            [Fact]
            public void UnequalsOperator_NoneWithNone_AreEqual()
            {
                Option<string> none = None.Value;
                Option<string> otherNone = None.Value;

                var result = none != otherNone;

                result.Should().BeFalse();
            }

            [Fact]
            public void UnequalsOperator_NoneWithSome_AreUnequal()
            {
                Option<string> none = None.Value;
                Option<string> some = Expected;

                var result = none != some;

                result.Should().BeTrue();
            }

            [Fact]
            public void UnequalsOperator_NoneWithNull_AreUnequal()
            {
                Option<string> none = None.Value;
                Option<string> nullOption = null;

                var result = none != nullOption;

                result.Should().BeTrue();
            }
        }
    }
}
