using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;

namespace Reclutamiento.Tests.Integration;

[TestClass]
public class SolicitudesVacantesIntegrationTest
{
    private IWebDriver _driver;

    private const string BaseUrl = "https://localhost:44358";

    private const int VacanteIdConSolicitudes = 3; // Debe tener solicitudes enviadas (CP-HU08-1)
    private const int VacanteIdSinSolicitudes = 4; // Debe estar vacía (CP-HU08-2)

    // Credenciales de Admin (Walter)
    private const string UsuarioValido = "Walter";
    private const string EmailValido = "eteriumtech@gmail.com";
    private const string ContrasenaValida = "uapycxlmtlrhowcu";

    [TestInitialize]
    public void Setup()
    {
        var driverPath = Path.GetDirectoryName(typeof(SolicitudesVacantesIntegrationTest).Assembly.Location);
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


    /// CP-HU08-1: Listar solicitudes de una vacante con datos completos.
    [TestMethod]
    public void HU08_ListarSolicitudes_ConDatosCompletos()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        string solicitudesUrl = $"{BaseUrl}/Vacantes/Solicitudes?vacanteId={VacanteIdConSolicitudes}";
        _driver.Navigate().GoToUrl(solicitudesUrl);

        wait.Until(ExpectedConditions.UrlContains($"/Vacantes/Solicitudes?vacanteId={VacanteIdConSolicitudes}"));
        wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("h4")));


        // ASSERT
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("solicitudesTable")));

        string primerFilaSelector = "#solicitudesTable tbody tr.border-bottom";

        IWebElement primeraFila = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(primerFilaSelector)));

        Assert.IsNotNull(primeraFila, $"CP-HU08-1 FALLIDO: No se encontraron filas en la tabla para la Vacante {VacanteIdConSolicitudes}.");

        int rowCount = _driver.FindElements(By.CssSelector("#solicitudesTable tbody tr.border-bottom")).Count;
        Assert.IsTrue(rowCount > 0, $"CP-HU08-1 FALLIDO: La tabla se cargó, pero tiene {rowCount} filas. Se esperaba al menos 1.");
    }

    /// CP-HU08-2: Vacante sin solicitudes.
    [TestMethod]
    public void HU08_ListarSolicitudes_SinSolicitudes()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        string solicitudesUrl = $"{BaseUrl}/Vacantes/Solicitudes?vacanteId={VacanteIdSinSolicitudes}";
        _driver.Navigate().GoToUrl(solicitudesUrl);

        wait.Until(ExpectedConditions.UrlContains($"/Vacantes/Solicitudes?vacanteId={VacanteIdSinSolicitudes}"));


        // ASSERT
        string mensajeSinSolicitudesTexto = "¡Aún no hay Solicitudes!";
        string mensajeSelector = ".alert.alert-info";

        IWebElement mensajeInfo = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(mensajeSelector)));

        Assert.IsTrue(mensajeInfo.Text.Contains(mensajeSinSolicitudesTexto),
                      $"CP-HU08-2 FALLIDO: El mensaje '{mensajeSinSolicitudesTexto}' no apareció en la Vacante {VacanteIdSinSolicitudes}.");
    }
}