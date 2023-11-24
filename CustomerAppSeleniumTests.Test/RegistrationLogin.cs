using OpenQA.Selenium;

namespace CustomerAppSeleniumTests.Test
{
    public class RegistrationLogin
    {
        private IWebDriver _driver;
        public RegistrationLogin(IWebDriver driver)
        {
            _driver = driver;
        }

        private By regLogin = By.CssSelector("input[class='regInput']");
        private IWebElement RegLogin => _driver.FindElement(regLogin);

        private By regLoginClick = By.CssSelector("button[class='button']");
        private IWebElement BtnRegLoginClick => _driver.FindElement(regLoginClick);

        private By yesRadioBtn = By.CssSelector("input[id='Yes']");
        private IWebElement YesRadioBtn => _driver.FindElement(yesRadioBtn);

        private By noRadioBtn = By.CssSelector("input[id='No']");
        private IWebElement NoRadioBtn => _driver.FindElement(noRadioBtn);

        private By continueBtn = By.CssSelector("button[id='buttonactive']");
        private IWebElement ContinueBtn => _driver.FindElement(continueBtn);

        private By returnHomeLink = By.Id("return-to-homepage");
        private IWebElement ReturnHomeLink => _driver.FindElement(returnHomeLink);

        private By incorrectTaxStatus = By.CssSelector("button[id='IncorrectTaxStatus']");
        private IWebElement IncorrectTaxStatus => _driver.FindElement(incorrectTaxStatus);

        private By incorrectMOTStatus = By.CssSelector("button[id='IncorrectMOTStatus']");
        private IWebElement IncorrectMOTStatus => _driver.FindElement(incorrectMOTStatus);


        public void RegLoginApplication(string regLogin)
        {
            RegLogin.SendKeys(regLogin);
            BtnRegLoginClick.Click();
        }

        public void ReturnToHomePageFromConfirmationPage()
        {
            ReturnHomeLink.Click();
        }
        public void RegLoginDetails(string regLogin)
        {
            RegLogin.SendKeys(regLogin);
            BtnRegLoginClick.Click();
            YesRadioBtn.Click();
            ContinueBtn.Click();
            IncorrectTaxStatus.Click();
            IncorrectMOTStatus.Click();
        }

        public void ConfirmationPageNoClick()
        {
            NoRadioBtn.Click();
            ContinueBtn.Click();
        }
    }

}
