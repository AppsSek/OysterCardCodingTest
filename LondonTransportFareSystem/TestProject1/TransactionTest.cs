

using LondonTransportFareSystem.BLL;
using LondonTransportFareSystem.DAL;
using LondonTransportFareSystem.Models;

namespace TestProject1
{
    [TestClass]
    public class TransactionTest
    {
        private Transaction _transaction;
        private void SetUp() 
        {
            _transaction = new Transaction(new CustomerTransactionDAL(new Microsoft.Data.SqlClient.SqlConnection()));
        }

        /// <summary>
        /// SwpipeIn And SwipeOut Tests
        /// 
        /// TopUp          ==>  INPUT :  customerId, amount              OUTPUT ==> Balance after update
        /// SwipeIn takes  ==>  INPUT :  customerId, modeID, stationID   OUTPUT ==> isAllowed
        /// SwipeOut takes ==>  INPUT :  customerId, modeID, stationID   OUTPUT ==> isSuccess
        /// ShowBalance    ==>  INPUT :  customerId                      OUTPUT ==> Current Balance 
        /// 
        /// 
        /// TEST DATA 
        /// MODE ==> (Tube-2 or Bus-1)
        /// STATION ==> (Holborn-1,Earl's Court - 2, Wimbledon - 3, Hammersmith - 4)
        /// 
        /// </summary>
        /// 
        [TestMethod]
        public void TestMethod_TestAll()
        {
            TestMethod_TopUpAccount_ValidID_ZeroOrPostiveExistingBalance_Successful();
            TestMethod_SwipeIn_ValidID_FirstTrip_ByTube_FromHolBorn();
            TestMethod_SwipeOut_ValidID_FirstTrip_ByTube_ToEarlsCourt();
            TestMethod_SwipeIn_ValidID_SecondTrip_ByBus_FromEarlsCourtToChelsea();
            TestMethod_SwipeIn_ValidID_ThirdTrip_ByTube_FromEalsCourt();
            TestMethod_SwipeOut_ValidID_ThirdTrip_ByTube_ToHammersmith();
            TestMethod_ShowBalance_Valid_Successful();
        }


        public void TestMethod_TopUpAccount_ValidID_ZeroOrPostiveExistingBalance_Successful()
        {
            SetUp();
            decimal balance = _transaction.TopUpAccount(Guid.Parse("4A5C7FB8-F1F1-4AEA-958F-2258EE9FCD6F"), 30);
            Assert.AreEqual(balance, 30);
        }

        public void TestMethod_SwipeIn_ValidID_FirstTrip_ByTube_FromHolBorn()
        {
            SetUp();
            bool isAllowed = _transaction.SwipeIn(Guid.Parse("4A5C7FB8-F1F1-4AEA-958F-2258EE9FCD6F"),2,1);
            Assert.AreEqual(true,isAllowed);
        }


        public void TestMethod_SwipeOut_ValidID_FirstTrip_ByTube_ToEarlsCourt()
        {
            SetUp();
            bool isAllowed = _transaction.SwipeOut(Guid.Parse("4A5C7FB8-F1F1-4AEA-958F-2258EE9FCD6F"), 2, 2);
            Assert.AreEqual(true, isAllowed);
        }


        public void TestMethod_SwipeIn_ValidID_SecondTrip_ByBus_FromEarlsCourtToChelsea()
        {
            SetUp();
            bool isAllowed = _transaction.SwipeIn(Guid.Parse("4A5C7FB8-F1F1-4AEA-958F-2258EE9FCD6F"), 1, null);
            Assert.AreEqual(true, isAllowed);
        }

        public void TestMethod_SwipeIn_ValidID_ThirdTrip_ByTube_FromEalsCourt()
        {
            SetUp();
            bool isAllowed = _transaction.SwipeIn(Guid.Parse("4A5C7FB8-F1F1-4AEA-958F-2258EE9FCD6F"), 2, 2);
            Assert.AreEqual(true, isAllowed);
        }


        public void TestMethod_SwipeOut_ValidID_ThirdTrip_ByTube_ToHammersmith()
        {
            SetUp();
            bool isAllowed = _transaction.SwipeOut(Guid.Parse("4A5C7FB8-F1F1-4AEA-958F-2258EE9FCD6F"), 2, 3);
            Assert.AreEqual(true, isAllowed);
        }


        public void TestMethod_ShowBalance_Valid_Successful()
        {
            SetUp();
            decimal balance = _transaction.ShowBalance(Guid.Parse("4A5C7FB8-F1F1-4AEA-958F-2258EE9FCD6F"));
            Assert.AreEqual(balance, Convert.ToDecimal(23.70));
        }


        /// <summary>
        /// TopupAccount Tests
        /// </summary>
        /*[TestMethod]
        public void TestMethod_TopUpAccount_EmptyCustomerID()
        {
            SetUp();
            Assert.ThrowsException<Exception>(() => _transaction.TopUpAccount(Guid.Empty, 30));
        }

        [TestMethod]
        public void TestMethod_TopUpAccount_InvalidAmount()
        {
            SetUp();
            Assert.ThrowsException<Exception>(() => _transaction.TopUpAccount(Guid.NewGuid(), 0));
        }

        [TestMethod]
        public void TestMethod_TopUpAccount_InvalidID_Unsuccessful()
        {
            SetUp();
            Assert.ThrowsException<InvalidCastException>(() => _transaction.TopUpAccount(Guid.Parse("4A5C7FB8-F1F1-4AEA-558F-2258EE9FCD6F"), 30));
        }

        [TestMethod]
        public void TestMethod_TopUpAccount_ValidID_ZeroOrPostiveExistingBalance_Successful()
        {
            SetUp();
            decimal balance = _transaction.TopUpAccount(Guid.Parse("4A5C7FB8-F1F1-4AEA-958F-2258EE9FCD6F"), 30);
            Assert.AreEqual(balance, 30);
        }

        [TestMethod]
        public void TestMethod_TopUpAccount_ValidID_NegativeExistingBalance_Successful()
        {
            SetUp();
            decimal balance = _transaction.TopUpAccount(Guid.Parse("461E630A-C338-42BC-B337-E51E1E7F826B"), 30);
            Assert.AreEqual(balance, Convert.ToDecimal(28.80));
        }*/


        /// <summary>
        /// ShowBalance Tests
        /// </summary>

        /*[TestMethod]
        public void TestMethod_ShowBalance_Valid_Successful()
        {
           SetUp();
           decimal balance = _transaction.ShowBalance(Guid.Parse("461E630A-C338-42BC-B337-E51E1E7F826B"));
           Assert.AreEqual(balance, Convert.ToDecimal(28.80));
        }

        [TestMethod]
        public void TestMethod_ShowBalance_ValidID_Unsuccessful()
        {
            SetUp();

        [TestMethod]
        public void TestMethod_ShowBalance_ValidID_Successful()
        {
            SetUp();
        }*/
    }
}