using ConsoleGrid;
using Microsoft.Extensions.Configuration;
using v2.UI;

namespace ControleFinanceiroBasico
{
    public delegate void CallBack();

    public class Program
    {
        public static IConfiguration configuration;
        private static DespesasUI _despesasUI;
        private static ReceitasUI _receitasUI;
        private static SaldoUI _saldoUI;

        public static void Main(string[] args)
        {
            LerConfiguracoes();
            IniciarDependencias();
            ImprimirMenu();
        }

        private static void IniciarDependencias()
        {
            _despesasUI = new DespesasUI(ImprimirMenu, configuration);
            _receitasUI = new ReceitasUI(ImprimirMenu, configuration);
            _saldoUI = new SaldoUI(configuration);
        }

        private static void LerConfiguracoes()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }

        private static void EncerrarPrograma()
        {
            Console.WriteLine("Deseja realmente sair do programa? [s/n]");
            var opcao = Console.ReadKey().KeyChar;

            if (opcao == 's')
            {
                Console.WriteLine("\nEncerrando o programa em: ");
                for (var i = 3; i > 0; i--)
                {
                    Console.WriteLine($"\n{i}");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Já Era!!!");
                Environment.Exit(0);
            }
            else
            {
                ImprimirMenu();
            }
        }

        private static void ImprimirMenu()
        {
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("Meu Controle Financeiro");
            Console.WriteLine("==============================");
            Console.WriteLine("1. Cadastrar despesa");
            Console.WriteLine("2. Cadastrar receita");
            Console.WriteLine("3. Visualizar despesas");
            Console.WriteLine("4. Visualizar receitas");
            Console.WriteLine("5. Visualizar saldo");
            Console.WriteLine("6. Sair");

            var valorInformado = Console.ReadLine();
            if (int.TryParse(valorInformado, out var opcaoSelecionada))
            {
                switch (opcaoSelecionada)
                {
                    case 1:
                        _despesasUI.Cadastrar(AguardarChamadaDoMenu);
                        break;
                    case 2:
                        _receitasUI.Cadastrar(AguardarChamadaDoMenu);
                        break;
                    case 3:
                        _despesasUI.Visualizar();
                        break;
                    case 4:
                        _receitasUI.Visualizar();
                        break;
                    case 5:
                        _saldoUI.VisualizarSaldo(AguardarChamadaDoMenu);
                        break;
                    case 6:
                        EncerrarPrograma();
                        break;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Opção inválida");
            }
        }

        private static void AguardarChamadaDoMenu()
        {
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
            ImprimirMenu();
        }
    }
}