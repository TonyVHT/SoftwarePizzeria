using ItaliaPizza.Cliente.Screens;
using ItaliaPizza.Cliente.Singleton;
using System.Linq;
using System.Windows;

namespace ItaliaPizza.Cliente.Utils
{
    public static class SessionManagerHelper
    {
        public static void CerrarSesionUniversal()
        {
            UserSessionManager.Instance.Logout();

            var login = new LogIn();
            login.Show();

            foreach (Window win in Application.Current.Windows.OfType<Window>().ToList())
            {
                if (win != login) 
                {
                    win.Close();
                }
            }
        }
    }
}
