using System;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public partial class BannerController : Controller
    {
         [ChildActionOnly]
         public virtual ActionResult Default()
         {
             var eventRepository = new EventRepository();
             var dddEvent = eventRepository.Get("DDDEA2013");
             var model = new BannerViewModel();

             if (dddEvent.CanSubmit())
             {
                 model.IsDuringSessionSubmission = true;
                 var submissionEnds = dddEvent.PreConferenceAgenda.First(i => i.DateType == DateType.SubmissionEnds);
                 model.CurrentEventExpirationDate = FormatExpiration(submissionEnds.Date);
             }
             else if (dddEvent.CanVote())
             {
                 model.IsDuringSessionVoting = true;
                 var votingEnds = dddEvent.PreConferenceAgenda.First(i => i.DateType == DateType.VotingEnds);
                 model.CurrentEventExpirationDate = FormatExpiration(votingEnds.Date);
             }
             
             return PartialView(model);
         }

        private string FormatExpiration(DateTime dateTime)
        {
            // has to be this format so that javascript can parse it back to a date later
            return dateTime.ToString("R");
        }
    }
}