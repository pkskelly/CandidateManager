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

        // [TestMethod]
        // public void DoesDeserilaizeHireNetworksCandidateChannel()
        // {
        //     Candidate candidate = new Candidate();
        //     candidate.Name = "Pete Skelly";
        //     candidate.Company = CandidateChannel.HireNetworks;
        //     candidate.EmailAddress = "pskelly@acme.com";
        //     candidate.Phone = "555-1212";
        //     string json = JsonConvert.SerializeObject(candidate);
        //     Assert.IsNotNull(json);
        //     Assert.AreEqual(CandidateChannel.HireNetworks, candidate.Company);
        //     Candidate output = JsonConvert.DeserializeObject<Candidate>(json);
        //     Assert.AreEqual(CandidateChannel.HireNetworks, output.Company);
        // }

        // [TestMethod]
        // public void DoesReturnThompsonCandidateProcessor()
        // {
        //     Candidate candidate = new Candidate();
        //     candidate.Name = "Pete Skelly";
        //     candidate.Company = CandidateChannel.Thompson;
        //     candidate.EmailAddress = "pskelly@acme.com";
        //     candidate.Phone = "555-1212";
        //     ICandidateProcessor processor = new CandidateProcessorFacotry().CreateCandidateProcessor(candidate.Company);
        //     Assert.IsNotNull(processor);
        //     Assert.IsInstanceOfType(processor, typeof(ThompsonTechCandidateProcessor));
        // }

        // [TestMethod]
        // public void DoesReturnThreeWillCandidateProcessor()
        // {
        //     Candidate candidate = new Candidate();
        //     candidate.Name = "Pete Skelly";
        //     candidate.Company = CandidateChannel.ThreeWill;
        //     candidate.EmailAddress = "pskelly@acme.com";
        //     candidate.Phone = "555-1212";
        //     ICandidateProcessor processor = new CandidateProcessorFacotry().CreateCandidateProcessor(candidate.Company);
        //     Assert.IsNotNull(processor);
        //     Assert.IsInstanceOfType(processor, typeof(ThreeWillCandidateProcessor));
        // }

        // [TestMethod]
        // public void DoesReturnMatrixCandidateProcessor()
        // {
        //     Candidate candidate = new Candidate();
        //     candidate.Name = "Pete Skelly";
        //     candidate.Company = CandidateChannel.Matrix;
        //     candidate.EmailAddress = "pskelly@acme.com";
        //     candidate.Phone = "555-1212";
        //     ICandidateProcessor processor = new CandidateProcessorFacotry().CreateCandidateProcessor(candidate.Company);
        //     Assert.IsNotNull(processor);
        //     Assert.IsInstanceOfType(processor, typeof(MatrixCandidateProcessor));
        // }

        // [TestMethod]
        // public void DoesReturnHireNetworksCandidateProcessor()
        // {
        //     Candidate candidate = new Candidate();
        //     candidate.Name = "Pete Skelly";
        //     candidate.Company = CandidateChannel.HireNetworks;
        //     candidate.EmailAddress = "pskelly@acme.com";
        //     candidate.Phone = "555-1212";
        //     ICandidateProcessor processor = new CandidateProcessorFacotry().CreateCandidateProcessor(candidate.Company);
        //     Assert.IsNotNull(processor);
        //     Assert.IsInstanceOfType(processor, typeof(HireNetworksCandidateProcessor));
        // }

        // [TestMethod]
        // public void ScraperUtiltiesGetTextElements()
        // {
        //     string curDir = Directory.GetCurrentDirectory();
        //     string filePath = Path.Combine(curDir, "..\\..\\ThreeWillFlowInput.json");
        //     string flowMessage = File.ReadAllText(filePath);

        //     FlowIngest flowInput = JsonConvert.DeserializeObject<FlowIngest>(flowMessage);
        //     List<string> textElements = ScraperUtilities.GetTextElements(flowInput.BodyHtml);
        //     Assert.AreEqual(85, textElements.Count);

        // }


        // [TestMethod]
        // public void DoesDeserilaizeThreeWillCandidateFlowMessage()
        // {
        //     CandidateChannel TEST_COMPANY = CandidateChannel.ThreeWill;
        //     string TEST_NAME = "Timm Peddie";
        //     string TEST_EMAIL = "timm_peddie@yahoo.com";
        //     string TEST_PHONE = "(415) 484-3004";

        //     string curDir = Directory.GetCurrentDirectory();
        //     string filePath = Path.Combine(curDir, "..\\..\\ThreeWillFlowInput.json");
        //     string flowMessage = File.ReadAllText(filePath);

        //     FlowIngest flowInput = JsonConvert.DeserializeObject<FlowIngest>(flowMessage);
        //     ICandidateProcessor processor = new CandidateProcessorFacotry().CreateCandidateProcessor(flowInput.Company);
           
        //     Assert.IsInstanceOfType(processor, typeof(ThreeWillCandidateProcessor));
        //     Candidate candidate = processor.Process(flowInput.BodyHtml);
        //     Assert.AreEqual(TEST_COMPANY, candidate.Company);
        //     Assert.AreEqual(TEST_NAME, candidate.Name);
        //     Assert.AreEqual(TEST_PHONE, candidate.Phone);
        //     Assert.AreEqual(TEST_EMAIL, candidate.EmailAddress);
        // }



        // [TestMethod]
        // public void DoesDeserilaizeThompsonCandidateFlowMessage()
        // {
        //     CandidateChannel TEST_COMPANY = CandidateChannel.Thompson;
        //     string TEST_NAME = "Sean Jones";
        //     string TEST_EMAIL = "seanj516@gmail.com";
        //     string TEST_PHONE = "(508) 410-5429";

        //     string curDir = Directory.GetCurrentDirectory();
        //     string filePath = Path.Combine(curDir, "..\\..\\ThompsonFlowInput.json");
        //     string flowMessage = File.ReadAllText(filePath);

        //     FlowIngest flowInput = JsonConvert.DeserializeObject<FlowIngest>(flowMessage);
        //     ICandidateProcessor processor = new CandidateProcessorFacotry().CreateCandidateProcessor(flowInput.Company);

        //     Assert.IsInstanceOfType(processor, typeof(ThompsonTechCandidateProcessor));
        //     Candidate candidate = processor.Process(flowInput.BodyHtml);
        //     Assert.AreEqual(TEST_COMPANY, candidate.Company);
        //     Assert.AreEqual(TEST_NAME, candidate.Name);
        //     Assert.AreEqual(TEST_PHONE, candidate.Phone);
        //     Assert.AreEqual(TEST_EMAIL, candidate.EmailAddress);
        // }

        // [TestMethod]
        // public void DoesDeserilaizeHireNetworksCandidateFlowMessage()
        // {
        //     CandidateChannel TEST_COMPANY = CandidateChannel.HireNetworks;
        //     string TEST_NAME = "Preston Harden";

        //     string curDir = Directory.GetCurrentDirectory();
        //     string filePath = Path.Combine(curDir, "..\\..\\HireNetworksFlowInput.json");
        //     string flowMessage = File.ReadAllText(filePath);

        //     FlowIngest flowInput = JsonConvert.DeserializeObject<FlowIngest>(flowMessage);
        //     ICandidateProcessor processor = new CandidateProcessorFacotry().CreateCandidateProcessor(flowInput.Company);

        //     Assert.IsInstanceOfType(processor, typeof(HireNetworksCandidateProcessor));
        //     Candidate candidate = processor.Process(flowInput.BodyHtml);
        //     Assert.AreEqual(TEST_COMPANY, candidate.Company);
        //     Assert.AreEqual(TEST_NAME, candidate.Name);
        //     Assert.AreEqual(Constants.FIELD_DEFAULT, candidate.Phone);
        //     Assert.AreEqual(Constants.FIELD_DEFAULT, candidate.EmailAddress);
        // }

    }
}
