using ItaliaPizza.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Singleton
{
    public class UserSessionManager
    {
        private static UserSessionManager instance;
        private UsuarioLoggeado usuario;

        private UserSessionManager() { }

        public static UserSessionManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new UserSessionManager();

                return instance;
            }
        }

        public void Login(UsuarioLoggeado usuarioEnSesion)
        {
            this.usuario = usuarioEnSesion;
        }

        public void Logout()
        {
            usuario = null;
        }

        public bool IsLoggedIn => usuario != null;

        public UsuarioLoggeado GetUsuario()
        {
            return usuario;
        }

        public string GetRol()
        {
            return usuario?.Rol;
        }

        public int? GetUsuarioId()
        {
            return usuario?.Id;
        }
    }

}
