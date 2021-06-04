using System;
using System.IO;
using System.Text.Json;
using CandidateManager.Core;
using CandidateManager.Core.Extensions;
using CandidateManager.Core.Interfaces;
using CandidateManager.Core.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CandidateManager.Tests
{
    [TestClass]
    public class CandidateTests
    {
        [TestMethod]
        public void ShouldThrowWithEmptyName()
        {
            Assert.ThrowsException<ArgumentException>(() => new Candidate("", "pskelly@threewill.com", "999-0000","ThreeWill"));
            Assert.ThrowsException<ArgumentNullException>(() => new Candidate(null, "pskelly@threewill.com","999-0000","ThreeWill"));
        }

        [TestMethod]
        public void ShouldThrowWithEmptyOrNullPhone()
        {
            Assert.ThrowsException<ArgumentException>(() => new Candidate("Pete Skelly",  "", "999-0000", "ThreeWill"));
            Assert.ThrowsException<ArgumentNullException>(() => new Candidate("Pete Skelly", null, "999-0000", "ThreeWill"));
        }

        [TestMethod]
        public void ShouldThrowWithNullOrEmptyEmail()
        {
            Assert.ThrowsException<ArgumentException>(() => new Candidate("Pete Skelly", "", "555-9999", "ThreeWill"));
            Assert.ThrowsException<ArgumentNullException>(() => new Candidate("Pete Skelly", null, "555-9999", "ThreeWill"));
        }

        [TestMethod]
        public void ShouldThrowWithNullOrEmptyCompany()
        {
            Assert.ThrowsException<ArgumentException>(() => new Candidate("Pete Skelly", "", "555-9999", ""));
            Assert.ThrowsException<ArgumentNullException>(() => new Candidate("Pete Skelly", null, "555-9999", null));
        }

        [TestMethod]
        public void DeserilaizesThreeWillIngest()
        {
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../ThreeWillFlowInput.json");
            string flowMessage = File.ReadAllText(filePath);
            JsonSerializerOptions options = new() { 
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
            FlowIngest flowInput = JsonSerializer.Deserialize<FlowIngest>(flowMessage,options);
            Assert.AreEqual(flowInput.Recruiter, "pskelly@threewill.com");
            Assert.AreEqual(flowInput.Company, "ThreeWill");
            Assert.AreEqual(flowInput.FileName, "8e294c8c-b3a2-43f8-8d91-51acf75fc5e4.docx");            
        }

        [TestMethod]
        public void DeserilaizesThompsonIngest()
        {
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../ThompsonFlowInput.json");
            string flowMessage = File.ReadAllText(filePath);
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();
            Assert.AreEqual(flowInput.Recruiter, "pskelly@threewill.com");
            Assert.AreEqual(flowInput.Company, "Thompson");
            Assert.AreEqual(flowInput.FileName, "5085578f-e0c1-4dd6-9d5e-80b7d2615d66.doc");            
        }

        [TestMethod]
        public void DeserilaizesHireNetworksIngest()
        {
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../HireNetworksFloInput.json");
            string flowMessage = File.ReadAllText(filePath);
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();
            Assert.AreEqual(flowInput.Recruiter, "pskelly@threewill.com");
            Assert.AreEqual(flowInput.Company, "HireNetworks");
            Assert.AreEqual(flowInput.FileName, "87048d1a-985a-4705-8eed-6f1562a98ea3.docx");            
        }


        [TestMethod]
        public void DoesDeserilaizeThreeWillCandidate()
        {
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../ThreeWillFlowInput.json");
            string flowMessage = File.ReadAllText(filePath);
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();
           Assert.AreEqual("pskelly@threewill.com", flowInput.Recruiter);
            Assert.AreEqual("ThreeWill", flowInput.Company);
            Assert.AreEqual("8e294c8c-b3a2-43f8-8d91-51acf75fc5e4.docx", flowInput.FileName);        

            ICandidateProcessor processor = new CandidateProcessorFactory().CreateCandidateProcessor(flowInput.Company);

            Assert.IsInstanceOfType(processor, typeof(ThreeWillCandidateProcessor));       
            Candidate candidate = processor.Process(flowInput.BodyHtml);
            Assert.AreEqual("ThreeWill", candidate.Company);
            Assert.AreEqual("Timm Peddie", candidate.Name);
            Assert.AreEqual("(415) 484-3004", candidate.Phone);
            Assert.AreEqual("timm_peddie@yahoo.com", candidate.EmailAddress);          
           
        }

        [TestMethod]
        public void DoesDeserilaizeThompsonCandidate()
        {
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../ThompsonFlowInput.json");
            string flowMessage = File.ReadAllText(filePath);
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();
            Assert.AreEqual("pskelly@threewill.com", flowInput.Recruiter);
            Assert.AreEqual("Thompson", flowInput.Company);
            Assert.AreEqual("5085578f-e0c1-4dd6-9d5e-80b7d2615d66.doc", flowInput.FileName);     

            ICandidateProcessor processor = new CandidateProcessorFactory().CreateCandidateProcessor(flowInput.Company);

            Assert.IsInstanceOfType(processor, typeof(ThompsonCandidateProcessor));       
            Candidate candidate = processor.Process(flowInput.BodyHtml);
            Assert.AreEqual("Thompson", candidate.Company);
            Assert.AreEqual("Sean Jones", candidate.Name);
            Assert.AreEqual("(508) 410-5429", candidate.Phone);
            Assert.AreEqual("seanj516@gmail.com", candidate.EmailAddress);          
        }

        [TestMethod]
        public void DoesDeserilaizeStratfieldCandidate()
        {
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../StratfieldFlowInput.json");
            string flowMessage = File.ReadAllText(filePath);
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();
            Assert.AreEqual("ebrady@stratfieldconsulting.com", flowInput.Recruiter);
            Assert.AreEqual("Stratfield", flowInput.Company);
            Assert.AreEqual("8b4a016a-a33d-416d-811c-204c4795b86a.docx", flowInput.FileName); 

            ICandidateProcessor processor = new CandidateProcessorFactory().CreateCandidateProcessor(flowInput.Company);
            Assert.IsInstanceOfType(processor, typeof(StratfieldCandidateProcessor));       
            Candidate candidate = processor.Process(flowInput.BodyHtml);
            Assert.AreEqual("Stratfield", candidate.Company);
            Assert.AreEqual("Jean-Patrick Guichard", candidate.Name);
            Assert.AreEqual(Constants.FIELD_DEFAULT, candidate.Phone);
            Assert.AreEqual(Constants.FIELD_DEFAULT, candidate.EmailAddress);          
        }
    }
}
