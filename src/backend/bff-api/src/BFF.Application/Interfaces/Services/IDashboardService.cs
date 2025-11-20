using BFF.Domain.DTOs;

namespace BFF.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardAdminDto> GetDashboardAdminAsync();
}