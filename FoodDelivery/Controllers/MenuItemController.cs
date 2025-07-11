﻿using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MenuItemController(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        [HttpGet]
        public IActionResult Get()
        {
            var menuItems = _unitOfWork.MenuItem
        .List(null, null, "Category,MenuItemFoodTypes.FoodType")
        .Select(m => new MenuItemDto
        {
            Id = m.Id,
            Name = m.Name,
            Price = m.price,
            CategoryName = m.Category.Name,
            FoodTypeNames = m.MenuItemFoodTypes
                .Select(mt => mt.FoodType.Name)
                .ToList()
        });

            return Json(new { data = menuItems });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.MenuItem.Get(c => c.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.MenuItem.Delete(objFromDb);
            _unitOfWork.Commit();
            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}
