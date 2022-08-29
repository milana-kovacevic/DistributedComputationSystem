using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class Job
    {
        public int Id { get; set; }

        public JobRequestData Data { get; set; }

        public JobState State { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndTime { get; set; }
    }
}