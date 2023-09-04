using BlogX.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogX.WebUI.ApiEndpoints
{
    public static class BlobStorageApi
    {
        public static RouteGroupBuilder MapBlobStorageApi(this RouteGroupBuilder group)
        {
            group.MapGet("/blob/{blobName}", GetByBlobName);
            group.MapPost("/blob", CreateBlob);

            return group;
        }

        public static async Task<IResult> GetByBlobName(string blobName, IBlobStorageService blobStorageService)
        {
            var stream = await blobStorageService.GetAsync(blobName);

            return Results.File(stream, "image/png");
        }

        public static async Task<IResult> CreateBlob(IFormFile file, IBlobStorageService blobStorageService, HttpContext context)
        {
            using var stream = file.OpenReadStream();
            
            var bolbName = Guid.NewGuid().ToString("N");

            var success = await blobStorageService.PutAsync(bolbName, stream);
            if (!success)
                return Results.BadRequest();

            return Results.Accepted($"{context.Request.Scheme}://{context.Request.Host}/api/blob/{bolbName}", bolbName);
        }

    }
}
