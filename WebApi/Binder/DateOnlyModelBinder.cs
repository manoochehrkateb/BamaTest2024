using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace WebApi.Binder
{
    public class DateOnlyModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var dateString = valueProviderResult.FirstValue;
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                bindingContext.Result = ModelBindingResult.Success(DateOnly.FromDateTime(date));
            }
            else
            {
                bindingContext.ModelState.AddModelError(bindingContext.FieldName, "Invalid date format.");
            }

            return Task.CompletedTask;
        }
    }
}
