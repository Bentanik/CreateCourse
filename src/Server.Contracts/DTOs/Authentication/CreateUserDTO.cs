﻿namespace Server.Contracts.DTOs.Authentication;
public class CreateUserDTO
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
