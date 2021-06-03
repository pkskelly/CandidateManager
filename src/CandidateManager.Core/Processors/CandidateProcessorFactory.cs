using System;
using CandidateManager.Core.Interfaces;

namespace CandidateManager.Core.Processors
{
    public class CandidateProcessorFactory
    {
        public ICandidateProcessor CreateCandidateProcessor(string company) {
            try {
                return (ICandidateProcessor)Activator.CreateInstance(Type.GetType($"CandidateManager.Core.Processors.{company}CandidateProcessor"));
            }
            catch {
                throw new ArgumentException("Factory generator paramter invalid!", company.ToString());
            }
        }
    }
}

 