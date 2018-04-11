using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestAmazon
{
    class Program
    {
        private const string PAGE = "https://www.amazon.com/";
        private const string CATEGORYNAME = "Departments";
        private const string CELLPHONECATNAME = "Cell Phones & Accessories";
        private const string CELLPHONECATCLASSNAME = "acs_tile__title-image";
        private const string PRODUCTCONTAINERCLASS = "s-access-detail-page";
        private const string BUTTONID = "add-to-cart-button";
        private const string CARTURL = "https://www.amazon.com/gp/cart/view.html/ref=nav_cart";
        private const string CARTPRODUCTLINK = "sc-product-link";
        static void Main(string[] args)
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl(PAGE);
            IWebElement categoryLink = driver.FindElement(By.LinkText(CATEGORYNAME));
            if (categoryLink != null)
            {
                categoryLink.Click();
                IWebElement cellPhoneLink = driver.FindElement(By.LinkText(CELLPHONECATNAME));
                if (cellPhoneLink!=null)
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
                                    Console.WriteLine("Product has not been added to the cart");
                                }
                                else
                                {
                                    Console.WriteLine("Product has been added to the cart");
                                }

                                Console.Read();
                            }
                        }
                    }
                }
            }
        }

        private static void ParseProduct(ChromeDriver driver)
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
    }
}
