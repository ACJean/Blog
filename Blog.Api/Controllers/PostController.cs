using Blog.Service;
using Blog.Service.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {

        protected readonly PostService PostService;

        public PostController(PostService postService)
        {
            PostService = postService;
        }

        [HttpGet]
        [Route("{uuid}")]
        public IActionResult Get([FromRoute] string uuid)
        {
            try
            {
                ServiceResponse<PostDTO> response = PostService.Get(uuid);
                int statusCode = ServiceResponse<PostDTO>.GetStatus(response);
                return StatusCode(statusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ServiceResponse<object>.Error(ex));
            }
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            try
            {
                ServiceResponse<List<PostDTO>> response = PostService.GetAll();
                int statusCode = ServiceResponse<List<PostDTO>>.GetStatus(response);
                return StatusCode(statusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ServiceResponse<object>.Error(ex));
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] PostDTO postDTO)
        {
            try
            {
                var userUuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ServiceResponse<PostDTO> response = PostService.Create(postDTO, userUuid);
                int statusCode = ServiceResponse<PostDTO>.GetStatusCreated(response);
                return StatusCode(statusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ServiceResponse<object>.Error(ex));
            }
        }

        [HttpPut]
        [Route("Update/{uuid}")]
        public IActionResult Update([FromBody] PostDTO postDTO, [FromRoute] string uuid)
        {
            try
            {
                var userUuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                postDTO.Uuid = uuid;
                ServiceResponse<PostDTO> response = PostService.Update(postDTO, userUuid);
                int statusCode = ServiceResponse<PostDTO>.GetStatus(response);
                return StatusCode(statusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ServiceResponse<object>.Error(ex));
            }
        }

        [HttpDelete]
        [Route("Delete/{uuid}")]
        public IActionResult Delete([FromRoute] string uuid)
        {
            try
            {
                ServiceResponse<PostDTO> response = PostService.Delete(uuid);
                int statusCode = ServiceResponse<PostDTO>.GetStatus(response);
                return StatusCode(statusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ServiceResponse<object>.Error(ex));
            }
        }

    }
}
