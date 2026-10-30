using Addition.StepDefinition;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V153.FedCm;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
namespace Addition.StepDefinition
{
    [Binding]
    public class MyAccounts
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
        public MyAccounts(WebDriverContext context)
        {
            _context = context;
        }
        [Given(@"Click on My Accounts")]
        public void Accounts()
        {
            // 1. ADD THIS: Wait up to 10 seconds for at least one row to appear on the page
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            IWebElement accountsTab = wait.Until(d => d.FindElement(By.XPath("//a[text()='Accounts']")));
            accountsTab.Click();
        }

        [When(@"Fetch the Account id and Account Name")]
        public void FetchAccidName()
        {
            //Dictionary<int, string> Account = new Dictionary<int, string>();

            HashSet<int> processedIds = new HashSet<int>();

            while (true)
            {
                var rows = Driver.FindElements(
                By.XPath("//div[contains(@class,'x-grid3-body')]//table"));

                int recordsAddedThisPage = 0;

                foreach (var row in rows)
                {
                    try
                    {
                        IWebElement idElement =
                            row.FindElement(By.CssSelector(".x-grid3-col-partyId"));

                        IWebElement nameElement =
                            row.FindElement(By.CssSelector(".x-grid3-col-groupName"));

                        int accountId = int.Parse(idElement.Text.Trim());
                        string accountName = nameElement.Text.Trim();

                        if (processedIds.Add(accountId))
                        {
                            recordsAddedThisPage++;

                            TestContext.WriteLine(
                                $"{accountId}  \t \t {accountName}");
                        }
                    }
                    catch (Exception e)
                    {
                        TestContext.WriteLine(e);
                    }
                }
                IWebElement nextButton = Driver.FindElement(
                    By.CssSelector("button.x-tbar-page-next"));

                // Last page reached
                if (!nextButton.Enabled || recordsAddedThisPage == 0)
                {
                    break;
                }
                nextButton.Click();

                //Screenshot sc = ((ITakesScreenshot)Driver).GetScreenshot();

                //sc.SaveAsFile(@"C:\Screenshots\MyAccounts.png");

                Thread.Sleep(2000);
            }
        }
        
    }
}