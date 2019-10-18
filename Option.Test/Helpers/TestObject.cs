namespace Option.Test.Helpers
{
    public class TestObject
    {
        public TestProperty Property { get; set; } = new TestProperty();

        public Option<TestProperty> Option { get; set; } = new TestProperty();

        
    }

    public class TestProperty { }
}