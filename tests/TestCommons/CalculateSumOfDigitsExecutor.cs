namespace TestCommons
{
    public class CalculateSumOfDigitsExecutor
    {
        public static int CalculateSumOfDigits(int[] numbers)
        {
            return numbers.Select(n => CalculateSumOfDigits(n))
                .Aggregate(0, (acc, x) => acc + x);
        }

        // Calculate sum of digits for all numbers from 1 to number.
        private static int CalculateSumOfDigits(int number)
        {
            int sumOfDigits = 0;

            for (int i = 1; i <= number; i++)
            {
                int current = i;
                while (current > 0)
                {
                    sumOfDigits += current % 10;
                    current /= 10;
                }
            }

            return sumOfDigits;
        }

        private static int Abs(int number)
        {
            return number >= 0 ? number : -number;
        }
    }
}
