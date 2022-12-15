using ControleFinanceiroBasico.Core.Dao;
using ControleFinanceiroBasico.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiroBasico.Web.Controllers
{
    [Authorize]
    public class DespesasController : Controller
    {
        private readonly DespesasDao _despesasDao;

        public DespesasController(IConfiguration config)
        {
            _despesasDao =  new DespesasDao(config.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public IActionResult Index([FromQuery]string textoPesquisa = "")
        {
            if(!string.IsNullOrEmpty(textoPesquisa))
            {
                TempData["textoPesquisa"] = textoPesquisa;
                var despesas = _despesasDao.Pesquisar(textoPesquisa);
                return View(despesas);
            }
            else
            {
                var despesas = _despesasDao.ListarDespesas();
                return View(despesas);
            }
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();   
        }

        [HttpPost]
        public IActionResult Cadastrar(Despesa despesa)
        {
            _despesasDao.Incluir(despesa);
            return RedirectToAction("Index");   
        }
        
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var despesa = _despesasDao.ObterDespesaPorId(id);
            return View(despesa);
        }

        [HttpPost]
        public IActionResult Editar(Despesa despesa)
        {
            _despesasDao.Atualizar(despesa);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var despesa = _despesasDao.ObterDespesaPorId(id);
            return View(despesa);
        }

        [HttpPost]
        public IActionResult Excluir(Despesa despesa)
        {
            _despesasDao.Excluir(despesa);
            return RedirectToAction("Index");
        }
    }
}