using Xunit;
using System;
using AdminApp.Controllers;

namespace TestMOTStatusApp.Test
{
    public class AdminAppTests
    {
       
        [Theory]
        [InlineData("")]
        [InlineData("!£$%^&*()_+=-|,@';:~#`¬./")]
        public void VehicleRegEx_Should_Not_Accept_Special_Characters(string reg)
        {
            bool regIsValid = HomeController.VehicleRegEx(reg);

            //Assert
            Assert.False(regIsValid);    
        }

        [Fact]
        public void VehicleRegEx_Accepts_UK_Reg_Format()
        {
            //Arrange
            string reg = "FD07HTY";

            //Act
            bool regIsValid = HomeController.VehicleRegEx(reg);

            //Assert
            Assert.True(regIsValid);
        }

        [Theory]
        [InlineData("A1")]
        [InlineData("AAA111")]
        [InlineData("AAA1111")]
        [InlineData("AA11AA")]
        [InlineData("A1AAA")]
        [InlineData("A11AAA")]
        [InlineData("A111AAA")]
        [InlineData("111A")]
        [InlineData("AA111")]
        public void VehicleRegEx_Accepts_Private_Registrations(string reg)
        {
            //Act
            bool regIsValid = HomeController.VehicleRegEx(reg);

            //Assert
            Assert.True(regIsValid);
        }

        [Fact]
        public void ForMatReg_Should_Return_CharSpace_Insert()
        {
            //Arrange
            string inputReg = "FG09DFR";
            string expectedReg = "FG09 DFR";

            //Act
            string actualReg = HomeController.FormatReg(inputReg);

            //Assert
            Assert.Equal(expectedReg, actualReg);
        }

        [Fact]      
        public void IsVehicleTaxedAndMOTed_Should_Return_Correct_Bool()
        {
            //Arrange
            string inputDateIsTaxed = DateTime.Now.AddDays(1).ToString();
            string inputDateIsNotTaxed = DateTime.Now.AddDays(-1).ToString();

            //Act
            bool actualOutputIsTaxed = HomeController.IsVehicleTaxedAndMOTed(inputDateIsTaxed);
            bool actualOutputIsNotTaxed = HomeController.IsVehicleTaxedAndMOTed(inputDateIsNotTaxed);

            //Assert
            Assert.True(actualOutputIsTaxed);
            Assert.False(actualOutputIsNotTaxed);
        }

        [Fact]
        public void UpdateVehicleDueDate_Should_Add_One_Year_To_Input_Date()
        {
            //Arrange
            string inputDate = "03/11/2022";
            string expectedOutput = "03/11/2023";

            //Act
            string outputDate = HomeController.UpdateVehicleDueDate(inputDate);

            //Assert
            Assert.Equal(expectedOutput, outputDate);
        }

        [Fact]    
        public void IsTaxedLabel_Should_Return_Correct_String_Given_Boolean_Input()
        {
            //Arrange
            string vehicleIsTaxed = "Taxed";
            string vehicleIsNotTaxed = "Not Taxed";

            //Act
            string actualTaxedLabel = HomeController.IsTaxedLabel(true);
            string actualNotTaxedLabel = HomeController.IsTaxedLabel(false);


            //Assert
            Assert.Equal(vehicleIsTaxed, actualTaxedLabel);
            Assert.Equal(vehicleIsNotTaxed, actualNotTaxedLabel);
        }

        [Fact]
        public void FormatDate_Should_Return_Formated_Date()
        {
            //Arrange
            string inputDate = "02/04/22";
            string expectedOutput = "02/04/2022";

            //Act
            string actualDate = HomeController.FormatDate(inputDate);

            //Assert
            Assert.Equal(expectedOutput, actualDate);
        }
    }
}