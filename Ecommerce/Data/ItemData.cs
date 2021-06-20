using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Data
{
    public class ItemData : Data
    {
        public List<Item> Read(int id)
        {
            List<Item> lista = new List<Item>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = base.connectionDB;
                cmd.CommandText = @"Select * from v_produto inner join ItemPedido on v_produto.IdProduto = ItemPedido.IdProduto inner join Pedido on ItemPedido.IdPedido = Pedido.IdPedido where Pedido.IdCliente = @id";

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) //Percorre a tabela até o EOF
                {
                    Item item = new Item();
                    item.IdPedido = (int)reader["IdPedido"];

                    item.Produto = new Produto
                    {
                        IdProduto = (int)reader["IdProduto"],
                        Nome = (string)reader["Nome"]
                    };

                    item.Quantidade = (int)reader["Quantidade"];
                    item.Valor = (decimal)reader["Preco"];

                    lista.Add(item);
                }
            }
            catch(SqlException ex) { 
            
            }

            return lista;
        }
    }
}