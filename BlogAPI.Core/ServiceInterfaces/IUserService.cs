namespace BlogAPI.Core.ServiceInterfaces;

public interface IUserService
{
    Task<bool> IsPhoneNumberAlreadyRegistered(string phoneNumber);
}

