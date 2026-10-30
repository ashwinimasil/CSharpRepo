using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using System;
using System.Threading;

namespace Addition.StepDefinition
{
    [Binding]
    public class CreateLeadCC
    {
        //private readonly IWebDriver _driver;
        private readonly WebDriverContext _context;
        private string _companyName;

        // Context injection safely connects this step file to the live browser session
        public CreateLeadCC(WebDriverContext context)
        {
            _context = context;
        }

       private IWebDriver Driver => _context.Driver ?? Hooks.Driver;
        // FIXED: Added the missing binding method for your Feature File's Background step
        
        [Given(@"Login to the Leaftaps application with valid credentials")]
        public void Background()
        {
            TestContext.WriteLine("Hooks have completed the setup and user authentication successfully.");
        }

        [Given(@"User logins to the application and navigate to the Home page")]
        public void NavigateToHomePage()
        {
            var shortcuts = Driver.FindElements(By.XPath("//div[@class='frameSectionBody']/ul/li"));
            Thread.Sleep(5000);

            foreach (IWebElement AllShortCuts in shortcuts)
            {
                TestContext.WriteLine(AllShortCuts.Text);
            }
        }

        [When(@"Click on Create Lead Button")]
        public void ClickCreateLead()
        {
            Thread.Sleep(2000);
            Driver.FindElement(By.XPath("//a[text()='Create Lead']")).Click();
            
        }
        [When(@"Enter the form with the details Company Name ""([^""]*)"" , Last Name ""([^""]*)"", First Name ""([^""]*)""")]
        public void EnterInfo(string companyNameInput, string lastNameInput,string firstNameInput)
        {
            _companyName = companyNameInput;
            // Note: If you face a NoSuchElementException at runtime here, remove the surrounding brackets 
            // and the index [2] to match the ID or name attribute uniquely.
            IWebElement CompanyElement = Driver.FindElement(By.XPath("//input[@id='createLeadForm_companyName']"));
            CompanyElement.Clear();
            CompanyElement.SendKeys(companyNameInput);

            IWebElement lastNameInputElement = Driver.FindElement(By.XPath("//input[@id='createLeadForm_lastName']"));
            lastNameInputElement.Clear();
            lastNameInputElement.SendKeys(lastNameInput);

            IWebElement FirstNameElement = Driver.FindElement(By.XPath("//input[@id='createLeadForm_firstName']"));
            FirstNameElement.Clear();
            FirstNameElement.SendKeys(firstNameInput);
            
        }
        [Then(@"Click on submit button")]
        public void ClickSubmitButton()
        {
            Driver.FindElement(By.XPath("//input[@name='submitButton']")).Click();
            string pageTitle = Driver.Title;

            Assert.That(pageTitle, Does.Contain("View Lead"));

            //Screenshot sc = ((ITakesScreenshot)Driver).GetScreenshot();

            //sc.SaveAsFile(@"C:\Screenshots\CreateLead.png");

            TestContext.WriteLine($"{_companyName} is created successfully and driver session ended.");
            Thread.Sleep(1000);
        }
    }
}
