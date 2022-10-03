using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class ProductsController : BaseController
    {
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}