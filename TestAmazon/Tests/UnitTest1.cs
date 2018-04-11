using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        private const string PAGE = "https://www.amazon.com/";
        private const string CATEGORYNAME = "Departments";
        private const string CELLPHONECATNAME = "Cell Phones & Accessories";
        private const string CELLPHONECATCLASSNAME = "acs_tile__title-image";
        private const string PRODUCTCONTAINERCLASS = "s-access-detail-page";
        private const string BUTTONID = "add-to-cart-button";
        private const string CARTURL = "https://www.amazon.com/gp/cart/view.html/ref=nav_cart";
        private const string CARTPRODUCTLINK = "sc-product-link";

        [TestMethod]
        public void TestMethod1()
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl(PAGE);
            IWebElement categoryLink = driver.FindElement(By.LinkText(CATEGORYNAME));
            if (categoryLink != null)
            {
                categoryLink.Click();
                IWebElement cellPhoneLink = driver.FindElement(By.LinkText(CELLPHONECATNAME));
                if (cellPhoneLink != null)
                {
                    cellPhoneLink.Click();
                    IWebElement cellPhoneLinkII = driver.FindElementByClassName(CELLPHONECATCLASSNAME);
                    if (cellPhoneLinkII != null)
                    {
                        cellPhoneLinkII.Click();
                        var product = driver.FindElementsByClassName(PRODUCTCONTAINERCLASS).Skip(1).FirstOrDefault();
                        if (product != null)
                        {
                            product.Click();
                            ParseProduct(driver);
                            var addToCartButton = driver.FindElementById(BUTTONID);
                            if (addToCartButton != null)
                            {
                                addToCartButton.Submit();
                                driver.Navigate().GoToUrl(CARTURL);
                                var productsInCart = driver.FindElementsByClassName(CARTPRODUCTLINK);
                                if (!productsInCart.Any())
                                {
                                    throw new Exception("Element not found");
                                }
                            }
                            else
                            {
                                throw new Exception("Element not found");
                            }
                        }
                        else
                        {
                            throw new Exception("Element not found");
                        }
                    }
                    else
                    {
                        throw new Exception("Element not found");
                    }
                }
                else
                {
                    throw new Exception("Element not found");
                }
            }
            else
            {
                throw new Exception("Element not found");
            }
            driver.Close();
        }

        private static void ParseProduct(ChromeDriver driver)
        {
            try
            {
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter("productInfo.txt"))
                {
                    var titleElement = driver.FindElementById("productTitle");
                    file.WriteLine($"Title:{titleElement.Text}");
                    var priceElement = driver.FindElementById("priceblock_ourprice");
                    file.WriteLine($"Price:{priceElement.Text}");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Parsing product failed");
            }

        }
       
    }
}
