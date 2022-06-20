using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using CreditCards.UITests.PageObjectModels;

namespace CreditCards.UITests
{
    public class CreditCardWebAppShould
    {
        private const string AboutUrl = "http://localhost:44108/Home/About";
       
        [Fact]
        [Trait("Category", "Smoke")]
        public void LoadHomePage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                var homePage = new HomePage(driver);
                homePage.NavigateTo();
                
                driver.Manage().Window.Maximize();
                DemoHelper.Pause();
                driver.Manage().Window.Minimize();
                DemoHelper.Pause();
                driver.Manage().Window.Size = new System.Drawing.Size(300, 400);
                DemoHelper.Pause();
                driver.Manage().Window.Position = new System.Drawing.Point(1, 1);
                DemoHelper.Pause();
                driver.Manage().Window.Position = new System.Drawing.Point(50, 50);
                DemoHelper.Pause();
                driver.Manage().Window.Position = new System.Drawing.Point(100, 100);
                DemoHelper.Pause();
                driver.Manage().Window.FullScreen();
                DemoHelper.Pause();

                homePage.ShurePageLoaded();
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void ReloadHomePageOnBack()
        {
            using (IWebDriver driver = new ChromeDriver())
            {

                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                string initialToken = homePage.GenerationToken;

                driver.Navigate().GoToUrl(AboutUrl);
                driver.Navigate().Back();

                homePage.ShurePageLoaded();


                string reloadedToken = homePage.GenerationToken;

                Assert.NotEqual(initialToken, reloadedToken);

            }
        }

        [Fact]
        public void DisplayProductAndRates()
        {
            using (IWebDriver driver = new ChromeDriver())
            {

                var homePage = new HomePage(driver);
                homePage.NavigateTo();

                DemoHelper.Pause();

                Assert.Equal("Easy Credit Card",homePage.Products[0].name);
                Assert.Equal("20% APR", homePage.Products[0].interestRate);

                Assert.Equal("Silver Credit Card", homePage.Products[1].name);
                Assert.Equal("18% APR", homePage.Products[1].interestRate);

                Assert.Equal("Gold Credit Card", homePage.Products[2].name);
                Assert.Equal("17% APR", homePage.Products[2].interestRate);


            }
        }
    }
}
