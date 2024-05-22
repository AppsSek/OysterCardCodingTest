using LondonTransportFareSystem.BLL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LondonTransportFareSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportController : ControllerBase
    {
        private readonly ProcessTransaction _transaction;

        public TransportController(ProcessTransaction transaction)
        {
            _transaction = transaction;
        }

        // GET: api/TransportController/GetCurrentBalance
        [HttpGet("GetCurrentBalance")]
        public async Task<decimal> GetCurrentBalance(Guid customerId)
        {
            return await _transaction.ShowBalance(customerId);
        }

        [HttpPost("TopUpAccount")]
        public async Task<decimal> TopUpAccount(Guid customerId, decimal amount)
        {
            return await _transaction.TopUpAccount(customerId, amount);
        }

        [HttpPost("SwipeIn")]
        public async Task<bool> SwipeIn(Guid customerId, int modeId, int zoneId)
        {
            return await _transaction.SwipeIn(customerId,modeId,zoneId);
        }

        [HttpPost("SwipeOut")]
        public async Task<bool> SwipeOut(Guid customerId, int modeId, int zoneId)
        {
            return await _transaction.SwipeOut(customerId, modeId, zoneId);
        }
    }
}
