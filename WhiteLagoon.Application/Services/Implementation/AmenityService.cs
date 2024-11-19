using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Interfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class AmenityService : IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateAmenity(Amenity amenity)
        {
            if (amenity != null)
            {
                _unitOfWork.Amenity.Add(amenity);
                _unitOfWork.Save();
            }
        }

        public bool DeleteAmenity(int id)
        {
            var amenity = _unitOfWork.Amenity.Get(u=>u.Id == id);
            try
            {
                if (amenity != null)
                {
                    _unitOfWork.Amenity.Remove(amenity);
                    _unitOfWork.Save();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException($"Amenity with Id={id} not found");
                }
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public IEnumerable<Amenity> GetAllAmenities()
        {
            return _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
        }

        public Amenity GetAmenityById(int id)
        {
            return _unitOfWork.Amenity.Get(u=> u.Id == id, includeProperties: "Villa");
        }

        public void UpdateAmenity(Amenity amenity)
        {
            if(amenity!=null)
            {
                _unitOfWork.Amenity.Update(amenity);
                _unitOfWork.Save();
            }
        }
    }
}
