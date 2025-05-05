using Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Borrowals : BaseEntity
    {
        public required DateOnly BorrowalDate { get; set; }
        public required DateOnly DueDate { get; set; }
        public required BorrowalStatus BorrowalStatus { get; set; }


        // Foreign Keys
        
        public required int BookId { get; set; }
        public required Book Book { get; set; }


        // Foreign Key for AppUser in AspNetUsers Table
        [ForeignKey("AppUser")]
        public required string AppUserId { get; set; }

        public required AppUser AppUser { get; set; }


    }
}
