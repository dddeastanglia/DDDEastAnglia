using System;
using DDDEastAnglia.DataAccess.MessageBus;

namespace DDDEastAnglia.DataAccess.Commands.Vote
{
    public class DeleteVoteCommand : ICommand
    {
        public int SessionId { get; set; }
        public Guid CookieId { get; set; }
    }
}