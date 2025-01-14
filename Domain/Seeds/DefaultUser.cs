using Domain.Entities;
using Domain.Enums;

namespace Domain.Seeds
{
    public static class DefaultUser
    {
        public static List<ApplicationUser> IdentityBasicUserList()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    UserName = "AmrEbied",
                    Email = "admin@gmail.com",
                    TypeUser = TypeUser.Admin,
                    ActiveCode = true,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    Code=1234,
                    IsDeleted=false,
                    FirstName="Amr",
                    LastName="Ebied"
                },

            };
        }
    }
}
