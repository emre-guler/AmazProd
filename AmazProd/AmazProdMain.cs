using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Events;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Remote;

namespace AmazProd
{
    class AmazProdMain
    {
        static void Main(string[] args)
        {
            string mainUrl = "https://www.amazon.com/";
            List<Product> confirmedProducts = new List<Product>();
            using (IWebDriver driver = new FirefoxDriver())
            {
                // Configure-To-UnitedArabEmirates
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
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
                        string productURL = product.FindElement(By.CssSelector(".a-spacing-mini  .a-spacing-none .s-access-detail-page")).GetAttribute("href");
                        string newTabScript= "window.open('" + productURL + "', '_blank	');";
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript(newTabScript);
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        string productTitle = driver.FindElement(By.Id("productTitle")).Text;
                        string seller = driver.FindElement(By.Id("bylineInfo")).Text;
                        bool haveShipmentForUAE;
                        try
                        {
                            if(driver.FindElement(By.Id("a-color-error")).Text != "This item cannot be shipped to your selected delivery location. Please choose a different delivery location.")
                            {
                                haveShipmentForUAE = true;
                            }
                            else
                            {
                                haveShipmentForUAE = false;
                            }

                        }
                        catch(Exception e)
                        {
                            haveShipmentForUAE = false;
                        }
                        string productPrice;
                        try
                        {
                            productPrice = driver.FindElement(By.ClassName("offer-price")).Text;
                            if (productPrice == "Currently unavailable.")
                            {
                                haveShipmentForUAE = false;
                                productPrice = "0";
                            }
                        }
                        catch (Exception e)
                        {
                            productPrice = "noStock";
                        };
                        if (!String.IsNullOrEmpty(productURL) && !String.IsNullOrEmpty(productTitle) && !String.IsNullOrEmpty(seller) && !String.IsNullOrEmpty(productPrice) && !haveShipmentForUAE)
                        {
                            bool result = usptoCompanyControl(seller, driver);
                            if(result)
                            {
                                Product newProduct = new Product
                                {
                                     Name = productTitle,
                                     Price = productPrice,
                                     Seller = seller,
                                     ProdcutURL = productURL
                                };
                                confirmedProducts.Add(newProduct);
                            }   
                        }
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        driver.Close();
                        driver.SwitchTo().Window(driver.WindowHandles.First());
                    }
                    var page = driver.FindElement(By.Id("pagn"));
                } 
                Console.ReadKey();
            }
        }

        static bool usptoCompanyControl(string companyName, IWebDriver driver)
        {
            companyName = controlCompanyName(companyName);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            string url = "http://tmsearch.uspto.gov/";
            string newTabScript = "window.open('" + url + "', '_blank	');";
            js.ExecuteScript(newTabScript);
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            string currentUrl = driver.Url;
            string newUrl = currentUrl.Replace("tess", "searchss");
            driver.Navigate().GoToUrl(newUrl);
            companyName = Regex.Replace(companyName, @"[^0-9a-zA-Z]+", "");
            js.ExecuteScript("document.getElementsByName('p_s_PARA2')[0].value = '" + companyName + "';");
            js.ExecuteScript("return document.getElementsByName('a_search')[0].remove();");
            driver.FindElement(By.Name("a_search")).Click();
            string responseString = js.ExecuteScript("return document.body.innerHTML;").ToString();
            string pattern = "<h1>[^<>]*</h1>";
            Regex rg = new Regex(pattern);
            MatchCollection matchedTag = rg.Matches(responseString);
            driver.Close();
            try
            {
                if (matchedTag[0].Value == "<h3>No TESS records were found to match the criteria of your query.</h3>")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } catch(Exception err)
            {
                Console.WriteLine(err);
                return false;
            }
        }

        static string controlCompanyName(string companyName)
        {
            // Remove "Visit The"
            companyName.Replace("Visit The", "");
            // Remove "Brand:"
            companyName.Replace("Brand:", "");
            // Remove "By"
            companyName.Replace("by", "");
            return companyName;
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
