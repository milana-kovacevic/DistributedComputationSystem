using ComputeNode.Exceptions;
using ComputeNode.Models;

namespace ComputeNode.Executors
{
    public class SpecificJobExecutorFactory : ISpecificJobExecutorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SpecificJobExecutorFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task<ISpecificJobExecutor> BuildAsync(AtomicJobType atomicJobType)
        {
            ISpecificJobExecutor builtExecutor = null;
            if (atomicJobType == AtomicJobType.CalculateSumOfDigits)
            {
                builtExecutor = (ISpecificJobExecutor)_serviceProvider.GetService(typeof(CalculateNumberOfDigitsExecutor));
            }

            return builtExecutor ?? throw new ArgumentException(String.Format(ExceptionMessages.NonexistentSpecificExecutor, atomicJobType));
        }
    }
}
