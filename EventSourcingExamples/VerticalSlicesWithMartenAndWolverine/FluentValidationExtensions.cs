using FluentValidation;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine;

public static class FluentValidationExtensions

{
    public static IRuleBuilderOptions<T, TU> MustBeValueObject<T, TValueObject, TU>(this IRuleBuilder<T, TU> ruleBuilder, Func<TU, TValueObject> factory)
    {
        return (IRuleBuilderOptions<T, TU>)ruleBuilder.Custom((s, context) =>
        {
            try
            {
                factory(s);
            }
            catch (Exception e)
            {
                context.AddFailure(e.Message);
            }
        });
    }

    public static IRuleBuilderOptions<T, TU> MustBeType<T, TValueObject, TU>(this IRuleBuilder<T, TU> ruleBuilder, Func<TU, TypeResult<TValueObject>> factory)
    {
        return (IRuleBuilderOptions<T, TU>)ruleBuilder.Custom((s, context) => { factory(s).MatchFailure(error => context.AddFailure(error.Error.Message), () => { }); });
    }

    public static IRuleBuilderOptions<T, DateTimeOffset> BeInTheFuture<T>(this IRuleBuilder<T, DateTimeOffset> ruleBuilder)
    {
        return (IRuleBuilderOptions<T, DateTimeOffset>)ruleBuilder.Custom((s, context) =>
        {
            if (s <= DateTimeOffset.UtcNow) context.AddFailure("Must be in the future");
        });
    }
}