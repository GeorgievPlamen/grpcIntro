using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PizzalandCore.Interfaces;
using PizzalandCore.Models;

namespace PizzalandCore.Services;

public class UsersService(IUserRepository userRepository) : UserService.UserServiceBase
{
    private readonly IUserRepository _userRepository = userRepository;

    public async override Task<UserResponse> RegisterUser(RegisterRequest request, ServerCallContext context)
    {
        var newUser = new User
        {
            DateRegistered = DateTime.UtcNow,
            Email = request.Email,
            Name = request.Name,
            Password = request.Password
        };

        var result = await _userRepository.AddUserAsync(newUser);

        var userResponse = MapToUserProto(result);

        return new UserResponse { User = userResponse };
    }
    public async override Task<UserResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var foundUser = await _userRepository.GetUserByEmailAsync(request.Email);

        if (foundUser!.Password != request.Password) return default!;

        var userResponse = MapToUserProto(foundUser!);

        return new UserResponse { User = userResponse };
    }
    public async override Task<UserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var newUser = new User
        {
            DateRegistered = DateTime.UtcNow,
            Email = request.Email,
            Name = request.Name,
            Password = request.Password
        };

        var result = await _userRepository.UpdateUserAsync(newUser);

        var userResponse = MapToUserProto(result!);

        return new UserResponse { User = userResponse };
    }
    public async override Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var result = await _userRepository.DeleteUserAsync(Guid.Parse(request.Id));

        var responseMsg =
            result ?
            $"Deleted user with id:{request.Id}." :
            $"Failed to delete or could not find user with id: {request.Id}.";

        return new DeleteUserResponse { Message = responseMsg };
    }
    public async override Task<UserResponse?> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var foundUser = await _userRepository.GetUserAsync(Guid.Parse(request.Id));
        if (foundUser is null) return null;
        var userResponse = MapToUserProto(foundUser);
        return new UserResponse { User = userResponse };
    }
    public async override Task<ListUsersResponse> ListUsers(ListUsersRequest request, ServerCallContext context)
    {
        var Users = await _userRepository.GetUsersAsync();

        var usersResponse = Users.Select(MapToUserProto).ToList();

        return new ListUsersResponse
        {
            Users = { usersResponse }
        };
    }

    private static UserProto MapToUserProto(User user) => new UserProto
    {
        Id = user.Id.ToString(),
        Name = user.Name,
        Email = user.Email,
        DateRegistered = Timestamp.FromDateTime(user.DateRegistered),
        IsActive = user.IsActive,
        Token = user.Token
    };
}