namespace API.Users.DTOs;

public record UserCreateRequestDto(string Name, string Email, string Password, DateTime Age);
