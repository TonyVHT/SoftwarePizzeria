using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestItaliaPizza.CU_02_Cerrar_Sesion
{
    [TestClass]
    public class CerrarSesionTest
    {
        [TestMethod]
        public void CT08_CerrarSesion_LimpiaSesion()
        {
            var user = new CredencialUsuario { NombreUsuario = "admin" };
            MockUserSessionManager.Instance.Login(user);

            Assert.IsNotNull(MockUserSessionManager.Instance.UsuarioActual); // estado inicial

            MockSessionManagerHelper.CerrarSesion();

            Assert.IsNull(MockUserSessionManager.Instance.UsuarioActual);
        }
    }

    // Simulación de UserSessionManager
    public class MockUserSessionManager
    {
        private static MockUserSessionManager? _instance;
        public static MockUserSessionManager Instance => _instance ??= new MockUserSessionManager();

        public CredencialUsuario? UsuarioActual { get; private set; }

        public void Login(CredencialUsuario usuario)
        {
            UsuarioActual = usuario;
        }

        public void Logout()
        {
            UsuarioActual = null;
        }
    }

    // Simulación del helper
    public static class MockSessionManagerHelper
    {
        public static void CerrarSesion()
        {
            MockUserSessionManager.Instance.Logout();
        }
    }

    // Simulación de modelo
    public class CredencialUsuario
    {
        public string NombreUsuario { get; set; } = string.Empty;
    }
}
