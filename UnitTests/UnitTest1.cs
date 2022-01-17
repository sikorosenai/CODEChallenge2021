using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TipOfTheDay
{
    [TestClass]
    public class AppTest
    {
        [TestMethod]
        public void TestTipParsing()
        {
            var tip = DailyCodingLanguagesApp.LanguageOfTheDay.Parse(System.DateTime.Now, "Language\r\nTip\r\nQuestion\r\nAnswer\r\n");
            Assert.AreEqual(tip.Language, "Language");

        }
    }
}
