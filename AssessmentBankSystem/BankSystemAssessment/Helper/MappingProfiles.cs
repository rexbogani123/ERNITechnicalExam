using AutoMapper;
using BankSystemAssessment.Dto;
using BankSystemAssessment.Model;

namespace BankSystemAssessment.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, User>();
            CreateMap<UserDto, User>();
        }
    }
}
