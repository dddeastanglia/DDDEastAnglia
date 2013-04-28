using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Builders.Calendar;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.DataAccess.EntityFramework.Queries;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Models.Query;
using Ninject.Modules;
using Ninject.Web.Common;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class EntityFrameworkDataAccessModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IConferenceRepository>().To<EntityFrameworkConferenceRepository>().InRequestScope();
            Kernel.Bind<IVoteRepository>().To<EntityFrameworkVoteRepository>().InRequestScope();
            Kernel.Bind<ISessionRepository>().To<EntityFrameworkSessionRepository>().InRequestScope();
            Kernel.Bind<IBannerModelQuery>().To<EntityFrameworkBannerModelQuery>().InRequestScope();
            Kernel.Bind<IBuild<Conference, Domain.Conference>>().To<ConferenceBuilder>().InRequestScope();
            Kernel.Bind<IBuild<CalendarItem, CalendarEntry>>().To<CalendarEntryBuilder>().InRequestScope();
            Kernel.Bind<IBuild<CalendarItem, SingleTimeEntry>>().To<CalendarItemToSingleTimeEntryConverter>().InRequestScope();
            Kernel.Bind<IBuild<CalendarItem, TimeRangeEntry>>().To<CalendarItemToTimeRangeEntryConverter>().InRequestScope();
        }
    }
}