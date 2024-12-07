using FluentValidation;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Models.Validations
{
    /// <summary>
    /// 备忘录验证器
    /// </summary>
    public class MemoValidator : AbstractValidator<MemoDto>
    {
        public MemoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("标题不能为空")
                .MaximumLength(50).WithMessage("标题长度不能超过50个字符");

            RuleFor(x => x.Content)
                .MaximumLength(500).WithMessage("内容长度不能超过500个字符");
        }
    }
}
