using System;
using System.Collections.Generic;
using CandidateManager.Core.Interfaces;
using CandidateManager.Core;

namespace CandidateManager.Core.Processors
{
    public class ThreeWillCandidateProcessor : ICandidateProcessor
    {
        public Candidate Process(string html)
        {
            List<string> readableElements = ScraperUtilities.GetTextElements(html);
            var name = ScraperUtilities.GetContentValue(readableElements, Constants.THREEWILL_CANDIDATE_NAME);
            var emailAddress = ScraperUtilities.GetContentValue(readableElements, Constants.CANDIDATE_EMAIL_FIELD);
            var phone = ScraperUtilities.GetContentValue(readableElements, Constants.THREEWILL_CANDIDATE_PHONE);
            var company = Constants.THREEWILL_COMPANY_NAME;
            Candidate newCandidate = new Candidate(name, emailAddress, phone, company);

            return newCandidate;    
        }
    }
}