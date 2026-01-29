using DieInterpreter;
namespace DieInterpreterTests;

public class Tests
{
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
                Assert.That(Operators.IsOperator("+"), Is.True);
                Assert.Pass();
        }
}
