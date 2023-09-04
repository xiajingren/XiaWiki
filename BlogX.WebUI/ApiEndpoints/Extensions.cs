namespace BlogX.WebUI.ApiEndpoints
{
    public static class Extensions
    {
        public static void MapBlogXApi(this WebApplication app)
        {
            app.MapGroup("/api")
               .MapBlobStorageApi();
        }
    }
}
