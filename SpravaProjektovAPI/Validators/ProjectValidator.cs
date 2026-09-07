using FluentValidation;
using SpravaProjektovAPI.Application.Projects;

namespace SpravaProjektovAPI.Validators
{
    public class ProjectValidator : AbstractValidator<ProjectRequest>
    {
        public ProjectValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Project name is required.")
                .MaximumLength(100).WithMessage("Project name must not exceed 100 characters.");

            RuleFor(x => x.Abbreviation)
                .MaximumLength(10).WithMessage("Project abbreviation must not exceed 10 characters.");

            RuleFor(x => x.Customer)
                .MaximumLength(100).WithMessage("Customer name must not exceed 100 characters.");
        }
    }
}
