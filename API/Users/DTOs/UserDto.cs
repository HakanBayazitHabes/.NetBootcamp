namespace API.Users.DTOs;

public record UserDto(int Id, string Name, string Email, string Password, int Age, string Created);

// public record UserDto
// {
//     public int Id { get; set; }
//     public string Name { get; set; }
//     public string Email { get; set; }
//     public string Password { get; set; }
//     public int Age { get; set; }

//     public DateTime Created { get; set; } = new();
// }
