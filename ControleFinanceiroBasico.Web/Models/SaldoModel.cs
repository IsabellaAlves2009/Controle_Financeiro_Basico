namespace ControleFinanceiroBasico.Web.Models;

public class SaldoModel
{
    public double Despesas { get; set; }
    public double Receitas { get; set; }
    public double Saldo => Receitas - Despesas;
}
