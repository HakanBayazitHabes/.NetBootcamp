namespace Service.Users.UserCreateUseCase;

public record UserCreateRequestDto(string Name, string Email, string Password, DateTime Age);
