using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class JobResult
    {
        public int JobId { get; set; }

        public JobState State { get; set; }

        public string? Error { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndTime { get; set; }

        // Forgein key.
        [JsonIgnore]
        public virtual Job Job { get; set; }
    }
}
