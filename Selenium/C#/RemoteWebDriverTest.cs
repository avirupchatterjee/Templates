using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Reportium.test;
using Reportium.test.Result;
using Reportium.client;
using Reportium.model;

namespace PerfectoLabSeleniumTestProject
{
    /// <summary>
    /// This template is for users that use DigitalZoom Reporting (ReportiumClient).
    /// For any other use cases please see the basic template at https://github.com/PerfectoCode/Templates.
    /// For more programming samples and updated templates refer to the Perfecto Documentation at: http://developers.perfectomobile.com/
    /// </summary>
    [TestClass]
    public class RemoteWebDriverTest
    {
        private RemoteWebDriver driver;
        private ReportiumClient reportiumClient;

        [TestInitialize]
        public void PerfectoOpenConnection()
        {
            var browserName = "mobileOS";
            var host = "myHost.perfectomobile.com";
            var token = "myToken";

            //var user = "My Username";
            //var password = "My Password";

            DesiredCapabilities capabilities = new DesiredCapabilities(browserName, string.Empty, new Platform(PlatformType.Any));
            capabilities.SetCapability("securityToken", token);

            //capabilities.SetCapability("user", user);
            //capabilities.SetCapability("password", password);

            capabilities.SetPerfectoLabExecutionId(host);

            // Name your script
            // capabilities.SetCapability("scriptName", "RemoteWebDriverTest");

            var url = new Uri(string.Format("http://{0}/nexperience/perfectomobile/wd/hub", host));
            driver = new RemoteWebDriver(url, capabilities);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));

            // Reporting client. For more details, see http://developers.perfectomobile.com/display/PD/Reporting
            PerfectoExecutionContext perfectoExecutionContext = new PerfectoExecutionContext.PerfectoExecutionContextBuilder()
                    .withProject(new Project("My Project", "1.0"))
                    .withJob(new Job("My Job", 45))
                    .withContextTags(new[] { "tag1" })
                    .withWebDriver(driver)
                    .build();
            reportiumClient = PerfectoClientFactory.createPerfectoReportiumClient(perfectoExecutionContext);
        }

        [TestCleanup]
        public void PerfectoCloseConnection()
        {
            driver.Quit();

            // Retrieve the URL of the Single Test Report, can be saved to your execution summary and used to download the report at a later point
            String reportURL = reportiumClient.getReportUrl();

            // For documentation on how to export reporting PDF, see https://github.com/PerfectoCode/Reporting-Samples
            String reportPdfUrl = (String)(driver.Capabilities.GetCapability("reportPdfUrl"));

            // For detailed documentation on how to export the Execution Summary PDF Report, the Single Test report and other attachments such as
            // video, images, device logs, vitals and network files - see http://developers.perfectomobile.com/display/PD/Exporting+the+Reports
        }

        [TestMethod]
        public void WebDriverTestMethod()
        {
            try
            {
                reportiumClient.testStart("My test mame", new TestContextTags("tag2", "tag3"));

                // write your code here

                // reportiumClient.testStep("step1"); // this is a logical step for reporting
                // add commands...
                // reportiumClient.testStep("step2");
                // add commands...

                reportiumClient.testStop(TestResultFactory.createSuccess());
            }
            catch (Exception e)
            {
                reportiumClient.testStop(TestResultFactory.createFailure(e.Message, e));
            }
        }
    }
}
