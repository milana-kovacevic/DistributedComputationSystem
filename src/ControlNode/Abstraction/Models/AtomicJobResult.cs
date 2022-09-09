using System.Text.Json.Serialization;

namespace ControlNode.Abstraction.Models
{
    public class AtomicJobResult
    {
        public int AtomicJobId { get; set; }

        public int JobId { get; set; }

        public string? Result { get; set; }

        public AtomicJobState State { get; set; }

        public string? Error { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        // Foreign key.
        [JsonIgnore]
        public virtual AtomicJob AtomicJob { get; set; }
    }
}
