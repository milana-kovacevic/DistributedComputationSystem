using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonConverterAttribute = Newtonsoft.Json.JsonConverterAttribute;

namespace FrontendService.Models
{
    public class Job
    {
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public JobState State { get; set; }
    }
}