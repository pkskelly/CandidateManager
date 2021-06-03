using System;
using Ardalis.GuardClauses;

namespace CandidateManager.Core
{
    public class Candidate
    {
        public Candidate()
        {
        }
        
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}; Company: {Company.ToString()}; Email: {EmailAddress};  Phone: {Phone}";
        }
    }
}
