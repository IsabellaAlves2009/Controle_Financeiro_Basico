namespace ControleFinanceiroBasico.Core.Models
{
    public class Receita
    {   
        public Receita()
        {
            
        }
        public Receita(double valor , string descricao , DateTime data)
        {
            Data = data;
            Descricao = descricao;
            Valor = valor;
        }
        public Receita(int id, double valor , string descricao , DateTime data)
        {   
            Id = id;
            Valor = valor;
            Descricao = descricao;
            Data = data;
        }
      
        public int Id { get; set; }
        public DateTime Data{get; set;}
        public string Descricao{get; set;}
        public double Valor{get; set;}
    }
}