using FluentValidation;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Models.Validations
{
    /// <summary>
    /// 用户验证器
    /// </summary>
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("用户名不能为空")
                .Length(3, 20).WithMessage("用户名长度必须在3-20个字符之间");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("密码不能为空")
                .MinimumLength(6).WithMessage("密码长度不能少于6个字符");
        }
    }
}
