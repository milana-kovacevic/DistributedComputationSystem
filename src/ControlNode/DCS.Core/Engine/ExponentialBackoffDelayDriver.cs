using Microsoft.AspNetCore.Server.IIS.Core;

namespace ControlNode.DCS.Core.Engine
{
    /// <summary>
    /// Driver that manages and calculates exponential backoff.
    /// Resets the value after it reaches MAX.
    /// </summary>
    public class ExponentialBackoffDelayDriver
    {
        private TimeSpan _currentDelay;
        private readonly TimeSpan _initialDelay = TimeSpan.FromMilliseconds(100);
        private readonly TimeSpan _defaultInitialDelay = TimeSpan.FromMilliseconds(100);
        private readonly int ExponentialConst = 2;
        private readonly TimeSpan MaxWaittime = TimeSpan.FromSeconds(3);

        public ExponentialBackoffDelayDriver()
        {
            _initialDelay = _defaultInitialDelay;
            _currentDelay = _defaultInitialDelay;
        }

        public ExponentialBackoffDelayDriver(TimeSpan initialDelay)
        {
            _initialDelay = initialDelay;
            _currentDelay = initialDelay;
        }

        public async Task Delay()
        {
            await Task.Delay(_currentDelay);

            UpdateCurrentDelay();
        }

        private void UpdateCurrentDelay()
        {
            _currentDelay *= ExponentialConst;

            // Reset the delay to initial if it's bigger than MAX.
            if (_currentDelay > MaxWaittime)
            {
                _currentDelay = _initialDelay;
            }
        }
    }
}
