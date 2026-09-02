using EvoSchool.Domain.Exceptions;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace EvoSchool.Api.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;

            //404
            if (exception is NotFoundException notFound)
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.NotFound, new { mensagem = notFound.Message });
                return;
            }

            //409
            if (exception is BusinessRuleException business)
            {
                context.Response = context.Request.CreateResponse( HttpStatusCode.Conflict, new { mensagem = business.Message });
                return;
            }

            //400
            if (exception is ArgumentException || exception is ArgumentNullException)
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, new { mensagem = exception.Message });
                return;
            }

            base.OnException(context);
        }
    }
}