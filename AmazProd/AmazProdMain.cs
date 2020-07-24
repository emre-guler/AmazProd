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
                for (int i = 0; i < 22; i++)
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
                    var products = driver.FindElement(By.CssSelector("#mainResults > ul > li"));
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
                mainUrl + "s?i=stripbooks-intl-ship&ref=nb_sb_noss",
                mainUrl + "s?i=computers-intl-ship&ref=nb_sb_noss",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Delectronics-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dfashion-womens-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dfashion-mens-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dfashion-girls-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dfashion-boys-intl-ship&field-keywords=",
                mainUrl + "s?i=deals-intl-ship&ref=nb_sb_noss",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dhpc-intl-ship",
                mainUrl + "s?i=kitchen-intl-ship&ref=nb_sb_noss",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dindustrial-intl-ship",
                mainUrl + "s/ref=nb_sb_noss?url=search-alias%3Dluggage-intl-ship",
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
                "Books",
                "Computers",
                "Electronics",
                "Women's Fashion",
                "Man's Fashion",
                "Girl's Fashion",
                "Boy's Fashion",
                "Deals",
                "Health & Household",
                "Home & Kitchen",
                "Industrial & Scientific",
                "Luggage",
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
