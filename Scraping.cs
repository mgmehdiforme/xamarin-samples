using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

class Program
{
    static void Main(string[] args)
    {
        // Specify the path to the ChromeDriver executable
        string chromeDriverPath = @"C:\path\to\chromedriver.exe";

        // Set up ChromeDriver options (optional)
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--headless"); // Run Chrome in headless mode, without a visible window

        // Initialize ChromeDriver with options
        IWebDriver driver = new ChromeDriver(chromeDriverPath, options);

        try
        {
            // Navigate to the URL you want to scrape
            driver.Navigate().GoToUrl("https://example.com");

            // Use web scraping techniques to extract data from the page
            // For example, find elements by their HTML tags or classes
            IWebElement element = driver.FindElement(By.CssSelector(".example-class"));
            string scrapedData = element.Text;

            // Process the scraped data as needed
            Console.WriteLine(scrapedData);
        }
        finally
        {
            // Close the ChromeDriver when you're done
            driver.Quit();
        }
    }
}
