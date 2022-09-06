namespace ComputeNode.Models
{
    public class AtomicJob
    {
        public int Id { get; set; }

        public int ParentJobId { get; set; }

        public string InputData { get; set; }

        public AtomicJobType JobType { get; set; }

        public AtomicJobResult AtomicJobResult { get; set; }
    }
}
