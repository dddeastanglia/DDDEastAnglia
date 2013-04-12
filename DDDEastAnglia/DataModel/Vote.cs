namespace DDDEastAnglia.DataModel
{
    public class Vote
    {
        public string Event { get; set; }
        public int SessionId { get; set; }
        public string IPAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long WebSessionId { get; set; }
        public string UserAgent { get; set; }
        public bool IsVote { get; set; }
    }
}