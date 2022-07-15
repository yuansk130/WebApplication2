using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Context.ApplicationUser
{
    public class ApplicationUsers
    {
        public int Id { get; set; }
        public Guid UserGuid { get; set; }
        [StringLength(255)]
        public string username { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        [StringLength(255)]
        public string firstname { get; set; }
        [StringLength(255)]
        public string lastname { get; set; }
        [DefaultValue(true)]
        public Boolean isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
