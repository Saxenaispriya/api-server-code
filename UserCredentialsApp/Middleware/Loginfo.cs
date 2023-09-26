namespace UserCredentialsApp.Middleware
{
    public class Loginfo
    {
        private readonly RequestDelegate _next;

        public Loginfo(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("i am middleware");
            await _next(context);
        }

    }
}
