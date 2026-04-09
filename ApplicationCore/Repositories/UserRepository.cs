using ApplicationCore.Helpers;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApplicationCore.Repositories
{
    public class UserRepository(IConnectionProvider connectionProvider) : IUserRepository
    {
        private readonly IConnectionProvider _connectionProvider = connectionProvider;
        public async Task<User> GetUserAsync(LoginRequest request)
        {
            User user = new();
            await using var conn = await _connectionProvider.ConnectAsync();
            var result = await conn.QueryFirstOrDefaultAsync("spGetUserByUsername", new
            {
                username = request.Username
            },
            commandType: CommandType.StoredProcedure);
            if (result != null )
            {

                // TODO : Move to mapper
                user.guid = result.guid;
                user.FirstName = result.firstName;
                user.LastName = result.lastName;
                user.DateOfBirth = DateOnly.FromDateTime((DateTime)result.dateOfBirth);
                user.Email = result.email;
                user.Credentials = new LoginRequest { Username = result.username, Password = result.password };

                return user;
            }
            return result;
        }

        public async Task<RegisterResponse> CreateUserAsync(User user)
        {
            RegisterResponse resp = new();
            await using var conn = await _connectionProvider.ConnectAsync();
            try
            {
                var result = await conn.ExecuteAsync("spCreateUser", new
                {
                    guid = user.guid,
                    first_name = user.FirstName,
                    last_name = user.LastName,
                    date_of_birth = user.DateOfBirth.ToDateTime(TimeOnly.MinValue), //  Dapper cannot send a System.DateOnly value directly as a SQL parameter because ADO.NET expects ADO types (e.g. DateTime
                    email = user.Email,
                    role = user.Role,
                    username = user.Credentials.Username,
                    password = user.Credentials.Password
                }, commandType: CommandType.StoredProcedure);

                if(result == 1)
                {
                    resp.IsCreated = true;
                    resp.ErrorMessage = string.Empty;
                }
                return resp;
            }
            catch(SqlException ex)
            {
                resp.IsCreated = false;
                if(ex.Number == DbExceptions.EmailAlreadyExists)
                    resp.ErrorMessage = "Email already exists. Please try a different email address.";

                if (ex.Number == DbExceptions.UsernameAlreadyExists)
                    resp.ErrorMessage = "Username already exists. Please try a different username.";

                return resp;
            }
        }
    }
}
