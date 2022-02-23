using AutoMapper;
using IndacoProject.Corso.Core;
using IndacoProject.Corso.Data.Entities;
using IndacoProject.Corso.Data.Models;
using IndacoProject.Corso.Api.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Api.ControllersApi
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<ApplicationRole> _roleManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly ITokenService _tokenService;
        protected readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] ApplicationUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<ApplicationUser>(model);
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return Ok(_tokenService.GenerateAccessToken(model.UserName));
                }
            }
            return BadRequest();
        }

        [HttpPost("login")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        AccessToken = _tokenService.GenerateAccessToken(model.UserName),
                        RefreshToken = _tokenService.GenerateRefreshToken(model.UserName),
                    });
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        [HttpPost("refresh")]
        [SampleResultFilter]
        [SampleAuthorizationFilterFactory]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Refresh()
        {
            return Ok(new
            {
                AccessToken = _tokenService.GenerateAccessToken(User.Identity.Name),
                RefreshToken = _tokenService.GenerateRefreshToken(User.Identity.Name),
            });
        }

        [HttpPost("error")]
        //[TypeFilter(typeof(SampleExceptionFilter))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Error()
        {
            throw new System.Exception("Error");
        }
    }
}
