using System;
using System.IO;
using System.Text.Json;
using CandidateManager.Core;
using CandidateManager.Core.Extensions;
using CandidateManager.Core.Interfaces;
using CandidateManager.Core.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace CandidateManager.Tests
{
    [TestClass]
    public class CandidateTests
    {
        [TestMethod]
        public void ShouldThrowWithEmptyName()
        {
            Should.Throw<ArgumentException>(() => new Candidate("", "pskelly@threewill.com","999-0000","ThreeWill"));
            Should.Throw<ArgumentNullException>(() => new Candidate(null, "pskelly@threewill.com","999-0000","ThreeWill"));
        }

        [TestMethod]
        public void ShouldThrowWithEmptyOrNullPhone()
        {
            Should.Throw<ArgumentException>(() => new Candidate("Pete Skelly",  "", "999-0000", "ThreeWill"));
            Should.Throw<ArgumentNullException>(() => new Candidate("Pete Skelly", null, "999-0000", "ThreeWill"));
        }

        [TestMethod]
        public void ShouldThrowWithNullOrEmptyEmail()
        {
            Assert.ThrowsException<ArgumentException>(() => new Candidate("Pete Skelly", "", "555-9999", "ThreeWill"));
            Assert.ThrowsException<ArgumentNullException>(() => new Candidate("Pete Skelly", null, "555-9999", "ThreeWill"));

            Should.Throw<ArgumentException>(() => new Candidate("Pete Skelly", "", "555-9999", "ThreeWill"));
            Should.Throw<ArgumentNullException>(() => new Candidate("Pete Skelly", null, "555-9999", "ThreeWill"));

        }

        [TestMethod]
        public void ShouldThrowWithNullOrEmptyCompany()
        {
           Should.Throw<ArgumentException>(() => new Candidate("Pete Skelly", "", "555-9999", ""));
           Should.Throw<ArgumentNullException>(() => new Candidate("Pete Skelly", null, "555-9999", null));
        }

        [TestMethod]
        public void DeserilaizesThreeWillIngest()
        {
            //Arrange
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../ThreeWillFlowInput.json");
            string flowMessage = File.ReadAllText(filePath);

            //Act
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();

            //Assert
            flowInput.Recruiter.ShouldBe("pskelly@threewill.com");
            flowInput.Company.ShouldBe("ThreeWill");
            flowInput.FileName.ShouldBe("8e294c8c-b3a2-43f8-8d91-51acf75fc5e4.docx");            

        }

        [TestMethod]
        public void DeserilaizesThompsonIngest()
        {
            //Arrange
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../ThompsonFlowInput.json");
            string flowMessage = File.ReadAllText(filePath);

            //Act
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();
            
            //Assert
            flowInput.Recruiter.ShouldBe("pskelly@threewill.com");
            flowInput.Company.ShouldBe("Thompson");
            flowInput.FileName.ShouldBe("5085578f-e0c1-4dd6-9d5e-80b7d2615d66.doc");            
        }

        [TestMethod]
        public void DeserilaizesHireNetworksIngest()
        {
            //Arrange
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../HireNetworksFloInput.json");
            string flowMessage = File.ReadAllText(filePath);
            
            //Act
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();
            
            //Assert
            flowInput.Recruiter.ShouldBe("pskelly@threewill.com");
            flowInput.Company.ShouldBe("HireNetworks");
            flowInput.FileName.ShouldBe("87048d1a-985a-4705-8eed-6f1562a98ea3.docx");
        }


        [TestMethod]
        public void DoesDeserilaizeThreeWillCandidate()
        {
            //Arrange
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../ThreeWillFlowInput.json");
            string flowMessage = File.ReadAllText(filePath);
            
            //Act
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();           
            ICandidateProcessor processor = CandidateProcessorFactory.CreateCandidateProcessor(flowInput.Company);
            Candidate candidate = processor.Process(flowInput.BodyHtml);

            //Assert
            processor.ShouldBeAssignableTo<ThreeWillCandidateProcessor>();
            candidate.Company.ShouldBe("ThreeWill");
            candidate.Name.ShouldBe("Timm Peddie");
            candidate.Phone.ShouldBe("(415) 484-3004");
            candidate.EmailAddress.ShouldBe("timm_peddie@yahoo.com");          
           
        }

        [TestMethod]
        public void DoesDeserilaizeThompsonCandidate()
        {

            //Arrange
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../ThompsonFlowInput.json");
            string flowMessage = File.ReadAllText(filePath);

            //Act
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();
            ICandidateProcessor processor = CandidateProcessorFactory.CreateCandidateProcessor(flowInput.Company);
            Candidate candidate = processor.Process(flowInput.BodyHtml);

            //Assert
            processor.ShouldBeAssignableTo<ThompsonCandidateProcessor>();
            candidate.Company.ShouldBe("Thompson");
            candidate.Name.ShouldBe("Sean Jones");
            candidate.Phone.ShouldBe("(508) 410-5429");
            candidate.EmailAddress.ShouldBe("seanj516@gmail.com");          

        }

        [TestMethod]
        public void DoesDeserilaizeStratfieldCandidate()
        {

            //Arrange
            string curDir = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(curDir, "../../../StratfieldFlowInput.json");
            string flowMessage = File.ReadAllText(filePath);
    
            //Act
            FlowIngest flowInput = flowMessage.FromJson<FlowIngest>();
            ICandidateProcessor processor = CandidateProcessorFactory.CreateCandidateProcessor(flowInput.Company);
            Candidate candidate = processor.Process(flowInput.BodyHtml);
    
            //Assert
            processor.ShouldBeAssignableTo<StratfieldCandidateProcessor>();
            candidate.Company.ShouldBe("Stratfield");
            candidate.Name.ShouldBe("Jean-Patrick Guichard");
            candidate.Phone.ShouldBe(Constants.FIELD_DEFAULT); 
            candidate.EmailAddress.ShouldBe(Constants.FIELD_DEFAULT);           
        }
    }
}
