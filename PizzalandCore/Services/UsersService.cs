using Grpc.Core;

namespace PizzalandCore.Services;

public class UsersService : UserService.UserServiceBase
{
    public override Task<ListUsersResponse> ListUsers(ListUsersRequest request, ServerCallContext context)
    {
        return base.ListUsers(request, context);
    }
    public override Task<UserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        return base.UpdateUser(request, context);
    }
    public override Task<UserResponse> RegisterUser(RegisterRequest request, ServerCallContext context)
    {
        return base.RegisterUser(request, context);
    }
    public override Task<UserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        return base.GetUser(request, context);
    }
    public override Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        return base.DeleteUser(request, context);
    }
}