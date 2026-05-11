using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KooperatifYonetim.Web.Infrastructure;

public class InvariantDoubleModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;
        if (string.IsNullOrWhiteSpace(value))
        {
            bindingContext.Result = ModelBindingResult.Success(0d);
            return Task.CompletedTask;
        }

        // Accept both '.' and ',' as decimal separator
        var normalized = value.Trim().Replace(',', '.');
        if (double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName,
                $"'{value}' geçerli bir sayı değil.");
        }

        return Task.CompletedTask;
    }
}
