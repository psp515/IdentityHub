﻿using ImageHub.Api.Contracts.Image.DeleteImage;
using ImageHub.Api.Features.Images.DeteleImage;
using ImageHub.Api.Features.Images.Repositories;

namespace ImageHub.Api.Features.Images.DeleteImage;

public class GetImageHandler(IImageRepository imageRepository) : IRequestHandler<DeleteImageCommand, Result<DeleteImageResponse>>
{
    public async Task<Result<DeleteImageResponse>> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        var guid = Guid.Parse(request.Id);
        var image = await imageRepository.GetImageById(guid, cancellationToken);

        if (image is null)
        {
            var error = DeleteImageErrors.ImageNotFound;
            return Result<DeleteImageResponse>.Failure(error);
        }

        await imageRepository.DeleteImage(image, cancellationToken);

        return Result<DeleteImageResponse>.Success(new() { Id = request.Id});
    }
}
