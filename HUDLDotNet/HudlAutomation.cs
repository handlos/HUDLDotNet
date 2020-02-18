using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace NUnitHUDL
{
    /*
     Class: HUDLBasic

     Description: As the name describes, this is a very simple set of selenium-based tests to access the HUDL site.
     These are strictly tests that each focus on logging into the HUDL site, and each querying a specific property/node on the page to ensure the login was valid.
     
     Notes: Beyond ensuring that specific content on the page exists, ideally the tests should be data driven, not statically entered into the tests as below.
     This will make the tests less fragile and easier to maintain as the code changes.
     */

    [TestFixture]
    public class HUDLBasic
    {
        //Essential global variables, used across the tests
        private IWebDriver driver;
        private string UserEmail = "handlos@gmail.com";
        private string UserPass = "7980HUDL";
       

        //Global "setup" function that happens at the beginning of each test.
        //A Chrome browser is initialized, implict waits are set to 30 seconds, and the window is maximized.
        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver(@"C:\VS\Hudl\WebDrivers");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
        }

        //Global "teardown" to kill the instance of the Chrome driver to avoid issues from having the open window to stay open
        [TearDown]
        public void TeardownTest()
        {
            driver.Quit();
        }

        //Basic "LoginSteps" method to repeat the same login steps per test to avoid redundant code
        //Note: The "Wait" step was added to give the main page time to load, but this should normally be avoided by doing an explicit wait instead.
        public void LoginSteps(string user, string pass)
        {
            driver.Navigate().GoToUrl("https://www.hudl.com/");

            driver.FindElement(By.LinkText("Log in")).Click();
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys(user);
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys(pass);
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);
            Wait(5000);

        }
        public void Wait(int WaitMilliseconds)
        {
            System.Threading.Thread.Sleep(WaitMilliseconds);
        }

        //HUDLBasicLogin_EmailExists - Logs into the HUDL page, and then confirms the correct email address exists.
        [Test]
        public void HUDLBasicLogin_EmailExists()
        {
            LoginSteps(UserEmail, UserPass);

            string PageContent = driver.FindElement(By.XPath("//div[@class='hui-globaluseritem__email']")).GetAttribute("innerHTML");
            bool TestPass = false;
            if (PageContent == UserEmail)
            {
                TestPass = true;
            }
            Assert.IsTrue(TestPass);
        }

        //HUDLBasicLogin_DisplayNameExists - Logs into the HUDL page, and then confirms the correct email address exists.
        [Test]
        public void HUDLBasicLogin_DisplayNameExists()
        {
            LoginSteps(UserEmail, UserPass);

            string PageContent = driver.FindElement(By.XPath("//div[@class='hui-globaluseritem__display-name']/span")).GetAttribute("innerHTML");
            bool TestPass = false;
            if (PageContent == "David H")
            {
                TestPass = true;
            }
            Assert.IsTrue(TestPass);
        }

        //HUDLBasicLogin_TeamNameExists - Logs into the HUDL page, and then confirms the "QA Meetings" team name is being applied.
        [Test]
        public void HUDLBasicLogin_TeamNameExists()
        {
            LoginSteps(UserEmail, UserPass);

            string PageContent = driver.FindElement(By.XPath("//div[@class='hui-primaryteamswitcher__display-name']/span")).GetAttribute("innerHTML");
            bool TestPass = false;
            if (PageContent == "QA Meetings")
            {
                TestPass = true;
            }
            Assert.IsTrue(TestPass);
        }


        [Test]
        public void HUDLBasicLogin_UploadExists()
        {
            LoginSteps(UserEmail, UserPass);

            string PageContent = driver.FindElement(By.XPath("//a[@class='hui-globalnav__upload hui-globalnav__upload--button-display uni-btn--secondary uni-btn--small']/span")).Text;
            bool TestPass = false;
            if (PageContent == "Upload")
            {
                TestPass = true;
            }
            Assert.IsTrue(TestPass);
        }

        //HUDLBasicLogin_BadPassword - Basic bad path test to confirm that using a bad user name will result in the expected error message on the page.
        [Test]
        public void HUDLBasicLogin_BadUser()
        {
            string BadUserEmail = "me@me.com";
            LoginSteps(BadUserEmail, UserPass);

            string PageContent = driver.FindElement(By.XPath("//div[@class='login-error-container']/p")).GetAttribute("innerHTML");
            bool TestPass = false;
            if (PageContent.Contains("We didn't recognize that email and/or password"))
            {
                TestPass = true;
            }
            Assert.IsTrue(TestPass);
        }

        //HUDLBasicLogin_BadPassword - Basic bad path test to confirm that using a bad password will result in the expected error message on the page.
        [Test]
        public void HUDLBasicLogin_BadPassword()
        {
            string BadUserPass = "7980HUDLBad";
            LoginSteps(UserEmail, BadUserPass);

            string PageContent = driver.FindElement(By.XPath("//div[@class='login-error-container']/p")).GetAttribute("innerHTML");
            bool TestPass = false;
            if (PageContent.Contains("We didn't recognize that email and/or password"))
            {
                TestPass = true;
            }
            Assert.IsTrue(TestPass);
        }







    }
}