using Microsoft.EntityFrameworkCore;
using YiraHealthCampManagerAPI.Context;
using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;
using YiraHealthCampManagerAPI.Models.CampRequest;
using YiraHealthCampManagerAPI.Models.Common.Enum;
using YiraHealthCampManagerAPI.Models.Request;
using YiraHealthCampManagerAPI.Models.Response;

namespace YiraHealthCampManagerAPI.Repositories
{
    public class HealthCampRepository : IHealthCampRepository
    {
        private readonly YiraDbContext _context;
        public HealthCampRepository(YiraDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAndUpdateHealthCampRequest(HealthCampRequestModel healthCampRequest)
        {
            try
            {
                HealthCampRequest requestEntity;

                if (healthCampRequest.Id > 0)
                {
                    requestEntity = await _context.HealthCampRequest.FirstOrDefaultAsync(h => h.Id == healthCampRequest.Id);

                    if (requestEntity != null)
                    {
                        requestEntity.CampDuration = healthCampRequest.CampDuration;
                        requestEntity.CampName = healthCampRequest.CampName;
                        requestEntity.EmployeesCount = healthCampRequest.EmployeesCount;
                        requestEntity.PreferredDate = healthCampRequest.PreferredDate;
                        requestEntity.SpecialMedicalRequirements = healthCampRequest.SpecialMedicalRequirements;
                        requestEntity.AvailableFacilities = healthCampRequest.AvailableFacilities;
                        requestEntity.AdditionalNote = healthCampRequest.AdditionalNote;
                        requestEntity.UpdatedAt = DateTime.UtcNow;

                        _context.HealthCampRequest.Update(requestEntity);
                        await _context.SaveChangesAsync();
                    }
                    else return false;
                }
                else
                {
                    requestEntity = new HealthCampRequest
                    {
                        CampDuration = healthCampRequest.CampDuration,
                        CampName = healthCampRequest.CampName,
                        EmployeesCount = healthCampRequest.EmployeesCount,
                        PreferredDate = healthCampRequest.PreferredDate,
                        OrgId = Convert.ToInt32(healthCampRequest.OrgId),
                        SpecialMedicalRequirements = healthCampRequest.SpecialMedicalRequirements,
                        AvailableFacilities = healthCampRequest.AvailableFacilities,
                        AdditionalNote = healthCampRequest.AdditionalNote,
                        CreatedBy = healthCampRequest.UserId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        ApprovalStatus = ApprovalStatus.Pending.ToString()
                    };

                    await _context.HealthCampRequest.AddAsync(requestEntity);
                    await _context.SaveChangesAsync();
                }

                if (healthCampRequest.ServiceRequest?.Any() == true)
                {
                    if(healthCampRequest.Id > 0)
                    {
                        var newServiceIds = healthCampRequest.ServiceRequest.Select(s => s.Id).ToHashSet();

                        var existingServices = await _context.RequestedService
                            .Where(rs => rs.HealthCampRequestId == requestEntity.Id)
                            .ToListAsync();

                        var existingServiceIds = existingServices.Select(s => s.Id).ToHashSet();

                        foreach (var es in existingServices)
                        {
                            if (!newServiceIds.Contains(es.Id))
                            {
                                es.Status = false;
                                es.UpdatedAt = DateTime.UtcNow;
                                es.UpdatedBy = healthCampRequest.UserId;
                            }
                        }

                        var servicesToAdd = newServiceIds
                            .Where(id => !existingServiceIds.Contains(id))
                            .Select(id => new RequestedService
                            {
                                HealthCampRequestId = requestEntity.Id,
                                ServiceId = id,
                                Status = true,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = healthCampRequest.UserId
                            }).ToList();

                        if (servicesToAdd.Any())
                        {
                            await _context.RequestedService.AddRangeAsync(servicesToAdd);
                        }

                    }
                    else
                    {
                        var servicesToAdd = healthCampRequest.ServiceRequest
                            .Select(s => new RequestedService
                            {
                                HealthCampRequestId = requestEntity.Id,
                                ServiceId = s.ServiceId,
                                Status = true,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = healthCampRequest.UserId
                            })
                            .ToList();

                        if (servicesToAdd.Any())
                        {
                            await _context.RequestedService.AddRangeAsync(servicesToAdd);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<HealthCampServiceRequestResponse>> ActiveHealthCampServices()
        {
            try
            {
                var activeServices = await _context.HealthService
                                     .Where(HealthService => HealthService.Status == true)
                                     .Select(healthServices => new HealthCampServiceRequestResponse
                                     {
                                         Id = healthServices.ServiceID,
                                         ServiceName = healthServices.ServiceName
                                     }).ToListAsync();
                return activeServices;

            }
            catch(Exception ex)
            {
                return null;
            }

        }



        public async Task<List<HealthCampResponseModel>> GetAllHealthCampRequestsByOrgId(int OrgId)
        {
            try
            {
                var healthCampRequests = await (from h in _context.HealthCampRequest
                                                join s in _context.RequestedService on h.Id equals s.HealthCampRequestId
                                                join healthService in _context.HealthService on s.ServiceId equals healthService.ServiceID into healthServices
                                                from healthService in healthServices.DefaultIfEmpty()
                                                where OrgId != 0 ? h.OrgId == OrgId && healthService.Status == true : healthService.Status == true
                                                select new
                                                {
                                                    Id = h.Id,
                                                    OrgId = h.OrgId,
                                                    CampName = h.CampName,
                                                    EmployeesCount = h.EmployeesCount,
                                                    CampDuration = h.CampDuration,
                                                    PreferredDate = h.PreferredDate,
                                                    SpecialMedicalRequirements = h.SpecialMedicalRequirements,
                                                    AvailableFacilities = h.AvailableFacilities,
                                                    AdditionalNote = h.AdditionalNote,
                                                    CreatedBy = h.CreatedBy,
                                                    CreatedAt = h.CreatedAt.ToString(),
                                                    UpdatedAt = h.UpdatedAt.ToString(),
                                                    ApprovalStatus = h.ApprovalStatus,
                                                    ServiceRequest = healthService.ServiceName
                                                }).GroupBy(h => h.Id).Select (g => new HealthCampResponseModel
                                                {
                                                    Id = g.Key,
                                                    OrgId = g.FirstOrDefault().OrgId,
                                                    CampName = g.FirstOrDefault().CampName,
                                                    EmployeesCount = g.FirstOrDefault().EmployeesCount,
                                                    CampDuration = g.FirstOrDefault().CampDuration,
                                                    PreferredDate = g.FirstOrDefault().PreferredDate,
                                                    SpecialMedicalRequirements = g.FirstOrDefault().SpecialMedicalRequirements,
                                                    AvailableFacilities = g.FirstOrDefault().AvailableFacilities,
                                                    AdditionalNote = g.FirstOrDefault().AdditionalNote,
                                                    CreatedBy = g.FirstOrDefault().CreatedBy,
                                                    CreatedAt = g.FirstOrDefault().CreatedAt,
                                                    UpdatedAt = g.FirstOrDefault().UpdatedAt,
                                                    ApprovalStatus = g.FirstOrDefault().ApprovalStatus,
                                                    ServiceRequest = g.Select(s => new HealthCampServiceRequestResponse
                                                    {
                                                            Id = s.Id,
                                                            ServiceName = s.ServiceRequest
                                                    }).ToList(),
                                                }).ToListAsync();

                return healthCampRequests;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HealthCampResponseModel> GetHealthCampRequestById(int id)
        {
            try
            {
                var healthCampRequest = await (from h in _context.HealthCampRequest
                                               join s in _context.RequestedService on h.Id equals s.HealthCampRequestId into services
                                               from service in services.DefaultIfEmpty()
                                               join healthService in _context.HealthService on service.ServiceId equals healthService.ServiceID into healthServices
                                               from healthService in healthServices.DefaultIfEmpty()
                                               where h.Id == id && healthService.Status == true && service.Status == true
                                               select new
                                               {
                                                   Id = h.Id,
                                                   OrgId = h.OrgId,
                                                   CampName = h.CampName,
                                                   EmployeesCount = h.EmployeesCount,
                                                   CampDuration = h.CampDuration,
                                                   PreferredDate = h.PreferredDate,
                                                   SpecialMedicalRequirements = h.SpecialMedicalRequirements,
                                                   AvailableFacilities = h.AvailableFacilities,
                                                   AdditionalNote = h.AdditionalNote,
                                                   CreatedBy = h.CreatedBy,
                                                   CreatedAt = h.CreatedAt.ToString(),
                                                   UpdatedAt = h.UpdatedAt.ToString(),
                                                   ApprovalStatus = h.ApprovalStatus,
                                                   ServiceRequest = healthService.ServiceName
                                               }).GroupBy(h => h.Id).Select(g => new HealthCampResponseModel
                                               {
                                                   Id = g.Key,
                                                   OrgId = g.FirstOrDefault().OrgId,
                                                   CampName = g.FirstOrDefault().CampName,
                                                   EmployeesCount = g.FirstOrDefault().EmployeesCount,
                                                   CampDuration = g.FirstOrDefault().CampDuration,
                                                   PreferredDate = g.FirstOrDefault().PreferredDate,
                                                   SpecialMedicalRequirements = g.FirstOrDefault().SpecialMedicalRequirements,
                                                   AvailableFacilities = g.FirstOrDefault().AvailableFacilities,
                                                   AdditionalNote = g.FirstOrDefault().AdditionalNote,
                                                   CreatedBy = g.FirstOrDefault().CreatedBy,
                                                   CreatedAt = g.FirstOrDefault().CreatedAt,
                                                   UpdatedAt = g.FirstOrDefault().UpdatedAt,
                                                   ApprovalStatus = g.FirstOrDefault().ApprovalStatus,
                                                   ServiceRequest = g.Select(s => new HealthCampServiceRequestResponse
                                                   {
                                                       Id = s.Id,
                                                       ServiceName = s.ServiceRequest
                                                   }).ToList(),
                                               }).FirstOrDefaultAsync();
                return healthCampRequest;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> HealthCampStatusUpdate(int camp , string ApprovalStatus)
        {
            try
            {
                ApprovalStatus status = (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), ApprovalStatus, true);
                var healthCampRequest = await _context.HealthCampRequest.FirstOrDefaultAsync(h => h.Id == camp);
                if (healthCampRequest != null)
                {
                    healthCampRequest.ApprovalStatus = status.ToString();
                    healthCampRequest.UpdatedAt = DateTime.UtcNow;
                    _context.HealthCampRequest.Update(healthCampRequest);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

        public async Task<List<HealthCampResponseModel>> HealthCampDataByStatusAndOrg(int OrgId , string ApprovalStatus)
        {
            try
            {
                ApprovalStatus status = (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), ApprovalStatus, true);

                var healthCampRequests = await (from h in _context.HealthCampRequest
                                                join s in _context.RequestedService on h.Id equals s.HealthCampRequestId
                                                join healthService in _context.HealthService on s.ServiceId equals healthService.ServiceID into healthServices
                                                from healthService in healthServices.DefaultIfEmpty()
                                                where healthService.Status == true && s.Status == true &&
                                                          (OrgId == 0 || h.OrgId == OrgId) &&
                                                          (status == null || h.ApprovalStatus == status.ToString())
                                                select new
                                                {
                                                    Id = h.Id,
                                                    OrgId = h.OrgId,
                                                    CampName = h.CampName,
                                                    EmployeesCount = h.EmployeesCount,
                                                    CampDuration = h.CampDuration,
                                                    PreferredDate = h.PreferredDate,
                                                    SpecialMedicalRequirements = h.SpecialMedicalRequirements,
                                                    AvailableFacilities = h.AvailableFacilities,
                                                    AdditionalNote = h.AdditionalNote,
                                                    CreatedBy = h.CreatedBy,
                                                    CreatedAt = h.CreatedAt.ToString(),
                                                    UpdatedAt = h.UpdatedAt.ToString(),
                                                    ApprovalStatus = h.ApprovalStatus,
                                                    ServiceRequest = healthService.ServiceName
                                                }).GroupBy(h => h.Id).Select(g => new HealthCampResponseModel
                                                {
                                                    Id = g.Key,
                                                    OrgId = g.FirstOrDefault().OrgId,
                                                    CampName = g.FirstOrDefault().CampName,
                                                    EmployeesCount = g.FirstOrDefault().EmployeesCount,
                                                    CampDuration = g.FirstOrDefault().CampDuration,
                                                    PreferredDate = g.FirstOrDefault().PreferredDate,
                                                    SpecialMedicalRequirements = g.FirstOrDefault().SpecialMedicalRequirements,
                                                    AvailableFacilities = g.FirstOrDefault().AvailableFacilities,
                                                    AdditionalNote = g.FirstOrDefault().AdditionalNote,
                                                    CreatedBy = g.FirstOrDefault().CreatedBy,
                                                    CreatedAt = g.FirstOrDefault().CreatedAt,
                                                    UpdatedAt = g.FirstOrDefault().UpdatedAt,
                                                    ApprovalStatus = g.FirstOrDefault().ApprovalStatus,
                                                    ServiceRequest = g.Select(s => new HealthCampServiceRequestResponse
                                                    {
                                                        Id = s.Id,
                                                        ServiceName = s.ServiceRequest
                                                    }).ToList(),
                                                }).ToListAsync();
                return healthCampRequests;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<DashboardStatsResponse> GetDashboardStatsAsync()
        {
            var now = DateTime.UtcNow;
            var firstDayThisMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayThisMonth.AddMonths(-1);

            // Total requests
            var thisMonthRequests = await _context.HealthCampRequest.CountAsync(r => r.CreatedAt >= firstDayThisMonth);
            var lastMonthRequests = await _context.HealthCampRequest.CountAsync(r => r.CreatedAt >= firstDayLastMonth && r.CreatedAt < firstDayThisMonth);
            var totalRequests = await _context.HealthCampRequest.CountAsync();
            var requestsChange = thisMonthRequests - lastMonthRequests;

            // Approval rate
            var thisMonthApproved = await _context.HealthCampRequest.CountAsync(r => r.ApprovalStatus == ApprovalStatus.Approved.ToString() && r.CreatedAt >= firstDayThisMonth);
            var lastMonthApproved = await _context.HealthCampRequest.CountAsync(r => r.ApprovalStatus == ApprovalStatus.Approved.ToString() && r.CreatedAt >= firstDayLastMonth && r.CreatedAt < firstDayThisMonth);
            var thisMonthRate = thisMonthRequests > 0 ? (double)thisMonthApproved / thisMonthRequests * 100 : 0;
            var lastMonthRate = lastMonthRequests > 0 ? (double)lastMonthApproved / lastMonthRequests * 100 : 0;
            var approvalRate = thisMonthRate;
            var approvalRateChange = thisMonthRate - lastMonthRate;

            // Avg processing time (in days)
            var thisMonthProcessingTimes = await _context.HealthCampRequest
                .Where(r => r.ApprovalStatus == ApprovalStatus.Approved.ToString() && r.CreatedAt >= firstDayThisMonth && r.CreatedAt != null && r.UpdatedAt != null)
                .Select(r => EF.Functions.DateDiffDay(r.CreatedAt, r.UpdatedAt))
                .ToListAsync();
            var lastMonthProcessingTimes = await _context.HealthCampRequest
                .Where(r => r.ApprovalStatus == ApprovalStatus.Approved.ToString() && r.CreatedAt >= firstDayLastMonth && r.CreatedAt < firstDayThisMonth && r.CreatedAt != null && r.UpdatedAt != null)
                .Select(r => EF.Functions.DateDiffDay(r.CreatedAt, r.UpdatedAt))
                .ToListAsync();
            var avgProcessingTime = thisMonthProcessingTimes.Any() ? thisMonthProcessingTimes.Average() : 0;
            var lastMonthAvg = lastMonthProcessingTimes.Any() ? lastMonthProcessingTimes.Average() : 0;
            var processingTimeChange = lastMonthAvg - avgProcessingTime;

            var mostRequestedServices = await _context.RequestedService
                .Where(rs => rs.Status && rs.CreatedAt >= firstDayThisMonth)
                .GroupBy(rs => rs.ServiceId)
                .Select(g => new
                {
                    ServiceId = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToListAsync();

            var serviceNames = await _context.HealthService
                .Where(s => mostRequestedServices.Select(m => m.ServiceId).Contains(s.ServiceID))
                .ToDictionaryAsync(s => s.ServiceID, s => s.ServiceName);

            var mostRequested = mostRequestedServices
                .Select(m => new RequestedServiceStat
                {
                    ServiceName = serviceNames.TryGetValue(m.ServiceId, out var name) ? name : "Unknown",
                    Count = m.Count
                })
                .ToList();

            return new DashboardStatsResponse
            {
                TotalRequests = thisMonthRequests,
                RequestsChange = requestsChange,
                ApprovalRate = Math.Round(approvalRate, 2),
                ApprovalRateChange = Math.Round(approvalRateChange, 2),
                AvgProcessingTime = Math.Round(avgProcessingTime, 1),
                ProcessingTimeChange = Math.Round(processingTimeChange, 1),
                MostRequestedServices = mostRequested
            };
        }


    }
}
