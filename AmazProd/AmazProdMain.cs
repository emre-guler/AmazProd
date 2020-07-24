using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Events;
using System.Collections.Generic;

namespace AmazProd
{
    class AmazProdMain
    {
        static void Main(string[] args)
        {
            string mainUrl = "https://www.amazon.com/";
            using (IWebDriver driver = new FirefoxDriver())
            {
                // Configure-To-UnitedArabEmirates
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                driver.Navigate().GoToUrl(mainUrl);
                driver.FindElement(By.ClassName("a-popover-trigger")).Click();
                driver.FindElement(By.Id("GLUXCountryValue")).Click();
                driver.FindElement(By.Id("GLUXCountryList_232")).Click();
                driver.FindElement(By.Name("glowDoneButton")).Click();
                // Configure-The-URL and Names
                List<string> URL = getUrls(mainUrl);
                List<string> Name = getNames();
                List<Department> departments = new List<Department>();
                for (int i = 0; i < URL.Count; i++)
                {
                    Department newDepartment = new Department
                    {
                        Name = Name[i],
                        URL = URL[i]
                    };
                    departments.Add(newDepartment);
                }
                // Start-To-Search
                foreach (Department departmend in departments)
                {
                    driver.Navigate().GoToUrl(departmend.URL);
                    var products = driver.FindElements(By.CssSelector("#mainResults > ul > li"));
                    foreach(var product in products)
                    {
                        product.FindElement(By.CssSelector(".a-spacing-mini  .a-spacing-none .s-access-detail-page")).Click();
                        string prodcutTitle = driver.FindElement(By.Id("productTitle")).Text;
                        string productPrice;
                        try {
                            productPrice = driver.FindElement(By.ClassName("offer-price")).Text;
                        }
                        catch(Exception e)
                        {
                            productPrice = "noStock";
                        }
                        var haveShipmentForUAE = false;
                        var currentUrl = driver.Url;
                        driver.Navigate().Back();
                        Console.WriteLine("Ürün bilgileri:" + prodcutTitle + productPrice + currentUrl, haveShipmentForUAE);
                    }
                    var page = driver.FindElement(By.Id("pagn"));
                }
                Console.ReadKey();
            }
        }
        static List<string> getUrls(string mainUrl)
        {
            List<string> URL = new List<string>
            {
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Darts-crafts-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dautomotive-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dbaby-products-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dbeauty-intl-ship",
                mainUrl + "s?i=computers-intl-ship&ref=nb_sb_noss",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Delectronics-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dhpc-intl-ship",
                mainUrl + "s?i=kitchen-intl-ship&ref=nb_sb_noss",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dindustrial-intl-ship",
                mainUrl + "s?i=pets-intl-ship&ref=nb_sb_noss",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dsoftware-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dsporting-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dtools-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dtoys-and-games-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dvideogames-intl-ship"
            };
            return URL;
        }
        static List<string> getNames()
        {
            List<string> name = new List<string>
            {
                "Arts & Crafts",
                "Automotive",
                "Baby",
                "Beauty & Personal Care",
                "Computers",
                "Electronics",
                "Health & Household",
                "Home & Kitchen",
                "Industrial & Scientific",
                "Pet Supplies",
                "Software",
                "Sports & Outdoors",
                "Tools & Home Improvement",
                "Toys & Games",
                "Video Games"
            };
            return name;
        }
    }
}
