using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
             _db = db;
        }
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if (ModelState.IsValid) 
            {
                _db.Villas.Add(villa);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been created Successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(villa);
        }

        public IActionResult Edit(int id)
        {
            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }
        [HttpPost]
        public IActionResult Edit(Villa villa)
        {
            if (ModelState.IsValid)
            {
                _db.Villas.Update(villa);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been updated Successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(villa);
        }

        public IActionResult Delete(int id)
        {
            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);
            return View(villa);
        }
        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            if(villa is not null)
            {
                _db.Villas.Remove(villa);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been deleted Successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(villa);
        }
    }
}
