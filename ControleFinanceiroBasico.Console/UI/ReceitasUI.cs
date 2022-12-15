using ConsoleGrid;
using ControleFinanceiroBasico;
using ControleFinanceiroBasico.Core.Dao;
using ControleFinanceiroBasico.Core.Models;
using Microsoft.Extensions.Configuration;


namespace v2.UI
{
    public class ReceitasUI
    {
        private readonly CallBack _imprimirMenuPrincipal;
        private readonly ReceitasDao _receitasDao;

        public ReceitasUI(CallBack imprimirMenuPrincipal, IConfiguration config)
        {
            _imprimirMenuPrincipal = imprimirMenuPrincipal;
            _receitasDao = new ReceitasDao(config.GetConnectionString("DefaultConnection"));
        }

        public void ImprimirMenu()
        {
            Console.WriteLine("===============================");
            Console.WriteLine("Receitas - O que deseja fazer?");
            Console.WriteLine("===============================");
            Console.WriteLine("1- Pesquisar");
            Console.WriteLine("2- Editar");
            Console.WriteLine("3- Excluir");
            Console.WriteLine("4- Menu principal");

            var valorInformado = Console.ReadLine();
            if (int.TryParse(valorInformado, out var opcaoselecionada))
            {
                switch (opcaoselecionada)
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
            var receitas = _receitasDao.ListarReceitas();

            var grid = new Grid("Lista de Receitas", 2, 12, 20, 12);
            grid.ColumnsName("Id", "Data", "Descricao", "Valor");
            grid.ColumnsFormat(string.Empty, "{0:dd/MM/yyyy}", string.Empty, "{0:c2}");
            grid.ColumnsAlign(Align.RIGHT, Align.RIGHT, Align.LEFT, Align.RIGHT);
            grid.DataSource(receitas);
            grid.Footer("Total: " + receitas.Sum(d => d.Valor).ToString("c2"), Align.RIGHT);
            grid.Print();

            ImprimirMenu();
        }

        public void Pesquisar()
        {
            Console.Clear();
            Console.WriteLine("Informe o texto que deseja pesquisar: ");
            var texto = Console.ReadLine();

            var receitas = _receitasDao.Pesquisar(texto);

            if (receitas.Count > 0)
            {
                var grid = new Grid("Lista de Receitas", 2, 12, 20, 12);
                grid.ColumnsName("Id", "Data", "Descricao", "Valor");
                grid.ColumnsFormat(string.Empty, "{0:dd/MM/yyyy}", string.Empty, "{0:c2}");
                grid.ColumnsAlign(Align.RIGHT, Align.RIGHT, Align.LEFT, Align.RIGHT);
                grid.DataSource(receitas);
                grid.Footer("Total: " + receitas.Sum(r => r.Valor).ToString("c2"), Align.RIGHT);
                grid.Print();

                ImprimirMenu();
            }
            else
            {
                Console.WriteLine("Nenhuma receita foi encontrada.");
                Thread.Sleep(3000);
                Visualizar();
            }
        }

        public void Cadastrar(CallBack callBack)
        {
            Console.Clear();
            Console.WriteLine("Informe o valor da receita");
            var valor = Double.Parse(Console.ReadLine());

            Console.WriteLine("Informe a descrição da receita");
            var descricao = Console.ReadLine();

            Console.WriteLine("Informe a data da receita: [Enter para data de hoje]");
            var teclaPressionada = Console.ReadKey().Key;

            var data = DateTime.Now;
            if (teclaPressionada != ConsoleKey.Enter)
            {
                data = DateTime.Parse(Console.ReadLine());
            }
            var receita = new Receita(1, valor, descricao, data); //TODO: remover id
            _receitasDao.Incluir(receita);

            Console.WriteLine("receita cadastrada com sucesso.");
            callBack.Invoke();
        }

        public void Editar()
        {
            Console.WriteLine("Informe o Id da receita: ");
            var id = int.Parse(Console.ReadLine());
            var receita = _receitasDao.ObterReceitaPorId(id);

            if (receita == null)
            {
                var segundos = 3;
                Console.WriteLine($"Nenhuma receita encontrada.");
                Thread.Sleep(TimeSpan.FromSeconds(segundos));
                Visualizar();
            }

            Console.Write("\nDeseja alterar o campo \"data\"? [s/n]");
            var teclaPressionada = Console.ReadKey().KeyChar;
            if (teclaPressionada == 's')
            {
                Console.WriteLine("\nInforme a nova data: ");
                receita.Data = DateTime.Parse(Console.ReadLine());
            }
            Console.Write("\nDeseja alterar o campo \"descrição\"? [s/n]");
            teclaPressionada = Console.ReadKey().KeyChar;
            if (teclaPressionada == 's')
            {
                Console.WriteLine("\nInforme a nova descrição: ");
                receita.Descricao = Console.ReadLine();
            }

            Console.Write("\nDeseja alterar o campo \"Valor\"?[s/n]");
            teclaPressionada = Console.ReadKey().KeyChar;
            if (teclaPressionada == 's')
            {
                Console.WriteLine("\n Informe o novo valor: ");
                receita.Valor = double.Parse(Console.ReadLine());

            }

            _receitasDao.Atualizar(receita);

            Console.WriteLine("\n***Receita atualizada com sucesso***");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Visualizar();
        }
        public void Excluir()
        {
            Console.WriteLine("Informe o Id da receita:");
            var id = int.Parse(Console.ReadLine());
            var receita = _receitasDao.ObterReceitaPorId(id);


            if (receita == null)
            {
                var segundos = 3;
                Console.WriteLine($"Nenhuma receita encontrada.");
                Thread.Sleep(TimeSpan.FromSeconds(segundos));
                Visualizar();
            }
            Console.WriteLine($"Deseja realmente excluir a receita \"{receita.Descricao}\"?[s/n]");
            var teclaPressionada = Console.ReadKey().KeyChar;
            if (teclaPressionada == 's')
            {
                _receitasDao.Excluir(receita);
                Console.WriteLine("**Receita excluida com sucesso**");
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