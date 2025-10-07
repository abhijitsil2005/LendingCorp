using BusinessEntities;
using Core.Services.Users;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("users")]
    public class UserController : BaseApiController
    {
        private readonly ICreateUserService _createUserService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IGetUserService _getUserService;
        private readonly IUpdateUserService _updateUserService;

        public UserController(ICreateUserService createUserService, IDeleteUserService deleteUserService, IGetUserService getUserService, IUpdateUserService updateUserService)
        {
            _createUserService = createUserService;
            _deleteUserService = deleteUserService;
            _getUserService = getUserService;
            _updateUserService = updateUserService;
        }

        [Route("{userId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateUser(Guid userId, [FromBody] UserModel model)
        {
            // Check if user already exists
            var user = _getUserService.GetUser(userId);
            if (user != null)
            {
                // if user exists, return 409 Conflict
                return AlreadyExists("User exists with ID: " + userId);
            }
            user = _createUserService.Create(userId, model.Name, model.Email, model.Type, model.Age, model.AnnualSalary, model.Tags);
            return Found(new UserData(user));
        }

        [Route("{userId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateUser(Guid userId, [FromBody] UserModel model)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                string message = "User not found.";
                return DoesNotExist(message);
            }
            _updateUserService.Update(user, model.Name, model.Email, model.Type, model.Age, model.AnnualSalary, model.Tags);
            return Found(new UserData(user));
        }

        [Route("{userId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                string message = "User not found.";
                return DoesNotExist(message);
            }
            _deleteUserService.Delete(user);
            // return 200 OK with success message
            string successMessage = $"User with ID: {userId} deleted successfully.";
            return TaskSuccess(successMessage);
        }

        [Route("{userId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                string message = "User not found.";
                return DoesNotExist(message);
            }
            return Found(new UserData(user));
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetUsers(int skip, int take, UserTypes? type = null, string name = null, string email = null)
        {
            var users = _getUserService.GetUsers(type, name, email)
                                       .Skip(skip).Take(take)
                                       .Select(q => new UserData(q))
                                       .ToList();
            return Found(users);
        }

        [Route("clear")]
        [HttpDelete]
        public HttpResponseMessage DeleteAllUsers()
        {
            _deleteUserService.DeleteAll();
            string successMessage = "All users deleted successfully.";
            return TaskSuccess(successMessage);
        }

        [Route("list/tag")]
        [HttpGet]
        public HttpResponseMessage GetUsersByTag(string tag)
        {
            var users = _getUserService.GetUsers(tag)
                                       .Select(q => new UserData(q))
                                       .ToList();
            return Found(users);
        }
    }
}