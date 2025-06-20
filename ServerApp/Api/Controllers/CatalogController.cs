using Application.Queries.CatalogsQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Search an especific catalog con DB.
        /// </summary>
        /// <param></param>
        /// <returns>return all type of request available to create</returns>
        /// <response code="200">Returns a list of strings and Ids</response>
        /// <response code="401">Invalid Credentials</response>
        [HttpGet("RequestTypeCatalog")]
        [ProducesResponseType(typeof(List<string>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetRequestTypes()
        {
            try
            {
                var result = await _mediator.Send(new GetRequestTypesQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
