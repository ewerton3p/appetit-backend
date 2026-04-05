using Appetit.Domain.Common.Exceptions;
using Appetit.Domain.Common.Responses;
using System.Net;
using System.Text.Json;

namespace Appetit.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await SetResponseException(context, HttpStatusCode.NotFound, ex);
            }
            catch (BadRequestException ex)
            {
                await SetResponseException(context, HttpStatusCode.BadRequest, ex);
            }
            catch (InternalServerErrorException ex)
            {
                await SetResponseException(context, HttpStatusCode.InternalServerError, ex);
            }
            catch (Exception ex)
            {
                await SetResponseException(context, HttpStatusCode.InternalServerError, ex);
            }
        }

        public async Task SetResponseException(HttpContext context, HttpStatusCode httpStatusCode, Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            var response = new Response();

            response.Message = ex.Message;
            response.Success = false;

            response.Details = _environment.IsDevelopment() ? ex.StackTrace?.ToString() : "Internal Server Error";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);

        }

    }
}
