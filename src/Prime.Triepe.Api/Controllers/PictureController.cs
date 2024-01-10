using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Triepe.Domain.Abstractions.Services;
using Triepe.Domain.Dtos.PictureDtos;
using Triepe.Domain.Pagination;
using Triepe.Domain.RequestForms;

namespace Triepe.Api.Controllers
{
    [Route("api/pictures")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IPictureService _service;

        public PictureController(IPictureService PictureService)
        {
            _service = PictureService;
        }

        [HttpPost]
        public async Task<ActionResult<PictureResponseDto>> CreatePictureAsync([FromForm] PictureCreateRequestForm form)
        {
            var createdPicture = await _service.CreateAsync(form);

            return CreatedAtAction(nameof(GetPictureAsync), new { id = createdPicture.Id }, createdPicture);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PictureResponseDto>> GetPictureAsync([FromRoute] Guid id)
        {
            var Picture = await _service.GetByIdAsync(id);

            return Ok(Picture);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PictureResponseDto>>> GetPageAsync(
            [FromQuery] PagingInfo pagingInfo)
        {
            var activities = await _service.GetPaginatedResultAsync(pagingInfo);

            return Ok(activities);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPictureAsync([FromRoute] Guid id, [FromForm] PictureUpdateRequestForm form)
        {
            await _service.UpdateAsync(id, form);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePictureAsync([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
