namespace UserCredentialsApp.Middleware
{
    public class TraceIdMiddleware
    {
        private readonly RequestDelegate _next;
        public TraceIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var traceId = Guid.NewGuid().ToString();
            context.Request.Headers["trace-id"] = traceId;
            await _next(context);
        }
    }
}
