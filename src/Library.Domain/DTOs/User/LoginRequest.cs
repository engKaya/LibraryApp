namespace Library.Domain.DTOs.User
{
    public record LoginRequest(
            string Email,
            string Password
        );
}
