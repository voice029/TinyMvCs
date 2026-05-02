using TinyMvCs.MvLib;
MvBinder<int> binder = new MvBinder<int>(0);
TestMv testMv = new TestMv();



Console.WriteLine("Hello, World!");
testMv.TestInt.OnUpdate.Bind(binder[0], update =>
{
    Console.WriteLine($"TestInt updated to {update.CurrentValue} to {update.NewValue} state 0");
});

testMv.TestInt.OnUpdate.Bind(binder[1], update =>
{
    Console.WriteLine($"TestInt updated to {update.CurrentValue} to {update.NewValue} state 1");
});

testMv.TestInt.Set(10);
binder.ActivateState(1);

testMv.TestInt.Set(20);

binder.ActivateState(0);
testMv.TestInt.Set(30);

class TestMv : MvMvvm<TestMv>
{
    public MvValue<int, TestMv> TestInt = new();
}




