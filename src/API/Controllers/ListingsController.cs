using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net;
using Business.API;
using ViewModels.API;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly ILogger<ListingsController> _logger;
        private readonly IPropertyListingBusiness _listingBL;

        public ListingsController(ILogger<ListingsController> logger, IPropertyListingBusiness listingBL)
        {
            _logger = logger;
            _listingBL = listingBL;

        }

        [HttpGet]
        [ProducesResponseType(typeof(PropertyListingResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(PropertyListingResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(PropertyListingResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PropertyListingResponse>> GetListings([FromQuery] PropertyListingRequest request)
        {
            //Put Validation here (if required) and Send a Bad Request

            //Create logs here
            var result = await _listingBL.GetListing(request);
            if (result.Items != null)
                return Ok(result);
            else
                return NotFound("Some Error Occured, Please contact Admin");

        }

    }
}
