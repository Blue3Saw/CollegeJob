using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Http.Hosting;
using DAO;
using BO;
using System.Data;

namespace ProyectoUniJob.Controllers
{
    public class EjemploApiController : ApiController
    {
        EstatusDAO ObjDAO = new EstatusDAO();
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]

        public DataTable Estatus()
        {
            return ObjDAO.VerEstatus();
        }

        //public List<object> Estatus()
        //{
        //    List<object> listausu = ObjDAO.VerEstatus().AsEnumerable().ToList<object>();
        //    return listausu;
        //}


        //public List<EstatusBO> Estatus()
        //{
        //    List<EstatusBO> estado = new List<EstatusBO>();
        //    DataTable estadoTable = ObjDAO.VerEstatus();
        //    foreach (DataRow row in estadoTable.Rows)
        //    {
        //        EstatusBO newEstatus = new EstatusBO();
        //        newEstatus.Codigo = int.Parse( row["Codigo"].ToString());
        //        newEstatus.Estatus = row["Estatus"].ToString();
        //        estado.Add(newEstatus);
        //    }
        //    return estado;
        //}

    }
}
