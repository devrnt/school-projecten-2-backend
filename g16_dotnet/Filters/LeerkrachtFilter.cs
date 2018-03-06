using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc.Filters;

namespace g16_dotnet.Filters
{
    [System.AttributeUsageAttribute(System.AttributeTargets.All, AllowMultiple = false)]
    public class LeerkrachtFilter : ActionFilterAttribute
    {
        private Leerkracht _leerkracht;
        private readonly ILeerkrachtRepository _leerkrachtRepository;

        public LeerkrachtFilter(ILeerkrachtRepository leerkrachtRepository)
        {
            _leerkrachtRepository = leerkrachtRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _leerkracht = _leerkrachtRepository.GetByEmail(context.HttpContext.User.Identity.Name);
            context.ActionArguments["leerkracht"] = _leerkracht;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _leerkrachtRepository.SaveChanges();
            base.OnActionExecuted(context);
        }
    }
}
