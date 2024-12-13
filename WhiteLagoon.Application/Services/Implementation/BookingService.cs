﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Interfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }
        public void CreateBooking(Booking booking)
        {
            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
        }

        public IEnumerable<Booking> GetAllBookings(string userId = "", string statusFilterList = "")
        {
            IEnumerable<string> statusList = statusFilterList.ToLower().Split(",");

            if (!string.IsNullOrEmpty(statusFilterList) && !string.IsNullOrEmpty(userId))
            {
                return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()) 
                &&  u.UserId == userId
                , includeProperties: "User,Villa");
            }
            else
            {
                if (!string.IsNullOrEmpty(statusFilterList))
                {
                    return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()), includeProperties: "User,Villa");
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    return _unitOfWork.Booking.GetAll(
                        u => u.UserId == userId,
                        includeProperties: "User,Villa");
                }
            }
            return _unitOfWork.Booking.GetAll(includeProperties: "User, Villa");
        }

        public Booking GetBookingById(int bookingId)
        {
            return _unitOfWork.Booking.Get(u=>u.Id == bookingId, includeProperties: "User, Villa");
        }

        public IEnumerable<int> GetCheckedInVillaNumbers(int villaId)
        {
            return _unitOfWork.Booking.GetAll(u => u.VillaId == villaId && u.Status == SD.StatusCheckedIn)
               .Select(u => u.VillaNumber);
        }

        public void UpdateStatus(int bookingId, string bookingStatus, int villaNumber=0)
        {
            var bookingDb = _unitOfWork.Booking.Get(u=> u.Id == bookingId, tracked: true);
            if (bookingDb != null)
            {
                bookingDb.Status = bookingStatus;
                if (bookingStatus == SD.StatusCheckedIn)
                {
                    bookingDb.VillaNumber = villaNumber;
                    bookingDb.ActualCheckInDate = DateTime.Now;
                }
                if(bookingStatus == SD.StatusCompleted)
                {
                    bookingDb.ActualCheckOutDate = DateTime.Now;
                }
            }
            _unitOfWork.Save();
        }

        public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingDb = _unitOfWork.Booking.Get(u => u.Id == bookingId, tracked: true);

            if (bookingDb != null)
            {
                if (!string.IsNullOrEmpty(sessionId)) 
                { 
                    bookingDb.StripeSessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    bookingDb.StripePaymentIntentId = paymentIntentId;
                    bookingDb.PaymentDate = DateTime.Now;
                    bookingDb.IsPaymentSuccessful = true;
                }
            }
            _unitOfWork.Save();
        }
    }
}
