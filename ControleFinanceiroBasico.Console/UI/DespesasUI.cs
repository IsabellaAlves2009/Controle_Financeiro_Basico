using ConsoleGrid;
using ControleFinanceiroBasico;
using ControleFinanceiroBasico.Core.Dao;
using ControleFinanceiroBasico.Core.Models;
using Microsoft.Extensions.Configuration;

namespace v2.UI
{
    public class DespesasUI
    {
        //Dao -> Data Access Object
        //Dal -> Data Access Layer
        //UI -> User Interface

        private readonly DespesasDao _despesasDao;
        private readonly CallBack _imprimirMenuPrincipal;
        public DespesasUI(CallBack imprimirMenuPrincipal, IConfiguration config)
        {
            _despesasDao = new DespesasDao(config.GetConnectionString("DefaultConnection"));
            _imprimirMenuPrincipal = imprimirMenuPrincipal;
        }

        public void ImprimirMenu()
        {
            Console.WriteLine("===============================");
            Console.WriteLine("Despesas - O que deseja fazer?");
            Console.WriteLine("===============================");
            Console.WriteLine("1. Pesquisar");
            Console.WriteLine("2. Editar");
            Console.WriteLine("3. Excluir");
            Console.WriteLine("4. Menu principal");

            var valorInformado = Console.ReadLine();
            if (int.TryParse(valorInformado, out var opcaoSelecionada))
            {
                switch (opcaoSelecionada)
                {
                    case 1:
                        Pesquisar();
                        break;
                    case 2:
                        Editar();
                        break;
                    case 3:
                        Excluir();
                        break;
                    case 4:
                        _imprimirMenuPrincipal.Invoke();
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

        public void Visualizar()
        {
            Console.Clear();
            var despesas = _despesasDao.ListarDespesas();

            var grid = new Grid("Lista de Despesas", 2, 12, 20, 12);
            grid.ColumnsName("Id", "Data", "Descricao", "Valor");
            grid.ColumnsFormat(string.Empty, "{0:dd/MM/yyyy}", string.Empty, "{0:c2}");
            grid.ColumnsAlign(Align.RIGHT, Align.RIGHT, Align.LEFT, Align.RIGHT);
            grid.DataSource(despesas);
            grid.Footer("Total: " + despesas.Sum(d => d.Valor).ToString("c2"), Align.RIGHT);
            grid.Print();

            ImprimirMenu();
        }

        public void Pesquisar()
        {
            Console.Clear();
            Console.WriteLine("Informe o texto que deseja pesquisar: ");
            var texto = Console.ReadLine();

            var despesas = _despesasDao.Pesquisar(texto);

            if (despesas.Count > 0)
            {
                var grid = new Grid("Lista de Despesas", 2, 12, 20, 12);
                grid.ColumnsName("Id", "Data", "Descricao", "Valor");
                grid.ColumnsFormat(string.Empty, "{0:dd/MM/yyyy}", string.Empty, "{0:c2}");
                grid.ColumnsAlign(Align.RIGHT, Align.RIGHT, Align.LEFT, Align.RIGHT);
                grid.DataSource(despesas);
                grid.Footer("Total: " + despesas.Sum(d => d.Valor).ToString("c2"), Align.RIGHT);
                grid.Print();

                ImprimirMenu();
            }
            else
            {
                Console.WriteLine("Nenhuma despesa foi encontrada para a sua pesquisa.");
                Thread.Sleep(3000);
                Visualizar();
            }
        }

        public void Cadastrar(CallBack callBack)
        {
            Console.Clear();
            Console.WriteLine("Informe o valor da despesa: ");
            var valor = Double.Parse(Console.ReadLine());

            Console.WriteLine("Informe a descrição da despesa: ");
            var descricao = Console.ReadLine();

            Console.WriteLine("Informe a data da despesa: [Enter para data de hoje]");
            var teclaPressionada = Console.ReadKey().Key;

            var data = DateTime.Now;
            if (teclaPressionada != ConsoleKey.Enter)
            {
                data = DateTime.Parse(Console.ReadLine());
            }

            var despesa = new Despesa(valor, descricao, data);
            _despesasDao.Incluir(despesa);

            Console.WriteLine("Despesa cadastrada com sucesso.");
            callBack.Invoke();
        }

        public void Editar()
        {
            Console.WriteLine("Informe o Id da despesa: ");
            var id = int.Parse(Console.ReadLine());
            var despesa = _despesasDao.ObterDespesaPorId(id);

            if (despesa == null)
            {
                var segundos = 3;
                Console.WriteLine($"Nenhuma despesa encontrada. \nTente novamente em {segundos} segundos.");
                Thread.Sleep(TimeSpan.FromSeconds(segundos));
                Visualizar();
            }

            Console.Write("\nDeseja alterar o campo \"data\"? [s/n]: ");
            var teclaPressionada = Console.ReadKey().KeyChar;
            if (teclaPressionada == 's')
            {
                Console.WriteLine("\nInforme a nova data: ");
                despesa.Data = DateTime.Parse(Console.ReadLine());
            }

            Console.Write("\nDeseja alterar o campo \"descricao\"? [s/n]: ");
            teclaPressionada = Console.ReadKey().KeyChar;
            if (teclaPressionada == 's')
            {
                Console.WriteLine("\nInforme a nova descricao:");
                despesa.Descricao = Console.ReadLine();
            }

            Console.Write("\nDeseja alterar o campo \"valor\"? [s/n]: ");
            teclaPressionada = Console.ReadKey().KeyChar;
            if (teclaPressionada == 's')
            {
                Console.WriteLine("\nInforme o novo valor");
                despesa.Valor = Double.Parse(Console.ReadLine());
            }

            _despesasDao.Atualizar(despesa);

            Console.WriteLine("\n***Despesa atualizada com sucesso***");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Visualizar();
        }
        public void Excluir()
        {
            Console.WriteLine("Informe o id da despesa: ");
            var id = int.Parse(Console.ReadLine());
            var despesa = _despesasDao.ObterDespesaPorId(id);

            if (despesa == null)
            {
                var segundos = 3;
                Console.WriteLine($"Nenhuma despesa encontrada.");
                Thread.Sleep(TimeSpan.FromSeconds(segundos));
                Visualizar();
            }
            Console.WriteLine($"Deseja realmente excluir a despesa \"{despesa.Descricao}\"?[s/n]");
            var teclaPressionada = Console.ReadKey().KeyChar;

            if (teclaPressionada == 's')
            {
                _despesasDao.Excluir(despesa);

                Console.WriteLine("***Despesa excluida com sucesso***");
                Thread.Sleep(3000);
                Visualizar();
            }
            else
            {
                Visualizar();
            }
        }
    }
}