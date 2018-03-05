using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAO;
using BO;
using System.Net.Mail;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;

namespace ProyectoUniJob.Controllers.FrontEnd
{
    public class TareasController : Controller
    {        
        TareasDAO ObjDAO = new TareasDAO();
        FotosDAO DAOFoto = new FotosDAO();        
        // GET: Tareas
        public ActionResult Index()
        {      
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarTarea(string agregar, string modificar, string eliminar, string IdTarea, string NombreUsu, string Titulo, string Direccion,
            string Latitud, string Longitud, string FechaTarea, string HoraInicioTarea, string HoraFinTarea, string cmbClas, string Descripcion, string inputLabel,
            string CantPersonas, HttpPostedFileBase[] Imagen)
        {
            TareasBO obj = new TareasBO();
            int A = Convert.ToInt32(agregar);
            int M = Convert.ToInt32(modificar);
            int E = Convert.ToInt32(eliminar);

            if (IdTarea != "")
            {
                obj.Codigo = Convert.ToInt32(IdTarea);
            }

            //obj.CodigoEmpleador = 3;// (int)Session["Codigo"]; //Convert.ToInt32(NombreUsu);

           obj.CodigoEmpleador = int.Parse(Session["Codigo"].ToString()); //Convert.ToInt32(NombreUsu);

            obj.Titulo = Titulo;
            obj.Direccion = Direccion;
            obj.Latitud = float.Parse(Latitud);
            obj.Longitud = float.Parse(Longitud);
            obj.Fecha = DateTime.Parse(FechaTarea);
            obj.HoraInicio = DateTime.Parse(HoraInicioTarea);
            obj.HoraFin = DateTime.Parse(HoraFinTarea);
            obj.TipoTarea = Convert.ToInt32(cmbClas);
            obj.Descripcion = Descripcion;
            obj.CodigoEstatus = Convert.ToInt32(inputLabel);
            obj.CantPersonas = Convert.ToInt32(CantPersonas);
           
            if (A > 0)
            {               
                //obj.Codigo = ObjDAO.AgregarTarea(obj);
                
                FotosBO FotBO = new FotosBO();
                FotosDAO fotdao = new FotosDAO();
                FotBO.CodigoTarea=ObjDAO.AgregarTarea(obj);
                //FotBO.CodigoTarea = fotdao.idinsetado();
                for (int i = 0; i < Imagen.Length; i++)
                {
                    FotBO.Imagen = new byte[Imagen[i].ContentLength];
                    Imagen[i].InputStream.Read(FotBO.Imagen, 0, Imagen[i].ContentLength);
                    //AgregarImagenTarea(Imagen, obj.Codigo);
                    fotdao.AgregarFoto(FotBO);
                }
                ViewBag.Script = "Agregado";
            }
            else if (M > 0)
            {
                ObjDAO.ActualizarTarea(obj);
            }
            return Redirect("/Usuario/IndexEmpleador#parentHorizontalTab2");
        }
        //public void AgregarImagenTarea(IEnumerable<HttpPostedFileBase> Imagen, int IdTarea)
        //{
        //    FotosBO FotBO = new FotosBO();
        //    FotosDAO DAOFotos = new FotosDAO();
        //    FotBO.CodigoTarea = IdTarea;
        //    if (Imagen != null)
        //    {
        //        foreach ( var item in Imagen)
        //        {
        //            //var filename = Path.GetFileName(item.FileName);
        //            //var path2 = Path.Combine(Server.MapPath("~/Recursos/FontEnd/images/"), filename);
        //            //item.SaveAs(path2);
        //            //FotBO.Imagen = filename;
        //            //DAOFotos.AgregarFoto(FotBO);
        //            //FotBO.Imagen = new byte[item.ContentLength];
        //            //Imagen.InputStream.Read(bo.Imagen, 0, Imagen.ContentLength);
        //        }
        //    }
        //    else
        //    {
        //        FotBO.Imagen = "Ninguna";
        //    }           
        //}

        public ActionResult Upload(IEnumerable<HttpPostedFileBase> Imagen)
        {
            return new EmptyResult();
        }

        public ActionResult BuscarTarea(string Codigo)
        {
            int Clave = int.Parse(Codigo);
            return View(ObjDAO.BuscarTarea(Clave));
        }

        public ActionResult ActualizarTarea()
        {
            return View();
        }

        public ActionResult EliminarTarea(string id)
        {
            TareasBO objBO = new TareasBO();
            objBO.Codigo = int.Parse(id);
            ObjDAO.EliminarTarea(objBO);
            return View("Index");
        }
        public ActionResult ListarTipoTarea()
        {
            ClasificacionTareaDAO ObjCla = new ClasificacionTareaDAO();
            var Tipo = new ClasificacionTareaBO();
            Tipo.TipoTarea = ObjCla.ListaTipo();
            return PartialView(Tipo.TipoTarea);
        }
        public ActionResult TodasTareas()
        {
            return View(ObjDAO.TodasTareas());
        }

        public ActionResult TodasTareasEmpleador()
        {
            int filtro = 0;
            int Codigo = int.Parse(Session["Codigo"].ToString());
            if (Session["Filtro"].ToString() == null)
            {
                filtro = 0;
            }
            else
            {
                filtro = int.Parse(Session["Filtro"].ToString());
            }
            return View(ObjDAO.TodasTareasEmpleador(Codigo, filtro));
        }


        [HttpPost]
        public ActionResult FiltroTareas(string Filtro)
        {
            int entrada = int.Parse(Filtro);
            Session["Filtro"] = entrada;
            return Redirect("~/Usuario/IndexEmpleador");
        }
        public ActionResult FiltroTareasView()
        {
            return View("FiltroTareas");
        }

        public ActionResult TareasAcepUsuario()
        {
            UsuarioBO dato = new UsuarioBO();
            dato.Codigo = int.Parse(Session["Codigo"].ToString());
            return View(ObjDAO.TareasAcepUsuario(dato.Codigo));
        }

        public ActionResult TareaAceptSeleccionada(string Codigo)
        {
            int Clave = int.Parse(Codigo);
            return View(ObjDAO.TareaSeleccionada(Clave));
        }

        public ActionResult TareaSeleccionada(string Codigo)
        {
            ViewData["Fecha"] = ObjDAO.BuscarFecha(Codigo);
            ViewData["permiso"] = Session["Permiso"].ToString();
            int Clave = int.Parse(Codigo);
            //int Clave = 12;
            ViewData["Tarea"] = Clave;
            ViewData["Postulados"] = ObjDAO.postulados(Clave);
            ViewData["Imagenes"] = ObjDAO.ImgenesTarea(Clave);
            //ViewData["NFilas"] = ObjDAO.ImgenesTarea(Clave).Rows.Count;
            return View(ObjDAO.TareaSeleccionada(Clave));
        }

        
        public ActionResult VistaTarea()
        {
            
            return PartialView("VistaTarea");
        }

        [HttpPost]
        public ActionResult HacerTarea(string Tarea2,string oferta)
        {
            int Estudiante = int.Parse(Session["Codigo"].ToString());
            int Clave = int.Parse(Tarea2);
            int precio = int.Parse(oferta);
            //ObjDAO.AceptarTarea(Clave);
            ObjDAO.AceptarTarea2(Estudiante, Clave,precio);
            //Session["Tarea"] = Codigo;
            return RedirectToAction("IndexEstudiante", "Usuario");
        }
        public ActionResult FinalizarTarea()
        {
            
            return Redirect("/Usuario/IndexEmpleador#parentHorizontalTab2");
        }
        public ActionResult AgregarCalif(string calif, string comentario, int tarea, int empleador)
        {
            CalificacionesBO OBO = new CalificacionesBO();
            CalificacionesDAO obj = new CalificacionesDAO();
            int Tarea = tarea;
            
            OBO.CodigoTarea = Tarea;
            OBO.UsCalifica = int.Parse(Session["codigo"].ToString());
            OBO.UsCalificado =empleador;
            OBO.Calificacion = int.Parse(calif);
            OBO.Comentario = comentario;
            obj.AgregarCalificacion(OBO);
            //Session["Tarea"] = codigo;
            return RedirectToAction("IndexEstudiante", "Usuario");
        }
        [HttpGet]
        public ActionResult EnviarCorreoView()
        {
            DAO.TareasDAO tareas = new TareasDAO();
            var tarea = tareas.BuscarTareaSaidy(4);
            return PartialView("EnviarCorreo", tarea);
        }

        [HttpPost]
        public ActionResult EnviarCorreo()
        {
            DAO.TareasDAO tareas = new TareasDAO();
            var tarea = tareas.BuscarTareaSaidy(4);
            DAO.UsuariosDAO User = new UsuariosDAO();
            UsuarioBO Usuario = User.PerfilUsuario(int.Parse(Session["codigo"].ToString()));
            string CorreoRemitente = "collegeJobSGM@gmail.com";
            string CorreoDestinatario = Usuario.Email;
            MailMessage Correo = new MailMessage();
            Correo.To.Add(new MailAddress(CorreoDestinatario));
            Correo.From = new MailAddress(CorreoRemitente);
            Correo.Subject = "Tarea: " + tarea.Titulo;
            Correo.Body = "La tarea"+tarea.Titulo+" ha finalizado <a href='http://pinia.gear.host/Tareas/VistaCAlif?idtarea="+4+"'><img src='http://noeliareginelli.com/wp-content/uploads/2017/10/boton-clic-aqui.png' width='120px'/></a>";
            Correo.IsBodyHtml = true;
            Correo.Priority = MailPriority.Normal;
            SmtpClient Cliente = new SmtpClient();
            Cliente.Host = "smtp.gmail.com";
            Cliente.Port = 587;
            Cliente.EnableSsl = true;
            Cliente.Credentials = new NetworkCredential("collegeJobSGM@gmail.com", "SGM123456");
            
            {
                Cliente.Send(Correo);
                return Redirect("/Usuario/IndexEmpleador#parentHorizontalTab5");
            }
            
            {
                //return Content("Error");
            }
        }
        public ActionResult VistaCAlif(int idtarea)
        {
            DAO.TareasDAO tareas = new TareasDAO();
            var tarea = tareas.BuscarTareaSaidy(5);
            return View(tarea);
        }
        //metodos para la vista de ver perfil usuario por parte del empleador  
        
        
        [HttpPost]
        public ActionResult AceptarEmpleador(string Tarea,string Codigo )
        {
            int ta = int.Parse(Tarea);
            int cod = int.Parse(Codigo);
            int solucion=ObjDAO.Aceptarpostulados(cod,ta);

            string pagina = "/Tareas/TareaSeleccionada?Codigo="+ta;

            return Redirect("/Tareas/TareaSeleccionada?Codigo=" + ta);
        }
        
        //devuelve las tareas a aceptar por parte del usuario
        public ActionResult AceptartareasEmpleados()
        {
            int codigo = int.Parse(Session["Codigo"].ToString());
            //int codigo = 4;
            DataTable tareas = ObjDAO.aceptartareasempleador(codigo);

            DataTable tareas2 = new DataTable();

            //creamos las columnas del datatable
            tareas2.Columns.Add("Titulo");
            tareas2.Columns.Add("Fecha");
            tareas2.Columns.Add("Precio");
            tareas2.Columns.Add("Codigo");
            foreach (DataRow row in tareas.Rows)
            {
                DataRow fila = tareas2.NewRow();
                fila["Titulo"] = row[0].ToString();
                fila["Fecha"] = row[1].ToString();
                fila["Precio"] = row[2].ToString();
                fila["Codigo"] = row[3].ToString();

                int npos = ObjDAO.NopersonasTareas(int.Parse(row[3].ToString()));
                int pos = int.Parse(row[4].ToString())+1;
                if (npos<pos)
                {
                    tareas2.Rows.Add(fila);
                }
                
            }




            return View(tareas2);
        }
        
        //saber cuantas personas se pueden postular a las tareas
        
        public ActionResult Aceptotarea(string Codigo,string Opcion)
        {
            int estudiante= int.Parse(Session["Codigo"].ToString());
            if (Opcion=="Si")
            {
                string estado = "En curso";
                ObjDAO.AceptoTareaEmpleador(int.Parse(Codigo),estado,estudiante);
            }
            else
            {
                string estado = "Rechazado";
                ObjDAO.AceptoTareaEmpleador(int.Parse(Codigo), estado, estudiante);
            }

            return Redirect("/Usuario/IndexEstudiante#parentHorizontalTab3");
        }
    }
}