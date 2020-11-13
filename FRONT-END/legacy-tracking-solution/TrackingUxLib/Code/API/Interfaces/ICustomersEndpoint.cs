using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingModels.Dtos;

namespace TrackingUxLib.Code.API.Interfaces
{
    public interface ICustomersEndpoint : IDisposable
    {
        Task<CustomerDto> GetAsync(int id);
        Task<List<CustomerDto>> GetLatestsAsync(int count = 20);
        Task CreateAsync(CustomerDto dto);
        Task UpdateAsync(CustomerDto dto);
        Task DeleteAsync(int id);
    }
}
