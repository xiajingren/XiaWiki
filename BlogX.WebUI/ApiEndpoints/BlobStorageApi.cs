using BlogX.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

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

            if (!new FileExtensionContentTypeProvider().TryGetContentType(blobName, out var contentType) || string.IsNullOrEmpty(contentType))
                contentType = "application/octet-stream";

            return Results.File(stream, contentType);
        }

        public static async Task<IResult> CreateBlob(IFormFile file, IBlobStorageService blobStorageService, HttpContext context)
        {
            using var stream = file.OpenReadStream();

            var bolbName = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";

            var success = await blobStorageService.PutAsync(bolbName, stream);
            if (!success)
                return Results.BadRequest();

            return Results.Accepted($"/api/blob/{bolbName}", bolbName);
        }

    }
}
