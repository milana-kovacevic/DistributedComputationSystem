using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ControlNode.Frontend.Models
{
    public class JobResult
    {
        public JobState State { get; set; }

        public string? Result { get; set; }

        public string? Error { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndTime { get; set; }

        // Foreign key.
        [JsonIgnore]
        public int JobId { get; set; }

        [JsonIgnore]
        public virtual Job Job { get; set; }
    }
}
