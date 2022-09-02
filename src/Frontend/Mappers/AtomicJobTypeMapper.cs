using ComputeNodeSwaggerClient;
using AtomicJobTypeFrontend = Frontend.Models.AtomicJobType;

namespace Frontend.Mappers
{
    public static class AtomicJobTypeMapper
    {
        public static AtomicJobType Map(AtomicJobTypeFrontend atomicJobType)
        {
            string value = atomicJobType.ToString();

            return (AtomicJobType)Enum.Parse(typeof(AtomicJobType), value);
        }
    }
}
