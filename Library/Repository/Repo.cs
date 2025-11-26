using eTrack.Models;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Library.Repository
{
    public class Repo:ControllerBase
    {
        private readonly Library_Context _db;
        public Repo(Library_Context db)
        {
            _db = db;
        }

        public ActionResult Insertuser(Member_Class Request)
        {
            try
            {
                    var newMember = new Member_Class
                    {
                       member_name = Request.member_name,
                        member_address = Request.member_address,
                        member_contactno = Request.member_contactno,
                        member_depositeamount = Request.member_depositeamount,
                        member_refundablemoney = Request.member_refundablemoney,
                        member_monthlyfees = Request.member_monthlyfees,
                        member_validity = Request.member_validity,
                        member_regdate = DateTime.Now.ToString(),
                    };
                    _db.member_Class.Add(newMember);
                    _db.SaveChanges();
                
                return Ok("record saved");
            }
            catch (Exception ex)
            {
              return BadRequest("Error: " + ex.Message);
            }
        }


        public async Task<ActionResult> AddBook()
        {
            try
            {
 
                    var newMember = new Book_Class
                    {
                       book_name = "Request.book_name",
                        book_author = "Request.book_author",
                        book_description = "Request.book_description",
                        book_category = "Request.book_category",
                        book_publisher = "Request.book_publisher",
                        book_publishYear = "Request.book_publishYear",
                        book_type = "Request.book_type",
                        book_price = "Request.book_price",
                        book_language = "Request.book_language",
                        book_pages = "Request.book_pages",
                        book_title = "Request.book_title",
                        inserteddate = DateTime.Now,
                        updateddate = DateTime.Now,
                    };
                    _db.book_Class.Add(newMember);                   
                    await _db.SaveChangesAsync();
                   return Ok("record saved");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        public ActionResult issueReceiveBook(Book_IssueReturn_Class Request)
        {
            try
            {
                   var newMember = new Book_IssueReturn_Class
                    {
                        book_Id = Request.book_Id,
                        userId = Request.userId,
                        issuedby = Request.issuedby,
                        isuuedate = Request.isuuedate,
                        receivedby = Request.receivedby,
                        returndate = Request.returndate,                  
                        inserteddate = DateTime.Now,
                        updateddate = DateTime.Now             
                    };
                    _db.book_IssueReturn_Class.Add(newMember);
                    _db.SaveChanges();
                
                return Ok("record saved");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }


        [HttpPut("update-book/{reqId}")]
        public async Task<IActionResult> UpdateBook(int reqId, Book_Class request)
        {
              try
                {
                    var result = await _db.book_Class.FirstOrDefaultAsync(p => p.book_Id == reqId);
                    if (result == null)
                    {
                        return NotFound(new { message = "Book record not found." });
                    }

                    result.book_description = request.book_description;
                    result.book_author      = request.book_author;
                    result.book_publisher = request.book_publisher;
                    result.book_title = request.book_title;
                    result.book_category = request.book_category;
                    result.book_name = request.book_name;
                    result.book_pages = request.book_pages;
                    result.book_price = request.book_price;
                    result.book_type = request.book_type;
                    result.book_language= request.book_language;
                    result.updateddate = DateTime.Now;
                    await _db.SaveChangesAsync();

                    return Ok(new { message = "Book record updated successfully." });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Error occurred while updating the record", details = ex.Message });
                }
            }


        [HttpGet("search-book/{bookName}")]
        public  ActionResult<List<Book_Class>> SearchBook(string bookName)
        {

            try
            {
                return _db.book_Class.Where(p => p.book_name == bookName).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while retrieving the record.", details = ex.Message });
            }
          
        }


        public bool ValidateUser(string userName, string Password)
        {
            return _db.user_Class.Any(x => x.username == userName && x.password == Password);
        }          

        [HttpGet]
        public ActionResult<List<Library_User>> userlist()
        {
            return _db.user_Class.ToList();           
        }


        [HttpGet]
        public ActionResult<List<Book_Class>> bookdetails()
        {
            return _db.book_Class.ToList();
        }
        
    }
}

