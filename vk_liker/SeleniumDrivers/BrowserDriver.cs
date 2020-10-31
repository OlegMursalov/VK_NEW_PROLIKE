using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Opera;
using System;
using System.Drawing;

namespace vk_liker.SeleniumDrivers
{
    public static class BrowserDriver
    {
        public static IWebDriver GetRandDriverAndProxy()
        {
            IWebDriver webDriver = null;

            int i = new Random().Next(0, 1);

            if (i == 0)
            {
                webDriver = new ChromeDriver(driverOptions);
            }
            else if (i == 1)
            {
                webDriver = new OperaDriver(driverOptions);
            }



            var manageOptions = webDriver.Manage();
            manageOptions.Window.Size = new Size(1920, 1080);
            manageOptions.Window.Position = new Point(0, 0);
        }

        public static IWebDriver GetDriver(BrowserType browserType)
        {
            IWebDriver webDriver = null;

            if (browserType == BrowserType.Chrome)
            {
                var driverOptions = new ChromeOptions();
                driverOptions.AddArguments("--proxy-server=http://ZefWDt:aciLSVguDa@188.130.185.251:5500");
                webDriver = new ChromeDriver(driverOptions);
            }
            else if (browserType == BrowserType.Opera)
            {
                var driverOptions = new OperaOptions();
                driverOptions.AddArguments("--proxy-server=http://ZefWDt:aciLSVguDa@188.130.185.251:5500");
                webDriver = new OperaDriver(driverOptions);
            }

            var manageOptions = webDriver.Manage();
            manageOptions.Window.Size = new Size(1920, 1080);
            manageOptions.Window.Position = new Point(0, 0);

            return webDriver;
        }
    }
}