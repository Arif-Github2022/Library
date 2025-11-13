using Library.Models;
using Library.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("libraryAPI")]
    [ApiController]
    public class Library_Controller : ControllerBase
    {
        private readonly Library_Context _db;
        private Repo _repo;
        private readonly IConfiguration _config;
        public Library_Controller(Library_Context db, Repo repo, IConfiguration config)
        {
            _config = config;
            _db = db;
            _repo = repo;
        }

        [HttpGet("gettoken")]
        public object GetToken(string username, string password)
        {
            if (_repo.ValidateUser(username, password))
            {
                var jwtSettings = _config.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"];
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtSettings["ExpiryMinutes"])),
                    signingCredentials: creds
                );
                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return NotFound("token");
        }

        [HttpGet("userlist")]
        public IActionResult GetUserList()
        {
            try
            {
                var users = _repo.userlist();
                return Ok(users);
            }
            catch (Exception ex)
            {
                 return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPost("update-detail")]
        public async Task<IActionResult> UpdateDetail([FromBody] Book_Class request)
        {
            try
            {
                if (request == null)
                {
                    return NotFound(new { message = "Book record not found." });
                }
                await _repo.UpdateBook(request.book_Id, request);
                return Ok(new { message = "Book record updated successfully." });     
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }




        [HttpPost("insert-user")]
        public IActionResult insertUser([FromBody] Member_Class request)
        {
            try
            {
                if (request == null)
                {
                    return NotFound(new { message = "User can't be null." });
                }
                _repo.Insertuser(request);
                return Ok(new { message = "user sdded successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }


        [HttpPost("issue-receive_book")]
        public IActionResult issueReceiveBook([FromBody] Book_IssueReturn_Class request)
        {
            try
            {
                if (request == null)
                {
                    return NotFound(new { message = "detail's not found" });
                }
                _repo.issueReceiveBook(request);
                return Ok(new { message = "user sdded successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPost("Add_Book")]
        public IActionResult AddBook([FromBody] Book_Class request)
        {
            try
            {
                if (request == null)
                {
                    return NotFound(new { message = "User can't be null." });
                }
              var x=  _repo.AddBook(request);
                //if(x.ExecuteResult  == true)
               return Ok(new { message = "Book record updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPost("Book-details")]
        public IActionResult bookdetails()
        {
            try
            {
                var users = _repo.bookdetails();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}