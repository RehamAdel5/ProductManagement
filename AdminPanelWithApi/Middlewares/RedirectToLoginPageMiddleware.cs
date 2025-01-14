
namespace AdminPanelWithApi.Middlewares
{
    public class RedirectToLoginPageMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectToLoginPageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check if the user is authenticated
            if (context.User.Identity.IsAuthenticated)
            {
                // User is authenticated, proceed with the request
                await _next(context);
            }
            else
            {
                // User is not authenticated, redirect to the login page
                context.Response.Redirect("/Identity/Account/Login");
                return;
            }
        }
    }

}
