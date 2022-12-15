using ControleFinanceiroBasico.Core.Dao;
using ControleFinanceiroBasico.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiroBasico.Web.Controllers
{
    [Authorize]
    public class ReceitasController : Controller
    {
     
          private readonly ReceitasDao _receitasDao;

          public ReceitasController(IConfiguration config)
          {
              _receitasDao = new ReceitasDao(config.GetConnectionString("DefaultConnection"));
          }
          [HttpGet]
          public IActionResult Index([FromQuery] string textoPesquisa= "")
          { 
              if(!string.IsNullOrEmpty(textoPesquisa))
              {
                  TempData["textoPesquisa"] = textoPesquisa;
                  var receitas = _receitasDao.Pesquisar(textoPesquisa);
                  return View(receitas);
              }
              else
              {
                var receitas = _receitasDao.ListarReceitas();
                return View(receitas);
              }
          }

          [HttpGet]
          public IActionResult Cadastrar()
          {
              return View();
          }
          
          [HttpPost]
          public IActionResult Cadastrar(Receita receita)
          {
              _receitasDao.Incluir(receita);
              return RedirectToAction("Index");
          }

          [HttpGet]
          public IActionResult Editar(int id)
          {
              var receita = _receitasDao.ObterReceitaPorId(id);
              return View(receita);
          }

          [HttpPost]
          public IActionResult Editar(Receita receita)
          {
              _receitasDao.Atualizar(receita);
              return RedirectToAction("Index");
          }
          [HttpGet]
          public IActionResult Excluir(int id)
          {
            var receita = _receitasDao.ObterReceitaPorId(id);
            return View(receita);
          }
          
          [HttpPost]
          public IActionResult Excluir(Receita receita)
          {
                _receitasDao.Excluir(receita);
                return RedirectToAction("Index");
          }
    }
}