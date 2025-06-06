using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Net;

namespace VisualTimeTracking.Server.Middleware
{
    public class ExceptionhandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (SocketException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                await context.Response.WriteAsync(e.Message);
            }
            catch (TimeoutException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                await context.Response.WriteAsync(e.Message);
            }
            catch (WebException e)
            {
#nullable disable
                var hwr = (HttpWebResponse)e.Response;
                var responseex = hwr.StatusCode;
                int statcode = (int)responseex;
                if (statcode == 404)
                {
                    await context.Response.WriteAsync(e.Message);
                }
                if (statcode == 401)
                {
                    await context.Response.WriteAsync(e.Message);
                }
                if (statcode == 408)
                {
                    await context.Response.WriteAsync(e.Message);
                }

                //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //await context.Response.WriteAsync(e.Message);
            }
            catch (HttpRequestException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                await context.Response.WriteAsync(e.Message);
            }
            catch (ValidationException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(e.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(e.Message);
            }

        }
    }
}
