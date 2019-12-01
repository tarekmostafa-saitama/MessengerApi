using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using MessengerApi.Core;
using MessengerApi.Core.DataTransferObjects;
using MessengerApi.Core.DbEntities;
using MessengerApi.Persistence;
using MessengerApi.Persistence.Identity;
using MessengerApi.Persistence.Repositories;

namespace MessengerApi.Controllers
{
    [Authorize(Roles ="Member")]
    [RoutePrefix("api/Member")]
    public class MemberController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork(new ApplicationDbContext(), new FileHandlerRepository());
        //public MemberController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}
        // GET api/<controller>
        [HttpGet]
        [Route("CheckUserExist")]
        [AllowAnonymous]
        public IHttpActionResult CheckUserExist(string name)
        {

            bool found = _unitOfWork.MembersRepository.UserExist(name);
            if (found)
                return Ok(true);
            else
                return Ok(false);
        }
        [HttpGet]
        [Route("Search")]
        [AllowAnonymous]
        public IHttpActionResult Search(string name)
        {
            var users = _unitOfWork.MembersRepository.SearchMembers(name);
            return Ok(Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserSearchDataDTO>>(users));
        }
        [HttpGet]
        [Route("ChangeNickName")]

        public IHttpActionResult ChangeNickName(string nickName,Guid id)
        {
            var i = User;
            var relation = _unitOfWork.RelationsRepository.GetRelation(id);
            if(relation != null)
            {
                relation.NickName = nickName;
                _unitOfWork.Complete();
                return Ok();
            }
            return NotFound();
        }

  

        [HttpGet]
        [Route("DeleteFriend")]
        public IHttpActionResult DeleteFriend(Guid id)
        {

            var relation = _unitOfWork.RelationsRepository.GetRelation(id);
            if(relation!=null)
            {
                var messages = _unitOfWork.MessagesRepository.GetMessages(id).ToList();
                _unitOfWork.MessagesRepository.RemoveMessages(messages);
                _unitOfWork.RelationsRepository.RemoveRelation(relation);
                _unitOfWork.Complete();
                return Ok();
            }
            return NotFound();
        }

 

        [HttpGet]
        [Route("ProfileDetails")]
        public ProfileDetailsDTO ProfileDetails()
        {
            var user = _unitOfWork.MembersRepository.GetUser(User.Identity.Name);
            return Mapper.Map<ApplicationUser, ProfileDetailsDTO>(user);
        }
        [HttpPost]
        [Route("UpdateProfilePicture")]
        public IHttpActionResult UpdateProfilePicture()
        {
            var image = HttpContext.Current.Request.Files["Image"];
            string newName = Guid.NewGuid().ToString();
            string path = null;
            if (image != null && image.ContentLength > 0)
            {
                var extenstion = Path.GetExtension(image.FileName);
                path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/ProfilePictures/"), newName + extenstion);
                image.SaveAs(path);
            }
            path = "Content/ProfilePictures/" + newName + Path.GetExtension(image.FileName);

            var context = new ApplicationDbContext();
            var user = context.Users.First(x => x.UserName == User.Identity.Name);
            user.Image = path;
            context.SaveChanges();

            return Ok(path);
        }
        [HttpPost]
        [Route("UpdateSetting")]
        public IHttpActionResult UpdateSetting(ProfileDetailsDTO model)
        {
            var user = _unitOfWork.MembersRepository.GetUser(User.Identity.Name);
            user.AppearInSearch = model.AppearInSearch;
            user.Status = model.Status;
            _unitOfWork.Complete();


            return Ok(new ProfileDetailsDTO { AppearInSearch = model.AppearInSearch, Status = model.Status, Image = user.Image });
        }


    }
}