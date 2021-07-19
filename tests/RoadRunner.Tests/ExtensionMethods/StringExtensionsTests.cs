using System;
using System.Text;
using Xunit;

namespace RoadRunner.Tests.ExtensionMethods
{
    public class StringExtensionsTests
    {
        [Fact]
        public void EmptyString_ShouldBeEmpty()
        {
            var value = "";
            Assert.True(value.IsNullOrEmpty());
        }
        
        [Fact]
        public void NullString_ShouldBeEmpty()
        {
            string value = null;
            Assert.True(value.IsNullOrEmpty());
        }
        
        [Fact]
        public void NotEmptyString_MustBecomeBytes()
        {
            string value = "test";
            byte[] byteValue = Encoding.UTF8.GetBytes(value);
            Assert.True(byteValue.Length == value.ToBytes().Length);
            Assert.Equal(byteValue, value.ToBytes());
        }
    }
}