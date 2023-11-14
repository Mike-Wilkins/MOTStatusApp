using Moq;
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
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
                },
                new MOTStatusDetails()
                {
                    Id = 12,
                    RegistrationNumber = "GK86RPJ",
                    Make = "Range Rover",
                    DateOfRegistration = "27/11/2019",
                    CylinderCapacity = "2895",
                    CO2Emissions = "230",
                    FuelType = "DIESEL",
                    EuroStatus = "Not available",
                    RealDrivingEmissions = "Not available",
                    ExportMarker = "NO",
                    VehicleStatus = "Not Taxed",
                    VehicleColour = "BLACK",
                    VehicleTypeApproval = "M1",
                    WheelPlan = "4X4",
                    RevenueWeight = "Not available",
                    DateOfLastV5C = "17/08/2022",
                    Taxed = false,
                    TaxDueDate = "17/08/2023",
                    MOTed = false,
                    MOTDueDate = "04/08/2023",
                    DateOfLastMOT = "04/08/2022"
                }
            };

            Mock<IMOTStatusDetailsRepository> mockDetailRepository = new Mock<IMOTStatusDetailsRepository>();


            mockDetailRepository.Setup(p => p.Delete(It.IsAny<MOTStatusDetails>())).Returns(
                 (MOTStatusDetails target) =>
                 {

                     if (target.Id.Equals(0))
                     {
                         return false;
                     }
                     else
                     {
                         target.Id = details.Count() - 1;
                         details.Remove(target);
                     }

                     return true;
                 });


            mockDetailRepository.Setup(p => p.Update(It.IsAny<MOTStatusDetails>())).Returns(
                (MOTStatusDetails target) => 
                {
                    if (target.Id.Equals(0))
                    {
                        return false;
                    }
                    return true;
                });


            mockDetailRepository.Setup(p => p.GetStatusDetail(It.IsAny<int>())).Returns((int i) => details.FirstOrDefault(x => x.Id == i));
            mockDetailRepository.Setup(p => p.GetRegistrationNumber(It.IsAny<string>())).Returns((string i) => details.FirstOrDefault(x => x.RegistrationNumber == i));
            mockDetailRepository.Setup(p => p.GetStatusDetails()).Returns(details);
            mockDetailRepository.Setup(p => p.Add(It.IsAny<MOTStatusDetails>())).Returns(
                (MOTStatusDetails target) =>
                {
                    if (target.Id.Equals(0))
                    {
                        target.Id = details.Count() + 1;
                        details.Add(target);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                });

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

        [Fact]
        public void GetRegistrationNumber_Should_Return_Vehicle_Details()
        {
            MOTStatusDetails details = this.MockDetailRepository.GetRegistrationNumber("NA27TSQ");

            Assert.NotNull(details);
            Assert.IsType<MOTStatusDetails>(details);
            Assert.Equal("NA27TSQ", details.RegistrationNumber);
            Assert.Equal("Ford", details.Make);

        }
        [Fact]
        public void Add_Should_Insert_New_Vehicle_Details()
        {
            int detailsCount = this.MockDetailRepository.GetStatusDetails().Count;
            Assert.Equal(2, detailsCount);

            MOTStatusDetails newVehicle = new MOTStatusDetails
            {
                RegistrationNumber = "RY07KEJ",
                Make = "Audi",
                DateOfRegistration = "10/11/2019",
                CylinderCapacity = "1800",
                CO2Emissions = "106",
                FuelType = "DiESEL",
                EuroStatus = "Not available",
                RealDrivingEmissions = "Not available",
                ExportMarker = "NO",
                VehicleStatus = "Taxed",
                VehicleColour = "BLUE",
                VehicleTypeApproval = "M1",
                WheelPlan = "2 AXLE RIGID BODY",
                RevenueWeight = "Not available",
                DateOfLastV5C = "04/02/2023",
                Taxed = true,
                TaxDueDate = "04/02/2024",
                MOTed = true,
                MOTDueDate = "18/01/2024",
                DateOfLastMOT = "18/01/2023"
            };


            this.MockDetailRepository.Add(newVehicle);
            this.MockDetailRepository.Save();

            int updatedDetailsCount = this.MockDetailRepository.GetStatusDetails().Count;
            Assert.Equal(3, updatedDetailsCount);

            //Verify new Vehicle detail has been saved
            MOTStatusDetails details = this.MockDetailRepository.GetRegistrationNumber("RY07KEJ");

            Assert.NotNull(details);
            Assert.IsType<MOTStatusDetails>(details);
            Assert.Equal("RY07KEJ", details.RegistrationNumber);
            Assert.Equal("Audi", details.Make);

        }

        [Fact]
        public void Delete_Should_Remove_Vehicle_Details()
        {
            int detailsCount = this.MockDetailRepository.GetStatusDetails().Count;
            Assert.Equal(2, detailsCount);

            MOTStatusDetails newVehicle = new MOTStatusDetails
            {
                RegistrationNumber = "RY07KEJ",
                Make = "Audi",
                DateOfRegistration = "10/11/2019",
                CylinderCapacity = "1800",
                CO2Emissions = "106",
                FuelType = "DiESEL",
                EuroStatus = "Not available",
                RealDrivingEmissions = "Not available",
                ExportMarker = "NO",
                VehicleStatus = "Taxed",
                VehicleColour = "BLUE",
                VehicleTypeApproval = "M1",
                WheelPlan = "2 AXLE RIGID BODY",
                RevenueWeight = "Not available",
                DateOfLastV5C = "04/02/2023",
                Taxed = true,
                TaxDueDate = "04/02/2024",
                MOTed = true,
                MOTDueDate = "18/01/2024",
                DateOfLastMOT = "18/01/2023"
            };


            this.MockDetailRepository.Add(newVehicle);
            this.MockDetailRepository.Save();

            int updatedDetailsCount = this.MockDetailRepository.GetStatusDetails().Count;
            Assert.Equal(3, updatedDetailsCount);

            this.MockDetailRepository.Delete(newVehicle);
            this.MockDetailRepository.Save();

            int updatedDeleteCount = this.MockDetailRepository.GetStatusDetails().Count;
            Assert.Equal(2, updatedDeleteCount);

        }

        [Fact]
        public void Update_Should_Ammend_Vehicle_Details()
        {
            MOTStatusDetails details = this.MockDetailRepository.GetStatusDetail(11);

            Assert.NotNull(details);
            Assert.IsType<MOTStatusDetails>(details);
            Assert.Equal("NA27TSQ", details.RegistrationNumber);
            Assert.Equal("Ford", details.Make);

            details.RegistrationNumber = "GT04PRQ";

            this.MockDetailRepository.Update(details);
            this.MockDetailRepository.Save();

            Assert.Equal("GT04PRQ", details.RegistrationNumber);


            MOTStatusDetails updatedDetails = this.MockDetailRepository.GetStatusDetail(11);
            Assert.Equal("GT04PRQ", updatedDetails.RegistrationNumber);

            int verifyVehicleCount = this.MockDetailRepository.GetStatusDetails().Count;
            Assert.Equal(2, verifyVehicleCount);
        }


    }
}
