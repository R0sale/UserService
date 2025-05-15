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

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(bool trackChanges)
        {
            var users = await _repository.GetAllUsers(trackChanges);

            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);

            return usersDTO;
        }

        public async Task<UserDTO> GetUserAsync(Guid id, bool trackChanges)
        {
            var user = await FindAndCheckIfExistsUser(id, trackChanges);

            var userDTO = _mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public async Task<UserDTO> CreateUser(UserForCreationDTO userForCreation)
        {
            var user = _mapper.Map<User>(userForCreation);

            _repository.CreateUser(user);
            await _repository.SaveAsync();

            var userDTO = _mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public async Task DeleteUser(Guid id, bool trackChanges)
        {
            var product = await FindAndCheckIfExistsUser(id, trackChanges);

            _repository.DeleteUser(product);
            await _repository.SaveAsync();
        }

        public async Task UpdateUser(Guid id, UserForUpdateDTO userForUpd, bool trackChanges)
        {
            var user = await FindAndCheckIfExistsUser(id, trackChanges);

            _mapper.Map(userForUpd, user);
            await _repository.SaveAsync();
        }

        public async Task<(UserForUpdateDTO userForUpd, User userEntity)> GetUserForPatialUpdate(Guid id, bool trackChanges)
        {
            var user = await FindAndCheckIfExistsUser(id, trackChanges);

            var userForUpd = _mapper.Map<UserForUpdateDTO>(user);

            return (userForUpd, user);
        }

        public async Task SaveChangesForPatrialUpdate(UserForUpdateDTO userForUpd, User user)
        {
            _mapper.Map(userForUpd, user);
            await _repository.SaveAsync();
        }

        private async Task<User> FindAndCheckIfExistsUser(Guid id, bool trackChanges)
        {
            var user = await _repository.GetUser(id, trackChanges);

            return user;
        }
    }
}
