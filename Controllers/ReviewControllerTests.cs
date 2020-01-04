using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using nailmvc2.Controllers;
using nailmvc2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;
using Assert = Xunit.Assert;

namespace nailmvc2.Controllers.Tests
{
    public class ReviewControllerTests
    {
        
        [Theory]
        //[MemberData(nameof(GetData))]
        [InlineData(606060, typeof(ViewResult))]
        [InlineData(112323, typeof(HttpNotFoundResult) )]
        public void IndexTest(int id, object expected)
        {
            //fake customer data
            var Customers = new List<CustomerInfomation>
            {
                new CustomerInfomation() { BusinessName = "merchant", BusinessID = "1234", CustomerID =606060 },

            }.AsQueryable();
            //fake merchant data
            var Merchant = new List<MerchantAccount>
            {
                new MerchantAccount() { BusinessName = "merchant", BusinessID = "1234"},

            }.AsQueryable();
            // this line below add data to Dbset to create fake Dbset<MerchantAccount> Object
            var dbsetFakeMerchant = new Mock<DbSet<MerchantAccount>>();
            dbsetFakeMerchant.As<IQueryable>().Setup(x => x.Provider).Returns(Merchant.Provider);
            dbsetFakeMerchant.As<IQueryable>().Setup(x => x.ElementType).Returns(Merchant.ElementType);
            dbsetFakeMerchant.As<IQueryable>().Setup(x => x.Expression).Returns(Merchant.Expression);
            dbsetFakeMerchant.As<IQueryable>().Setup(x => x.GetEnumerator()).Returns(Merchant.GetEnumerator());
            // this line below add data to Dbset to create fake Dbset<CustomerInfomation> Object
            var dbsetFakeCustomer = new Mock<DbSet<CustomerInfomation>>();
            dbsetFakeCustomer.As<IQueryable>().Setup(x => x.Provider).Returns(Customers.Provider);
            dbsetFakeCustomer.As<IQueryable>().Setup(x => x.ElementType).Returns(Customers.ElementType);
            dbsetFakeCustomer.As<IQueryable>().Setup(x => x.Expression).Returns(Customers.Expression);
            dbsetFakeCustomer.As<IQueryable>().Setup(x => x.GetEnumerator()).Returns(Customers.GetEnumerator());

            var mockSet = new Mock<Entities>();
            mockSet.Setup(s => s.CustomerInfomations).Returns(dbsetFakeCustomer.Object);
            mockSet.Setup(s => s.MerchantAccounts).Returns(dbsetFakeMerchant.Object);

            var R = new ReviewController(mockSet.Object);

            var Result = R.Index(id);

            Assert.Equal(expected, Result.GetType());
        }

    }
}