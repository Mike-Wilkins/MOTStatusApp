using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace CustomerAppSeleniumTests.Test
{
    public class SeleniumTests : IDisposable
    {
        private IWebDriver _driver;
        private RegistrationLogin _registrationLogin;
        public SeleniumTests()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl("https://localhost:7131/");
            _registrationLogin = new RegistrationLogin(_driver);
        }

        [Fact]
        public void Navigate_To_VehicleDetails_ConfirmationPage_And_Return_To_Homepage()
        {
            
            Assert.Equal("Home Page - AdminApp", _driver.Title);
            _registrationLogin.RegLoginApplication("FD09ENX");
            Assert.Equal("- AdminApp", _driver.Title);
            _registrationLogin.ReturnToHomePageFromConfirmationPage();
            Assert.Equal("Home Page - AdminApp", _driver.Title);         

        }

        [Fact]
        public void Return_To_HomePage_On_No_Click_From_Confirmation_Page()
        {
            Assert.Equal("Home Page - AdminApp", _driver.Title);
            _registrationLogin.RegLoginApplication("FD09ENX");
            Assert.Equal("- AdminApp", _driver.Title);
            _registrationLogin.ConfirmationPageNoClick();
            Assert.Equal("Home Page - AdminApp", _driver.Title);
        }

        [Fact]
        public void Navigate_To_VehicleDetails_Page()
        {
            Assert.Equal("Home Page - AdminApp", _driver.Title);
            _registrationLogin.RegLoginDetails("FD09ENX");
            Assert.Equal("- AdminApp", _driver.Title);
            Assert.Contains("If you've just taxed or made a SORN ", _driver.PageSource);
            Assert.Contains("If the MOT status is incorrect", _driver.PageSource);
          
        }

        [Fact]
        public void Show_Error_Message_When_VehicleNotFound_Submitted()
        {
            _registrationLogin.RegLoginApplication("RT04UYT");
            Assert.Equal("Home Page - AdminApp", _driver.Title);
            Assert.Contains("Vehicle registration was not found", _driver.PageSource);

        }

        [Fact]
        private void Show_Error_Message_When_Invalid_Characters_Submitted()
        {
            _registrationLogin.RegLoginApplication("%^$£!^");
            Assert.Equal("Home Page - AdminApp", _driver.Title);
            Assert.Contains("Provide a valid vehicle registration", _driver.PageSource);
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}