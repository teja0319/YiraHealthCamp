namespace YiraHealthCampManagerAPI.Models.Common.Enum
{
    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected ,
        Cancelled,
        Completed
    }

    public enum EnumRoles
        {
            SuperAdmin, OrganizationAdmin, User, Counsellor, SupportTeam, OrganizationUser, Hospital, Doctor, OrgnizationScout
        }
        public enum EnumGender
        {
            Male = 1, FeMale = 0, NG = -1
        }
        public enum EnumUserTypes
        {
            Admin, User, Guest, Normal, Hospital, Doctor, yirauser
        }
    public enum EnumGenderTypes
    {
        Male = 1, Female = 0, NG = -1
    }

}
