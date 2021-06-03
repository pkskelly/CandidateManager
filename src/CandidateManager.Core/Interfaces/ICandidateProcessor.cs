namespace CandidateManager.Core.Interfaces
{
    public interface ICandidateProcessor    
    {
        Candidate Process(string html);     
    }
}
