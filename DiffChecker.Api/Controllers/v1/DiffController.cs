using System.Net;
using DiffChecker.Api.Model;
using DiffChecker.Api.Services.Interfaces;
using DiffChecker.Domain.Error;
using DiffChecker.Domain.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace DiffChecker.Api.Controllers.v1
{
    [ApiController]
    [Route("v1/[controller]")]
    [Produces("application/json")]
    public class DiffController : ControllerBase
    {
        private readonly IDiffCheckerService _diffCheckerService;
        private readonly ILogger<DiffController> _logger;

        public DiffController(
            IDiffCheckerService diffCheckerService,
            ILogger<DiffController> logger)
        {
            _diffCheckerService = diffCheckerService;
            _logger = logger;
        }

        /// <summary>
        /// Sets left data for comparison
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "data": "VGVzdA==",
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The key whose left is to be set</param>
        /// <param name="requestBody">Json with string information</param>
        /// <returns>The newly created record</returns>
        /// <response code="200">Data set correctly</response>
        /// <response code="400">Missing data, nothing set</response>
        [HttpPost]
        [EnableCors("CORSPolicy")]
        [Route("{id}/left")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Record Information", typeof(DiffData))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Error Message and Status", Type = typeof(ErrorResponse))]
        public IActionResult SetLeft(string id, [FromBody] SetDataRequest requestBody)
        {
            _logger.LogInformation($"Updating left data for id {id}, {requestBody}");
            var data = requestBody?.Data;

            if (string.IsNullOrEmpty(data)) throw new InvalidInputException("Missing data field");

            return Ok(_diffCheckerService.SetLeft(id, data));
        }

        /// <summary>
        /// Sets right data for comparison
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "data": "VGVzdA==",
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The key whose right is to be set</param>
        /// <param name="requestBody">Json with string information</param>
        /// <returns>The newly created record</returns>
        /// <response code="200">Data set correctly</response>
        /// <response code="400">Missing data, nothing set</response>
        [HttpPost]
        [EnableCors("CORSPolicy")]
        [Route("{id}/right")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Record Information", typeof(DiffData))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Error Message and Status", Type = typeof(ErrorResponse))]
        public IActionResult SetRight(string id, [FromBody] SetDataRequest requestBody)
        {
            _logger.LogInformation($"Updating right data for id {id}, {requestBody}");
            var data = requestBody?.Data;

            if (string.IsNullOrEmpty(data)) throw new InvalidInputException("Missing data field");

            return Ok(_diffCheckerService.SetRight(id, data));
        }

        /// <summary>
        /// Performs the comparison with the data set at the id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">The key whose data is to be compared</param>
        /// <returns>The comparison performed or the error encountered.</returns>
        /// <response code="200">Comparison done correctly</response>
        /// <response code="400">Missing Data for comparison</response>
        /// <response code="500">Error performing comparison, such as error decoding the base64 string</response>
        [HttpGet]
        [EnableCors("CORSPolicy")]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Comparison result", typeof(ComparisonResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Error Message and Status", Type = typeof(ErrorResponse))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Error Message and Status", Type = typeof(ErrorResponse))]
        public IActionResult FindDiff(string id)
        {
            _logger.LogInformation($"Calculating diff for id {id}");
            return Ok(_diffCheckerService.FindDifference(id));
        }
    }
}