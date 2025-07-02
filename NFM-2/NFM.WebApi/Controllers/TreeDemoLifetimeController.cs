using Microsoft.AspNetCore.Mvc;
using NFM.Business.DemoLifetimeServices;
using NFM.Business.ModelDTOs;

namespace NFM.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TreeDemoLifetimeController : ControllerBase
    {
        private readonly CurrentRequest _currentRequest;
        private readonly ServerConnectionDb _serverConnectionDb;
        private readonly TransientB _transientB;

        public TreeDemoLifetimeController(CurrentRequest currentRequest, ServerConnectionDb serverConnectionDb, TransientB transientB)
        {
            _currentRequest = currentRequest;
            _serverConnectionDb = serverConnectionDb;
            _transientB = transientB;
        }

        [HttpGet]
        public ActionResult<Node> GetDependencyTree()
        {
            var root = new Node()
            {
                Name = "controller",
                Children = new List<Node>()
                {
                    _currentRequest.Node,
                    _serverConnectionDb.Node,
                    _transientB.Node
                }
            };
            return Ok(root);
        }
    }
}
