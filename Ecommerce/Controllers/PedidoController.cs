using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    public class PedidoController : Controller
    {
        public IActionResult Index(int id)
        {
            using (var data = new ItemData())
            {
                return View(data.Read(id));
            }
        }
    }
}
