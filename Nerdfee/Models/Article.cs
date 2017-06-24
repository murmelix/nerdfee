using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdfee.Models
{
    public class Article
    {
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string FacebookId { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "created_time")]
        public DateTime Erstellt { get; set; }
        public string Kategorie { get; set; }
        public string Teaser { get; set; }
        public string Titel { get; set; }
        public long ThreadId { get; set; }
        public DateTime? Veroeffentlicht { get; set; }
    }
}
