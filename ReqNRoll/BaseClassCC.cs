using Reqnroll;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace Addition.StepDefinition
{
    [Binding]
    public class Hooks
    {
        // 1. We completely removed the private "driver" variable to prevent confusion
        private readonly WebDriverContext _context;

        public static IWebDriver Driver;

        public Hooks(WebDriverContext context)
        {
            _context = context;

            if (_context != null && Driver != null)
            {
                _context.Driver = Driver;
            }
        }
        [BeforeFeature]
        public static void SetUP()
        {
            ChromeOptions options = new ChromeOptions();

            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-infobars");

            // 2. FIXED: Save the running session directly to the shared context container
            Driver = new ChromeDriver(options);           
         
        
            Driver.Navigate().GoToUrl("https://leaftaps.com/opentaps/control/main");
            Driver.Manage().Window.Maximize();
            // 3. FIXED: Use the shared context container to look up the login elements
            Driver.FindElement(By.Id("username")).SendKeys("DemoCSR2");
            Driver.FindElement(By.Id("password")).SendKeys("crmsfa");
            Driver.FindElement(By.XPath("//input[@value='Login']")).Click();

            Thread.Sleep(5000);

            Driver.FindElement(By.XPath("//a[contains(text(),'CRM/SFA')]")).Click();
                    }
        [AfterFeature]
        public static void Logout()
        {
            try
            {
                // Logout from Leaftaps
                Driver.FindElement(By.LinkText("Logout")).Click();

                TestContext.WriteLine("User logged out successfully.");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Logout failed: {ex.Message}");
            }
        
        // 4. FIXED: Clean up using the shared context instance
        
            if (Driver != null)
            {
                Driver.Quit();
            }
        }
    }
}