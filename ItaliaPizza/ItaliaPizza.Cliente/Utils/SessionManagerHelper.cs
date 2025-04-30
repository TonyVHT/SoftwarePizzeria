using ItaliaPizza.Cliente.Screens;
using ItaliaPizza.Cliente.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            // Cierra cualquier ventana que no sea LogIn
            foreach (Window win in Application.Current.Windows)
            {
                if (win is not LogIn)
                {
                    win.Close();
                    break;
                }
            }
        }
    }
}
