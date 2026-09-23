using Microsoft.AspNetCore.Identity;

var hasher = new PasswordHasher<IdentityUser>();

Console.WriteLine("Admin:");
Console.WriteLine(hasher.HashPassword(null, "Admin1234!"));

Console.WriteLine("\nCompany:");
Console.WriteLine(hasher.HashPassword(null, "Company1234!"));

Console.WriteLine("\nStudent:");
Console.WriteLine(hasher.HashPassword(null, "Student1234!"));