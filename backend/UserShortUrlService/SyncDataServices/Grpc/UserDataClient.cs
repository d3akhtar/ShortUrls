using AuthService;
using AutoMapper;
using Grpc.Net.Client;
using UserShortUrlService.Model;

namespace UserShortUrlService.SyncDataServices.Grpc
{
    public class UserDataClient : IUserDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public IEnumerable<User> ReturnAllUsers()
        {
            var channel = GrpcChannel.ForAddress(_configuration["GrpcUserService"]);
            var client = new GrpcUsers.GrpcUsersClient(channel);

            UserResponse usersResponse = client.GetAllUsers(new GetAllUsersRequest());
            IEnumerable<User> users = _mapper.Map<IEnumerable<User>>(usersResponse.Users);

            return users;
        }
    }
}