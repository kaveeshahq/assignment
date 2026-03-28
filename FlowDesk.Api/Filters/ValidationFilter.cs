using FluentValidation;
using FlowDesk.Api.Middleware;
using Microsoft.AspNetCore.Mvc.Filters;
using ValidationException = FlowDesk.Api.Middleware.ValidationException;

namespace FlowDesk.Api.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var errors = new Dictionary<string, string[]>();

        foreach (var argument in context.ActionArguments)
        {
            var value = argument.Value;
            if (value == null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(value.GetType());
            var validator = _serviceProvider.GetService(validatorType);

            if (validator == null)
            {
                await next();
                return;
            }

            var validationMethod = validatorType.GetMethod("ValidateAsync", new[] { value.GetType() });
            if (validationMethod == null)
            {
                await next();
                return;
            }

            var result = await (dynamic)validationMethod.Invoke(validator, new[] { value })!;

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    if (!errors.ContainsKey(failure.PropertyName))
                        errors[failure.PropertyName] = new string[] { };

                    errors[failure.PropertyName] = errors[failure.PropertyName]
                        .Append(failure.ErrorMessage)
                        .ToArray();
                }
            }
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }

        await next();
    }
}
