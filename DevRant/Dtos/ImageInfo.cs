using Newtonsoft.Json;

namespace DevRant.Dtos
{
    /// <summary>
    /// Image Info
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// Full URL
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        [JsonProperty("height")]
        public string Height { get; set; }
    }
}