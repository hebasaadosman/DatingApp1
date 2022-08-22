
namespace API.Controllers;

public class AccountController : BaseAPIController
{
    private readonly ITokenService _tokenservice;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenservice, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenservice = tokenservice;
        _mapper = mapper;
    }
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {

        if (await UserExists(registerDto.Username)) return BadRequest("this name is taken");

        var user = _mapper.Map<AppUser>(registerDto);


        user.UserName = registerDto.Username.ToLower();

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);
        var roleResults = await _userManager.AddToRoleAsync(user, "Member");
        if (!roleResults.Succeeded) return BadRequest(result.Errors);
        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenservice.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender

        };
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {

        var user = await _userManager.Users
        .Include(p => p.Photos)
        .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
        if (user == null) return Unauthorized("invalid user");
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded) return Unauthorized();

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenservice.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };


    }
    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }

}
