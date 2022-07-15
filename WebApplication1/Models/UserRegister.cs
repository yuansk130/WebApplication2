using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserRegister
    {
        public Guid UserGuid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
    public class EditUser
    {
        public Guid UserGuid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
