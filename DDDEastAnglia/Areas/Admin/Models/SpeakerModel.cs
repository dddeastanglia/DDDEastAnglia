using System.ComponentModel;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class SpeakerModel
    {
        public int UserId { get; set; }

        [DisplayName("username")]
        public string UserName { get; set; }

        [DisplayName("name")]
        public string Name { get; set; }

        [DisplayName("new speaker")]
        public bool NewSpeaker { get; set; }

        public string GravatarUrl { get; set; }

        [DisplayName("submitted sessions")]
        public int SubmittedSessionCount { get; set; }
    }
}
