namespace CandidateManager.Core
{
    public class FlowOutput
    {
        public string FlowId { get; set; }
        public string Company { get; set; }
        public string Recruiter { get; set; }
        public string FileName { get; set; }
        public Candidate Candidate { get; set; }
    }
}