namespace DDDEastAnglia.Domain
{
    public class Conference
    {
        private readonly int _id;
        private readonly string _name;
        private readonly string _shortName;
        private bool _isReadyForVoting;

        public Conference(int id, string name, string shortName)
        {
            _id = id;
            _name = name;
            _shortName = shortName;
        }

        public bool CanVote()
        {
            return _isReadyForVoting;
        }

        public void ReadyForVoting()
        {
            _isReadyForVoting = true;
        }
    }
}