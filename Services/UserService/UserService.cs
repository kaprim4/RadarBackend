using Domain.DTOs;
using Domain.Helper;
using Domain.Models;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserService
{
    public class UserService : AService, IUserService
    {
        public UserManager<User> _userManager { get; set; }
        public SignInManager<User> _signInManager { get; set; }
        public TokenService _tokenService { get; set; }
        public UserService(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService) : base(context) { 
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<ResponseAuthModel<UserDTO>> Login(LoginDTO model)
        {
            ResponseAuthModel<UserDTO> output = new(true);
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = _tokenService.GenerateToken(user);
                output.Token = token;
                output.IsAuthentificated = true;
            }
            else
            {
                output.AddErrorMessage("utilisateur introuvable");
            }
            return output;
        }

        public async Task<ResponseModel<UserDTO>> Register(RegisterDTO model)
        {
            ResponseModel<UserDTO> output = new(true);

            var user = new User { UserName = model.UserName, Email = model.Email, FullName = model.FullName, PhoneNumber = model.Phone };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                output.AddValidationErrors(result.Errors.Select(x=>x.Description).ToList());
            }

            return output;
        }

        public async Task<ResponseModel<UserDTO>> Update(UserDTO model)
        {
            ResponseModel<UserDTO> output = new(true);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                output.AddErrorMessage("utilisateur introuvable");
            }
            else
            {
                user.FullName = model.FullName;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    output.AddValidationErrors(result.Errors.Select(x => x.Description).ToList());

            }

            return output;
        }

        public async Task<ResponseModel<UserDTO>> List(PagableDTO<UserDTO> pagable)
        {
            var output = new ResponseModel<UserDTO>();
            try
            {
                var query = _context.Users.Select(x => new UserDTO()
                {
                    Email = x.Email,
                    FullName = x.FullName,
                    UserName = x.UserName,
                    Id = x.Id,
                }).AsQueryable();
                output.Pagable.TotalContent = await query.CountAsync();
                output.Pagable.Content = await query.Filter(pagable).ToListAsync();
            }
            catch (Exception ex)
            {
                output.AddErrorMessage(ex.ToString());
            }
            
            return output;
        }
    }
}
