using AgGateway.ADAPT.ApplicationDataModel.Representations;
using NUnit.Framework;

namespace AgGateway.ADAPT.AcceptanceTest
{
    public static class AssertValue
    {
        public static void VerifyNumericRepresentationValue(double expectedValue, string expectedUnit, string expectedRepresentation,
            NumericRepresentationValue actualValue)
        {
            Assert.AreEqual(expectedValue, actualValue.Value.Value);
            Assert.AreEqual(expectedUnit, actualValue.Value.UnitOfMeasure.Code);
            Assert.AreEqual(expectedRepresentation, actualValue.Representation.Code);
        }
    }
}
