﻿using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BanBan.Controls
{
    class LoginControl : Utilidades
    {
        private readonly IQueryable<usuario> usu;
        public static bool UsuarioValido { get; set; }
        public static string tipoUsuario { get; set; }
        public static bool offline { get; set; }
        public LoginControl()
        {
            UsuarioValido = false;
            tipoUsuario = null;
            offline = false;
            usu = from us in sb.usuario select us;
        }
        public void isUsuarioValido(string usuario, string clave)
        {
            Mouse.OverrideCursor = Cursors.AppStarting;
            try
            {
                if (usu != null)
                {
                    UsuarioValido = (from us in usu
                                     where us.usuario1.Equals(usuario) &&
                                     us.contrasena.Equals(clave)
                                     select us).Any();
                    if (UsuarioValido)
                    {
                        tipoUsuario = (from us in usu
                                       join tp in sb.tipousuario on us.idTipo equals tp.idTipo
                                       where us.usuario1 == usuario &&
                                       us.contrasena == clave
                                       select tp.tipo).First();
                    }
                }
            }
            catch (System.Exception)
            {
                UsuarioValido = new HorasExtraOfflineControl().OfflineLogin(usuario,clave);
                tipoUsuario = "Supervisor";
                offline = true;
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public async Task<bool> verificarUsuario()
        {
            while (!UsuarioValido)
            {
                await Task.Delay(25);
            }
            return true;
        }
    }
}