using OpenQA.Selenium;
using System;
using System.Threading;
using vk_liker.DTO;
using vk_liker.Proxy;
using vk_liker.SeleniumDrivers;

namespace vk_liker.Structure
{
    public class Liker : IDisposable
    {
        private int _delay = 100;

        private Random _random = new Random();

        private VkUserInfo[] _vkUserInfos;

        private IWebDriver _webDriver;

        private string _anonymizerUri = "http://noblockme.ru/";

        private string _vkUri = "https://vk.com/";

        private ProxyInfo[] _httpProxies;

        public Liker(VkUserInfo[] vkUserInfos)
        {
            _vkUserInfos = vkUserInfos;
            _httpProxies = ProxyWorker.GetAll();
            _webDriver = BrowserDriver.GetDriver(BrowserType.Chrome);
        }

        public void ExecuteForVkUserId(long vkUserId)
        {
            for (int i = 0; i < _vkUserInfos.Length; i++)
            {
                ClickRemindMeLater();
                Thread.Sleep(_delay);
                _webDriver.Navigate().GoToUrl(_vkUri);
                Thread.Sleep(_delay);
                ClickRemindMeLater();
                Thread.Sleep(_delay);
                Logout();
                Thread.Sleep(_delay);
                ClickRemindMeLater();
                Login(_vkUserInfos[i]);
                Thread.Sleep(_delay);
                ClickNextBtn();
                Thread.Sleep(_delay);
                ClickRemindMeLater();
                Thread.Sleep(_delay);

                if (CheckCaptcha())
                {
                    TryPassCaptcha();
                }

                NavigateToProfileAvaAndLike(vkUserId);
            }
        }

        private void TryPassCaptcha()
        {
            try
            {
                var recaptcha = _webDriver.FindElement(By.Id("recaptcha0"));
                recaptcha.Click();
                Thread.Sleep(2500);
            }
            catch (NoSuchElementException ex)
            {
            }
            catch (ElementNotVisibleException ex)
            {
            }
            catch (WebDriverException ex)
            {
            }
        }

        private void NavigateToVK()
        {
            try
            {
                var inputs = _webDriver.FindElements(By.TagName("input"));
                foreach (var input in inputs)
                {
                    var attrVal = input.GetAttribute("type");
                    if (attrVal == "text")
                    {
                        input.SendKeys(_vkUri);
                    }
                }
                var form = _webDriver.FindElement(By.TagName("form"));
                form.Submit();
            }
            catch (NoSuchElementException ex)
            {
            }
            catch (ElementNotVisibleException ex)
            {
            }
            catch (WebDriverException ex)
            {
            }
        }

        private void ClickRemindMeLater()
        {
            try
            {
                var buttons = _webDriver.FindElements(By.TagName("button"));
                foreach (var btn in buttons)
                {
                    var attrVal = btn.GetAttribute("value");
                    if (attrVal == "Напомнить позже")
                    {
                        btn.Click();
                    }
                }
            }
            catch (NoSuchElementException ex)
            {
            }
            catch (ElementNotVisibleException ex)
            {
            }
            catch (WebDriverException ex)
            {
            }
        }

        private void ClickNextBtn()
        {
            try
            {
                var continueButton = _webDriver.FindElement(By.Id("vkconnect_continue_button"));
                continueButton.Click();
            }
            catch (NoSuchElementException ex)
            {
            }
            catch (ElementNotVisibleException ex)
            {
            }
            catch (WebDriverException ex)
            {
            }
        }

        private bool CheckCaptcha()
        {
            bool result = false;

            try
            {
                var recaptchaCheckbox = _webDriver.FindElement(By.ClassName("recaptcha"));
                result = recaptchaCheckbox != null;
            }
            catch (NoSuchElementException ex)
            {

            }
            catch (ElementNotVisibleException ex)
            {

            }
            catch (WebDriverException ex)
            {
            }

            return result;
        }

        private void CommonSendKeys(IWebElement webElement, string text)
        {
            foreach (var key in text)
            {
                Thread.Sleep(_random.Next(50, 350));
                webElement.SendKeys(key.ToString());
            }
        }

        private void Login(VkUserInfo vkUserInfo)
        {
            try
            {
                var loginElem = _webDriver.FindElement(By.Id("index_email"));
                CommonSendKeys(loginElem, vkUserInfo.Login);
                var passElem = _webDriver.FindElement(By.Id("index_pass"));
                CommonSendKeys(passElem, vkUserInfo.Pass);
                var indexLoginElem = _webDriver.FindElement(By.Id("index_login_button"));
                indexLoginElem.Click();
            }
            catch (NoSuchElementException ex)
            {

            }
            catch (ElementNotVisibleException ex)
            {

            }
            catch (WebDriverException ex)
            {
                ClickToBoxLayerWrapButton();
                Login(vkUserInfo);
            }
        }

        private void NavigateToProfileAvaAndLike(long vkUserId)
        {
            try
            {
                _webDriver.Navigate().GoToUrl($"https://vk.com/id{vkUserId}");
                var profilePhotoLinkElem = _webDriver.FindElement(By.Id("profile_photo_link"));
                var hrefProfile = profilePhotoLinkElem.GetAttribute("href");
                _webDriver.Navigate().GoToUrl(hrefProfile);
                var likeBtnElem = _webDriver.FindElement(By.ClassName("like_btn"));
                var @class = likeBtnElem.GetAttribute("class");
                if (@class.IndexOf("active") == -1)
                {
                    likeBtnElem.Click();
                }
            }
            catch (NoSuchElementException ex)
            {

            }
            catch (ElementNotVisibleException ex)
            {

            }
            catch (WebDriverException ex)
            {
                ClickToBoxLayerWrapButton();
                NavigateToProfileAvaAndLike(vkUserId);
            }
        }

        private void Logout()
        {
            try
            {
                var topProfileLinkElem = _webDriver.FindElement(By.Id("top_profile_link"));
                topProfileLinkElem.Click();
                var topLogoutLinkElem = _webDriver.FindElement(By.Id("top_logout_link"));
                topLogoutLinkElem.Click();
            }
            catch (NoSuchElementException ex)
            {

            }
            catch (ElementNotVisibleException ex)
            {

            }
            catch (WebDriverException ex)
            {
                ClickToBoxLayerWrapButton();
                Logout();
            }
        }

        private void ClickToBoxLayerWrapButton()
        {
            try
            {
                var popupBoxContainerElem = _webDriver.FindElement(By.ClassName("popup_box_container"));
                var boxXButtonElem = popupBoxContainerElem.FindElement(By.ClassName("box_x_button"));
                boxXButtonElem.Click();
            }
            catch (NoSuchElementException ex)
            {

            }
            catch (ElementNotVisibleException ex)
            {

            }
            catch (WebDriverException ex)
            {
            }
        }

        public void Dispose()
        {
            if (_webDriver != null)
            {
                _webDriver.Dispose();
            }
        }
    }
}
