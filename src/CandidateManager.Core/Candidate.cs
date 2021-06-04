using System;
using Ardalis.GuardClauses;

namespace CandidateManager.Core
{
    public record Candidate
    {
        private readonly string _name;
        private readonly string _emailAddress;
        private readonly string _company;
        private readonly string _phone;

        public Candidate(string name, string emailAddress, string phone, string company)
        {
            Name = name;
            Phone = phone;
            EmailAddress = emailAddress;
            Company = company;
        }

        public string Name
        {
            get => _name;
            init => _name = (Guard.Against.NullOrEmpty(value, nameof(Name), $"{nameof(Name)} must be provided!") ?? value);
        }
        public string EmailAddress
        {
            get => _emailAddress;
            init => _emailAddress = (Guard.Against.NullOrEmpty(value, nameof(EmailAddress), $"{nameof(EmailAddress)} must be provided!") ?? value);
        }
        public string Company
        {
            get => _company;
            init => _company = (Guard.Against.NullOrEmpty(value, nameof(Company), $"{nameof(Company)} must be provided!") ?? value);
        }

        public string Phone
        {
            get => _phone;
            init => _phone = (Guard.Against.NullOrEmpty(value, nameof(Phone), $"{nameof(Phone)} must be provided!") ?? value);
        }
    }
}
