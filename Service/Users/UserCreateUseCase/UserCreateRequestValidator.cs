using FluentValidation;
using Repository.Products;
using Repository.Users;
using Service.Products.ProductCreateUseCase;

namespace Service.Users.UserCreateUseCase
{
    public class UserCreateRequestValidator : AbstractValidator<UserCreateRequestDto>
    {
        public UserCreateRequestValidator(IUserRepositoryAsync userRepositoryAsync)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required")
                .Length(5, 25).WithMessage("{PropertyName} must be between 5 and 10 characters")
                .MustAsync(async (userName, cancellationToken) => await ExistUserName(userRepositoryAsync, userName)).WithMessage("User name already exists");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("{PropertyName} is not valid")
                .MustAsync(async (userEmail, cancellationToken) => await ExistUserEmail(userRepositoryAsync, userEmail)).WithMessage("User email already exists");

        }

        public async Task<bool> ExistUserName(IUserRepositoryAsync userRepositoryAsync, string name)
        {
            return await userRepositoryAsync.IsExistsName(name);
        }

        public async Task<bool> ExistUserEmail(IUserRepositoryAsync userRepositoryAsync, string email)
        {
            return await userRepositoryAsync.IsExistsEmail(email);
        }
    }
}