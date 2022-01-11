using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions;

public static class ModelStateExtension
{
    // Com a classe e métodos estáticos, ao utilizar o this no parâmetro é possível adicionar esse método a uma outra classe
    // a qual o tipo foi declarado após o this.
    public static List<string> GetErrors(this ModelStateDictionary modelState)
    {
        var result = new List<string>();
        foreach (var item in modelState.Values)
            result.AddRange(item.Errors.Select(error => error.ErrorMessage)); // Método do Linq para adicionar a um array um range de valores.
        return result;
    }
}
