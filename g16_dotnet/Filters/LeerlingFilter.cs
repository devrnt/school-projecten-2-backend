using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc.Filters;

namespace g16_dotnet.Filters
{
    [System.AttributeUsageAttribute(System.AttributeTargets.All, AllowMultiple = false)]
    public class LeerlingFilter : ActionFilterAttribute
    {
        private Leerling _leerling;
        private readonly ILeerlingRepository _leerlingRepository;

        public LeerlingFilter(ILeerlingRepository leerlingRepository)
        {
            _leerlingRepository = leerlingRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _leerling = _leerlingRepository.GetByEmail(context.HttpContext.User.Identity.Name);
            context.ActionArguments["leerling"] = _leerling;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
