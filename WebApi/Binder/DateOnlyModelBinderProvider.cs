using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApi.Binder
{
    public class DateOnlyModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(DateOnly))
            {
                return new BinderTypeModelBinder(typeof(DateOnlyModelBinder));
            }

            return null;
        }
    }
}
