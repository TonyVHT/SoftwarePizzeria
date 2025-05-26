using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Resources
{
    class ValidadorReglas
    {
        private static string PatronEmail = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";

        private static string PatronContraseña = @"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&()_+\[\]{};':""\\|,.<>/?-]).{8,64}$";

        public void AñadirLimiteACamposDeTexto(TextBox textBox, int limite)
        {
            textBox.TextChanged += (sender, e) =>
            {
                if (textBox.Text.Length > limite)
                {
                    textBox.Text = textBox.Text.Substring(0, limite);
                    textBox.CaretIndex = textBox.Text.Length;
                }

                string textoActual = textBox.Text.Replace("  ", " ");
                if (textoActual != textBox.Text)
                {
                    textBox.Text = textoActual;
                    textBox.CaretIndex = textBox.Text.Length;
                }

                if (textBox.Text.StartsWith(" "))
                {
                    textBox.Text = textBox.Text.TrimStart();
                    textBox.CaretIndex = textBox.Text.Length;
                }

                if (textBox.Text.Length == 1 && textBox.Text == " ")
                {
                    textBox.Text = string.Empty;
                }

                if (textBox.Text.Length > 1 &&
                    textBox.Text.Substring(textBox.Text.Length - 2) == "  ")
                {
                    textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - 1);
                    textBox.CaretIndex = textBox.Text.Length;
                }
            };
        }


        public void AñadirLimiteACamposDeContraseña(PasswordBox passwordBox, int limite)
        {
            passwordBox.PasswordChanged += (sender, e) =>
            {

                string currentPassword = passwordBox.Password;

                if (currentPassword.Length > limite)
                {
                    currentPassword = currentPassword.Substring(0, limite);
                }

                if (currentPassword.StartsWith(" "))
                {
                    currentPassword = currentPassword.TrimStart();
                }

                string cleanPassword = currentPassword.Replace(" ", "");
                if (cleanPassword != passwordBox.Password)
                {
                    passwordBox.Password = cleanPassword;
                }

                passwordBox.Focus();
            };
        }

        public bool ValidarEmail(string email)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(email))
            {
                isValid = false;
            }
            return isValid;
        }

        public bool ValidarContraseña(string contraseña)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(contraseña) || !Regex.IsMatch(contraseña, PatronContraseña))
            {
                isValid = false;
            }
            return isValid;
        }

        public void EvitarCaracteresPeligrosos(TextBox textBox)
        {
            textBox.TextChanged += (sender, e) =>
            {
                string caracteresPeligrosos = @"'"";--/*()%[]{}!#$&=?¡_:+¿|,~°´¨";

                string textoFiltrado = new string(textBox.Text
                    .Where(c => !caracteresPeligrosos.Contains(c))
                    .ToArray());

                if (textoFiltrado != textBox.Text)
                {
                    int posicionCursor = textBox.CaretIndex;
                    textBox.Text = textoFiltrado;
                    textBox.CaretIndex = Math.Min(posicionCursor, textoFiltrado.Length);
                }
            };
        }

        public void EvitarCaracteresPeligrososEnContraseña(PasswordBox passwordBox)
        {
            passwordBox.PasswordChanged += (sender, e) =>
            {
                string caracteresPeligrosos = @"'"";--/*()%[]{}!#$&=?¡_:+¿|,~°´¨";

                string textoFiltrado = new string(passwordBox.Password
                    .Where(c => !caracteresPeligrosos.Contains(c))
                    .ToArray());

                if (textoFiltrado != passwordBox.Password)
                {
                    passwordBox.Password = textoFiltrado;
                }
            };
        }

    }
}
