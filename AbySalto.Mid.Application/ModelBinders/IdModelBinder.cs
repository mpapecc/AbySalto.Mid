using AbySalto.Mid.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AbySalto.Mid.Application.ModelBinders
{
    public class IdModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(nameof(Id));

            if (valueProviderResult.Length == 0)
                valueProviderResult = bindingContext.ValueProvider.GetValue(nameof(Id.EncryptedId));

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            string encryptedId = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(encryptedId))
            {
                return Task.CompletedTask;
            }

            try
            {
                var result = new Id(Id.Decrypt(encryptedId));
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            catch
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid Id.");
            }

            return Task.CompletedTask;
        }
    }

    public class IdAttribute : ModelBinderAttribute
    {
        public IdAttribute() : base(typeof(IdModelBinder))
        {
            BindingSource = BindingSource.Query;
            Name = "id";
        }
    }

}
