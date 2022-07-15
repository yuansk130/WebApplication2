using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Context.ApplicationUser;
using WebApplication1.Context.Auth;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IAuth
    {
        Task<ApplicationUsers> Register(ApplicationUsers user, string password);
        Task<bool> Login(string username, string password);
        Task<bool> CheckUser(string username);
        Task<ApplicationUsers> GetUserInfo(string username);
        Task<ApplicationUsers> UpdateUserInfo(EditUser user);
        Task<bool> checkPasswordlastFive(string username, string pass);
        Task<Guid?> GetGuidFromUsername(string username);
    }
    public class AuthRepository : IAuth
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AuthRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Login(string username, string password)
        {
            var user = await _context.ApplicationUser.FirstOrDefaultAsync(x => x.username == username && x.isActive == true);
            if (user == null)
                return false;
            if (PasswordHashV(password, user.PasswordHash, user.PasswordSalt) != true)
                return false;
            return true;
        }
        private bool PasswordHashV(string password, byte[] passHash, byte[] passSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passSalt))
            {
                var Hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int x = 0; x < Hash.Length; x++)
                {
                    if (Hash[x] != passHash[x])
                        return false;
                }
            }
            return true;
        }
        public async Task<bool> checkPasswordlastFive(string username ,string password)
        {
            Guid? userGuid = await GetGuidFromUsername(username);
            if (userGuid == null)
                return false;

            var User = _context.ApplicationUser.Where(x => x.UserGuid == userGuid).OrderByDescending(x => x.CreatedDate).Take(5);

            //User = User.Where(x => PasswordHashV(password, x.PasswordHash, x.PasswordSalt) == true);

            foreach(var x in User)
            {
                if (PasswordHashV(password, x.PasswordHash, x.PasswordSalt) == true)
                    return true;
            }
            return false;
        }
        public async Task<ApplicationUsers> Register(ApplicationUsers user,string password)
        {

            var key = new System.Security.Cryptography.HMACSHA512();
            if(user.UserGuid == Guid.Parse("00000000-0000-0000-0000-000000000000")) 
                user.UserGuid = Guid.NewGuid();
            user.PasswordSalt = key.Key;
            user.PasswordHash = key.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            user.CreatedDate = DateTime.Now;
            user.isActive = true;
            await _context.ApplicationUser.AddAsync(user);
            _context.SaveChanges();

            return user;
        }

        public async Task<bool> CheckUser(string username)
        {
            if (await _context.ApplicationUser.AnyAsync(x => x.username == username && x.isActive == true))
                return true;

            return false;
        }

        public async Task<ApplicationUsers?> GetUserInfo(string username)
        {
            ApplicationUsers user = await _context.ApplicationUser.Where(x => x.username == username && x.isActive == true).FirstOrDefaultAsync();
            return user != null ? user : null ;
        }
        public async Task<Guid?> GetGuidFromUsername(string username)
        {
            ApplicationUsers user = await _context.ApplicationUser.Where(x => x.username == username).FirstOrDefaultAsync();
            return user != null ? user.UserGuid : null;
        }
        public async Task<ApplicationUsers?> UpdateUserInfo(EditUser user)
        {
            ApplicationUsers userExit = await _context.ApplicationUser.Where(x => x.UserGuid == user.UserGuid && x.isActive == true).FirstOrDefaultAsync();

            if (userExit == null)
                return null;

            if (!String.IsNullOrEmpty(user.password))
            {
                userExit.isActive = false;
                await _context.SaveChangesAsync();

                string password = user.password;
                ApplicationUsers userInsert = _mapper.Map<ApplicationUsers>(user);
                userInsert.UserGuid = userExit.UserGuid;
                return await Register(userInsert, password);
            }
            else
            {
                userExit.username = user.username;
                userExit.lastname = user.lastName;
                userExit.firstname = user.firstName;
                await _context.SaveChangesAsync();
                return userExit;
            }

        }
    }
}
