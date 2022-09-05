using FrontendAtomicJobResult = Frontend.Models.AtomicJobResult;
using FrontendAtomicJobState = Frontend.Models.AtomicJobState;
using ComputeNodeAtomicJobResult = ComputeNodeSwaggerClient.AtomicJobResult;
using ComputeNodeAtomicJobState = ComputeNodeSwaggerClient.AtomicJobState;

namespace Frontend.Mappers
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
                //StartTime = atomicJobResult.StartTime,
                //EndTime = atomicJobResult.EndTime,
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
