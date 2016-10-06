using System.Collections.Generic;
using MergeTextLib;
using NUnit.Framework;

namespace MergeTextLibTests
{
    [TestFixture]
    class StrComparerTests
    {
        [Test]
        public void CompareIgnoreSpaceTest1()
        {
            // arraange
            var str1 = "  a ";
            var str2 = "   a";
            IComparer<string> c = new StrComparer();

            // act
            var res = c.Compare(str1, str2);

            //assert
            Assert.AreEqual(1, res, "Сравнение строк");
        }

        [Test]
        public void CompareIgnoreSpaceTest2()
        {
            // arraange
            var str1 = "  a ";
            var str2 = "   a";
            IComparer<string> c = new StrComparerIgnoreSpace();

            // act
            var res = c.Compare(str1, str2);

            //assert
            Assert.AreEqual(0, res, "Сравнение строк, игнорируя пробелы");
        }
    }
}
