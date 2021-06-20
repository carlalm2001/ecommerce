using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Data
{
    public class ClienteData : Data
    {
        public void Create(Cliente cliente)
        {
            //permitir executar comando no SQL
            SqlCommand cmd = new SqlCommand();

            //conecta com banco
            cmd.Connection = connectionDB;

            //criando string SQL
            cmd.CommandText = @"Insert into Cliente Values (@Nome, @Email, @Senha)";

            //colocando os dados recebidos pelo objeto cliente, na string SQL
            cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@Email", cliente.Email);
            cmd.Parameters.AddWithValue("@Senha", cliente.Senha);
            
            //execução da string SQL no Banco de Dados
            cmd.ExecuteNonQuery();
        }

        //método read executa SELECT no Banco de Dados
        //faz uma consulta no BD e retorna uma lista
        //com vários regitros (no caso todos os registros da tablea)
        //caso a tabela esteja vazia retornará uma lista vazia

        public List<Cliente> Read()
        {
            List<Cliente> lista = new List<Cliente>();

            //criando cmd (comando) para exucetar query SQL
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.connectionDB;
            cmd.CommandText = "Select * From Cliente";

            //execução do Select no BD
            //o reader vai receber o retorno do Select
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())//percorrendo a table até o EOF (End of File)
            {
                Cliente cli = new Cliente();

                cli.IdCliente = (int)reader["IdCliente"];
                cli.Nome = (string)reader["Nome"];
                cli.Email = (string)reader["Email"];
                cli.Senha = (string)reader["Senha"];

                lista.Add(cli);
            }


            return lista;
        }

        public Cliente Read(int id)
        {
            Cliente cliente = null;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.connectionDB;
            cmd.CommandText = @"Select * From Cliente Where IdCliente = @id";

            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                cliente = new Cliente
                {
                    IdCliente = (int)reader["IdCliente"],
                    Nome = (string)reader["Nome"],
                    Email = (string)reader["Email"],
                    Senha = (string)reader["Senha"]
                };
            }

            return cliente;
        }


        public Cliente Read(string email)
        {
            Cliente cliente = null;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.connectionDB;//conexão com o Banco de Dados

            //string SQL para ser executada no Banco de Dados
            cmd.CommandText = @"Select * From Cliente where Email = @Email";

            //inserindo o valor do id recebido a string SQL
            cmd.Parameters.AddWithValue("@Email", email);

            //Executando o comando SQL no Banco de Dados
            SqlDataReader reader = cmd.ExecuteReader();

            //verificando se, após a consulta, retornou um registro
            if (reader.Read())
            {
                //instanciando o objeto cliente declarado anteriormente
                //e colocando os dados do registro da tabela no objeto
                cliente = new Cliente
                {
                    IdCliente = (int)reader["IdCliente"],
                    Nome = (string)reader["Nome"],
                    Email = (string)reader["Email"],
                    Senha = (string)reader["Senha"]
                };
            }
            //retornando o objeto cliente, que pode ser null ou com as informações
            //recebidas na consulta
            return cliente;
        }// consulta email
        public void Update(Cliente cliente)
        {
            //criado o cmd (comando SQl) para executar um comando SQL no Banco de Dados
            SqlCommand cmd = new SqlCommand();
            //conexão com o Banco de Dados
            cmd.Connection = base.connectionDB;
            //criada a String SQL
            cmd.CommandText = @"Update Cliente set Nome = @nome, Email = @email, Senha = @senha
                                Where IdCliente = @id";

            cmd.Parameters.AddWithValue("@id", cliente.IdCliente);
            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@email", cliente.Email);
            cmd.Parameters.AddWithValue("@senha", cliente.Senha);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            
                //criado o cmd (comando SQl) para executar um comando SQL no Banco de Dados
                SqlCommand cmd = new SqlCommand();
                //conexão com o Banco de Dados
                cmd.Connection = base.connectionDB;
                //criada a String SQL
                cmd.CommandText = @"Delete from Cliente Where IdCliente = @id";

                cmd.Parameters.AddWithValue("@id", id);

                //Execução do comando Delete no Banco de Dados
                cmd.ExecuteNonQuery();
           
        }
        public Cliente Read(ClienteViewModel model)
        {
            //declarando um objeto cliente e inicializando como null

            Cliente cliente = null;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.connectionDB;//conexão com o Banco de Dados

            //string SQL para ser executada no Banco de Dados
            cmd.CommandText = @"Select * from Cliente Where Email = @email and Senha = @senha";

            //inserindo o valor do id recebido a string SQL
            cmd.Parameters.AddWithValue("@email", model.Email);
            cmd.Parameters.AddWithValue("@senha", model.Senha);

            //Executando o comando SQL no Banco de Dados
            SqlDataReader reader = cmd.ExecuteReader();

            //verificando se, após a consulta, retornou um registro
            if (reader.Read())
            {
                //instanciando o objeto cliente declarado anteriormente
                //e colocando os dados do registro da tabela no objeto

                cliente = new Cliente
                {
                    IdCliente = (int)reader["IdCliente"],
                    Nome = (string)reader["Nome"],
                    Email = (string)reader["Email"]
                };
            }

            //retornando o objeto cliente, que pode ser null ou com as informações
            //recebidas na consulta
            return cliente;
        }//consulta id

    }
}
