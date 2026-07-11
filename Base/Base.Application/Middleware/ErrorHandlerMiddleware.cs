using Base.Application.Contracts.DTOs.Common;
using Base.Application.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace Base.Application.Middleware
{
    class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = 500;
                var errorModel = Result.Failure(new Error(ex.GetHashCode().ToString(), ex.ToString()));
                errorModel.Message = ex.Message;

                switch (ex)
                {
                    case CustomValidationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case LoginException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case RequestCounselorException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case InvalidOperationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case DbUpdateConcurrencyException:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        errorModel.Message = "اطلاعات مورد نظر توسط کاربر دیگری تغییر کرده است. لطفاً صفحه را مجدداً بارگذاری کرده و دوباره تلاش کنید.";
                        break;
                    default:
                        if (ex.Message == null)
                            errorModel.Message = " خطایی پیش آمده!";
                        break;
                }

                var result = JsonSerializer.Serialize(errorModel, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
                await response.WriteAsync(result);
            }
        }
    }

    public static class ErrorHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
