using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;

namespace Reclutamiento.Tests.Integration;

[TestClass]
public class VisualizacionSolicitudIntegrationTest
{
    private IWebDriver _driver;

    private const string BaseUrl = "https://localhost:44358";

    // IDs Fijos
    private const int SolicitudIdFija = 11;
    private const int VacanteIdDeSolicitud = 2;

    // Credenciales de Admin (Walter)
    private const string UsuarioValido = "Walter";
    private const string EmailValido = "eteriumtech@gmail.com";
    private const string ContrasenaValida = "uapycxlmtlrhowcu";

    [TestInitialize]
    public void Setup()
    {
        var driverPath = Path.GetDirectoryName(typeof(VisualizacionSolicitudIntegrationTest).Assembly.Location);
        _driver = new ChromeDriver(driverPath);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        _driver.Manage().Window.Maximize();
    }

    [TestCleanup]
    public void Teardown()
    {
        _driver.Quit();
    }


    /// Método auxiliar para realizar el Login del Administrador.
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



    /// CP-HU12-1: Visualización parseada en UI admin (Navegación con URLs directas).
    [TestMethod]
    public void HU12_NavegacionCompleta_DebeLlegarAVistaDetalles()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        // ACT
        _driver.Navigate().GoToUrl(BaseUrl + "/Vacantes");
        wait.Until(ExpectedConditions.UrlContains("/Vacantes"));

        // Navegación a la Vista de Solicitudes (Vacante 2)
        string solicitudesUrl = $"{BaseUrl}/Vacantes/Solicitudes?vacanteId={VacanteIdDeSolicitud}";
        _driver.Navigate().GoToUrl(solicitudesUrl);
        wait.Until(ExpectedConditions.UrlContains($"/Vacantes/Solicitudes?vacanteId={VacanteIdDeSolicitud}"));
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("solicitudesTable")));

        // Navegación a la Vista de Detalles (Solicitud 11)
        string detallesUrl = $"{BaseUrl}/Solicitudes/Details/{SolicitudIdFija}";
        _driver.Navigate().GoToUrl(detallesUrl);

        // ASSERT
        wait.Until(ExpectedConditions.UrlContains($"/Solicitudes/Details/{SolicitudIdFija}"));


    }
}