using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BankSystemAssessment.Dto;
using BankSystemAssessment.Interfaces;
using BankSystemAssessment.Model;

namespace BankSystemAssessment.Controllers
{
    [ApiController]
    [Route("api/BankSystem")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("UserList")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<User>>(_userRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }    

        [HttpPost("CreateUser")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromQuery] string UserName, [FromQuery] string Password,User userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            var users = _userRepository.GetUsers()
               .Where(c => c.Username.Trim().ToUpper() == UserName.TrimEnd().ToUpper())
               .FirstOrDefault();


            if (users != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);


            if (!_userRepository.CreateUser(UserName, Password, userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpGet("Balance")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetBalance(int userId)
        {
            if (!_userRepository.userExists(userId))
                return NotFound();

            var users = _mapper.Map<User>(_userRepository.GetUser(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Balance: " + users.Balance);
        }


        [HttpPost("Withdraw")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Withdraw([FromQuery] int UserID,
           [FromQuery] decimal Amount)
        {

            if (!_userRepository.userExists(UserID))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var users = _mapper.Map<User>(_userRepository.GetUser(UserID));

            if (users.Balance < Amount)
            {
                return BadRequest("Insufficient balance.");
            }

            if (!_userRepository.Withdraw(UserID, Amount))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Withdraw");
        }

        [HttpPost("Deposit")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Deposit([FromQuery] int UserID,
          [FromQuery] decimal Amount)
        {

            if (!_userRepository.userExists(UserID))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_userRepository.Deposit(UserID, Amount))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Deposit");
        }

        [HttpPost("Transfer Money")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Transfer([FromQuery] int SenderUserID, [FromQuery] int ReceiverUserID, [FromQuery] decimal Amount)
        {
            var users = _mapper.Map<User>(_userRepository.GetUser(SenderUserID));

            if (users.Balance < Amount)
            {
                return BadRequest("Insufficient balance.");
            }

            if (!_userRepository.Transfer(SenderUserID, ReceiverUserID, Amount))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }
            return Ok("Transfer Successfully");
        }


    }
}
