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
    class MergeTwoChangedAndOriginalLinesTests
    {
        [Test]
        public void MergeTest_StatusRemoved()
        {
            CoreMergeTest_Status(LineStatus.Survived, LineStatus.Removed, LineStatus.Removed);
        }

        [Test]
        public void MergeTest_StatusSurvived()
        {
            CoreMergeTest_Status(LineStatus.Survived, LineStatus.Survived, LineStatus.Survived);
        }

        [Test]
        public void MergeTest_StatusFirstEmptyRow()
        {
            CoreMergeTest_Status(LineStatus.Removed, LineStatus.FirstEmptyRow, LineStatus.FirstEmptyRow);
        }

        [Test]
        public void MergeTest_StatusNotSpecified()
        {
            CoreMergeTest_Status(LineStatus.NotSpecified, LineStatus.NotSpecified, LineStatus.Survived);
        }

        private void CoreMergeTest_Status(LineStatus status1, LineStatus status2, LineStatus expectedStatus)
        {
            // arraange
            IChangedLine changedLine1 = new ChangedLine("1", 1, 1, status1);
            IChangedLine changedLine2 = new ChangedLine("1", 1, 3, status2);
            LineStatus expected = expectedStatus;

            // act
            var result = new MergeTwoChangedAndOriginalLines(new StrComparer()).Merge(
                "1", changedLine1, changedLine2, "fvn", "svn").Status;

            //assert
            Assert.AreEqual(expected, result, "Статус результирующей строки");
        }

        [Test]
        public void MergeTest_IsHaveConflict_SameSublines()
        {
            CoreMergeTest_IsHaveConflict(
                new List<string> {"1", "2", "3"},
                new List<string> {"1", "2", "3"},
                new StrComparer(),
                false);
        }

        [Test]
        public void MergeTest_IsHaveConflict_OneEmplty()
        {
            CoreMergeTest_IsHaveConflict(
                new List<string> { "1", "2", "3" },
                new List<string> {},
                new StrComparer(),
                false);
        }

        [Test]
        public void MergeTest_IsHaveConflict_NotSame1()
        {
            CoreMergeTest_IsHaveConflict(
                new List<string> { "1", "2", "3" },
                new List<string> { "1", "2"},
                new StrComparer(),
                true);
        }

        [Test]
        public void MergeTest_IsHaveConflict_NotSame2()
        {
            CoreMergeTest_IsHaveConflict(
                new List<string> { "1", "2" },
                new List<string> { "1", "2", "3" },
                new StrComparer(),
                true);
        }

        [Test]
        public void MergeTest_IsHaveConflict_NotSame3()
        {
            CoreMergeTest_IsHaveConflict(
                new List<string> { "1", "2", "4" },
                new List<string> { "1", "2", "3" },
                new StrComparer(),
                true);
        }

        [Test]
        public void MergeTest_IsHaveConflict_SameSublinesIgnoreSpace()
        {
            CoreMergeTest_IsHaveConflict(
                new List<string> { "1 ", "2", "  3" },
                new List<string> { "1", "2", "3  " },
                new StrComparerIgnoreSpace() , 
                false);
        }

        [Test]
        public void MergeTest_IsHaveConflict_NotSameIgnoreSpace()
        {
            CoreMergeTest_IsHaveConflict(
                new List<string> { "1", "2 " },
                new List<string> { "1", "  2", "3" },
                new StrComparer(),
                true);
        }


        private void CoreMergeTest_IsHaveConflict(List<string> addedLinesAfter1, List<string> addedLinesAfter2,
            IComparer<string> comparer, bool expected)
        {
            // arraange
            IChangedLine changedLine1 = new ChangedLine("1", 1, 1, LineStatus.Survived)
            {
                AddedLinesAfter = addedLinesAfter1
            };
            IChangedLine changedLine2 = new ChangedLine("1", 1, 3, LineStatus.Survived)
            {
                AddedLinesAfter = addedLinesAfter2
            };

            // act
            var result = new MergeTwoChangedAndOriginalLines(comparer).Merge(
                "1", changedLine1, changedLine2, "fvn", "svn"
                ).IsHaveConflict;

            //assert
            Assert.AreEqual(expected, result, "Проверка конфликта в строках");
        }

    }
}
