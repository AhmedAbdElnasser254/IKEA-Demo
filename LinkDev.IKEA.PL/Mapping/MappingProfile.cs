using AutoMapper;
using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.PL.ViewModels.Departments;

namespace LinkDev.IKEA.PL.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            #region Employee

            #endregion


            #region Department



            CreateMap<DepartmentDetailsDto, DepartmentEditViewModel>()
               /* .ForMember(dest => dest.Name, config => config.MapFrom(src => src.Name))*/;



            CreateMap<DepartmentEditViewModel, UpdatedDepartmentDto>();


            CreateMap<DepartmentEditViewModel, CreatedDepartmentDto>();




            #endregion


        }
    }
}
