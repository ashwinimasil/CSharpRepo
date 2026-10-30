using OpenQA.Selenium;

namespace Addition.StepDefinition
{
    public class WebDriverContext
    {
        public IWebDriver Driver { get; set; } = null!;
    }
}