using Newtonsoft.Json;

namespace OmnicellAPI.DTO
{ /// <summary>Represents an error response</summary>
    public class ErrorResponse
    {
        [JsonProperty(PropertyName = "message")]
        public string? Message { get; set; }
    }
}
