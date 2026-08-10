using System.ComponentModel.DataAnnotations;

namespace libraryModel.Entity
{
    public class Member_Class
    {
        [Key]
        public int member_regno { get; set; }

        public string? member_name { get; set; }

        public string? member_address { get; set; }

        public string? member_contactno { get; set; }

        public string? member_depositeamount { get; set; } 

        public string? member_refundablemoney { get; set; } 

        public string? member_monthlyfees{ get; set; } 

        public string? member_validity { get; set; } 

        public string? member_regdate { get; set; }
    }
}
