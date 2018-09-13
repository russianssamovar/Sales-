using System.ComponentModel.DataAnnotations;

namespace Sales.ViewModels
{
    public class LoginModel
    {
            [Required(ErrorMessage = "Укажите промокод")]
            public string Code { get; set; }
    }
}
