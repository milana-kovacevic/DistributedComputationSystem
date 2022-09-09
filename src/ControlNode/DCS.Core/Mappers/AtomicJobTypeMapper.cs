using ComputeNodeAtomicJobType = ComputeNodeSwaggerClient.AtomicJobType;
using FrontendAtomicJobType = ControlNode.Frontend.Models.AtomicJobType;

namespace ControlNode.DCS.Core.Mappers
{
    public static class AtomicJobTypeMapper
    {
        public static ComputeNodeAtomicJobType Map(FrontendAtomicJobType atomicJobType)
        {
            string value = atomicJobType.ToString();

            return (ComputeNodeAtomicJobType)Enum.Parse(typeof(ComputeNodeAtomicJobType), value);
        }

        public static FrontendAtomicJobType Map(ComputeNodeAtomicJobType atomicJobType)
        {
            string value = atomicJobType.ToString();

            return (FrontendAtomicJobType)Enum.Parse(typeof(FrontendAtomicJobType), value);
        }
    }
}
