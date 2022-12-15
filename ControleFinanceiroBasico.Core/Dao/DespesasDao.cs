using ControleFinanceiroBasico.Core.Models;
using MySqlConnector;
using System.Data;

namespace ControleFinanceiroBasico.Core.Dao
{
    public class DespesasDao
    {
        public readonly string _connectionString;

        public DespesasDao(string strConn)
        {
            _connectionString = strConn;
        }

        public List<Despesa> ListarDespesas()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM Despesas ORDER BY Data ASC";
            var cmd = new MySqlCommand(sql, conn);
            var reader = cmd.ExecuteReader();

            var despesas = new List<Despesa>();
            while (reader.Read())
            {
                var id = int.Parse(reader[0].ToString());
                var descricao = reader[1].ToString();
                var valor = Double.Parse(reader[2].ToString());
                var data = DateTime.Parse(reader[3].ToString());

                var despesa = new Despesa(id, valor, descricao, data);
                despesas.Add(despesa);
            }

            reader.Close();
            cmd.Dispose();
            conn.Dispose();
            conn.Close();
            return despesas;
        }

        public List<Despesa> Pesquisar(string texto)
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM Despesas WHERE Descricao LIKE @texto  ORDER BY Data ASC";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@texto", $"%{texto}%");
            var reader = cmd.ExecuteReader();

            var despesas = new List<Despesa>();
            while (reader.Read())
            {
                var id = int.Parse(reader[0].ToString());
                var descricao = reader[1].ToString();
                var valor = Double.Parse(reader[2].ToString());
                var data = DateTime.Parse(reader[3].ToString());

                var despesa = new Despesa(id, valor, descricao, data);
                despesas.Add(despesa);
            }

            reader.Close();
            cmd.Dispose();
            conn.Dispose();
            conn.Close();

            return despesas;
        }

        public Despesa ObterDespesaPorId(int id)
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM Despesas WHERE Id=@id";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            var descricao = reader[1].ToString();
            var valor = Double.Parse(reader[2].ToString());
            var data = DateTime.Parse(reader[3].ToString());
            var despesa = new Despesa(id, valor, descricao, data);

            reader.Close();
            cmd.Dispose();
            conn.Dispose();
            conn.Close();
            return despesa;
        }

        public void Incluir(Despesa despesa)
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "INSERT INTO Despesas (Data, Descricao, Valor) VALUES (@data, @descricao, @valor)";

            var cmd = new MySqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@data", despesa.Data);
            cmd.Parameters.AddWithValue("@descricao", despesa.Descricao);
            cmd.Parameters.AddWithValue("@valor", despesa.Valor);

            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Dispose();
            conn.Close();
        }

        public void Atualizar(Despesa despesa)
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "UPDATE Despesas SET Data=@data, Descricao=@descricao, Valor=@valor WHERE Id=@id";

            var cmd = new MySqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@id", despesa.Id);
            cmd.Parameters.AddWithValue("@data", despesa.Data);
            cmd.Parameters.AddWithValue("@descricao", despesa.Descricao);
            cmd.Parameters.AddWithValue("@valor", despesa.Valor);

            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Dispose();
            conn.Close();
        }
        public void Excluir(Despesa despesa)
        {   
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "DELETE FROM Despesas WHERE Id=@id";

            var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", despesa.Id);            

            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Dispose();
            conn.Close();
        }

        public double Total()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT SUM(Valor) FROM Despesas";
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