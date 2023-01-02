namespace Televent.Core.Users.Models;

public static class RegistrationStates
{
    public const string Prefix = "/reg";
    public const string NameAndSurname = $"{Prefix}/name";
    public const string Age = $"{Prefix}/age";
    public const string Squad = $"{Prefix}/squad";
    public const string Building = $"{Prefix}/building";
    public const string Room = $"{Prefix}/room";
    public const string AdditionalInfo = $"{Prefix}/additional_info";
    public const string Finish = $"{Prefix}/finish";
}