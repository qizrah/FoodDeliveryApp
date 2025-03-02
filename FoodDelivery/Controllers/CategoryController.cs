using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { data = _unitOfWork.Category.GetAll() });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.Category.Get(c=>c.Id == id);
            if (obj == null)
            {
                return Ok(new { success = "false", message = "Error While Deleting Record" });
            }
            _unitOfWork.Category.Delete(obj);
            _unitOfWork.Commit();
            return Ok(new { success = "true", message = "Deleted Successfully" });
        }
    }
}
