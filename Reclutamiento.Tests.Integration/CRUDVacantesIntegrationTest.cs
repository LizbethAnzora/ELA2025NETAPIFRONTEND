using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;

namespace Reclutamiento.Tests.Integration;

[TestClass]
public class CRUDVacantesIntegrationTest
{
    private IWebDriver _driver;

    private const string BaseUrl = "https://localhost:44358";
    private const int VacanteIdFijaParaEdicion = 2; // ID fijo usado solo para validar la funcionalidad de la vista Edit

    // Credenciales de Admin (Walter)
    private const string UsuarioValido = "Walter";
    private const string EmailValido = "eteriumtech@gmail.com";
    private const string ContrasenaValida = "uapycxlmtlrhowcu";

    // Nombres de prueba
    private const string TituloBase = "Integración QA - Desarrollador .NET";
    private const string TituloEditado = "Integración QA - Arquitecto Cloud (EDITADO)";


    [TestInitialize]
    public void Setup()
    {

        var driverPath = Path.GetDirectoryName(typeof(CRUDVacantesIntegrationTest).Assembly.Location);
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



    /// CP-HU06-1 y CP-HU06-3: Crear y editar una vacante válida.
    [TestMethod]
    public void HU06_GestionVacantes_DebeCrearYEditar()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        _driver.Navigate().GoToUrl(BaseUrl + "/Vacantes/Create");
        wait.Until(ExpectedConditions.UrlContains(BaseUrl + "/Vacantes/Create"));

        // ACT: CREAR VACANTE (CP-HU06-1) 

        // Llenar formulario con datos válidos
        _driver.FindElement(By.Id("Titulo")).SendKeys(TituloBase);
        _driver.FindElement(By.Id("Ubicacion")).SendKeys("Remoto - América Central");
        _driver.FindElement(By.Id("Descripcion")).SendKeys("Responsable del desarrollo y mantenimiento de sistemas backend con .NET Core.");
        _driver.FindElement(By.Id("Requisitos")).SendKeys("3+ años de experiencia, C#, SQL Server, Microservicios.");


        _driver.FindElement(By.CssSelector("button[type='submit'].btn-success")).Click();


        // ASSERT: CREACIÓN EXITOSA
        wait.Until(ExpectedConditions.UrlContains(BaseUrl + "/Vacantes"));
        wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("table-responsive")));
        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//td[contains(text(), '{TituloBase}')]")));


        // ACT: EDITAR VACANTE (CP-HU06-3)
        string editUrl = $"{BaseUrl}/Vacantes/Edit/{VacanteIdFijaParaEdicion}";
        _driver.Navigate().GoToUrl(editUrl);


        wait.Until(ExpectedConditions.UrlContains($"/Vacantes/Edit/{VacanteIdFijaParaEdicion}"));
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Titulo")));


        IWebElement tituloInput = _driver.FindElement(By.Id("Titulo"));
        tituloInput.Clear();
        tituloInput.SendKeys(TituloEditado);

        IWebElement descripcionInput = _driver.FindElement(By.Id("Descripcion"));
        descripcionInput.Clear();
        descripcionInput.SendKeys("Descripción Modificada para la prueba de edición.");

        _driver.FindElement(By.CssSelector("button[type='submit'].btn-warning")).Click();



        // ASSERT: EDICIÓN EXITOSA
        wait.Until(ExpectedConditions.UrlContains(BaseUrl + "/Vacantes"));
        wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("table-responsive")));


        IWebElement vacanteEditada = wait.Until(ExpectedConditions.ElementIsVisible(
            By.XPath($"//td[contains(text(), '{TituloEditado}')]")
        ));
        Assert.IsNotNull(vacanteEditada, "CP-HU06-3 FALLIDO: La vacante editada no se encontró en el listado. Confirma que la Vacante ID 1 existe.");
    }
}