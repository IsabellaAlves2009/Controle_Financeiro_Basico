using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ControleFinanceiroBasico.Web.Models;
using ControleFinanceiroBasico.Core.Dao;
using Microsoft.AspNetCore.Authorization;

namespace ControleFinanceiroBasico.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DespesasDao _despesasDao;
    private readonly ReceitasDao _receitasDao;

    public HomeController(ILogger<HomeController> logger, IConfiguration config)
    {
        _logger = logger;
        var strConn = config.GetConnectionString("DefaultConnection");
        _despesasDao = new DespesasDao(strConn);
        _receitasDao = new ReceitasDao(strConn);
    }

    public IActionResult Index()
    {
        var model = new SaldoModel();
        model.Despesas = _despesasDao.Total();
        model.Receitas = _receitasDao.Total();
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
