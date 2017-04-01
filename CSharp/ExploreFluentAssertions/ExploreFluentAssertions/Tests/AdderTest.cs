using ExploreFluentAssertions.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using System.Threading.Tasks;

namespace ExploreFluentAssertions.Tests
{
    [TestClass]
    public class AdderTest
    {
        [TestMethod]
        public void TestThese()
        {
            Adder a = new Adder();
            //var result1 = a.AddMe(1, 2);
            //result1.Should().Be(3, "we are trying to add 1, 2");

            //var result2 = a.AddMe(1, -1);
            //result2.Should().Be(0, " we added 1, -1 and it will result in 0");

            var result3 = a.AddMe(1, 2);
            result3.Should().NotBe(-1, "we expect 3 as a result");
        }
    }
}
