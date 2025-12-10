using GymManagementSystem.Dtos;

namespace GymManagementSystem.Services;

public interface IRegisterService
{
    Task<string> CompleteInvitation(CompleteInvitationDto dto);
}