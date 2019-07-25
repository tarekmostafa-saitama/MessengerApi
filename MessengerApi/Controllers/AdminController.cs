using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MessengerApi.Core;
using MessengerApi.Core.Enums;
using MessengerApi.Core.ViewModels;
using MessengerApi.Persistence.Identity;

namespace Messenger_API.Controllers
{
    [Authorize(Roles ="Admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Route("StaticsCount")]
        public IHttpActionResult StaticsCount()
        {
            var count = new StaticsCount
            {
                MembersCount = _unitOfWork.EventTracerRepository.GetEventCount(EventType.UserRegistered),
                MemberMessagesCount = _unitOfWork.EventTracerRepository.GetEventCount(EventType.MemberMessage),
                StrangerMessagesCount = _unitOfWork.EventTracerRepository.GetEventCount(EventType.StrangerMessage),
                ErrorsCount = _unitOfWork.EventTracerRepository.GetEventCount(EventType.ErrorHappen),
            };
            return Ok(count);
        }
        [HttpGet]
        [Route("Errors")]
        public IHttpActionResult Errors()
        {
            return Ok(_unitOfWork.ErrorLogRepository.GetError());
        }
    }
}