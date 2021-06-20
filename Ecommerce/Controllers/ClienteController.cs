using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    public class ClienteController : Controller
    {
        //private static List<ClienteL clientes = new List<Cliente>();

        [HttpGet]
        public IActionResult Index(Cliente novoCliente)
        {

            using (var data = new ClienteData())
                return View(data.Read());
        }

       

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection cliente)
        {
            string nome = cliente["Nome"];
            string email = cliente["Email"];
            string senha = cliente["Senha"];

            if (nome == null) {
                ViewBag.Mensagem = "Digite um nome válido";
                return View();
            }
            if (!email.Contains("@"))
            {
                ViewBag.Mensagem = "Email inválido";
                return View();
            }
            if (senha.Length < 6)
            {
                ViewBag.Mensagem = "Senha deve conter 6 caracteres ou mais";
                return View();
            }

            var novoCliente = new Cliente();
            novoCliente.Nome = cliente["nome"];
            novoCliente.Email = cliente["email"];
            novoCliente.Senha = cliente["senha"];

            using (var data = new ClienteData())
                data.Create(novoCliente);

            return RedirectToAction("Login", novoCliente);
        }

        [HttpGet]
        public IActionResult Read()
        {
            return View();
        }
        /*
                [HttpPost]
                public IActionResult Read(IFormCollection cliente)
                {
                    string nome = cliente["Nome"];
                    string email = cliente["Email"];
                    string senha = cliente["Senha"];

                    if (!email.Equals(" "))
                    {
                        var cli = new Cliente();

                        cli.Nome = cliente["Nome"];
                        cli.Email = cliente["Email"];
                        cli.Senha = cliente["Senha"];

                        Cliente c = new Cliente();

                        using (var data = new ClienteData())
                            c = data.Read(cli.Email);

                        if (c.Senha == cli.Senha)
                        {
                            ViewBag.Mensagem = "Olá";
                            return View("Index", c);
                        }
                        if (c.Senha != cli.Senha)
                        {
                            ViewBag.Mensagem = "Usuário ou senha inválidos";
                            return View("Index", null);
                        }

                    }

                    return View("Create");
                }*/

      
        public IActionResult Delete(int id, Cliente cliente)
        {
            cliente.IdCliente = id;

            using (var data = new ClienteData())
                data.Delete(id);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (var data = new ClienteData())
                return View(data.Read(id));
        }

        [HttpPost]
        public IActionResult Update(int id, Cliente cliente)
        {
            cliente.IdCliente = id;

            using (var data = new ClienteData())
                data.Update(cliente);

            return RedirectToAction("Index");
            
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View(new ClienteViewModel());
        }

        [HttpPost]
        public IActionResult Login(ClienteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var data = new ClienteData())
            {
                var user = data.Read(model);

                if (user == null)
                {
                    ViewBag.Message = "Email e/ou senha incorretos!";
                    return View(model);
                }

                HttpContext.Session.SetString("user", JsonSerializer.Serialize<Cliente>(user));

                return RedirectToAction("Index", "Produto");
            }
        }

        



    }
}
