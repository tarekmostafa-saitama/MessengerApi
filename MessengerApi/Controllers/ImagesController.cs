using MessengerAPI.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MessengerApi.Core;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.Enums;
using MessengerApi.Core.ViewModels;
using MessengerApi.Persistence.Identity;

namespace Messenger_API.Controllers
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
        public IHttpActionResult UploadMemberImage([FromBody]MemberChatImage Data)
        {
            Data.Image = HttpContext.Current.Request.Files["Image"];
            string NewName = Data.RelationId.ToString() + "@" + Guid.NewGuid().ToString();
            string path = null;
            if (Data.Image != null && Data.Image.ContentLength > 0)
            {
                var extention = Path.GetExtension(Data.Image.FileName);
                path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/MCP/"), NewName + extention);
                Data.Image.SaveAs(path);
            }
            path = "Uploads/MCP/" + NewName + Path.GetExtension(Data.Image.FileName);
      
            _unitOfWork.MessagesRepository.AddMessage(new Message { relation_id = Data.RelationId, Type = MessageType.ImageMessage, MessageData = path, Date = DateTime.UtcNow, Sender = Data.Sender });
            _unitOfWork.Complete();
            var hub = new MessagesHub(_unitOfWork);
            hub.RoutingImageMessage(Data.RelationId, path, Data.Sender);
            return Ok(path);
        }
        [Route("UploadAnonymousImage")]
        [HttpPost]
        public IHttpActionResult UploadAnonymousImage()
        {
            var Image = HttpContext.Current.Request.Files["Image"];
            string NewName = Guid.NewGuid().ToString();
            string path = null;
            if (Image != null && Image.ContentLength > 0)
            {
                var extention = Path.GetExtension(Image.FileName);
                path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/ACP/"), NewName + extention);
                Image.SaveAs(path);
            }
            path = "Uploads/ACP/" + NewName + Path.GetExtension(Image.FileName);

            return Ok(path);
        }
  

    }
    
}