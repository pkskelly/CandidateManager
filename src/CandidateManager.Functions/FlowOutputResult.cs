using CandidateManager.Functions;
using CandidateManager.Core;
using Microsoft.Azure.Functions.Worker;

namespace CandidateManager.Core
{
    public class FlowOutputResult
    {
        [QueueOutput(Constants.OUTPUT_QUEUE)]
        public FlowOutput Output { get; set; }

        [TableOutput("OutputTable", Connection = "AzureWebJobsStorage")]
        public CandidateProcessingEntity CandidateProcessingEntity { get; set; }
        
    }
}