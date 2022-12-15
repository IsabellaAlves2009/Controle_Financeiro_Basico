using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiroBasico.Web.Models
{
    public class UsuarioLogin
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O e-mail informado é inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Senha { get; set; }
    }
}