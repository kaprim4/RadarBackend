
using Domain.DTOs;
using Domain.Helper;

namespace Services.Auth
{
    public interface IUserService
    {
        Task<ResponseAuthModel<UserDTO>> Login(LoginDTO model);

        Task<ResponseModel<UserDTO>> Register(RegisterDTO model);

        Task<ResponseModel<UserDTO>> Update(UserDTO model);

        Task<ResponseModel<UserDTO>> List(PagableDTO<UserDTO> pagablev);
    }
}