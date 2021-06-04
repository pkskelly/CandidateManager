using System.Collections.Generic;
using CandidateManager.Core.Interfaces;

namespace CandidateManager.Core.Processors
{
    public class StratfieldCandidateProcessor : ICandidateProcessor
    {
        public Candidate Process(string html)
        {
            List<string> readableElements = ScraperUtilities.GetTextElements(html);
            var name = ScraperUtilities.GetContentValue(readableElements, Constants.STRATFIELD_CANDIDATE_NAME).TrimEnd(',');
            var emailAddress = Constants.FIELD_DEFAULT;
            var phone = Constants.FIELD_DEFAULT;
            var company = Constants.STRATFIELD_COMPANY_NAME;
            Candidate newCandidate = new Candidate(name, emailAddress, phone, company);
            return newCandidate;    
        }
    }
}