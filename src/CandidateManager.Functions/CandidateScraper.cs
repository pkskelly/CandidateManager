using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using CandidateManager.Core;
using CandidateManager.Core.Extensions;
using CandidateManager.Core.Interfaces;
using CandidateManager.Core.Processors;

namespace CandidateManager.Functions
{
    public static class CandidateScraper
    {
        [Function(nameof(CandidateScraper))]
        [QueueOutput(Constants.OUTPUT_QUEUE)]
        public static FlowOutput Run([QueueTrigger(Constants.INPUT_QUEUE)] string queueMessage, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(CandidateScraper));
            logger.LogInformation($"C# Queue trigger function processed: {queueMessage}");
        
            // 1. Deserialize queue message
            FlowIngest flowInput = queueMessage.FromJson<FlowIngest>();
            
            // 2. Call the factory to create company processor
            ICandidateProcessor candidateProcessor = CandidateProcessorFactory.CreateCandidateProcessor(flowInput.Company);

            // 3. Once we have the processor back, process the html 
            Candidate candidate = candidateProcessor.Process(flowInput.BodyHtml);
           
            // Log some information just in case 
            logger.LogTrace($"Name: {candidate.Name}");
            logger.LogTrace($"Email Address: {candidate.EmailAddress}");
            logger.LogTrace($"Phone: {candidate.Phone}");

            logger.LogTrace($"Blob Name: {flowInput.FileName}");
            logger.LogTrace($"Returning flowOutput!");
            //added app offline settings in vsts
            FlowOutput flowOutput = new FlowOutput() { FlowId = flowInput.FlowId, Company = flowInput.Company, Recruiter = flowInput.Recruiter, FileName = flowInput.FileName, Candidate = candidate };
            return flowOutput;        
        }
    }
}