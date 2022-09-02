using ComputeNode.Exceptions;
using ComputeNode.Models;

namespace ComputeNode.Executors
{
    public class CalculateNumberOfDigitsExecutor : ISpecificJobExecutor
    {
        public async Task<AtomicJobResult> ExecuteAsync(AtomicJob atomicJob)
        {
            var result = new AtomicJobResult()
            {
                Id = atomicJob.Id,
                ParentJobId = atomicJob.ParentJobId
            };

            if (TryCalculateSumOfDigits(atomicJob.InputData, out long sumOfDigits))
            {
                result.Result = sumOfDigits.ToString();
                result.State = AtomicJobState.Succeeded;
            }
            else
            {
                result.Error = string.Format(ExceptionMessages.InvalidInputData, atomicJob.InputData);
                result.State = AtomicJobState.Failed;
            }

            return result;
        }

        private static bool TryCalculateSumOfDigits(string? data, out long result)
        {
            result = -1;
            if (long.TryParse(data, out long number))
            {
                result = CalculateSumOfDigits(Abs(number));

                return true;
            }

            return false;
        }

        private static long CalculateSumOfDigits(long number)
        {
            long sumOfDigits = 0;
            while (number > 0)
            {
                sumOfDigits += number % 10;
                number /= 10;
            }

            return sumOfDigits;
        }

        private static long Abs(long number)
        {
            return number >= 0 ? number : -number;
        }
    }
}
