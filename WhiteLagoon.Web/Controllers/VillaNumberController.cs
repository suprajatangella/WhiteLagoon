using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Application.Services.Implementation;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumber;
        private readonly IVillaService _villaService;
        public VillaNumberController(IVillaNumberService villaNumber, IVillaService villaService)
        {
            _villaNumber = villaNumber;
            _villaService = villaService;
        }
        public IActionResult Index()
        {
            var villanos = _villaNumber.GetAllVillaNumbers();
            return View(villanos);
        }

        public IActionResult Create()
        {
            
            VillaNumberVM vm = new();
            IEnumerable<SelectListItem> list = _villaService.GetAllVillas().Select(
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
            bool roomNumberExists = _villaNumber.CheckVillaNumberExists(vm.VillaNumber.Villa_Number);
            if (ModelState.IsValid && ! roomNumberExists) 
            {
                _villaNumber.CreateVillaNumber(vm.VillaNumber);
                TempData["success"] = "The Villa Number has been created Successfully.";
                return RedirectToAction(nameof(Index));
            }

            if(roomNumberExists)
            {
                TempData["error"] = "The Villa Number already Exists.";
            }
            
            IEnumerable<SelectListItem> list = _villaService.GetAllVillas().Select(
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
            var villaNum = _villaNumber.GetVillaNumberById(villaNo);
            if(villaNum == null)
            {
                return RedirectToAction("Error", "Home");
            }
            IEnumerable<SelectListItem> list = _villaService.GetAllVillas().Select(
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
                _villaNumber.UpdateVillaNumber(vm.VillaNumber);
                TempData["success"] = "The Villa Number has been updated Successfully.";
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<SelectListItem> list = _villaService.GetAllVillas().Select(
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
            var villaNum = _villaNumber.GetVillaNumberById(villaNo);
            if (villaNum == null)
            {
                return RedirectToAction("Error", "Home");
            }
            VillaNumberVM vm = new();
            IEnumerable<SelectListItem> list = _villaService.GetAllVillas().Select(
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
            if(vm.VillaNumber != null)
            {
                _villaNumber.DeleteVillaNumber(vm.VillaNumber.Villa_Number);
                TempData["success"] = "The Villa Number has been deleted Successfully.";
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<SelectListItem> list = _villaService.GetAllVillas().Select(
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
