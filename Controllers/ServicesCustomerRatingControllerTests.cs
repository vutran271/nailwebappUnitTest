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
using Xunit;
using Assert = Xunit.Assert;

namespace nailmvc2.Controllers.Tests
{
    public class SaveCustomerRatingControllerTests
    {
        [Theory]
        [InlineData(606060, "{ Result = save success }", "111111")]
        [InlineData(60346060, "{ Result = unsuccess }", "111111")]
        [InlineData(606060, "{ Result = unsuccess }", "")]

        public void SaveCustomerRatingControllerTest_query(long CId, string ExpectedValue, string BId)
        {
            // make a fake return data from sql

            var data = new List<CustomerInfomation> {
                new CustomerInfomation { BusinessID="111111", CustomerID=606060, BusinessName="merchant2",CustomerComment="bad comment b", CustomerStarRating = 1},
                new CustomerInfomation { BusinessID="111111", CustomerID=616060, BusinessName="merchant2",CustomerComment="bad comment", CustomerStarRating = 1}


            }.AsQueryable();


            var mockSet = new Mock<DbSet<CustomerInfomation>>();
            mockSet.As<IQueryable<CustomerInfomation>>().Setup(s => s.Provider)
                .Returns(data.Provider);
            mockSet.As<IQueryable<CustomerInfomation>>().Setup(s => s.Expression)
                .Returns(data.Expression);
            mockSet.As<IQueryable<CustomerInfomation>>().Setup(s => s.ElementType)
                .Returns(data.ElementType);
            mockSet.As<IQueryable<CustomerInfomation>>().Setup(s => s.GetEnumerator())
                .Returns(data.GetEnumerator());

            var mo = new Mock<Entities>(MockBehavior.Loose);
            mo.Setup(s => s.CustomerInfomations).Returns(mockSet.Object);
            mo.Setup(s => s.SaveChanges()).Returns(1);

            //test if customer exist - should return save success
            var fakeSRC = new SaveCustomerRatingController(mo.Object);
            var json = fakeSRC.Index(5, "Very good", CId, BId);

            Assert.Equal(ExpectedValue, json.Data.ToString());
        }

    }

}