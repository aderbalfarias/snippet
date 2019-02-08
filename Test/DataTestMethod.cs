[DataRow("a", "b")]
[DataRow("x", "a")]
[DataTestMethod]
public void TestMethod1(string value1, string value2)
{
    Assert.AreEqual(value1 + value2, string.Concat(value1, value2));
}

// In a nutshell, you will need to install MSTest.TestFramework and MSTest.TestAdapter, and remove references 
// to Microsoft.VisualStudio.QualityTools.UnitTestFramework. You can then indicate a parameterised test with 
// the [DataTestMethod] attribute, and can add your parameters using the [DataRow] attribute, as per your example.
// The values from the [DataRow] attribute will be passed to the test method in the order in which they are specified.

// Note that the values in the [DataRow] attribute must be primitives, so you can't use a DateTime or decimal for example. 
// If you want them, you will have to work around this limitation (e.g. instead of having a DateTime parameter to represent a date, 
// you could have three integer parameters representing year, month and day, and create the DateTime within the test body).
