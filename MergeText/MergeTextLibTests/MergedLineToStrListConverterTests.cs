using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MergeTextLib;
using NUnit.Framework;

namespace MergeTextLibTests
{
    [TestFixture]
    class MergedLineToStrListConverterTests
    {
        [Test]
        public void ConvertTest_ResultCount_RemovedWithoutAddedLines()
        {
            IMergedLine line = new MergedLine(
                "1", null, null, LineStatus.Removed, 
                new MergeConflict("fvn", "svn"));

            List<string> expected = new List<string>{};
            CoreConvertTest(line, expected);
        }

        [Test]
        public void ConvertTest_ResultCount_RemovedWithAddedLines()
        {
            IMergedLine line = new MergedLine(
                "1", null, null, LineStatus.Removed,
                new MergeConflict("fvn", "svn"));

            line.AddedLinesAfter = new List<string>{"2", "3"};

            List<string> expected = new List<string> {"2","3"};
            CoreConvertTest(line, expected);
        }

        [Test]
        public void ConvertTest_ResultCount_SurvivedWithAddedLines()
        {
            IMergedLine line = new MergedLine(
                "1", null, null, LineStatus.Survived,
                new MergeConflict("fvn", "svn"));

            line.AddedLinesAfter = new List<string> { "2", "3" };

            List<string> expected = new List<string> {"1", "2", "3" };
            CoreConvertTest(line, expected);
        }

        [Test]
        public void ConvertTest_ResultCount_SurvivedWithoutAddedLines()
        {
            IMergedLine line = new MergedLine(
                "1", null, null, LineStatus.Survived,
                new MergeConflict("fvn", "svn"));

            List<string> expected = new List<string> { "1"};
            CoreConvertTest(line, expected);
        }

        [Test]
        public void ConvertTest_ResultCount_FirstEmptyRowWithAddedLines()
        {
            IMergedLine line = new MergedLine(
                "1", null, null, LineStatus.FirstEmptyRow,
                new MergeConflict("fvn", "svn"));

            line.AddedLinesAfter = new List<string> { "2", "3" };

            List<string> expected = new List<string> { "2", "3" };
            CoreConvertTest(line, expected);
        }

        [Test]
        public void ConvertTest_ResultCount_SurvivedWithConflict()
        {
            IMergedLine line = new MergedLine(
                "1", null, null, LineStatus.Survived,
                new MergeConflict("fvn", "svn"));

            line.AddedLinesAfter = new List<string> { "2", "3" };

            line.IsHaveConflict = true;
            line.MergeConflict.FirstVariantLines = new List<string> { "b", "b" };
            line.MergeConflict.SecondVariantLines = new List<string> { "q", "q" };

            List<string> expected = new List<string>
            {
                "1",
                line.MergeConflict.GetFrameAbove(),
                "b", "b",
                line.MergeConflict.GetFrameBetween(),
                "q", "q",
                line.MergeConflict.GetFrameBelow(),
            };

            CoreConvertTest(line, expected);
        }

        public void CoreConvertTest(IMergedLine line, List<string> expected)
        {
            // arraange
            var mltslt = new MergedLineToStrListConverter();

            // act
            var result = mltslt.Convert(line);

            //assert
            Assert.AreEqual(expected.Count, result.Count, "Количество строк");
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i], result[i], "Ожидаемая строка");
            }
        }
    }
}
    