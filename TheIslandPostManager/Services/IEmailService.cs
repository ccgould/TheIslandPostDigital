using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;

public interface IEmailService
{
    Task<bool> SendEmail(Order order);
}