using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class AtomicJob
    {
        public int AtomicJobId { get; set; }

        public AtomicJobType JobType { get; set; }

        public string InputData { get; set; }

        public AtomicJobResult AtomicJobResult { get; set; }

        // Foreign key.
        public int JobId { get; set; }

        [JsonIgnore]
        public virtual Job Job { get; set; }

        public AtomicJob()
        {
            AtomicJobResult = new AtomicJobResult()
            {
                State = AtomicJobState.NotRan,
                StartTime = DateTime.UtcNow
            };
        }
    }
}
