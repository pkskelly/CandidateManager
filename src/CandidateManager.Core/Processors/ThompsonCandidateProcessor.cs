using System.Collections.Generic;
using CandidateManager.Core.Interfaces;

namespace CandidateManager.Core.Processors
{

    public class ThompsonCandidateProcessor : ICandidateProcessor
    {
         public Candidate Process(string html)
        {
            List<string> readableElements = ScraperUtilities.GetTextElements(html);
            var name = ScraperUtilities.GetContentValue(readableElements, Constants.THOMPSON_CANDIDATE_NAME);
            var emailAddress = ScraperUtilities.GetContentValue(readableElements, Constants.CANDIDATE_EMAIL_FIELD);
            var phone = ScraperUtilities.GetContentValue(readableElements, Constants.CANDIDATE_PHONE_FIELD);
            var company = Constants.THOMPSON_COMPANY_NAME;
            Candidate newCandidate = new Candidate(name, emailAddress, phone, company);
            return newCandidate;    

        }
    }
}