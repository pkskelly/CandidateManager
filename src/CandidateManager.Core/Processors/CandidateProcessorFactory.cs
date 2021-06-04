using System;
using CandidateManager.Core.Interfaces;

namespace CandidateManager.Core.Processors
{
    public class CandidateProcessorFactory
    {
        public static ICandidateProcessor CreateCandidateProcessor(string company) {
            try {
                return (ICandidateProcessor)Activator.CreateInstance(Type.GetType($"CandidateManager.Core.Processors.{company}CandidateProcessor"));
            }
            catch {
                throw new ArgumentException($"{nameof(CandidateProcessorFactory)} generator parameter invalid!", company.ToString());
            }
        }
    }
}

 