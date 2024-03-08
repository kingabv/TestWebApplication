using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebApplication.Exceptions;
using TestWebApplication.Extensions;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    [Route("api/Pictures")] 
    [ApiController]
    public class PictureItemController : ControllerBase
    {
        private readonly PictureContext _context;
        private readonly ILogger<PictureItemController> _logger;

        public PictureItemController(PictureContext context, ILogger<PictureItemController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/PictureItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PictureItem>>> GetPictureItems()
        {
            return await _context.PictureItems.ToListAsync();
        }

        // GET: api/PictureItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PictureItem>> GetPictureItem(long id)
        {
            var pictureItem = await _context.PictureItems.FindAsync(id);

            if (pictureItem == null)
            {
                var ex = new PictureItemException(ErrorCode.NotFound, string.Join(Environment.NewLine,
                    $"A picture with '{id}' identifier does not exists."));
                return this.LogAndReturnConflict(_logger, new ErrorModel(ex, ErrorCode.NotFound, ex.Message));
            }

            return pictureItem;
        }

        [HttpPost]
        public async Task<ActionResult<PictureItem>> PostPictureItem([FromForm] PictureDto picture)
        {
            try
            {
                var validationResults = picture.Properties.ValidatePictureItemModel().ToList();
                if (validationResults.Any())
                    throw new PictureItemException(ErrorCode.Validation, string.Join(Environment.NewLine,
                        validationResults.Select(v => v.ErrorMessage)));

                var pictureItem = await _context.PictureItems.FindAsync(picture.Properties.Id);
                if (pictureItem != null)
                    throw new PictureItemException(ErrorCode.Conflict, string.Join(Environment.NewLine,
                        $"A picture with '{picture.Properties.Id}' identifier already exists."));

                picture.Properties.File = ToByteModel(picture.FileToUpload);
                
                _context.PictureItems.Add(picture.Properties);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostPictureItem), new { id = picture.Properties.Id }, picture);
            }
            catch (PictureItemException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCode.Validation:
                        return this.LogAndReturnBadRequest(_logger,
                            new ErrorModel(ex, ErrorCode.Validation, $"Bad Request:{ex.Message}"));
                    case ErrorCode.Conflict:
                        return this.LogAndReturnConflict(_logger,
                            new ErrorModel(ex, ErrorCode.Conflict, $"Conflict: {ex.Message}"));
                    case ErrorCode.Forbidden:
                        return this.LogAndReturnForbidden(_logger, new ErrorModel(ex, ErrorCode.Forbidden));
                    default:
                        return this.LogAndReturnInternalServerError(_logger, ex,
                            new ErrorModel(ex, ErrorCode.InternalServerError, "Internal Server Error"));
                }
            }
            catch (Exception ex) //500
            {
                return this.LogAndReturnInternalServerError(_logger, ex,
                    new ErrorModel(ex, ErrorCode.InternalServerError, "Internal Server Error"));
            }
        }

        // DELETE: api/PictureItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePictureItem(long id)
        {
            var pictureItem = await _context.PictureItems.FindAsync(id);
            if (pictureItem == null)
            {
                var ex = new PictureItemException(ErrorCode.NotFound, string.Join(Environment.NewLine,
                    $"A picture with '{id}' identifier does not exists."));
                return this.LogAndReturnConflict(_logger, new ErrorModel(ex, ErrorCode.NotFound, ex.Message));
            }

            _context.PictureItems.Remove(pictureItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private  byte[] ToByteModel( IFormFile reportFile)
        {
            byte[] fileContent;
            using (var fileStream = reportFile.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);

                    fileContent = memoryStream.ToArray();
                }
            }
            return fileContent;
        }
    }
}
