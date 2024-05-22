using FastMember;
using LondonTransportFareSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LondonTransportFareSystem.BLL
{
    public class ProcessTransaction
    {
        private readonly LondonTransportContext _context;

        public ProcessTransaction(LondonTransportContext context)
        {
            _context = context;
        }
        public async Task<decimal> TopUpAccount(Guid customerID, decimal amount)
        {
            if (_context.Customers.Any(x => x.ID == customerID))
            {
                Customer customer1 = _context.Customers.Where(x => x.ID == customerID).ToList()[0];
                customer1.Balance = customer1.Balance + amount;
                _context.Customers.Update(customer1);
                await _context.SaveChangesAsync();
                return customer1.Balance;
            }
            return 0;
        }

        public async Task<decimal> ShowBalance(Guid customerId) 
        {
            decimal balance = 0;
            if (_context.Customers.Any(x => x.ID == customerId))
            {
                Customer customer1 = _context.Customers.Where(x => x.ID == customerId).ToList()[0];
                if(customer1.Status == (int)Status.Active)
                {
                    balance = _context.CustomerTransactions.OrderByDescending(x => x.DateTime).ToList()[0].CurrentBalance;
                }
                else
                {
                    balance = customer1.Balance;
                }
                _context.Customers.Update(customer1);
                await _context.SaveChangesAsync();
            }
            return balance;
        }


        public async Task<bool> SwipeIn(Guid customerId, int modeId, int stationId)
        {
            if (_context.Customers.Any(x => x.ID == customerId))
            {
                Customer customer1 = _context.Customers.Where(x => x.ID == customerId).ToList()[0];
                Mode mode = _context.ModeMaxFares.Where(x => x.ID == modeId).Any() ? _context.Modes.Where(x => x.ID == modeId).ToList()[0]: new Mode();

                if( mode.Name == "Bus" )
                {
                    if (customer1.Balance < mode.MaxFare)
                        return false;
                    else
                        return true;
                }
                else
                {
                    if (customer1.Status == (int)Status.Inactive)
                    {
                        //Get the ZoneIDs for the station (IF there is more than one FOR EX: Earl's Court), get the one with MINIMUM Fare
                        List<int> zoneIds = _context.TubeZones.Where(x => x.StationID == stationId).Select(x => x.ZoneID).ToList<int>();
                        decimal minFare = _context.Zones.Where(x => zoneIds.Contains(x.ID)).Min(x => x.MinFare);
                        if(customer1.Balance < minFare)
                            return false;
                        else
                        {
                            customer1.Status = (int)Status.Active;
                            CustomerTransaction ct = new CustomerTransaction
                            {
                                DateTime = DateTime.Now,
                                CustomerID = customerId,
                                ModeID = modeId,
                                Decription = "IN: Station " + stationId + " by " + mode.Name,
                                Debit = mode.MaxFare,
                                CurrentBalance = customer1.Balance - mode.MaxFare
                            };
                            await _context.CustomerTransactions.AddAsync(ct);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                _context.Customers.Update(customer1);
            }
            return true;
        }

        public async Task<bool> SwipeOut(Guid customerId, int modeId, int stationId)
        {
            if (_context.Customers.Any(x => x.ID == customerId))
            {
                Customer customer1 = _context.Customers.Where(x => x.ID == customerId).ToList()[0];
                Mode mode = _context.ModeMaxFares.Where(x => x.ID == modeId).Any() ? _context.Modes.Where(x => x.ID == modeId).ToList()[0] : new Mode();
                CustomerTransaction ctLatest = _context.CustomerTransactions.Where(x => x.CustomerID == customerId).OrderByDescending(x => x.DateTime).ToList()[0];
                if (customer1.Status == (int)Status.Inactive)
                {
                    customer1.Balance = mode.MaxFare;
                    _context.Customers.Update(customer1);
                }
                else
                {
                    //Get the ZoneIDs for the station (IF there is more than one FOR EX: Earl's Court), get the one with MINIMUM Fare
                    //IDEALLY we must get the station ID from the transaction table, so we can get MIN fare for the CLOSEST Station
                    List<int> zoneIds = _context.TubeZones.Where(x => x.StationID == stationId).Select(x => x.ZoneID).ToList<int>();
                    decimal minFare = calculateFare(zoneIds);
                    if (customer1.Balance < minFare)
                        return false;
                    else
                    {
                        customer1.Status = (int)Status.Active;
                        CustomerTransaction ct = new CustomerTransaction
                        {
                            DateTime = DateTime.Now,
                            CustomerID = customerId,
                            ModeID = modeId,
                            Decription = "IN: Station " + stationId + " by " + mode.Name,
                            Debit = mode.MaxFare,
                            CurrentBalance = customer1.Balance - mode.MaxFare
                        };
                        await _context.CustomerTransactions.AddAsync(ct);
                        await _context.SaveChangesAsync();
                        customer1.Balance = minFare;
                        _context.Customers.Update(customer1);

                    }
                }
                _context.Customers.Update(customer1);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public decimal calculateFare(List<int> zoneIds)
        {
            string prop = "_" + (Math.Abs(zoneIds[0] - zoneIds[1]) + 1).ToString() + "Zone";
            Zone fare = _context.Zones.Where(x => x.ID == Math.Min(zoneIds[0], zoneIds[1])).ToList()[0];
            var typeAccessor = TypeAccessor.Create(fare.GetType());
            return (decimal)typeAccessor[fare, "prop"];
        }

    }
}
