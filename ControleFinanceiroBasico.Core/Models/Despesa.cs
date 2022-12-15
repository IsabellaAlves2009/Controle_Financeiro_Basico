namespace ControleFinanceiroBasico.Core.Models
{
    public class Despesa
    {
        public Despesa()
        {
        }
        
        public Despesa(double valor, string descricao, DateTime data)
        {
            Data = data;
            Descricao = descricao;
            Valor = valor;
        }

        public Despesa(int id, double valor, string descricao, DateTime data)
        {
            Id = id;
            Valor = valor;
            Descricao = descricao;
            Data = data;
        }

        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
    }
}