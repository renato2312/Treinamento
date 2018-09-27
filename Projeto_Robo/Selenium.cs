using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Projeto_Robo
{
    
    public class Selenium
    {
        
        public void TestMethod()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();


            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }
    }
}
