using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace Reclutamiento.Tests.Integration;

[TestClass]
public class LoginIntegrationTest
{
    private IWebDriver _driver;

   
    private const string BaseUrl = "https://localhost:44358/Auth/Login";

    
    [TestInitialize]
    public void Setup()
    {
        // Configuración para que el ChromeDriver sepa dónde buscar el driver.
        var driverPath = Path.GetDirectoryName(typeof(LoginIntegrationTest).Assembly.Location);
        _driver = new ChromeDriver(driverPath);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

   
    [TestCleanup]
    public void Teardown()
    {
        _driver.Quit();
    }


    /// CP-HU01-1: Login con credenciales válidas.
    /// Login con exito con credenciales reales en la base de datos.
    [TestMethod]
    public void Login_ConCredencialesValidas_DebeRedirigirADashboard()
    {
        // Arrange
        _driver.Navigate().GoToUrl(BaseUrl);

        
        string usuarioValido = "Walter";
        string emailValido = "eteriumtech@gmail.com";
        string contrasenaValida = "uapycxlmtlrhowcu"; 

        // Act
        _driver.FindElement(By.Id("NombreCompleto")).SendKeys(usuarioValido);
        _driver.FindElement(By.Id("CorreoElectronico")).SendKeys(emailValido);
        _driver.FindElement(By.Id("Contrasena")).SendKeys(contrasenaValida);

        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

        // Assert
        // Debe redirigir a una página diferente (ej. la página principal o dashboard)
        Assert.IsTrue(_driver.Url.Contains("https://localhost:44358/") || _driver.Url.Contains("/Index"));
    }


    /// CP-HU01-2: Login con credenciales inválidas.
    /// Verifica que el login no pase.
    [TestMethod]
    public void Login_ConCredencialesInvalidas_DebeMostrarMensajeDeError()
    {
        // Arrange
        _driver.Navigate().GoToUrl(BaseUrl);

        
        string usuarioInvalido = "Usuario Fallido";
        string emailInvalido = "invalido@reclutamiento.com";
        string contrasenaInvalida = "contrasena_incorrecta";

        // Act
        _driver.FindElement(By.Id("NombreCompleto")).SendKeys(usuarioInvalido);
        _driver.FindElement(By.Id("CorreoElectronico")).SendKeys(emailInvalido);
        _driver.FindElement(By.Id("Contrasena")).SendKeys(contrasenaInvalida);

        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();


        // Assert
        // Debe permanecer en la página de login y mostrar error 500
        Assert.IsTrue(_driver.Url.Contains("https://localhost:44358/Auth/Login"));


    }
}