namespace Library.Domain.DTOs
{
    public record CreateUserRequest(
            string FullName,
            string Email,
            string Password
        );
}
