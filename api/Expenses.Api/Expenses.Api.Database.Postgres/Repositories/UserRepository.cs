using Dapper;
using Expenses.Api.Database.Repositories;
using Expenses.Api.Model;
using Npgsql;

namespace Expenses.Api.Database.Postgres.Repositories;

internal sealed class UserRepository(NpgsqlConnection connection) : IUserRepository
{
    public async Task CreateAsync(UserInfo info)
    {
        const String sql = "INSERT INTO users (id, email, name, encrypted_google_token) VALUES (@Id, @Email, @Name, @EncryptedGoogleToken)";
        await connection.ExecuteAsync(sql, info);
    }

    public async Task UpdateAsync(Guid id, UserInfo info)
    {
        const String sql = "UPDATE users SET email = @Email, name = @Name, encrypted_google_token = @EncryptedGoogleToken WHERE id = @Id";
        await connection.ExecuteAsync(sql, info);   
    }

    public async Task<UserInfo?> FindAsync(Guid id)
    {
        const String sql = "SELECT * FROM users WHERE id = @Id";
        return await connection.QuerySingleOrDefaultAsync<UserInfo>(sql, new { Id = id });
    }

    public async Task<UserInfo?> FindByEmailAsync(String email)
    {
        const String sql = "SELECT * FROM users WHERE email = @Email";
        return await connection.QuerySingleOrDefaultAsync<UserInfo>(sql, new { Email = email });
    }
}