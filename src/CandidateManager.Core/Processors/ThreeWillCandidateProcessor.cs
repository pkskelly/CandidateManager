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
            Candidate newCandidate = new Candidate();
            List<string> readableElements = ScraperUtilities.GetTextElements(html);
            newCandidate.Name = ScraperUtilities.GetContentValue(readableElements, Constants.THREEWILL_CANDIDATE_NAME);
            newCandidate.EmailAddress = ScraperUtilities.GetContentValue(readableElements, Constants.CANDIDATE_EMAIL_FIELD);
            newCandidate.Phone = ScraperUtilities.GetContentValue(readableElements, Constants.THREEWILL_CANDIDATE_PHONE);
            newCandidate.Company = "ThreeWill";
            return newCandidate;    
        }
    }
}