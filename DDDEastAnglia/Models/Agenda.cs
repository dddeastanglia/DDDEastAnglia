using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DDDEastAnglia.Models
{
    public sealed class Agenda
    {
        private readonly Dictionary<int, AgendaSession> sessions;

        public Agenda(IEnumerable<AgendaSession> sessions)
        {
            this.sessions = sessions.ToDictionary(s => s.SessionId, s => s);
        }

        public IHtmlString SessionDetails(HtmlHelper htmlHelper, int sessionId)
        {
            var session = sessions[sessionId];

            var sessionLink = htmlHelper.ActionLink(session.SessionTitle, "Details", "Session", new { id = sessionId }, new { @class = "sessionTitle" });
            var speakerLink = htmlHelper.ActionLink(session.SpeakerName, "Details", "Speaker", new { id = session.SpeakerUserId }, new { @class = "speakerName" });

            var html = string.Format("{0}<br/>{1}", sessionLink, speakerLink);
            return htmlHelper.Raw(html);
        }
    }
}
