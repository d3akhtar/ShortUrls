using AuthService.Data.Repository;
using AutoMapper;
using Azure;
using Grpc.Core;

namespace AuthService.SyncDataServices.Grpc
{
    public class GrpcUserService : GrpcUsers.GrpcUsersBase
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public GrpcUserService(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public override Task<UserResponse> GetAllUsers(GetAllUsersRequest getAllUsersRequest, ServerCallContext serverCallContext)
        {
            Console.WriteLine("--> Grpc call received, getting all users...");
            var response = new UserResponse();
            var users = _userManager.GetAllUsers();

            foreach (var user in users){
                response.Users.Add(_mapper.Map<GrpcUserModel>(user));
            }

            return Task.FromResult(response);
        }
    }
}