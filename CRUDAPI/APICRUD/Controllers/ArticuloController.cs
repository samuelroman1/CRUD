using APICRUD.Context;
using APICRUD.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace APICRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticuloController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ArticuloController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Leer()
        {
            try
            {
                var res = await _context.Articulo.ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {

                throw new Exception("El error es el siguiente" + ex.Message);
            }

        }

        [HttpGet("{id}")]

        public ActionResult<Articulo> Lista(int id)
        {
            try
            {
                var res = _context.Articulo.FirstOrDefault(x => x.Id == id);
                return res;
            }
            catch (Exception ex)
            {

                throw new Exception ("Ocurrio un error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Articulo i)
        {
            try
            {
                var res = new Articulo
                {
                    Nombre = i.Nombre,
                    Precio = i.Precio,
                    Proveedor = i.Proveedor,
                };

                _context.Articulo.Add(i);
                await _context.SaveChangesAsync();
                return Ok(i);
            } catch (Exception ex)
            {
                throw new Exception("Este es el siguiente error: " + ex.Message);
            }
        }

        #region ----------Codigo de actualizar ---------
        //[HttpPut]
        //public async Task<IActionResult> Editar([FromBody] Articulo i, int id)
        //{
        //    try
        //    {
        //        string coman = "update Articulo set Nombre='" + i.Nombre + "', Proveedor='" + i.Proveedor + "', Precio='" + i.Precio + "' where Id=" + id + "";
        //        SqlConnection conn = new SqlConnection("Data Source= DESKTOP-AQ0DCRI;initial catalog= Tienda25AM;Integrated Security=True");
        //        var res = _context.Articulo.Where(x => x.Id == id).ToListAsync();

        //        SqlCommand cmd = new SqlCommand(coman, conn);

        //        conn.Open();
        //        await cmd.ExecuteNonQueryAsync();
        //        conn.Close();

        //        await _context.SaveChangesAsync();
        //        return Ok(i);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("Este es el siguiente error" + ex.Message);
        //    }
        //}
        #endregion

        [HttpPut("{id}")]
        public ActionResult<Articulo>Editar([FromBody] Articulo i, int id)
        {
            try
            {
                var res = _context.Articulo.Find(id);

                if (res != null)
                {


                    res.Nombre = i.Nombre;
                    res.Proveedor = i.Proveedor;
                    res.Precio = i.Precio;


                    _context.Entry(res).State = EntityState.Modified;
                    _context.SaveChangesAsync();
                }
                return res;
            }
            catch (Exception ex)

            {

                throw new Exception("Sucedio un error: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar([FromBody] Articulo i, int id)
        {
            try
            {
                var res = _context.Articulo.FirstOrDefault(x => x.Id == id);

                if (res != null)
                {
                    _context.Articulo.Remove(res);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                throw new Exception("El error es el siguiente: " + ex.Message);
            }
        }
    }
}
