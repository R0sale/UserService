using Contracts;
using Shared.DTOObjects;
using Shared.Request;
using AutoMapper;
using Entities.Models;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = (await _repository.GetAllUsers()).ToList();

            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users).ToList();

            for (int i = 0; i < users.Count; i++)
            {
                usersDTO[i].Roles = (ICollection<string>)await _repository.GetRoles(users[i]);
            }

            return usersDTO;
        }

        public async Task<UserDTO> GetUserAsync(Guid id)
        {
            var user = await FindAndCheckIfExistsUser(id);

            var userDTO = _mapper.Map<UserDTO>(user);

            userDTO.Roles = (ICollection<string>)await _repository.GetRoles(user);

            return userDTO;
        }

        public async Task DeleteUser(Guid id)
        {
            var product = await FindAndCheckIfExistsUser(id);

            _repository.DeleteUser(product);
        }

        public async Task UpdateUser(Guid id, UserForUpdateDTO userForUpd)
        {
            var user = await FindAndCheckIfExistsUser(id);

            _mapper.Map(userForUpd, user);
            await _repository.UpdateUser(user, userForUpd);
        }

        public async Task<(UserForUpdateDTO userForUpd, User userEntity)> GetUserForPatialUpdate(Guid id)
        {
            var user = await FindAndCheckIfExistsUser(id);

            var userForUpd = _mapper.Map<UserForUpdateDTO>(user);

            return (userForUpd, user);
        }

        public async Task PartiallyUpdateUser(User user, UserForUpdateDTO userForUpd)
        {
            _mapper.Map(userForUpd, user);

            await _repository.PartiallyUpdateUser(user, userForUpd);
        }

        private async Task<User> FindAndCheckIfExistsUser(Guid id)
        {
            var user = await _repository.GetUser(id);

            return user;
        }
    }
}
