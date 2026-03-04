using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Cryptography;
using Scheduler.Database;
using Scheduler.Models;

namespace Scheduler.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(
    IRepository<User> userRepository,
    IMapper mapper,
    ISchedulerCryptography schedulerCryptography
) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly ISchedulerCryptography _schedulerCryptography = schedulerCryptography;

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
    {
        var users = await _userRepository.Query();
        return users.Select(_mapper.Map<UserModel>).ToList();
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserModel>> GetUser(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return _mapper.Map<UserModel>(user);
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(Guid id, UserUpdateModel userModel)
    {
        if (!await userExists(id)) { return NotFound(); }


        var user = _mapper.Map<User>(userModel);
        if (id != userModel.Id)
        {
            return BadRequest();
        }

        _userRepository.Update(user);


        return NoContent();
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserCreateModel>> PostUser(UserCreateModel userModel)
    {
        var salt = _schedulerCryptography.GenerateSalt();
        var password = _schedulerCryptography.GetSaltedPasswordHash(userModel.Password, salt);


        var user = new User(
                Guid.NewGuid(),
                userModel.Username,
                userModel.Email,
                password,
                userModel.FirstName,
                userModel.LastName,
                salt
            );
        await _userRepository.CreateAsync(user);
        return CreatedAtAction("GetUser", new { id = user.Id }, user);
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NoContent(); // Ensure idempotency of delete operation
        }

        await _userRepository.DeleteAsync(user);

        return NoContent();
    }

    [HttpPost]
    [Route("api/[controller]/login")]
    public async Task<bool> UserLogin(string username, string password)
    {
        var user = await _userRepository.SingleOrDefaultAsync(u => u.NormalisedUserName == username.ToUpperInvariant());
        if (user == null) 
        {  
            return false; 
        }

        var passwordHash = _schedulerCryptography.GetSaltedPasswordHash(password, user.Salt);

        return passwordHash == user.Password;
    }

    private async Task<bool> userExists(Guid id)
    {
        return await _userRepository.GetByIdAsync(id) != null;
    }
}
