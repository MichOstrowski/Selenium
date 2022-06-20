using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CreditCards.UITests.PageObjectModels
{
    class HomePage
    {
        private readonly IWebDriver Driver;
        private const string HomeUrl = "http://localhost:44108/";
        private const string HomeTitle = "Home Page - Credit Cards";

        public HomePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public ReadOnlyCollection<(string name, string interestRate)> Products
        {
            get
            {
                var products = new List<(string name, string interestRate)>();

                var productsCells = Driver.FindElements(By.TagName("td"));

                for (int i = 0; i < productsCells.Count; i += 2)
                {
                    string name = productsCells[i].Text;
                    string intrestRate = productsCells[i + 1].Text;
                    products.Add((name, intrestRate));
                }

                return products.AsReadOnly();
            }
        }

        public void NavigateTo()
        {
             Driver.Navigate().GoToUrl(HomeUrl);
            ShurePageLoaded();
        }

        public void ShurePageLoaded()
        {
            bool pageHasLoaded = (Driver.Url == HomeUrl) && (Driver.Title == HomeTitle);

            if (!pageHasLoaded)
            {
                throw new Exception($"Failed load page. Page URL = {Driver.Url} Page Souce: \r\n {Driver.PageSource}");
            }
        }

        public string GenerationToken => Driver.FindElement(By.Id("GenerationToken")).Text;
    }
}
