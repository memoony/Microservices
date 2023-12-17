using AutoMapper;
using Dapper;
using FreeCourse.Services.Discount.Dtos;
using FreeCourse.Shared.Dtos;
using Npgsql;
using System.Data;

namespace FreeCourse.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        private readonly IMapper _mapper;

        public DiscountService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));

            _mapper = mapper;
        }

        public async Task<Response<List<DiscountDto>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("SELECT * FROM discount");

            return Response<List<DiscountDto>>.Success(_mapper.Map<List<DiscountDto>>(discounts), 200);
        }

        public async Task<Response<DiscountDto>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("SELECT * FROM discount WHERE id = @Id", new { Id = id })).SingleOrDefault();

            if (discount  == null)
            {
                return Response<DiscountDto>.Fail("Discount not found!", 404);
            }

            return Response<DiscountDto>.Success(_mapper.Map<DiscountDto>(discount), 200);
        }

        public async Task<Response<DiscountDto>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("SELECT * FROM discount WHERE code = @Code AND userid = @UserId", new
            {
                Code = code,
                UserId = userId
            })).FirstOrDefault();

            if (discount == null)
            {
                return Response<DiscountDto>.Fail("Discount not found!", 404);
            }

            return Response<DiscountDto>.Success(_mapper.Map<DiscountDto>(discount), 200);
        }

        public async Task<Response<NoContent>> Save(DiscountDto discountDto)
        {
            int status = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid, rate, code) VALUES (@UserId, @Rate, @Code)", discountDto);

            if (status > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("An error occurred while adding!", 500);
        }

        public async Task<Response<NoContent>> Update(DiscountDto discountDto)
        {
            int status = await _dbConnection.ExecuteAsync("UPDATE discount SET userid = @UserId, rate = @Rate, code = @Code WHERE id = @Id", new
            {
                discountDto.Id,
                discountDto.UserId,
                discountDto.Rate,
                discountDto.Code
            });

            if (status > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("Discount not found!", 404);
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("DELETE FROM discount WHERE id = @Id", new
            {
                Id = id
            });

            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount not found!", 404);
        }
    }
}
