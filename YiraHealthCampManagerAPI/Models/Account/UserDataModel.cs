using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace YiraApi.Models.Authentication
{
    [Keyless]
    public class UserDataModel
    {
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public string UserName { get; set; }
        public string OrganizationName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneCode { get; set; }
        public string ProfileImage { get; set; }
        public string OrgId { get; set; }
        public string UserAge { get; set; }
        public string Gender { get; set; }
        public string UserBmi { get; set; }
        public string UserHeight { get; set; }
        public string UserWeight { get; set; }
        public string IsEmailVerified { get; set; }
        public string DateOfBirth { get; set; }
        public string UserAccessToken { get; set; }
        public string UserAccessTokenExpiration { get; set; }
        public List<ConfigUserData> ConfigUserData { get; set; }
    }

    public class ConfigUserData
    {
        public string OrgId { get; set; }
        public string UserRole { get; set; }
        public int Status { get; set; }
        public string DefaultConfig { get; set; }
    }

    [Keyless]
    public class UserData
    {
        public string Data { get; set; }
    }
    [Keyless]
    public class ThirdPartyTokenData
    {
        public string TokenDetails { get; set; }
    }
    [Keyless]
    public class Verifytokenstatus
    {
        public int Tokenstatus { get; set; }
    }
    [Keyless]
    public class GetLogoRes
    {
        public string OrgSpecific { get; set; }
    }
    [Keyless]
    public class UserHeightweight
    {
        public string UserHeightweightData { get; set; }
    }

    public class UserDetails
    {
        public Guid userId { get; set; }
        public string userName { get; set; }
        public int userAge { get; set; }
        public string userHeight { get; set; }
        public string userWeight { get; set; }
        public string userBMI { get; set; }
        public string gender { get; set; }
        public string temparature { get; set; }
        public string bloodPressure { get; set; }
        public string AdmissionNumber { get; set; }
    }
}
