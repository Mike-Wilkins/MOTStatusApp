using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Selenium.Test
{
    public class CustomerAppSeleniumTests
    {
        private IWebDriver _driver;

        public CustomerAppSeleniumTests()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
        }

        [Fact]
        public void CorrectTitleDisplayed_When_NavigateToHoePage()
        {
            _driver.Navigate().GoToUrl("https://www.lotuseaters.com/category/podcast");

            Assert.Equal("Podcast | Lotus Eaters", _driver.Title);

        }
    }
}