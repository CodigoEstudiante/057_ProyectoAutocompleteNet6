﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private readonly string cadenaSql;

        public HomeController(IConfiguration configuration)
        {
            cadenaSql = configuration.GetConnectionString("CadenaSQL");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult BusquedaProducto(string busqueda) { 
        
            List<Busqueda> Lista = new List<Busqueda>();

            using (var conexion = new SqlConnection(cadenaSql))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_busqueda_productos", conexion);
                cmd.Parameters.AddWithValue("busqueda", busqueda);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Lista.Add(new Busqueda()
                        {
                            value = Convert.ToInt32(dr["IdProducto"]),
                            label = dr["texto"].ToString(),
                            precio = Convert.ToDecimal(dr["Precio"]),
                            descripcion =dr["Descripcion"].ToString()
                        });
                    }
                }
            }

            return Json(Lista);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}