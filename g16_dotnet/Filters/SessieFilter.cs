﻿using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace g16_dotnet.Filters
{
    public class SessieFilter: ActionFilterAttribute {
        private Sessie _sessie;
        private readonly ISessieRepository _sessieRepository;

        public SessieFilter(ISessieRepository sessieRepository) {
            _sessieRepository = sessieRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context) {
            _sessie = _sessieRepository.GetById(System.Int32.Parse(context.HttpContext.Session.GetString("sessieCode")));
            context.ActionArguments["sessie"] = _sessie;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            _sessieRepository.SaveChanges();
            base.OnActionExecuted(context);
        }
    }
}
