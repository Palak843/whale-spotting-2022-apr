using Microsoft.AspNetCore.Mvc;
using WhaleSpotting.Models.Request;
using WhaleSpotting.Repositories;

namespace WhaleSpotting.Controllers {
  [ApiController]
  [Route("/debug")]
  public class DebugController : ControllerBase {
    private readonly IUsersRepo _users;

    public DebugController(
      IUsersRepo users
    ) {
      _users = users;
    }

    [HttpPost]
    public IActionResult DebugCreateAdmin([FromBody] CreateUserRequest userRequest) {
      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      var newUser = _users.Create(userRequest);

      return Created("/api", newUser);
    }
  }
}
