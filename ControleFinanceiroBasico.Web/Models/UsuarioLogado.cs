namespace ControleFinanceiroBasico.Web.Models
{
    public class UsuarioLogado
    {
        private readonly IHttpContextAccessor _acessor;

        public UsuarioLogado(IHttpContextAccessor acessor)
        {
            _acessor = acessor;
        }

        public string Nome => _acessor.HttpContext.User.Identity.Name;

        public bool EstaAutenticado()
        {
            return _acessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}