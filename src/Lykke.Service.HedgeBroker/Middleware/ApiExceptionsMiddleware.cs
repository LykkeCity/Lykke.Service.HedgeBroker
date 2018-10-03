using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Common.ExchangeAdapter.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Refit;

namespace Lykke.Service.HedgeBroker.Middleware
{
    public static class ApiExceptionsMiddleware
    {
        public static void UseApiExceptionsMiddleware(this IApplicationBuilder app)
        {
            app.Use(SetStatusOnError);
        }

        private static async Task SetStatusOnError(HttpContext context, Func<Task> next)
        {
            try
            {
                // First handle business exceptions
                await HandleBusinessExceptionsMiddleware.SetStatusOnBusinessError(context, next);
            }
            catch (ValidationApiException ex)
            {
                CreateErrorResponse(context, ex.ErrorResponse, ex.StatusCode);
            }
            catch (ClientApiException ex)
            {
                CreateErrorResponse(context, ex.ErrorResponse, ex.HttpStatusCode);
            }
            catch (ApiException ex)
            {
                CreateErrorResponse(context, ex.HasContent ? ex.Content : ex.Message, ex.StatusCode);
            }
            catch (Exception)
            {
                CreateErrorResponse(context,
                    new ErrorResponse
                    {
                        ErrorMessage = "Technical problem"
                    },
                    HttpStatusCode.InternalServerError);
            }
        }

        private static void CreateErrorResponse(HttpContext context, ErrorResponse response, HttpStatusCode status)
        {
            var json = JsonConvert.SerializeObject(response);

            using (var body = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)status;
                body.CopyTo(context.Response.Body);
            }
        }

        private static void CreateErrorResponse(HttpContext context, string error, HttpStatusCode status)
        {
            using (var body = new MemoryStream(Encoding.UTF8.GetBytes(error)))
            {
                context.Response.Clear();
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int) status;
                body.CopyTo(context.Response.Body);
            }
        }
    }
}
