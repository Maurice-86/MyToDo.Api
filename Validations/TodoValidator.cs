using FluentValidation;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Models.Validations
{
    /// <summary>
    /// 待办事项验证器
    /// </summary>
    public class TodoValidator : AbstractValidator<TodoDto>
    {
        public TodoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("标题不能为空")
                .MaximumLength(50).WithMessage("标题长度不能超过50个字符");

            RuleFor(x => x.Content)
                .MaximumLength(500).WithMessage("内容长度不能超过500个字符");

            RuleFor(x => x.Status)
                .InclusiveBetween(0, 1).WithMessage("状态值只能是0或1");
        }
    }
}
