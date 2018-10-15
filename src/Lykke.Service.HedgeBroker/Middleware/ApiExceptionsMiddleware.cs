using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
                await next();
            }
            catch (ApiException ex)
            {
                CreateErrorResponse(context, ex.HasContent ? ex.Content : ex.Message, ex.StatusCode);
            }
        }

        private static void CreateErrorResponse(HttpContext context, string content, HttpStatusCode status)
        {
            using (var body = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                context.Response.Clear();
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int) status;
                body.CopyTo(context.Response.Body);
            }
        }
    }
}
