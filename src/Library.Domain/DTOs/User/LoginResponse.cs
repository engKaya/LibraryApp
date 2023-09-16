namespace Library.Domain.DTOs.User
{
    public record LoginResponse (
            string Email,
            string Token,
            DateTime ExpireDate
        );
}
