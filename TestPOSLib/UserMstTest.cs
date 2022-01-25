using PowerPOS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace TestPOSLib
{
    
    
    /// <summary>
    ///This is a test class for UserMstTest and is intended
    ///to contain all UserMstTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UserMstTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetGoogleCalendarURI
        ///</summary>
        [TestMethod()]
        public void GetGoogleCalendarURITest()
        {
            UserMst target = new UserMst(); // TODO: Initialize to an appropriate value
            Color DisplayedColor = new Color(); // TODO: Initialize to an appropriate value
            Uri expected = null; // TODO: Initialize to an appropriate value
            Uri actual;
            actual = target.GetGoogleCalendarURI(DisplayedColor);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for isAbleToGiveDiscount
        ///</summary>
        [TestMethod()]
        public void isAbleToGiveDiscountTest()
        {
            UserMst target = new UserMst(); // TODO: Initialize to an appropriate value
            Decimal PercentageToBeChecked = new Decimal(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.isAbleToGiveDiscount(PercentageToBeChecked);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
