using DieInterpreter;
using DieInterpreter.Tokenizer;
using DieInterpreter.StateMachine;
namespace DieInterpreterTests;

public class Tests
{
        string? Statement;
        [SetUp]
        public void Setup()
        {
                Statement = "2+2*4d6+`hello`";
        }
        [TearDown]
        public void TearDown()
        {
                Statement = null;
        }

        [Test]
        public void Tokenizer()
        {
                Operators operators = new(new Dictionary<string, int>{
                                { "+", 1},
                                { "-", 1},
                                { "*", 2},
                                { "/", 2},
                                { "d", 3}
                        }
                );
                TokenizerData cargo = new(Statement!);
                TokenizerContext context = new(operators, cargo);
                StateMachineLoop.Run(context);
                Assert.That(cargo.Instructions, Is.EqualTo(new Instruction[] {
                        new(InstructionType.LiteralInteger, "2"),
                        new(InstructionType.Operator, "+"),
                        new(InstructionType.LiteralInteger, "2"),
                        new(InstructionType.Operator, "*"),
                        new(InstructionType.LiteralInteger, "4"),
                        new(InstructionType.Operator, "d"),
                        new(InstructionType.LiteralInteger, "6"),
                        new(InstructionType.Operator, "+"),
                        new(InstructionType.Variable, "hello"),
                }));
        }

}

public class Tests2
{
        string? Statement;
        [SetUp]
        public void Setup()
        {
                Statement = "2+2*MyFuNCtion42(`aAA`, 42)";
        }
        [TearDown]
        public void TearDown()
        {
                Statement = null;
        }

        [Test]
        public void Tokenizer()
        {
                Operators operators = new(new Dictionary<string, int>{
                                { "+", 1},
                                { "-", 1},
                                { "*", 2},
                                { "/", 2},
                                { "d", 3},
                        }
                );
                TokenizerData cargo = new(Statement!);
                TokenizerContext context = new(operators, cargo);
                StateMachineLoop.Run(context);
                Assert.That(cargo.Instructions, Is.EqualTo(new Instruction[] {
                        new(InstructionType.LiteralInteger, "2"),
                        new(InstructionType.Operator, "+"),
                        new(InstructionType.LiteralInteger, "2"),
                        new(InstructionType.Operator, "*"),
                        new(InstructionType.Function, "MyFuNCtion42"),
                        new(InstructionType.ParenthesesOpen, ""),
                        new(InstructionType.Variable, "aAA"),
                        new(InstructionType.LiteralInteger, "42"),
                        new(InstructionType.ParenthesesClosed, ""),
                }));
        }

}

public class ShuntingYardFunction
{
        string? Statement;
        [SetUp]
        public void Setup()
        {
                Statement = "2+MyFuNCtion42(42)";
        }
        [TearDown]
        public void TearDown()
        {
                Statement = null;
        }

        [Test]
        public void Tokenizer()
        {
                Operators operators = new(new Dictionary<string, int>{
                                { "+", 1},
                                { "-", 1},
                                { "*", 2},
                                { "/", 2},
                                { "d", 3},
                        }
                );
                TokenizerData cargo = new(Statement!);
                TokenizerContext context = new(operators, cargo);
                StateMachineLoop.Run(context);
                ShuntingYard yard = new(operators);

                Assert.That(yard.ReversePolish(cargo.Instructions), Is.EqualTo(new Instruction[] {
                        new(InstructionType.LiteralInteger, "2"),
                        new(InstructionType.LiteralInteger, "42"),
                        new(InstructionType.Function, "MyFuNCtion42"),
                        new(InstructionType.Operator, "+"),
                }));
        }

}
