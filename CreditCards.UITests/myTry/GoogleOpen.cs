using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CreditCards.UITests
{
    public class GoogleOpen
    {

        private const string googleHome = "https://www.google.com/";

        [Fact]
        public void OpenGoogleSearch()
        {
            using (IWebDriver googleDriver = new ChromeDriver())
            {
                googleDriver.Navigate().GoToUrl(googleHome);
                DemoHelper.Pause();
            }
        }

        [Fact]
        public void GoToGoogleGraphic()
        {
            using (IWebDriver googleDriver = new ChromeDriver())
            {
                googleDriver.Navigate().GoToUrl(googleHome);
                DemoHelper.Pause();

                IWebElement accept = googleDriver.FindElement(By.Id("L2AGLb"));
                accept.Click();
                DemoHelper.Pause();

                IWebElement graphicButton = googleDriver.FindElement(By.CssSelector("[data-pid='2']"));
                graphicButton.Click();
                DemoHelper.Pause();

                Assert.Equal("Grafika Google", googleDriver.Title);
                Assert.Equal("https://www.google.pl/imghp?hl=pl&ogbl", googleDriver.Url);
            }
        }
    }
}
