using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Interfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;
using Microsoft.AspNetCore.Hosting;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class VillaService : IVillaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public void CreateVilla(Villa villa)
        {
            throw new NotImplementedException();
        }

        public bool DeleteVilla(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Villa> GetAllVillas()
        {
            throw new NotImplementedException();
        }

        public Villa GetVillaById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Villa> GetVillasAvailabilityByDate(int nights, DateOnly checkInDate)
        {
            throw new NotImplementedException();
        }

        public bool IsVillaAvailableByDate(int villaId, int nights, DateOnly checkInDate)
        {
            throw new NotImplementedException();
        }

        public void UpdateVilla(Villa villa)
        {
            throw new NotImplementedException();
        }
    }
}
