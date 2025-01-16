using FluentValidation;


namespace Application.Handlers.Commands.Products.CreateProduct
{
    public class CreateCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateCommandValidator()
        {
            {
                RuleFor(x => x.TitleAr).NotEmpty().WithMessage("Arabic title is required.");
                RuleFor(x => x.TitleEn).NotEmpty().WithMessage("English title is required.");
                RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required.");
            }
        }
    }
}