using Moq;
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestMOTStatusApp.Test
{
    public class TestRepository
    {
        public readonly IMOTStatusDetailsRepository MockDetailRepository;
        public TestRepository()
        {
            IList<MOTStatusDetails> details = new List<MOTStatusDetails>()
            {
                new MOTStatusDetails()
                {
                    Id = 11,
                    RegistrationNumber = "NA27TSQ",
                    Make = "Ford",
                    DateOfRegistration = "09/09/2020",
                    CylinderCapacity = "2478",
                    CO2Emissions = "210",
                    FuelType = "PETROL",
                    EuroStatus = "Not available",
                    RealDrivingEmissions = "Not available",
                    ExportMarker = "NO",
                    VehicleStatus = "Not Taxed",
                    VehicleColour = "BLACK",
                    VehicleTypeApproval = "M1",
                    WheelPlan = "4X4",
                    RevenueWeight = "Not available",
                    DateOfLastV5C = "12/04/2022",
                    Taxed = false,
                    TaxDueDate = "12/04/2023",
                    MOTed = false,
                    MOTDueDate = "14/04/2023",
                    DateOfLastMOT = "14/04/2022"
                }
            };

            Mock<IMOTStatusDetailsRepository> mockDetailRepository = new Mock<IMOTStatusDetailsRepository>();

            mockDetailRepository.Setup(p => p.GetStatusDetail(It.IsAny<int>())).Returns((int i) => details.FirstOrDefault(x => x.Id == i));

            this.MockDetailRepository = mockDetailRepository.Object;           
        }

        [Fact]
        public void GetStatusDetail_Should_Return_Vehicle_Details()
        {
            MOTStatusDetails details = this.MockDetailRepository.GetStatusDetail(11);

            Assert.NotNull(details);
            Assert.IsType<MOTStatusDetails>(details);
            Assert.Equal("NA27TSQ", details.RegistrationNumber);
            Assert.Equal("Ford", details.Make);
        }




    }
}
