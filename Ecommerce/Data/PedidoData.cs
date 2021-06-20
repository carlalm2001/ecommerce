using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Data
{
    public class PedidoData : Data
    {
        public void Create(Pedido pedido)
        {
            SqlTransaction transaction = this.connectionDB.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connectionDB;
                cmd.Transaction = transaction;
                cmd.CommandText = @"Insert into Pedido values (@IdCliente, @Data); Select @@IDENTITY";
                cmd.Parameters.AddWithValue("@IdCliente", pedido.IdCliente);
                cmd.Parameters.AddWithValue("@Data", DateTime.Now);

                //ExecuteScalar: executa a consula e retorna a primeira coluna da primeira linha
                // no conjunto de resultados retornado pela consulta
                //colunas ou linhas adicionais são ignorados

                int idPedido = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var item in pedido.Itens)
                {
                    SqlCommand cmdItem = new SqlCommand();
                    cmdItem.Connection = connectionDB;
                    cmdItem.Transaction = transaction;
                    cmdItem.CommandText = @"Insert into ItemPedido values" +
                                           "(@IdPedido, @IdProduto, @Quantidade, @Preco)";

                    cmdItem.Parameters.AddWithValue("@IdPedido", idPedido);
                    cmdItem.Parameters.AddWithValue("@IdProduto", item.Produto.IdProduto);
                    cmdItem.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                    cmdItem.Parameters.AddWithValue("@Preco", item.Valor);

                    cmdItem.ExecuteNonQuery();
                }

                //Executa as inserções da transação nas tabelas
                transaction.Commit();
            }
            catch (Exception ex)
            {
                //desfaz as operações de insert caso dê algum problema e elas não
                //possam ser executadas
                Console.WriteLine(ex.Message);
                transaction.Rollback();
            }
        }

        public List<Pedido> Read(int idCliente)
        {
            List<Pedido> lista = new List<Pedido>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connectionDB;
            cmd.CommandText = @"Select * from Pedido where IdCliente = @IdCliente";
            cmd.Parameters.AddWithValue("@IdCliente", idCliente);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Pedido p = new Pedido();
                p.IdPedido = (int)reader["IdPedido"];
                p.IdCliente = (int)reader["IdCliente"];
                p.Data = (DateTime)reader["Data"];

                lista.Add(p);
            }

            return lista;

        }
    }
}