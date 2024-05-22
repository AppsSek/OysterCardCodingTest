using FastMember;
using LondonTransportFareSystem.DAL;
using LondonTransportFareSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LondonTransportFareSystem.BLL
{
    public class Transaction
    {
        private readonly CustomerTransactionDAL _transacDAL;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public Transaction(CustomerTransactionDAL customerTransactionDAL)
        {
            _transacDAL = customerTransactionDAL;
        }
        public decimal TopUpAccount(Guid customerId, decimal amount)
        {
            decimal balance = 0;

            
            try
            {
                if (customerId == Guid.Empty || amount <= 0)
                {
                    throw new Exception("Invalid customer Id or amount!!");
                }
                balance = _transacDAL.TopUpAccount(customerId, amount);
            }
            //All Possible Exceptions like INVALID CAST EXCEPTION goes here - with LOGGING - To NOT lose the STACK Trace
            catch (Exception ex)
            {
                log.Error("Exception occured while topping up for customer id:" + customerId + " for amount " + amount);
                log.Error(ex);
                throw ex;
            }
            return balance;
        }

        public decimal ShowBalance(Guid customerId)
        {
            decimal balance = 0;
            try
            {
                if( customerId == Guid.Empty)
                {
                    throw new Exception("Invalid customer Id !!");
                }
                balance =  _transacDAL.GetCurrentBalance(customerId);
            }
            //All Possible Exceptions like INVALID CAST EXCEPTION goes here - with LOGGING - To NOT lose the STACK Trace
            catch (Exception ex)
            {
                log.Error("Exception occured while showing balance for customer id:" + customerId);
                log.Error(ex);
                throw ex;
            }
            return balance;
        }


        public bool SwipeIn(Guid customerId, int modeId, int? stationId)
        {
            bool isAllowed = false;
            try
            {
                if (customerId == Guid.Empty)
                {
                    throw new Exception("Invalid customer Id !!");
                }
                isAllowed = _transacDAL.SwipeIn(customerId, modeId, stationId);
            }
            //All Possible Exceptions like INVALID CAST EXCEPTION goes here - with LOGGING - To NOT lose the STACK Trace
            catch (Exception ex)
            {
                log.Error("Exception occured while swiping IN for customer id:" + customerId);
                log.Error(ex);
                throw ex;
            }
            return isAllowed;
        }

        public bool SwipeOut(Guid customerId, int modeId, int stationId)
        {
            bool isSuccess = false;
            try
            {
                if (customerId == Guid.Empty)
                {
                    throw new Exception("Invalid customer Id !!");
                }
                isSuccess = _transacDAL.SwipeOut(customerId, modeId, stationId);
            }
            //All Possible Exceptions like INVALID CAST EXCEPTION goes here - with LOGGING - To NOT lose the STACK Trace
            catch (Exception ex)
            {
                log.Error("Exception occured while swiping OUT for customer id:" + customerId);
                log.Error(ex);
                throw ex;
            }
            return isSuccess;
        }
    }
}
