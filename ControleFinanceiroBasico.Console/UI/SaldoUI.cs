using ControleFinanceiroBasico;
using ControleFinanceiroBasico.Core.Dao;
using Microsoft.Extensions.Configuration;


namespace v2.UI
{
    public class SaldoUI
    {
        private readonly DespesasDao _despesasDao;
        private readonly ReceitasDao _receitasDao;

        public SaldoUI(IConfiguration config)
        {
            var strconn = config.GetConnectionString("DefaultConnection");
            _despesasDao = new DespesasDao(strconn);
            _receitasDao = new ReceitasDao(strconn);
        }

        public void VisualizarSaldo(CallBack callBack)
        {
            var totalDespesas = _despesasDao.Total();
            var totalReceitas = _receitasDao.Total();
            var saldo = totalReceitas - totalDespesas;

            Console.Clear();
            Console.WriteLine($"Extrato do dia {DateTime.Now.ToString("dd/MM/yyyy")}");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine($"Total de Receitas.......: {totalReceitas.ToString("c2")}");
            Console.WriteLine($"Total de Despesas.......: {totalDespesas.ToString("c2")}");
            Console.WriteLine("---------------------------------------------------------");

            if (saldo < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"Saldo...................: {saldo.ToString("c2")}");

            Console.ForegroundColor = ConsoleColor.White;
            callBack.Invoke();
        }
    }
}