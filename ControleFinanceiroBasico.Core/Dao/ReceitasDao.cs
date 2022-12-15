using System.Data;
using ControleFinanceiroBasico.Core.Models;
using MySqlConnector;

namespace ControleFinanceiroBasico.Core.Dao
{

    public class ReceitasDao
    {
        public readonly string _connectionString;

        public ReceitasDao(string strConn)
        {
            _connectionString = strConn;
        }

        public List<Receita> ListarReceitas()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM Receitas ORDER BY Data ASC";
            var cmd = new MySqlCommand(sql, conn);
            var reader = cmd.ExecuteReader();

            var receitas = new List<Receita>();

            while (reader.Read())
            {
                var id = int.Parse(reader[0].ToString());
                var descricao = reader[1].ToString();
                var valor = double.Parse(reader[2].ToString());
                var data = DateTime.Parse(reader[3].ToString());

                var receita = new Receita(id, valor, descricao, data);
                receitas.Add(receita);
            }

            reader.Close();
            cmd.Dispose();
            conn.Dispose();
            return receitas;

        }
        public List<Receita> Pesquisar(string texto)
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM Receitas WHERE Descricao LIKE @texto ORDER BY Data ASC";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@texto",$"%{texto}%");
            var reader = cmd.ExecuteReader();

            var receitas = new List<Receita>();
            while(reader.Read())
            {
                var id = int.Parse(reader[0].ToString());
                var descricao = reader[1].ToString();
                var valor = Double.Parse(reader[2].ToString());
                var data = DateTime.Parse(reader[3].ToString());

                var receita = new Receita(id, valor, descricao, data);
                receitas.Add(receita);
            }

            reader.Close();
            cmd.Dispose();
            conn.Dispose();
            conn.Close();

            return receitas;
        }

        public Receita ObterReceitaPorId(int id)
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM Receitas WHERE Id=@id";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id",id);
            var reader = cmd.ExecuteReader();

            if(!reader.Read())
            {
                return null;
            }

            var descricao = reader[1].ToString();
            var valor = double.Parse(reader[2].ToString());
            var data = DateTime.Parse(reader[3].ToString());
            var receita = new Receita(id, valor,descricao, data);

            reader.Close();
            cmd.Dispose();
            conn.Dispose();
            conn.Close();
            return receita;
        }
        public void Incluir(Receita receita)
        {
                var conn = new MySqlConnection(_connectionString);
                conn.Open();

                var sql = "INSERT INTO Receitas (Data, Descricao, Valor) VALUES (@data, @descricao, @valor)";

                var cmd = new MySqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@data",receita.Data);
                cmd.Parameters.AddWithValue("@descricao", receita.Descricao);
                cmd.Parameters.AddWithValue("@valor", receita.Valor);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Dispose();
                conn.Close();

        }
        
        public void Atualizar(Receita receita)
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "UPDATE Receitas SET Data=@data, Descricao=@descricao, Valor=@valor WHERE Id=@id";

            var cmd = new MySqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@id", receita.Id);
            cmd.Parameters.AddWithValue("@data",receita.Data);
            cmd.Parameters.AddWithValue("@descricao",receita.Descricao);
            cmd.Parameters.AddWithValue("@valor",receita.Valor);

            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Dispose();
            conn.Close();
;        }
        public void Excluir(Receita receita)
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "DELETE FROM Receitas WHERE Id=@id";

            var cmd = new MySqlCommand(sql , conn);

            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@id",receita.Id);            
            
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Dispose();
            conn.Close();

        }

        public double Total()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT SUM(Valor) FROM Receitas";
            var cmd = new MySqlCommand(sql, conn);            
            var reader = cmd.ExecuteReader();

            double total = 0;
            if (!reader.Read())
            {
                return total;
            }

            total = Double.Parse(reader[0].ToString());
            
            reader.Close();
            cmd.Dispose();
            conn.Dispose();
            conn.Close();
            return total;
        }
    }
}