using Microsoft.AspNetCore.Identity;

namespace WireCart.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
    }
}
