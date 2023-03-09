using ASP.Net_Meeting_18_Identity.Data;
using ASP.Net_Meeting_18_Identity.Models.DTOs.UserDTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.Net_Meeting_18_Identity.Controllers
{
    [Authorize(Roles="admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public UserController(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var users = userManager.Users;
            //Use AutoMapper in Future!!!
            //IEnumerable<UserDTO> usersDTO = await users.Select(t => new UserDTO
            //{
            //    Id = t.Id,
            //    Login = t.UserName,
            //    Email = t.Email,
            //    YearOfBirth = t.YearOfBirth,
            //}).ToListAsync();

            IEnumerable<UserDTO> usersDTO = mapper.Map<IEnumerable<UserDTO>>(users);
            return View(usersDTO);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDTO dto)
        {

            if (ModelState.IsValid)
            {
                //User user = new User()
                //{
                //    Email = dto.Email,
                //    UserName = dto.Login,
                //    YearOfBirth = dto.YearOfBirth
                //};
                User user = mapper.Map<User>(dto);
                var result = await userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            //AutoMapper in future!!!
            //EditUserDTO dto = new EditUserDTO()
            //{
            //    Id = user.Id,
            //    Email = user.Email,
            //    Login = user.UserName,
            //    YearOfBirth = user.YearOfBirth
            //};
            EditUserDTO dto = mapper.Map<EditUserDTO>(user);
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(dto.Id);
                if (user == null) { return NotFound(); }
                user.UserName = dto.Login;
                user.YearOfBirth = dto.YearOfBirth;
                user.Email = dto.Email;
                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(dto);
        }
        public async Task<IActionResult> ChangePassword(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            //mapper
            //ChangePasswordDTO dto = new ChangePasswordDTO()
            //{
            //    Id = user.Id,
            //    Email = user.Email
            //};
            ChangePasswordDTO dto = mapper.Map<ChangePasswordDTO>(user);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(dto.Id);
                var passwaordValidator = HttpContext.RequestServices.GetRequiredService<IPasswordValidator<User>>();
                var passwaordHashier = HttpContext.RequestServices.GetRequiredService<IPasswordHasher<User>>();
                if (user == null)
                {
                    return NotFound();
                }
                var identityResult = await passwaordValidator.ValidateAsync(userManager, user, dto.NewPassword);
                if (identityResult.Succeeded)
                {
                    string hashedPassword = passwaordHashier.HashPassword(user, dto.NewPassword);
                    user.PasswordHash = hashedPassword;
                    await userManager.UpdateAsync(user);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            //mapper
            //DeleteUserDTO dto = new DeleteUserDTO()
            //{
            //    Id = user.Id,
            //    Email = user.Email,
            //    Login = user.UserName,
            //    YearOfBirth = user.YearOfBirth
            //};
            DeleteUserDTO dto = mapper.Map<DeleteUserDTO>(user);
            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (userManager.Users == null)
            {
                return NotFound();
            }

            User user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            /*IdentityResult result = */
            await userManager.DeleteAsync(user);
            //if (result.Succeeded)
            //{
            //    return RedirectToAction("Index");
            //}
            //else
            //{
            //    return View((object)id);
            //}
            return RedirectToAction("Index");

        }
    }
}
