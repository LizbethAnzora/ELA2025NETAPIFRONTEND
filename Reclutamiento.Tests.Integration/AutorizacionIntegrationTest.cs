using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;

namespace Reclutamiento.Tests.Integration;

[TestClass]
public class AutorizacionIntegrationTest
{
    private IWebDriver _driver;

    private const string BaseUrl = "https://localhost:44358";
    private const string RutaProtegidaAdmin = "/Usuarios/Index"; 
    private const string UsuarioValido = "Walter";
    private const string EmailValido = "eteriumtech@gmail.com";
    private const string ContrasenaValida = "uapycxlmtlrhowcu";


    [TestInitialize]
    public void Setup()
    {
        var driverPath = Path.GetDirectoryName(typeof(AutorizacionIntegrationTest).Assembly.Location);
        _driver = new ChromeDriver(driverPath);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        _driver.Manage().Window.Maximize();
    }

    [TestCleanup]
    public void Teardown()
    {
        _driver.Quit();
    }


    private void PerformAdminLogin()
    {
        _driver.Navigate().GoToUrl(BaseUrl + "/Auth/Login");

        _driver.FindElement(By.Id("NombreCompleto")).SendKeys(UsuarioValido);
        _driver.FindElement(By.Id("CorreoElectronico")).SendKeys(EmailValido);
        _driver.FindElement(By.Id("Contrasena")).SendKeys(ContrasenaValida);

        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(ExpectedConditions.UrlContains(BaseUrl));
    }



    // CP-HU11-1: Acceso a rutas admin con token válido
    /// Verifica que un usuario con rol 'Admin' puede acceder a una ruta protegida.
    [TestMethod]
    public void HU11_1_AccesoAdmin_DebeAccederARutaProtegida()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        // ACT
        _driver.Navigate().GoToUrl(BaseUrl + RutaProtegidaAdmin);

        // ASSERT
        wait.Until(ExpectedConditions.UrlContains(RutaProtegidaAdmin));


    }
}