using Microsoft.AspNetCore.Mvc;
using PasswordManager.Models.DTOs;
using PasswordManager.Services;
using PasswordManager.Responses;

namespace PasswordManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger) {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto data) {
            ResponseData<string> response = new ResponseData<string>();

            try {
                string token = await _authService.SignInUser(data);
                response.Status = ResponseMessages.Success;
                response.Message = ResponseMessages.LoginSuccess;
                response.Data = token;

                return Ok(response);
            } catch (UnauthorizedAccessException e) {
                response.Status = ResponseMessages.Failure;
                response.Message = e.Message;
                return Unauthorized(response);
            } catch (InvalidEntityException e) {
                response.Status = ResponseMessages.Failure;
                response.Message = e.Message;
                return BadRequest(response);

            } catch (Exception e) {
                response.Status = ResponseMessages.Failure;
                response.Message = e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}