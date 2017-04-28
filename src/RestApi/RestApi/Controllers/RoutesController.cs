using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace RestApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Routes")]
    public class RoutesController : Controller
    {
        private IGraphClient _graphClient;

        public RoutesController(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }
    }
}