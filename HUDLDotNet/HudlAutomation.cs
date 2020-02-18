using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace NUnitHUDL
{
    [TestFixture]
    public class HUDLBasic
    {
        private IWebDriver driver;
        private string UserEmail = "handlos@gmail.com";
        private string UserPass = "7980HUDL";

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
        }

        [Test]
        public void HUDLBasicLogin_EmailExists()
        {
            driver.Navigate().GoToUrl("https://www.hudl.com/");

            driver.FindElement(By.LinkText("Log in")).Click();
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys(UserEmail);
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys(UserPass);
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);

            Wait(5000);

            string PageEmail = driver.FindElement(By.XPath("//div[@class='hui-globaluseritem__email']")).GetAttribute("innerHTML");
            bool TestPass = false;
            if (PageEmail == UserEmail)
            {
                TestPass = true;
            }
            Assert.IsTrue(TestPass);
        }

        [Test]
        public void HUDLBasicLogin_TeamNameExists()
        {
            driver.Navigate().GoToUrl("https://www.hudl.com/");

            driver.FindElement(By.LinkText("Log in")).Click();
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys(UserEmail);
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys(UserPass);
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);

            Wait(5000);

            string PageContent = driver.FindElement(By.XPath("//div[@class='hui-primaryteamswitcher__display-name']/span")).GetAttribute("innerHTML");
            bool TestPass = false;
            if (PageContent == "QA Meetings")
            {
                TestPass = true;
            }
            Assert.IsTrue(TestPass);
        }

        [Test]
        public void HUDLBasicLogin_BadUser()
        {
            string BadUserEmail = "me@me.com";
            driver.Navigate().GoToUrl("https://www.hudl.com/");

            driver.FindElement(By.LinkText("Log in")).Click();
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys(BadUserEmail);
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys(UserPass);
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);

            Wait(5000);

            string PageContent = driver.FindElement(By.XPath("//div[@class='login-error-container']/p")).GetAttribute("innerHTML");
            bool TestPass = false;
            if (PageContent.Contains("We didn't recognize that email and/or password"))
            {
                TestPass = true;
            }
            Assert.IsTrue(TestPass);
        }

        [Test]
        public void HUDLBasicLogin_BadPassword()
        {
            string BadUserPass = "7980HUDLBad";
            driver.Navigate().GoToUrl("https://www.hudl.com/");

            driver.FindElement(By.LinkText("Log in")).Click();
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys(UserEmail);
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys(BadUserPass);
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);

            Wait(5000);

            string PageContent = driver.FindElement(By.XPath("//div[@class='login-error-container']/p")).GetAttribute("innerHTML");
            bool TestPass = false;
            if (PageContent.Contains("We didn't recognize that email and/or password"))
            {
                TestPass = true;
            }
            Assert.IsTrue(TestPass);
        }





        public void Wait(int WaitMilliseconds)
        {
            System.Threading.Thread.Sleep(WaitMilliseconds);
        }

        public void ElementClick(IWebElement elem)
        {
            WebDriverWait waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            //  waitForElement.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(elem));
            //            waitForElement.Until(ExpectedConditions.ElementExists(By.XPath(button)));
            //Browser.Instance.FindElement(By.XPath(button)).Click();
            elem.Click();
        }


        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }


    }
}