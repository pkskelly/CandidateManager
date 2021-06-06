namespace CandidateManager.Functions
{
    public class CandidateProcessingEntity
    {
        public CandidateProcessingEntity()
        {
        }

        public CandidateProcessingEntity(string partitionKey, string rowKey, string flowId, string company, string recruiter, string fileName)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
            this.FlowId = flowId;
            this.Company = company;
            this.Recruiter = recruiter;
            this.FileName = fileName;

        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string FlowId { get; set; } 
        public string Company { get; set; }
        public string Recruiter { get; set; }
        public string FileName { get; set; }
    }
}