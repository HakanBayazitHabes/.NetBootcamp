namespace API.DelegateEvent.Delegate;

public class BasicDelegateExample
{
    public static void Main()
    {
        // Calculator sınıfının Main metodunu çağır
        Calculator.Main();
    }

}

public delegate int MathOperation(int x, int y);

public class Calculator
{
    public int Add(int x, int y)
    {
        return x + y;
    }

    public int Multiply(int x, int y)
    {
        return x * y;
    }

    public static void Main()
    {
        Calculator calc = new Calculator();
        MathOperation op = calc.Add;
        Console.WriteLine("Add: " + op(5, 3));

        op = calc.Multiply;
        Console.WriteLine("Multiply: " + op(5, 3));
    }

}

