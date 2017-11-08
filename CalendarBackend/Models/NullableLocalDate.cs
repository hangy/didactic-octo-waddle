namespace CalendarBackend.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using NodaTime;
    using NodaTime.Text;
    using System;
    using System.Threading.Tasks;

    [ModelBinder(BinderType = typeof(NullableLocalDateBinder))]
    public class NullableLocalDate
    {
        public NullableLocalDate(LocalDate? date)
        {
            this.Date = date;
        }

        public LocalDate? Date { get; }

        public class NullableLocalDateBinder : IModelBinder
        {
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                if (bindingContext == null)
                {
                    throw new ArgumentNullException(nameof(bindingContext));
                }

                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (valueProviderResult == ValueProviderResult.None)
                {
                    return Task.CompletedTask;
                }

                var value = valueProviderResult.FirstValue;
                if (string.IsNullOrWhiteSpace(value))
                {
                    bindingContext.Result = ModelBindingResult.Success(new NullableLocalDate(null));
                    return Task.CompletedTask;
                }

                var parseResult = LocalDatePattern.Iso.Parse(value);
                var localDate = new NullableLocalDate(parseResult.Value);
                bindingContext.Result = ModelBindingResult.Success(localDate);
                return Task.CompletedTask;
            }
        }
    }
}
