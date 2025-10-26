using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;

namespace Reclutamiento.Tests.Integration;

[TestClass]
public class CRUDAdministradoresIntegrationTest
{
    private IWebDriver _driver;

    private const string BaseUrl = "https://localhost:44358";

    // VALORES QUE DEBEN EXISTIR EN LA BASE DE DATOS PARA EDITAR Y ELIMINAR
    private const int AdminIdFijoParaEdicionYEliminacion = 10;
    private const string AdminEmailParaBusqueda = "admin.editar.existente@test.com";

    // Credenciales del Admin Principal (Walter)
    private const string UsuarioValido = "Walter";
    private const string EmailValido = "eteriumtech@gmail.com";
    private const string ContrasenaValida = "uapycxlmtlrhowcu";

    // Datos del Nuevo Administrador de Prueba (HU10-1)
    private const string NuevoAdminNombre = "Admin QA Test - CREAR";
    private const string NuevoAdminEmail = "admin.qa.creacion.1@test.com";
    private const string NuevoAdminPassword = "Password123*";

    // Datos de Edición (HU10-2)
    private const string AdminNombreEditado = "Admin QA Editado - CAMBIADO";
    private const string AdminNombreOriginal = "Nombre Original de Admin 3";

    [TestInitialize]
    public void Setup()
    {
        var driverPath = Path.GetDirectoryName(typeof(CRUDAdministradoresIntegrationTest).Assembly.Location);
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


    // CP-HU10-1: CREAR ADMINISTRADOR
    /// Verifica la creación de un nuevo administrador y su aparición en el listado.
    [TestMethod]
    public void HU10_1_CrearAdministrador_DebeCrearYMostrarEnListado()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        // ACT
        _driver.Navigate().GoToUrl(BaseUrl + "/Usuarios/Crear");
        wait.Until(ExpectedConditions.UrlContains("/Usuarios/Crear"));

        _driver.FindElement(By.Id("NombreCompleto")).SendKeys(NuevoAdminNombre);
        _driver.FindElement(By.Id("CorreoElectronico")).SendKeys(NuevoAdminEmail);
        _driver.FindElement(By.Id("Contrasena")).SendKeys(NuevoAdminPassword);

        _driver.FindElement(By.CssSelector("button[type='submit'].btn-success")).Click();

        // ASSERT
        wait.Until(ExpectedConditions.UrlContains("/Usuarios"));

        IWebElement nuevoAdmin = wait.Until(ExpectedConditions.ElementIsVisible(
            By.XPath($"//td[contains(text(), '{NuevoAdminEmail}')]")
        ));
        Assert.IsNotNull(nuevoAdmin, "CP-HU10-1 FALLIDO: El nuevo administrador no se encontró en el listado.");
    }



    // CP-HU10-2: EDITAR ADMINISTRADOR
    /// Verifica la edición del nombre de un administrador existente.
    [TestMethod]
    public void HU10_2_EditarAdministrador_DebeModificarNombre()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        // ACT 
        string editUrl = $"{BaseUrl}/Usuarios/Editar/{AdminIdFijoParaEdicionYEliminacion}";
        _driver.Navigate().GoToUrl(editUrl);

        wait.Until(ExpectedConditions.UrlContains($"/Usuarios/Editar/{AdminIdFijoParaEdicionYEliminacion}"));
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("NombreCompleto")));

        // Modificar el campo NombreCompleto
        IWebElement nombreInput = _driver.FindElement(By.Id("NombreCompleto"));
        nombreInput.Clear();
        nombreInput.SendKeys(AdminNombreEditado);

        _driver.FindElement(By.CssSelector("button[type='submit'].btn-warning")).Click();

        // ASSERT
        wait.Until(ExpectedConditions.UrlContains("/Usuarios"));

        // Buscar el administrador editado por su Nuevo Nombre Completo
        IWebElement adminEditado = wait.Until(ExpectedConditions.ElementIsVisible(
            By.XPath($"//td[contains(text(), '{AdminNombreEditado}')]")
        ));
        Assert.IsNotNull(adminEditado, "CP-HU10-2 FALLIDO: El administrador editado no se encontró con el nuevo nombre.");
    }



    // CP-HU10-3: ELIMINAR ADMINISTRADOR
    /// Verifica la eliminación de un administrador existente.
    [TestMethod]
    public void HU10_3_EliminarAdministrador_DebeEliminarYRevocarAcceso()
    {
        // ARRANGE
        PerformAdminLogin();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        // ACT
        string deleteConfirmUrl = $"{BaseUrl}/Usuarios/Eliminar/{AdminIdFijoParaEdicionYEliminacion}";
        _driver.Navigate().GoToUrl(deleteConfirmUrl);

        wait.Until(ExpectedConditions.UrlContains($"/Usuarios/Eliminar/{AdminIdFijoParaEdicionYEliminacion}"));

        _driver.FindElement(By.CssSelector("button[type='submit'].btn-danger")).Click();

        // ASSERT
        wait.Until(ExpectedConditions.UrlContains("/Usuarios"));

        // Intentar buscar el usuario eliminado por su email original.
        try
        {
            // Intentamos esperar solo 3 segundos para confirmar que NO está
            var tempWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            tempWait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//td[contains(text(), '{AdminEmailParaBusqueda}')]")));

            // Si llega aquí, significa que el elemento se encontró
            Assert.Fail("CP-HU10-3 FALLIDO: El administrador eliminado aún aparece en la lista.");
        }
        catch (WebDriverTimeoutException)
        {
            // Éxito: El elemento no se encontró, lo que indica que se eliminó correctamente.
            Assert.IsTrue(true, "CP-HU10-3 EXITOSO: El administrador fue eliminado y ya no aparece en el listado.");
        }
    }
}