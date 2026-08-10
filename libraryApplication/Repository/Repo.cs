using Azure.Core;
using Confluent.Kafka;
using libraryApplication.Model;
using libraryInfra;
using libraryModel.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
namespace libraryApplication.Repository
{
    public class Repo : ControllerBase
    {
        private readonly Library_Context _db;
        private readonly ILogger<Repo> _logger;
        public Repo(Library_Context db, ILogger<Repo>    logger)
        {
            _db = db;
            _logger = logger;
        }

        public ActionResult Insertuser(Member_Model Request)
        {
            try
            {
                Member_Class newMember = new Member_Class
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
                _db.Member_Class.Add(newMember);
                _db.SaveChanges();

                return Ok("record saved");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        public async Task<ActionResult> AddBook(Book_Model Request)
        {
            try
            {
                Book_Class book = new Book_Class
                {
                    book_name = Request.book_name,
                    book_author = Request.book_author,
                    book_description = Request.book_description,
                    book_category = Request.book_category,
                    book_publisher = Request.book_publisher,
                    book_publishYear = Request.book_publishYear,
                    book_type = Request.book_type,
                    book_price = Request.book_price,
                    book_language = Request.book_language,
                    book_pages = Request.book_pages,
                    book_title = Request.book_title,
                    inserteddate = DateTime.Now,
                    updateddate = DateTime.Now,
                };
                _db.Book_Class.Add(book);
                await _db.SaveChangesAsync();
                return Ok("record saved");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding book.");
                return BadRequest("Error: " + ex.Message);
            }
        }

        public ActionResult issueReceiveBook(Book_IssueReturn_Model Request)
        {
            try
            {

                Book_IssueReturn_Class book = new Book_IssueReturn_Class
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
                _db.book_IssueReturn_Class.Add(book);
                _db.SaveChanges();

                return Ok("record saved");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        public async Task<IActionResult> UpdateBook(int reqId, Book_Model request)
        {
            try
            {
                var result = await _db.Book_Class.FirstOrDefaultAsync(p => p.book_Id == reqId);
                if (result == null)
                {
                    return NotFound(new { message = "Book record not found." });
                }
                Book_Class book = new Book_Class
                {
                    book_description = request.book_description,
                    book_author = request.book_author,
                    book_publisher = request.book_publisher,
                    book_title = request.book_title,
                    book_category = request.book_category,
                    book_name = request.book_name,
                    book_pages = request.book_pages,
                    book_price = request.book_price,
                    book_type = request.book_type,
                    book_language = request.book_language,
                    updateddate = DateTime.Now,
                };
                await _db.SaveChangesAsync();
                return Ok(new { message = "Book record updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while updating the record", details = ex.Message });
            }
        }

        public async Task<ActionResult<Book_Model>> SearchBook(string bookName)
        {
            try
            {
                var book = await _db.Book_Class.FirstOrDefaultAsync(b => b.book_name == bookName);

                if (book == null)
                {
                    return new NotFoundResult();
                }

                var bookModel = new Book_Model
                {
                    book_description = book.book_description,
                    book_author = book.book_author,
                    book_publisher = book.book_publisher,
                    book_title = book.book_title,
                    book_category = book.book_category,
                    book_name = book.book_name,
                    book_pages = book.book_pages,
                    book_price = book.book_price,
                    book_type = book.book_type,
                    book_language = book.book_language,
                    updateddate = DateTime.Now
                };

                return new OkObjectResult(bookModel);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public bool ValidateUser(string userName, string Password)
        {
            return _db.Library_User.Any(x => x.username == userName && x.password == Password);
        }

        public async Task<ActionResult<List<Library_User_Model>>> userlist()
        {
           try
            {
               var books = await _db.Library_User.ToListAsync();
                var bookModels = books.Select(book => new Library_User_Model
                {
                    userId = book.userId,
                    username = book.username,
                    password = book.password,
                 }).ToList();
                return Ok(bookModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error occurred while retrieving the record.",
                    details = ex.Message
                });
            }
        }
        public async Task<ActionResult<List<Book_Model>>> BookDetails()
        {
            var books = await _db.Book_Class.ToListAsync();

            return Ok(books);
        }
        public async Task ProduceAsync(CancellationToken cancellationToken)
        {
            var config = new Confluent.Kafka.ProducerConfig { BootstrapServers = "localhost:9092", AllowAutoCreateTopics = true, Acks = Acks.All };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {

                //Console.WriteLine("Enter message: ");
                //DateTime x = DateTime.UtcNow ;//$"Hello, Kafka this is my first kafka testting with hamza! {DateTime.UtcNow}";
                //while (x.TimeOfDay.Minutes % 2 == 0)
                //{
                   var deliveryResult = await producer.ProduceAsync(topic: "test-topic",
                   new Message<Null, string>
                   {
                       Value = $"$\"Hello, Kafka this is my first kafka testting with hamza! {{DateTime.UtcNow}}\";"
                   },
                   cancellationToken);
                    _logger.LogInformation($"Delivered message to {deliveryResult.Value}, Offset: {deliveryResult.Offset}");
                }
          //  }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed: {e.Error.Reason}");
            }

            producer.Flush(cancellationToken);
        }
    }
}
    
