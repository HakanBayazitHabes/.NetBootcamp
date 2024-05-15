namespace API.Users.DTOs;

public record UserUpdateRequestDto(string Name, string Email, string Password, DateTime Age);
