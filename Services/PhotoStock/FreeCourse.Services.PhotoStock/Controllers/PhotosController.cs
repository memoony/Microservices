using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> SavePhoto(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo == null || photo.Length == 0)
            {
                return CreateActionResultInstance(Response<PhotoDto>.Fail("Photo is empty!", 400));
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

            using FileStream stream = new(path, FileMode.Create);
            await photo.CopyToAsync(stream, cancellationToken);

            var returnPath = $"photos/{photo.FileName}";

            PhotoDto photoDto = new()
            {
                Url = returnPath
            };

            return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
        }

        public IActionResult DeletePhoto(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("Photo not found!", 404));
            }

            System.IO.File.Delete(path);

            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}
