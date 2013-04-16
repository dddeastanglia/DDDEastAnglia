using System.Collections.Generic;

namespace DDDEastAnglia.Models
{
    public class SpeakerDisplayModel
    {
        public SpeakerDisplayModel()
        {
            Sessions = new Dictionary<int, string>();
        }

        public bool IsCurrentUser { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string TwitterHandle { get; set; }
        public string WebsiteUrl { get; set; }
        public string GravatarUrl { get; set; }
        public Dictionary<int, string> Sessions { get; set; }
    }
}