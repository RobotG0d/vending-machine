using MvpMatch.Challenges.VendingMachine.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace MvpMatch.Challenges.VendingMachine.API.Filters
{
    public class VendingMachineExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            if (actionExecutedContext.Exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }

            var errorResponse = new ErrorApiResponse(actionExecutedContext.Exception);

            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

            var errorMessage = new HttpResponseMessage(statusCode);
            errorMessage.Content = new ObjectContent(typeof(ErrorApiResponse), errorResponse, jsonFormatter, "application/json");

            throw new HttpResponseException(errorMessage);
        }
    }
}