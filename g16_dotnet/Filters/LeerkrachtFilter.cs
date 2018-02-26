using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc.Filters;

namespace g16_dotnet.Filters
{
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
            // Bij implementatie van users en accounts zal de ingelogde Leerkracht worden meegegeven
            _leerkracht = _leerkrachtRepository.GetById(1);
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
