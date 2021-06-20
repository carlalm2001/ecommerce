using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    public class CarrinhoController : Controller
    {
        public IActionResult Index()
        {
            List<Item> lista = new List<Item>();

            var carrinho = HttpContext.Session.GetString("Carrinho");

            if (carrinho != null)
            {
                //TODO Converter String para Lista(Json)
                lista = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(carrinho);
            }
            return View(lista);
        }

        [HttpGet]
        public IActionResult Finalizar()
        {
            List<Item> lista = new List<Item>();

            string carrinho = HttpContext.Session.GetString("Carrinho");

            var usuario = HttpContext.Session.GetString("user");

            var login = JsonSerializer.Deserialize<Cliente>(usuario);

            if (carrinho != null)
            {
                //TODO Converter String para Lista(Json)
                lista = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(carrinho);

                using (var data = new PedidoData())
                {
                    Pedido pedido = new Pedido();

                    pedido.IdCliente = Convert.ToInt32(login.IdCliente);

                    foreach (var item in lista)
                    {
                        pedido.Itens.Add(item);
                    }

                    data.Create(pedido);
                }

                HttpContext.Session.Remove("Carrinho");
            }

            return RedirectToAction("Index");
        }
       

        [HttpGet]
        public IActionResult Remover(int id)
        {
            List<Item> lista = new List<Item>();
            var carrinho = HttpContext.Session.GetString("Carrinho");
            if (carrinho != null)
            {
                //TODO Converter String para Lista(Json)
                lista = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(carrinho);
            }
            using (var data = new ProdutoData())
            {

                var item = lista.SingleOrDefault(i => i.Produto.IdProduto == id);

                lista.Remove(item);

                //TODO Converter Lista para String (Json)
                carrinho = System.Text.Json.JsonSerializer.Serialize<List<Item>>(lista);

                HttpContext.Session.SetString("Carrinho", carrinho);

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Comprar(int id)
        {
            List<Item> lista = new List<Item>();
            var carrinho = HttpContext.Session.GetString("Carrinho");
            if (carrinho != null)
            {
                //TODO Converter String para Lista(Json)
                lista = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(carrinho);
            }
            using (var data = new ProdutoData())
            {
                var item = lista.SingleOrDefault(i => i.Produto.IdProduto == id);
                if (item == null)
                {
                    Produto produto = data.Read(id);
                    item = new Item();
                    item.Produto = produto;
                    item.Quantidade = 1;
                    item.Valor = item.Produto.Valor;
                    lista.Add(item);
                }
                else
                {
                    item.Quantidade++;
                }
                //TODO Converter Lista para String (Json)
                carrinho = System.Text.Json.JsonSerializer.Serialize<List<Item>>(lista);
                HttpContext.Session.SetString("Carrinho", carrinho);
                return RedirectToAction("Index");
            }
        }

    }
}


