namespace API.DelegateEvent.Delegate
{
    public class FilteringUsingDelegate
    {
        public static void Main()
        {
            Program1.Main();
        }
    }

    public delegate bool Criteria(int x);

    public class Program1
    {
        public static void Main()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> evenNumbers = Filter(numbers, IsEven);
            List<int> oddNumbers = Filter(numbers, IsOdd);

            Console.WriteLine("Even Numbers: " + string.Join(", ", evenNumbers));
            Console.WriteLine("Odd Numbers: " + string.Join(", ", oddNumbers));
        }

        public static List<int> Filter(List<int> numbers, Criteria criteria)
        {
            List<int> result = new List<int>();
            foreach (int number in numbers)
            {
                if (criteria(number))
                {
                    result.Add(number);
                }
            }
            return result;
        }

        public static bool IsEven(int x)
        {
            return x % 2 == 0;
        }

        public static bool IsOdd(int x)
        {
            return x % 2 != 0;
        }
    }

}