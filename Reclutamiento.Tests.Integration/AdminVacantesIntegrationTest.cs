using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;

namespace Reclutamiento.Tests.Integration;

[TestClass]
public class AdminVacantesIntegrationTest
{
    private IWebDriver _driver;

    private const string BaseUrl = "https://localhost:44358";

    // Credenciales de Admin (Walter)
    private const string UsuarioValido = "Walter";
    private const string EmailValido = "eteriumtech@gmail.com";
    private const string ContrasenaValida = "uapycxlmtlrhowcu";


    [TestInitialize]
    public void Setup()
    {
        var driverPath = Path.GetDirectoryName(typeof(AdminVacantesIntegrationTest).Assembly.Location);
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

        // Esperar a la redirección a /Home/Index
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(ExpectedConditions.UrlContains("https://localhost:44358"));
    }


    /// CP-HU03-1 y CP-HU03-2: Botón 'Ver solicitudes' visible/habilitado y redirección.
    [TestMethod]
    public void HU03_VerSolicitudes_DebeSerVisibleYRedirigirAListado()
    {
        // ARRANGE

        PerformAdminLogin();

        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));


        _driver.Navigate().GoToUrl(BaseUrl + "/Vacantes");


        wait.Until(ExpectedConditions.UrlContains("/Vacantes"));
        wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("table-responsive")));


        // ACT: Localizar el botón "Ver Solicitudes" de la PRIMERA vacante de la lista
        IWebElement? botonVerSolicitudes = null;
        try
        {
            // Selector CSS para el botón de la primera fila
            botonVerSolicitudes = _driver.FindElement(By.CssSelector("tbody tr:first-child a.btn-info"));
        }
        catch (NoSuchElementException)
        {

            botonVerSolicitudes = _driver.FindElement(By.CssSelector(".d-grid.gap-2 a.btn-info.btn-sm"));
        }

        // CP-HU03-1 ASSERT (Visibilidad y Habilidad)
        Assert.IsNotNull(botonVerSolicitudes, "CP-HU03-1 FALLIDO: No se encontró el botón 'Ver Solicitudes' para la primera vacante. Asegúrate de que hay datos.");
        Assert.IsTrue(botonVerSolicitudes.Displayed, "CP-HU03-1 FALLIDO: El botón 'Ver Solicitudes' no es visible.");
        Assert.IsTrue(botonVerSolicitudes.Enabled, "CP-HU03-1 FALLIDO: El botón 'Ver Solicitudes' no está habilitado.");

        // ACT (Redirección)
        botonVerSolicitudes.Click();

        // CP-HU03-2 ASSERT (Redirección)
        wait.Until(ExpectedConditions.UrlContains("/Vacantes/Solicitudes"));

        // Verificación Adicional: El título de la página debe reflejar el cambio.
        string pageTitle = _driver.FindElement(By.TagName("h4")).Text;
        Assert.IsTrue(pageTitle.Contains("Solicitudes"), "CP-HU03-2 FALLIDO: No se redirigió a la vista de Solicitudes.");
        Assert.IsTrue(_driver.Url.Contains("vacanteId="), "CP-HU03-2 FALLIDO: La URL no contiene el ID de la vacante.");
    }
}