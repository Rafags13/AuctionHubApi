namespace AuctionHub.WebApi.Extensions
{
    internal static class ConfigureSwaggerExtensions
    {
        internal static WebApplication ConfigureSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DisplayRequestDuration();
                });
            }

            return app;
        }
    }
}
