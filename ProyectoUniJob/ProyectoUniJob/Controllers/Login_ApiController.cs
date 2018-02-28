using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using BO;
using DAO;
using System.Data;
using System.Web.Hosting;
using System.Web.Http.Routing;

namespace ProyectoUniJob.Controllers
{
    public class Login_ApiController : ApiController
    {
        UsuariosDAO ObjDAO = new UsuariosDAO();
        UsuarioBO ObjBO = new UsuarioBO();
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]

        public int Login(string Usu, string Contra)
        {
            ObjBO.Email = Usu;
            ObjBO.Contraseña = Contra;
            return ObjDAO.LoginEmpleador(ObjBO);
        }


    }
}
