using Refit;
using System.Text.Json.Serialization;

namespace LEARN_MVVM.DataAccess
{
    interface INotifyAndre
    {
        [Post("/alerts")]
        Task Collect([Body(BodySerializationMethod.Default)] string msg);
    }
}

public class Notification
{
    public class Attachment
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("size")]
        public int size { get; set; }

        [JsonPropertyName("expires")]
        public int expires { get; set; }

        [JsonPropertyName("url")]
        public string url { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("time")]
        public int time { get; set; }

        [JsonPropertyName("expires")]
        public int expires { get; set; }

        [JsonPropertyName("event")]
        public string @event { get; set; }

        [JsonPropertyName("topic")]
        public string topic { get; set; }

        [JsonPropertyName("priority")]
        public int priority { get; set; }

        [JsonPropertyName("tags")]
        public List<string> tags { get; set; }

        [JsonPropertyName("click")]
        public string click { get; set; }

        [JsonPropertyName("attachment")]
        public Attachment attachment { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("message")]
        public string message { get; set; }
    }
}
