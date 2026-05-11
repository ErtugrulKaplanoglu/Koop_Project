using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KooperatifYonetim.Web.Infrastructure;

public class InvariantDoubleModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(double) ||
            context.Metadata.ModelType == typeof(double?))
        {
            return new InvariantDoubleModelBinder();
        }
        return null;
    }
}
