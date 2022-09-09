using ComputeNodeAtomicJobResult = ComputeNodeSwaggerClient.AtomicJobResult;
using ComputeNodeAtomicJobState = ComputeNodeSwaggerClient.AtomicJobState;
using FrontendAtomicJobResult = ControlNode.Abstraction.Models.AtomicJobResult;
using FrontendAtomicJobState = ControlNode.Abstraction.Models.AtomicJobState;

namespace ControlNode.DCS.Core.Mappers
{
    public class AtomicJobResultMapper
    {
        public static FrontendAtomicJobResult Map(ComputeNodeAtomicJobResult atomicJobResult)
        {
            var value = new FrontendAtomicJobResult()
            {
                AtomicJobId = atomicJobResult.Id,
                JobId = atomicJobResult.ParentJobId,
                Result = atomicJobResult.Result,
                State = MapAtomicJobState(atomicJobResult.State),
                Error = atomicJobResult.Error,
                StartTime = atomicJobResult.StartTime.UtcDateTime,
                EndTime = atomicJobResult.EndTime?.UtcDateTime,
            };

            return value;
        }

        private static FrontendAtomicJobState MapAtomicJobState(ComputeNodeAtomicJobState jobState)
        {
            string value = jobState.ToString();

            return (FrontendAtomicJobState)Enum.Parse(typeof(FrontendAtomicJobState), value);
        }
    }
}
