using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit.Abstractions;
using System.Collections.ObjectModel;
using ApprovalTests.Reporters;
using ApprovalTests.Reporters.Windows;
using System.IO;
using ApprovalTests;

namespace CreditCards.UITests
{
    [Trait("Category", "Applications")]
    public class CreditCardApplicationShould
    {
        private const string HomeUrl = "http://localhost:44108/";
        private const string ApplyUrl = "http://localhost:44108/Apply";
        private const string AboutUrl = "http://localhost:44108/Home/About";
        private const string HomeTitle = "Home Page - Credit Cards";
        private const string ApplicationTitle = "Credit Card Application - Credit Cards";

        private readonly ITestOutputHelper output;

        public CreditCardApplicationShould(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void BeInitiatedFormHomePage_NewLowRate()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement applyLink = driver.FindElement(By.Name("ApplyLowRate"));
                applyLink.Click();
                DemoHelper.Pause();

                Assert.Equal(ApplicationTitle, driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_EasyApplication()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement nextButton = driver.FindElement(By.CssSelector("[data-slide='next']"));
                nextButton.Click();


                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                IWebElement apllyLink = wait.Until((d) => d.FindElement(By.LinkText("Easy: Apply Now!")));
                apllyLink.Click();
                DemoHelper.Pause();


                Assert.Equal(ApplicationTitle, driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomaPage_EasyApplication_Prebuilt_Conditions()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                driver.Manage().Window.Minimize();
                DemoHelper.Pause();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(11));
                IWebElement applyLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Easy: Apply Now!")));
                applyLink.Click();
                DemoHelper.Pause();

                Assert.Equal(ApplicationTitle, driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_CustomerService()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Navigate to '{HomeUrl}'");
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Finding Element");
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(35));

                /*
                Func<IWebDriver, IWebElement> findEnabledElement = delegate (IWebDriver d)
                {
                    var e = d.FindElement(By.ClassName(("customer-service-apply-now")));

                    if (e is null)
                    {
                        throw new NotFoundException();
                    }
                    if (e.Enabled && e.Displayed)
                    {
                        return e;
                    }

                    throw new NotFoundException();
                };    
                    
                IWebElement applyLink = wait.Until(findEnabledElement);
                */
                IWebElement applyLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("customer-service-apply-now")));

                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Found element Display={applyLink.Displayed} Enabled {applyLink.Enabled}");
                output.WriteLine($"{DateTime.Now.ToLongTimeString()} Clicking element");
                applyLink.Click();
                DemoHelper.Pause();


                Assert.Equal(ApplicationTitle, driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_CustomerServiceByClassName()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement nextButton = driver.FindElement(By.ClassName("glyphicon-chevron-right"));
                nextButton.Click();
                DemoHelper.Pause(1000); // allow carousel time to stroll
                nextButton.Click();
                DemoHelper.Pause(1000); // allow carousel time to stroll

                IWebElement applyLink = driver.FindElement(By.ClassName("customer-service-apply-now"));
                applyLink.Click();
                DemoHelper.Pause();


                Assert.Equal(ApplicationTitle, driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_RandomGreating()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement randomGreatingApplyLink = driver.FindElement(By.PartialLinkText("- Apply Now!"));
                randomGreatingApplyLink.Click();
                DemoHelper.Pause();

                Assert.Equal(ApplicationTitle, driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeInitiatedFromHomePage_RandomGreetin_Using_XPATH()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                IWebElement randomGreatingApplyLink = driver.FindElement(By.XPath("//a[text()[contains(.,'- Apply Now!')]]"));
                randomGreatingApplyLink.Click();
                DemoHelper.Pause();

                Assert.Equal(ApplicationTitle, driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void BeSumbmittedWhenValidationErrorsCorrected()
        {
            string firstName = "Sarah";
            string lastName = "Smith";
            string inValidAge = "17";
            string validAge = "18";
            string fNumber = "123456-A";

            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(ApplyUrl);
                DemoHelper.Pause();

                driver.FindElement(By.Id("FirstName")).SendKeys(firstName);
                //dont enter last name
                //driver.FindElement(By.Id("LastName")).SendKeys(lastName);
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys(fNumber);
                driver.FindElement(By.Id("Age")).SendKeys(inValidAge);
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("5000");
                driver.FindElement(By.Id("Single")).Click();

                IWebElement businessSourceSelectElement = driver.FindElement(By.Id("BusinessSource"));
                SelectElement businessSource = new SelectElement(businessSourceSelectElement);
                businessSource.SelectByValue("Email");
                driver.FindElement(By.Id("TermsAccepted")).Click();
                driver.FindElement(By.Id("SubmitApplication")).Click();

                // Asert that validation Failed
                var validationErrors = driver.FindElements(By.CssSelector(".validation-summary-errors > ul > li"));
                Assert.Equal(2, validationErrors.Count);
                Assert.Equal("Please provide a last name", validationErrors[0].Text);
                Assert.Equal("You must be at least 18 years old", validationErrors[1].Text);

                //Fix errors
                driver.FindElement(By.Id("LastName")).SendKeys(lastName);
                driver.FindElement(By.Id("Age")).Clear();
                driver.FindElement(By.Id("Age")).SendKeys(validAge);

                //Resubmit
                driver.FindElement(By.Id("SubmitApplication")).Click();

                //Check form submitted

                Assert.StartsWith("Application Complete", driver.Title);
                Assert.Equal("ReferredToHuman", driver.FindElement(By.Id("Decision")).Text);
                Assert.NotEmpty(driver.FindElement(By.Id("ReferenceNumber")).Text);
                Assert.Equal($"{firstName} {lastName}", driver.FindElement(By.Id("FullName")).Text);
                Assert.Equal($"{validAge}", driver.FindElement(By.Id("Age")).Text);
                Assert.Equal("5000", driver.FindElement(By.Id("Income")).Text);
                Assert.Equal("Single", driver.FindElement(By.Id("RelationshipStatus")).Text);
                Assert.Equal("Email", driver.FindElement(By.Id("BusinessSource")).Text);
            }
        }

        [Fact]
        public void BeSumbmittedWhenValid()
        {
            string firstName = "Sarah";
            string lastName = "Smith";
            string validAge = "18";
            string fNumber = "123456-A";

            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(ApplyUrl);
                DemoHelper.Pause();

                driver.FindElement(By.Id("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Id("LastName")).SendKeys(lastName);
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys(fNumber);
                driver.FindElement(By.Id("Age")).SendKeys(validAge);
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys("5000");
                driver.FindElement(By.Id("Single")).Click();

                IWebElement businessSourceSelectElement = driver.FindElement(By.Id("BusinessSource"));
                SelectElement businessSource = new SelectElement(businessSourceSelectElement);

                foreach (var options in businessSource.Options)
                {
                    output.WriteLine($"Value: {options.GetAttribute("value")} Text: {options.Text}");
                }
                Assert.Equal(5, businessSource.Options.Count);

                businessSource.SelectByValue("Internet");
                DemoHelper.Pause();
                businessSource.SelectByIndex(4);

                Assert.Equal("TV Ad", businessSource.SelectedOption.Text);

                driver.FindElement(By.Id("TermsAccepted")).Click();

                //driver.FindElement(By.Id("SubmitApplication")).Click();
                driver.FindElement(By.Id("Single")).Submit();
                DemoHelper.Pause();


                Assert.StartsWith("Application Complete", driver.Title);
                Assert.Equal("ReferredToHuman", driver.FindElement(By.Id("Decision")).Text);
                Assert.NotEmpty(driver.FindElement(By.Id("ReferenceNumber")).Text);
                Assert.Equal($"{firstName} {lastName}", driver.FindElement(By.Id("FullName")).Text);
                Assert.Equal($"{validAge}", driver.FindElement(By.Id("Age")).Text);
                Assert.Equal("5000", driver.FindElement(By.Id("Income")).Text);
                Assert.Equal("Single", driver.FindElement(By.Id("RelationshipStatus")).Text);
                Assert.Equal("TV", driver.FindElement(By.Id("BusinessSource")).Text);
            }
        }

        [Fact]
        public void OpenContactFooterLinkInNewTab()
        {
            using(IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                driver.FindElement(By.Id("ContactFooter")).Click();
                DemoHelper.Pause();

                ReadOnlyCollection<string> allTabs = driver.WindowHandles;
                string homePageTab = allTabs[0];
                string contactTab = allTabs[1];

                driver.SwitchTo().Window(contactTab);

                Assert.EndsWith("/Home/Contact", driver.Url);

            }
        }

        [Fact]
        public void AllertIfLiveChatClosed()
        {
            using(IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                driver.FindElement(By.Id("LiveChat")).Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));


                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());


                Assert.Equal("Live chat is currently closed.", alert.Text);

                alert.Accept();

            }
        }

        [Fact]
        public void NotNavigateToAboutUsWhenCancelClicked()
        {
            using(IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                Assert.Equal(HomeTitle, driver.Title);

                driver.FindElement(By.Id("LearnAboutUs")).Click();
                DemoHelper.Pause();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());

                alert.Dismiss();
                Assert.Equal(HomeTitle, driver.Title);
            }
        }

        [Fact]
        public void NavigateToAboutUsWhenOKClicked()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                Assert.Equal(HomeTitle, driver.Title);

                driver.FindElement(By.Id("LearnAboutUs")).Click();
                DemoHelper.Pause();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());

                alert.Accept();

                Assert.StartsWith("About", driver.Title);
            }
        }

        [Fact]
        public void NotDisplatCookieUseMessage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);
                DemoHelper.Pause();

                driver.Manage().Cookies.AddCookie(new Cookie("acceptedCookies", "true"));

                driver.Navigate().Refresh();
                DemoHelper.Pause();

                ReadOnlyCollection<IWebElement> message = driver.FindElements(By.Id("CookiesBeingUsed"));

                Assert.Empty(message);
            }

        }

        /*
        [Fact]
        [UseReporter(typeof(BeyondCompareReporter))]
        public void RenderAboutPage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(AboutUrl);
                DemoHelper.Pause();

                ITakesScreenshot screenShotDriver = (ITakesScreenshot)driver;

                Screenshot screenshot = screenShotDriver.GetScreenshot();
                screenshot.SaveAsFile("aboutpage.bmp", ScreenshotImageFormat.Bmp);

                FileInfo file = new FileInfo("aboutpage.bmp");

                Approvals.Verify(file);
                // coś nie poszło, bo nie wygenerował się plik który koleś miał w teście
            }
        }
        */

    }
}