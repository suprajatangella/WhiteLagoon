using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberController(ApplicationDbContext db)
        {
             _db = db;
        }
        public IActionResult Index()
        {
            var villanos = _db.VillaNumbers.Include("Villa").ToList();
            return View(villanos);
        }

        public IActionResult Create()
        {
            
            VillaNumberVM vm = new();
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(
                u=> new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            vm.VillaList = list;
            return View(vm);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberVM vm)
        {
            bool roomNumberExists = _db.VillaNumbers.Any(r => r.Villa_Number == vm.VillaNumber.Villa_Number);
            if (ModelState.IsValid && ! roomNumberExists) 
            {
                _db.VillaNumbers.Add(vm.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number has been created Successfully.";
                return RedirectToAction(nameof(Index));
            }

            if(roomNumberExists)
            {
                TempData["error"] = "The Villa Number already Exists.";
            }
            
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            vm.VillaList = list;

            return View(vm);
        }

        public IActionResult Edit(int villaNo)
        {
            var villaNum = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNo);
            if(villaNum == null)
            {
                return RedirectToAction("Error", "Home");
            }
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            VillaNumberVM vm = new();
            vm.VillaList = list;
            vm.VillaNumber = villaNum;
            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(VillaNumberVM vm)
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(vm.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number has been updated Successfully.";
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(
               u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               });
            vm.VillaList = list;
            return View(vm);
        }

        public IActionResult Delete(int villaNo)
        {
            var villaNum = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNo);
            VillaNumberVM vm = new();
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            vm.VillaList = list;
            vm.VillaNumber = villaNum;
            return View(vm);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM vm)
        {
            if(vm.VillaNumber is not null)
            {
                _db.VillaNumbers.Remove(vm.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number has been deleted Successfully.";
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            vm.VillaList = list;
            return View(vm);
        }
    }
}
