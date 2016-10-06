using System;
using System.Collections.Generic;
using MergeTextLib;
using Ninject;
using NUnit.Framework;

namespace MergeTextLibTests
{
    [TestFixture]
    class ChangedLinesCreatorTests
    {
        private static IKernel Kernel = new StandardKernel(new BaseNinjectModule());

        [Test]
        public void GetChangeOriginalLinesTest_Simple()
        {
            // arraange
            var origText = new List<string> {"0", "1", "2", "3", "4"};
            var changedText = new List<string> {"7", "8", "2", "3", "4"};
//            var survTextLines = new List<SurvivedTextLine>
//            {
//                new SurvivedTextLineLeaf("2", 2, 2),
//                new SurvivedTextLineLeaf("3", 3, 3),
//                new SurvivedTextLineLeaf("3", 3, 3),
//                new SurvivedTextLineLeaf("4", 4, 4),
//            };

            var expected = new List<IChangedLine>
            {
                new ChangedLine("", null, null, LineStatus.FirstEmptyRow){AddedLinesAfter = new List<string>{"7", "8"}},
                new ChangedLine("0", 0, null, LineStatus.Removed),
                new ChangedLine("1", 1, null, LineStatus.Removed),
                new ChangedLine("2", 2, 2, LineStatus.Survived),
                new ChangedLine("3", 3, 3, LineStatus.Survived),
                new ChangedLine("4", 4, 4, LineStatus.Survived),
            };

            IChangedLinesCreator colc = Kernel.Get<IChangedLinesCreator>();

            // act
            var result = colc.GetChangeOriginalLines(origText, changedText);

            //assert
            CoreCompare(expected, result);
        }


        [Test]
        public void GetChangeOriginalLinesTest_AddNewLinesAfterFirstEmptyRow()
        {
            // arraange
            var origText = new List<string> { "0", "1", "2", "3", "4" };
            var changedText = new List<string> { "7", "8", "0", "2", "3", "4" };
//            var survTextLines = new List<SurvivedTextLine>
//            {
//                new SurvivedTextLineLeaf("0", 0, 2),
//                new SurvivedTextLineLeaf("2", 2, 3),
//                new SurvivedTextLineLeaf("3", 3, 4),
//                new SurvivedTextLineLeaf("4", 4, 5),
//            };

            var expected = new List<IChangedLine>
            {
                new ChangedLine("", null, null, LineStatus.FirstEmptyRow){AddedLinesAfter = new List<string>{"7", "8"}},
                new ChangedLine("0", 0, 2, LineStatus.Survived),
                new ChangedLine("1", 1, null, LineStatus.Removed),
                new ChangedLine("2", 2, 3, LineStatus.Survived),
                new ChangedLine("3", 3, 4, LineStatus.Survived),
                new ChangedLine("4", 4, 5, LineStatus.Survived),
            };

            IChangedLinesCreator colc = Kernel.Get<IChangedLinesCreator>();

            // act
            var result = colc.GetChangeOriginalLines(origText, changedText);

            //assert
            CoreCompare(expected, result);
        }

        [Test]
        public void GetChangeOriginalLinesTest_AddNewLinesAfterLastRow()
        {
            // arraange
            var origText = new List<string> { "0", "1", "2", "3", "4" };
            var changedText = new List<string> { "0", "1", "2", "7","8" };
//            var survTextLines = new List<SurvivedTextLine>
//            {
//                new SurvivedTextLineLeaf("0", 0, 0),
//                new SurvivedTextLineLeaf("1", 1, 1),
//                new SurvivedTextLineLeaf("2", 2, 2),
//            };

            var expected = new List<IChangedLine>
            {
                new ChangedLine("", null, null, LineStatus.FirstEmptyRow),
                new ChangedLine("0", 0, 0, LineStatus.Survived),
                new ChangedLine("1", 1, 1, LineStatus.Survived),
                new ChangedLine("2", 2, 2, LineStatus.Survived){AddedLinesAfter = new List<string>{"7", "8"}},
                new ChangedLine("3", 3, null, LineStatus.Removed),
                new ChangedLine("4", 4, null, LineStatus.Removed)
            };

            IChangedLinesCreator colc = Kernel.Get<IChangedLinesCreator>();

            // act
            var result = colc.GetChangeOriginalLines(origText, changedText);

            //assert
            CoreCompare(expected, result);
        }

        [Test]
        public void GetChangeOriginalLinesTest_OrigIsEmpty()
        {
            // arraange
            var origText = new List<string> {};
            var changedText = new List<string> { "7", "8", "0", "2", "3", "4" };
//            var survTextLines = new List<SurvivedTextLine>{};

            var expected = new List<IChangedLine>
            {
                new ChangedLine("", null, null, LineStatus.FirstEmptyRow)
                {
                    AddedLinesAfter = new List<string>{"7", "8", "0", "2", "3", "4"}
                },
            };

            IChangedLinesCreator colc = Kernel.Get<IChangedLinesCreator>();

            // act
            var result = colc.GetChangeOriginalLines(origText, changedText);

            //assert
            CoreCompare(expected, result);
        }

        [Test]
        public void GetChangeOriginalLinesTest_ChangedTextIsEmpty()
        {
            // arraange
            var origText = new List<string> { "0", "1", "2", "3", "4" };
            var changedText = new List<string> {};
//            var survTextLines = new List<SurvivedTextLine>{};

            var expected = new List<IChangedLine>
            {
                new ChangedLine("", null, null, LineStatus.FirstEmptyRow),
                new ChangedLine("0", 0, null, LineStatus.Removed),
                new ChangedLine("1", 1, null, LineStatus.Removed),
                new ChangedLine("2", 2, null, LineStatus.Removed),
                new ChangedLine("3", 3, null, LineStatus.Removed),
                new ChangedLine("4", 4, null, LineStatus.Removed),
            };

            IChangedLinesCreator colc = Kernel.Get<IChangedLinesCreator>();

            // act
            var result = colc.GetChangeOriginalLines(origText, changedText);

            //assert
            CoreCompare(expected, result);
        }

        [Test]
        public void GetChangeOriginalLinesTest_ChangedTextIsTotalyNew()
        {
            // arraange
            var origText = new List<string> { "0", "1", "2", "3", "4" };
            var changedText = new List<string> { "7", "8", "9"};
//            var survTextLines = new List<SurvivedTextLine>{};

            var expected = new List<IChangedLine>
            {
                new ChangedLine("", null, null, LineStatus.FirstEmptyRow){AddedLinesAfter = new List<string>{"7", "8", "9"}},
                new ChangedLine("0", 0, null, LineStatus.Removed),
                new ChangedLine("1", 1, null, LineStatus.Removed),
                new ChangedLine("2", 2, null, LineStatus.Removed),
                new ChangedLine("3", 3, null, LineStatus.Removed),
                new ChangedLine("4", 4, null, LineStatus.Removed),
            };

            IChangedLinesCreator colc = Kernel.Get<IChangedLinesCreator>();

            // act
            var result = colc.GetChangeOriginalLines(origText, changedText);

            //assert
            CoreCompare(expected, result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChangeOriginalLinesTest_OrigTextIsNull()
        {
            // arraange
            List<String> origText = null;
            var changedText = new List<string> { "7", "8", "9" };
//            var survTextLines = new List<SurvivedTextLine> { };

            IChangedLinesCreator colc = Kernel.Get<IChangedLinesCreator>();
            var result = colc.GetChangeOriginalLines(origText, changedText);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChangeOriginalLinesTest_NewTextIsNull()
        {
            // arraange
            var origText = new List<string> { "0", "1", "2", "3", "4" };
            List<String> changedText = null;
//            var survTextLines = new List<SurvivedTextLine> { };

            IChangedLinesCreator colc = Kernel.Get<IChangedLinesCreator>();
            var result = colc.GetChangeOriginalLines(origText, changedText);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetChangeOriginalLinesTest_SurvivedTextIsNull()
        {
            // arraange
            var origText = new List<string> { "0", "1", "2", "3", "4" };
            var changedText = new List<string> { "7", "8", "9" };
//            List<SurvivedTextLine> survTextLines = null;

            IChangedLinesCreator colc = new ChangedLinesCreator(null);
        }

        private void CoreCompare(IList<IChangedLine> expected, IList<IChangedLine> result)
        {
            Assert.AreEqual(expected.Count, result.Count, "Количество строк в результате");
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i].Text, result[i].Text, 
                    String.Format("Текст в строке {0} ({1})", i, expected[i].Text));
                Assert.AreEqual(expected[i].Status, result[i].Status, 
                    String.Format("Статус в строке {0} ({1})", i, expected[i].Text));
                Assert.AreEqual(expected[i].OriginalLineIndex, result[i].OriginalLineIndex, 
                    String.Format("Индекс строки в исходном тексте {0}",i));
                Assert.AreEqual(expected[i].ChangedLineIndex, result[i].ChangedLineIndex, 
                    String.Format("Индекс строки в измененном тексте {0}",i));
                Assert.AreEqual(expected[i].AddedLinesAfter.Count, result[i].AddedLinesAfter.Count, 
                    String.Format("Количество вставленных строк {0}",i));

                for (int j = 0; j < result[i].AddedLinesAfter.Count; j++)
                {
                    Assert.AreEqual(expected[i].AddedLinesAfter[j], result[i].AddedLinesAfter[j], 
                        String.Format("Вставленная строка {0} - {1}", i, j));
                }
            }
        }
    }
}
