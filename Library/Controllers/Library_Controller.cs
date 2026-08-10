using libraryApplication.Model;
using libraryApplication.Repository;
using libraryInfra;
using libraryModel.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace libraryApi.Controllers
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
        [HttpGet("event-producing")]
        public async Task<IActionResult> Producing(CancellationToken cancellationToken)
        {
            await _repo.ProduceAsync(cancellationToken);

            return Ok("Event Sent!");
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
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtSettings["DurationInMinutes"])),

                    signingCredentials: creds
                );
                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return NotFound("token");

        }

        [HttpGet("userlist")]
        [Authorize]
        public IActionResult GetUserList()
        {
            try
            {
                var users = _repo.userlist();
                if(users.Result.Result != null)
                    return Ok(users.Result.Result);
                else
                    return NotFound(new { message = "No users found." });
            }
            catch (Exception ex)
            {
                 return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("update-book")]
        [Authorize]
        public async Task<IActionResult> UpdateDetail([FromBody] Book_Model request)
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


        [HttpGet("search-book")]
        [Authorize]
        public async Task<ActionResult<Book_Model>> SearchBook(string bookName)
        {
            try
            {
                var result =  await _repo.SearchBook(bookName);   
                if (result == null)
                {
                    return NotFound(new { message = "Book not found." });
                }
                else
                    {


                   return Ok(result.Result);
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }


        [HttpPost("insert-user")]
        [Authorize]
        public IActionResult insertUser([FromBody] Member_Model request)
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
        [Authorize]
        public IActionResult issueReceiveBook([FromBody] Book_IssueReturn_Model request)
        {
            try
            {
                if (request == null)
                {
                    return NotFound(new { message = "detail's not found" });
                }
              var result = _repo.issueReceiveBook(request);
                if(result != null)
                return Ok(new { message = "user sdded successfully." });
                else
                  return NotFound(new { message = "operation failed." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPost("Add_Book")]
        [Authorize]
        public async Task<IActionResult> AddBook(Book_Model request)
        {
            try
            {
               var result= await _repo.AddBook(request);
                if (result != null)
                {
                 return Ok(new { message = "Book record added successfully." });
                }
                else
                { 
                    return NotFound(new { message = "Book record not added successfully." });
                }
            }
              catch (Exception ex)
                {
                 return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
              }
        }

        [HttpGet("Book-details")]
        [Authorize]
        public async Task<ActionResult<List<Book_Model>>> bookdetails()
        {
            try
            {
                var users = await _repo.BookDetails();
                if (users != null)
                    return Ok(users.Result);
                else
                    return NotFound(new { message = "No books found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}


