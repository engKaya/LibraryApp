﻿namespace Library.Domain.DTOs.User
{
    public record CreateUserRequest(
            string FullName,
            string Email,
            string Password
        );
}
