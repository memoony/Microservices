using FreeCourse.Services.Discount.Dtos;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<List<DiscountDto>>> GetAll();

        Task<Response<DiscountDto>> GetById(int id);

        Task<Response<DiscountDto>> GetByCodeAndUserId(string code, string userId);

        Task<Response<NoContent>> Save(DiscountDto discountDto);

        Task<Response<NoContent>> Update(DiscountDto discountDto);

        Task<Response<NoContent>> Delete(int id);
    }
}
