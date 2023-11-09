using Droits.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Droits.ModelBinders;

public class DateTimeModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if ( context == null )
        {
            throw new ArgumentNullException(nameof(context));
        }

        if ( context.Metadata.ModelType == typeof(DateTime) ||
             context.Metadata.ModelType == typeof(DateTime?) )
        {
            return new DateTimeModelBinder();
        }

        return null;
    }
}

public class DateTimeModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if ( valueProviderResult == ValueProviderResult.None )
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if ( bindingContext.ModelType == typeof(DateTime?) && !value.HasValue() )
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        if ( DateTime.TryParse(value, out var parsedValue) )
        {
            bindingContext.Result = ModelBindingResult.Success(parsedValue);
        }
        else
        {
            bindingContext.Result = ModelBindingResult.Failed();
            bindingContext.ModelState.AddModelError(modelName, "Invalid date format.");
        }

        return Task.CompletedTask;
    }
}