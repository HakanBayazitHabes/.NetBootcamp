namespace Service.Users.DTOs;

public record UserNameUpdateRequestDto(int Id, string Name, string Email, string Password, DateTime Age);
