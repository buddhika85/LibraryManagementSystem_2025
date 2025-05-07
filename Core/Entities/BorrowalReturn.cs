
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class BorrowalReturn : BaseEntity
    {
        public bool WasOverdue { get; set; }
        public bool WasPaid { get; set; }
        public decimal AmountAccepted { get; set; }
        public DateTime ReturnDate => DateTime.Now;


        // Foreign Key for Borrowal
        public required int BorrowalsId { get; set; }

        public required Borrowals Borrowals { get; set; }


        // Foreign Key for AppUser in AspNetUsers Table
        [ForeignKey("AppUser")]
        public required string AppUserId { get; set; }

        public required AppUser AppUser { get; set; }
    }
}
