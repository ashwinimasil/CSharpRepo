using Addition.StepDefinition;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace Addition.stepDefinition
{
    [Binding]
    public class Case
    {
        private readonly WebDriverContext _context;
        public WebDriverWait wait;
        private IWebDriver Driver
        {
            get
            {
                if (_context?.Driver != null) return _context.Driver;
                if (Hooks.Driver != null) return Hooks.Driver;

                throw new NullReferenceException("WebDriver context is null. Ensure Hooks.Driver or WebDriverContext is initialized before this step runs.");
            }
        }
        public Case (WebDriverContext context)
        {
            _context = context;
        }
        [Given(@"Click on the Accounts")]
        public void learnCase()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            Driver.FindElement(By.XPath("//a[text()='Create Case']")).Click();

            string mainWindowHandle = Driver.CurrentWindowHandle;

            wait.Until(d => d.FindElement(By.XPath("//input[@id='createCaseForm_accountPartyId']/following-sibling::a"))).Click();

            // driver.FindElement(By.XPath("//input[@id='createCaseForm_accountPartyId']/following::a[1]")).Click();

            Thread.Sleep(5000);

            int windowCount = Driver.WindowHandles.Count;
            System.Console.Out.WriteLine($"Total open windows: {windowCount}");

            Driver.SwitchTo().Window(Driver.WindowHandles[1]);

            System.Console.Out.WriteLine("Control successfully moved to the 2nd window!");
        }
        [When(@"Enter the Account name Find Accounts")]
        public void FindAccounts() {

            Driver.FindElement(By.XPath("//input[@name='accountName']")).SendKeys("Test");

            TestContext.WriteLine("Clicked case");

            Thread.Sleep(5000);

            Driver.FindElement(By.XPath("//button[text()='Find Accounts']")).Click();

            System.Console.Out.WriteLine("Clicked Find Accounts");

            Driver.FindElement(By.XPath("(//label[text()='Account Name:']/following::div/input)[1]")).Click();
        }
        [Then(@"Print the number of pages")]
        public void PageNo() 
        {
            string page1 = Driver.FindElement(By.XPath("(//span[@class='ytb-text'])[1]")).Text;

            string pageno = Driver.FindElement(By.XPath("(//span[@class='ytb-text'])[1]/following::td[1]/input")).GetAttribute("value");

            string page2 = Driver.FindElement(By.XPath("(//span[@class='ytb-text'])[2]")).Text;

            string page3 = Driver.FindElement(By.XPath("(//span[@class='ytb-text'])[1]")).Text;

            System.Console.Out.WriteLine($"{page1} {pageno} {page2} {page3}");

            var AccID = Driver.FindElements(By.XPath("//div[@class='x-grid3-cell-inner x-grid3-col-partyId']"));

            foreach (var AccountID in AccID)
            {
                System.Console.Out.WriteLine(AccountID.Text);

            }
            System.Console.Out.Flush();

            //ReadOnlyCollection<IWebElement> AccNames =driver.FindElements(By.XPath(""));

            //foreach (IWebElement AccNames in Driver.FindElements(By.XPath("//td[@class='x-grid3-col x-grid3-cell x-grid3-td-groupName ']")))
            //{
            //    System.Console.Out.WriteLine(AccNames.Text);

            //}
            Driver.SwitchTo().Window(Driver.WindowHandles[0]);

            Driver.FindElement(By.XPath("//input[@value='Create Case']")).Click();

            System.Console.Out.WriteLine("Create case button is clicked");

            //Screenshot sc = ((ITakesScreenshot)Driver).GetScreenshot();

            //sc.SaveAsFile(@"C:\Screenshots\CreateCase.png");

        }
    }
}