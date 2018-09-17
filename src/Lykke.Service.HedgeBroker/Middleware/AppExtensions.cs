using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Refit;

namespace Lykke.Service.HedgeBroker.Middleware
{
    public static class AppExtensions
    {
        public static void RegisterExceptionHandler(this IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var ex = errorFeature.Error;

                if (ex is ValidationApiException validationApiException)
                {
                    await CreateErrorResponse(context,
                        validationApiException.ErrorResponse,
                        validationApiException.StatusCode);
                }
                else if (ex is ClientApiException || ex is ApiException)
                {
                    var clientApiEx = ex as ClientApiException;
                    var apiEx = ex as ApiException;

                    var code = clientApiEx?.HttpStatusCode ?? apiEx.StatusCode;
                    var error = clientApiEx?.ErrorResponse ?? new ErrorResponse();

                    await CreateErrorResponse(context, error, code);
                }
                else
                {
                    await CreateErrorResponse(context, 
                        new ErrorResponse { ErrorMessage = "Technical problem" }, 
                        HttpStatusCode.InternalServerError);
                }
            });
        }

        private static async Task CreateErrorResponse(HttpContext context, ErrorResponse response, HttpStatusCode status)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var json = JsonConvert.SerializeObject(new ErrorResponse
            {
                ErrorMessage = response?.ErrorMessage ?? "Technical problem",
                ModelErrors = response?.ModelErrors ?? new Dictionary<string, List<string>> { }
            });

            await context.Response.WriteAsync(json);
        }
    }
}
