using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PubSubApp.Models;
using PubSubApp.Services;
using PubSubApp.WebSocketModule;

namespace PubSubApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IWebSocketHandler _chat;
        readonly UserService userService;
        public UserController(IWebSocketHandler chat, UserService userService)
        {
            _chat = chat;
            this.userService = userService;
        }
        // GET: api/Chat
        [HttpGet]
        public ActionResult<List<User>> Get() => userService.Get();

        // GET: api/Chat/5
        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var user = userService.Get(id);
            if (user == null) return NotFound();
            return user;
        }


        // POST: api/Chat
        [HttpPost]
        public ActionResult<User> Post([FromForm] User user)
        {
            userService.Create(user);

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        // PUT: api/Chat/5
        [HttpPut("{id:length(24)}")]
        public ActionResult Put(string id, [FromBody] User userIn)
        {
            var user = userService.Get(userIn.UserID);
            if (user == null) return NotFound();
            userService.Update(id, userIn);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:length(24)}")]
        public ActionResult Delete(string id)
        {
            var user = userService.Get(id);
            if (user == null) return NotFound();
            userService.Remove(id);
            return NoContent();
        }
    }
}
