using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public TypeUser TypeUser { get; set; }
        public bool ActiveCode { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int Code { get; set; } = 0;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; } 
    }
}
