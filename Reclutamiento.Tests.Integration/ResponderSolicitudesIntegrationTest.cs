using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;

namespace Reclutamiento.Tests.Integration;

[TestClass]
public class ResponderSolicitudesIntegrationTest
{
    private IWebDriver _driver;

    private const string BaseUrl = "https://localhost:44358";

    // ID Fijo de la Solicitud (Debe existir, pertenecer a una vacante y no tener respuesta previa)
    private const int SolicitudIdFija = 11;
    // ID de la vacante a la que pertenece la solicitud
    private const int VacanteIdDeSolicitud = 2;

    // Credenciales de Admin (Walter)
    private const string UsuarioValido = "Walter";
    private const string EmailValido = "eteriumtech@gmail.com";
    private const string ContrasenaValida = "uapycxlmtlrhowcu";

    [TestInitialize]
    public void Setup()
    {
        var driverPath = Path.GetDirectoryName(typeof(ResponderSolicitudesIntegrationTest).Assembly.Location);
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


    /// CP-HU09-1: Crear respuesta para solicitud.
    [TestMethod]
    public void HU09_ResponderSolicitud_ConContenidoValido()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        // Navegación directa a la vista de Solicitudes
        string solicitudesUrl = $"{BaseUrl}/Vacantes/Solicitudes?vacanteId={VacanteIdDeSolicitud}";
        _driver.Navigate().GoToUrl(solicitudesUrl);
        wait.Until(ExpectedConditions.UrlContains($"/Vacantes/Solicitudes?vacanteId={VacanteIdDeSolicitud}"));

        // ACT
        string modalTarget = $"#responderModal-{SolicitudIdFija}";
        string responderButtonSelector = $"button[data-bs-target='{modalTarget}']";

        IWebElement botonResponder = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(responderButtonSelector)));
        botonResponder.Click();


        IWebElement modal = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"responderModal-{SolicitudIdFija}")));

        //ACT: Llenar y Enviar el Formulario
        string contenidoMensaje = "Su solicitud ha sido revisada y quisiéramos invitarle a una entrevista inicial.";
        IWebElement textarea = modal.FindElement(By.Name("ContenidoMensaje"));
        textarea.SendKeys(contenidoMensaje);

        IWebElement botonEnviar = modal.FindElement(By.CssSelector("button[type='submit'].btn-success"));
        botonEnviar.Click();

        // ASSERT
        string successMessageSelector = ".alert.alert-success";
        IWebElement successAlert = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(successMessageSelector)));

        Assert.IsTrue(successAlert.Text.Contains("éxito"),
                      "CP-HU09-1 FALLIDO: El mensaje de respuesta no se guardó correctamente o la alerta de éxito no apareció.");


    }

    /// CP-HU09-2: Enviar respuesta sin contenido (Debe fallar).
    [TestMethod]
    public void HU09_ResponderSolicitud_SinContenidoDebeFallar()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        // Navegación directa a la vista de Solicitudes
        string solicitudesUrl = $"{BaseUrl}/Vacantes/Solicitudes?vacanteId={VacanteIdDeSolicitud}";
        _driver.Navigate().GoToUrl(solicitudesUrl);
        wait.Until(ExpectedConditions.UrlContains($"/Vacantes/Solicitudes?vacanteId={VacanteIdDeSolicitud}"));

        // ACT
        string modalTarget = $"#responderModal-{SolicitudIdFija}";
        string responderButtonSelector = $"button[data-bs-target='{modalTarget}']";

        IWebElement botonResponder = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(responderButtonSelector)));
        botonResponder.Click();

        IWebElement modal = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"responderModal-{SolicitudIdFija}")));

        // ACT: DEJAR VACÍO y Enviar el Formulario
        IWebElement textarea = modal.FindElement(By.Name("ContenidoMensaje"));
        textarea.Clear();

        IWebElement botonEnviar = modal.FindElement(By.CssSelector("button[type='submit'].btn-success"));
        botonEnviar.Click();

        // ASSERT
        string errorMessageSelector = ".alert.alert-danger";

        wait.Until(ExpectedConditions.UrlContains($"/Vacantes"));

        IWebElement errorAlert = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(errorMessageSelector)));


    }
}