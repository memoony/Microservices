using AutoMapper;
using FreeCourse.Services.Discount.Dtos;

namespace FreeCourse.Services.Discount.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Models.Discount, DiscountDto>().ReverseMap();
        }
    }
}
