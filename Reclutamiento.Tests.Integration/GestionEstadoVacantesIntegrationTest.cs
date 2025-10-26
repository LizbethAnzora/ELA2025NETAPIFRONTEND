using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;

namespace Reclutamiento.Tests.Integration;

[TestClass]
public class GestionEstadoVacantesIntegrationTest
{
    private IWebDriver _driver;

    private const string BaseUrl = "https://localhost:44358";


    private const int VacanteIdFijaParaEstado = 2;

    // Credenciales de Admin (Walter)
    private const string UsuarioValido = "Walter";
    private const string EmailValido = "eteriumtech@gmail.com";
    private const string ContrasenaValida = "uapycxlmtlrhowcu";

    [TestInitialize]
    public void Setup()
    {

        var driverPath = Path.GetDirectoryName(typeof(GestionEstadoVacantesIntegrationTest).Assembly.Location);
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



    /// CP-HU07-1 y CP-HU07-2: Desactivar y Reactivar una vacante existente.
    [TestMethod]
    public void HU07_GestionEstadoVacante_DebeDesactivarYReactivar()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        _driver.Navigate().GoToUrl(BaseUrl + "/Vacantes");
        wait.Until(ExpectedConditions.UrlContains(BaseUrl + "/Vacantes"));
        wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("table-responsive")));


        // ACT: DESACTIVAR VACANTE (CP-HU07-1)
        string desactivarButtonSelector = $"button[data-vacante-id='{VacanteIdFijaParaEstado}'].btn-outline-danger";

        IWebElement botonDesactivar = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(desactivarButtonSelector)));

        botonDesactivar.Click();
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("deshabilitarModal")));

        IWebElement confirmarDesactivar = _driver.FindElement(By.CssSelector("#deshabilitarModal button[type='submit'].btn-danger"));
        confirmarDesactivar.Click();


        // ASSERT: DESACTIVACIÓN EXITOSA
        wait.Until(ExpectedConditions.UrlContains(BaseUrl + "/Vacantes"));

        string estadoInactivoSelector = $"tr:has(button[data-vacante-id='{VacanteIdFijaParaEstado}']) .badge.bg-secondary";
        IWebElement estadoInactivo = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(estadoInactivoSelector)));
        Assert.IsNotNull(estadoInactivo, "CP-HU07-1 FALLIDO: La vacante no cambió a estado Inactivo.");


        // ACT: REACTIVAR VACANTE (CP-HU07-2)
        string reactivarButtonSelector = $"button[data-vacante-id='{VacanteIdFijaParaEstado}'].btn-outline-success";
        IWebElement botonReactivar = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(reactivarButtonSelector)));

        botonReactivar.Click();
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("habilitarModal")));

        IWebElement confirmarReactivar = _driver.FindElement(By.CssSelector("#habilitarModal button[type='submit'].btn-success"));
        confirmarReactivar.Click();

        // ASSERT: REACTIVACIÓN EXITOSA
        wait.Until(ExpectedConditions.UrlContains(BaseUrl + "/Vacantes"));

        string estadoActivoSelector = $"tr:has(button[data-vacante-id='{VacanteIdFijaParaEstado}']) .badge.bg-success";
        IWebElement estadoActivo = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(estadoActivoSelector)));
        Assert.IsNotNull(estadoActivo, "CP-HU07-2 FALLIDO: La vacante no cambió a estado Activo.");
    }
}