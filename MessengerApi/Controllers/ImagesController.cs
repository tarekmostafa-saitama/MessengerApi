using System;
using System.Web;
using System.Web.Http;
using MessengerApi.Core;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.Enums;
using MessengerApi.Core.ViewModels;
using MessengerApi.Hubs;

namespace MessengerApi.Controllers
{
    [RoutePrefix("api/Image")]
    public class ImagesController : ApiController
    {

        private readonly IUnitOfWork _unitOfWork;
        public ImagesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Route("UploadMemberImage")]
        [HttpPost]
        public IHttpActionResult UploadMemberImage([FromBody]MemberChatImage data)
        {
            data.Image = HttpContext.Current.Request.Files["Image"];
            var path  = _unitOfWork.ImageRepository.SaveMemberImageMessage(data.Image,data.RelationId.ToString());

            _unitOfWork.MessagesRepository.AddMessage(new Message { relation_id = data.RelationId, Type = MessageType.ImageMessage, MessageData = path, Date = DateTime.UtcNow, Sender = data.Sender });
            _unitOfWork.Complete();
            var hub = new MessagesHub(_unitOfWork);
            hub.RoutingImageMessage(data.RelationId, path, data.Sender);
            return Ok(path);
        }
        [Route("UploadAnonymousImage")]
        [HttpPost]
        public IHttpActionResult UploadAnonymousImage()
        {
            var image = HttpContext.Current.Request.Files["Image"];
            var path = _unitOfWork.ImageRepository.SaveStrangerImageMessage(image);
            return Ok(path);
        }
  

    }
    
}