using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MergeTextLib;
using Ninject;
using Ninject.Modules;
using NUnit.Framework;

namespace MergeTextLibTests
{
    [TestFixture]
    class MergedLinesCreatorTests
    {
        private static IKernel Kernel = new StandardKernel(new BaseNinjectModule());

        [Test]
        public void GetMergeResult_Simple_1()
        {
            List<string> origText = new List<string> {"0","1","2"};

            List<IChangedLine> firstVariantChange = new List<IChangedLine>
            {
                new ChangedLine("0", 0, 0, LineStatus.Survived),
                new ChangedLine("1", 1, 1, LineStatus.Survived),
                new ChangedLine("2", 2, 2, LineStatus.Survived),
            }; 

            List<IChangedLine> secondVariantChange = new List<IChangedLine>
            {
                new ChangedLine("0", 0, 0, LineStatus.Survived),
                new ChangedLine("1", 1, 1, LineStatus.Survived),
                new ChangedLine("2", 2, 2, LineStatus.Survived){AddedLinesAfter = {"7", "8"}},
            };

            List<IMergedLine> expected = new List<IMergedLine>
            {
                new MergedLine("0", 0, 0, LineStatus.Survived, new MergeConflict("q", "q")),
                new MergedLine("1", 1, 1, LineStatus.Survived, new MergeConflict("q", "q")),
                new MergedLine("2", 2, 2, LineStatus.Survived, 
                    new MergeConflict("q", "q"){SecondVariantLines = new List<string>{"7", "8"}})
                    {AddedLinesAfter = {"7", "8"}},
            };
            CoreGetMergeResult(origText, firstVariantChange, secondVariantChange, expected);
        }

        [Test]
        public void GetMergeResult_Simple_2()
        {
            List<string> origText = new List<string> { "0", "1", "2" };

            List<IChangedLine> firstVariantChange = new List<IChangedLine>
            {
                new ChangedLine("0", 0, 0, LineStatus.Survived),
                new ChangedLine("1", 1, 1, LineStatus.Removed),
                new ChangedLine("2", 2, 2, LineStatus.Removed),
            };

            List<IChangedLine> secondVariantChange = new List<IChangedLine>
            {
                new ChangedLine("0", 0, 0, LineStatus.Survived),
                new ChangedLine("1", 1, 1, LineStatus.Survived),
                new ChangedLine("2", 2, 2, LineStatus.Survived){AddedLinesAfter = {"7", "8"}},
            };

            List<IMergedLine> expected = new List<IMergedLine>
            {
                new MergedLine("0", 0, 0, LineStatus.Survived, new MergeConflict("q", "q")),
                new MergedLine("1", 1, 1, LineStatus.Removed, new MergeConflict("q", "q")),
                new MergedLine("2", 2, 2, LineStatus.Removed, 
                    new MergeConflict("q", "q"){SecondVariantLines = new List<string>{"7", "8"}})
                    {AddedLinesAfter = {"7", "8"}},
            };
            CoreGetMergeResult(origText, firstVariantChange, secondVariantChange, expected);
        }

        [Test]
        public void GetMergeResult_IsConflict1()
        {
            List<string> origText = new List<string> { "0", "1", "2" };

            List<IChangedLine> firstVariantChange = new List<IChangedLine>
            {
                new ChangedLine("0", 0, 0, LineStatus.Survived),
                new ChangedLine("1", 1, 1, LineStatus.Removed),
                new ChangedLine("2", 2, 2, LineStatus.Removed){AddedLinesAfter = {"8", "8"}},
            };

            List<IChangedLine> secondVariantChange = new List<IChangedLine>
            {
                new ChangedLine("0", 0, 0, LineStatus.Survived),
                new ChangedLine("1", 1, 1, LineStatus.Survived),
                new ChangedLine("2", 2, 2, LineStatus.Survived){AddedLinesAfter = {"9", "9"}},
            };

            List<IMergedLine> expected = new List<IMergedLine>
            {
                new MergedLine("0", 0, 0, LineStatus.Survived, new MergeConflict("q", "q")),
                new MergedLine("1", 1, 1, LineStatus.Removed, new MergeConflict("q", "q")),
                new MergedLine("2", 2, 2, LineStatus.Removed, 
                    new MergeConflict("q", "q")
                    {
                        FirstVariantLines = new List<string>{"8", "8"},
                        SecondVariantLines = new List<string>{"9", "9"}
                    })
                {
                    IsHaveConflict = true,
                },
            };
            CoreGetMergeResult(origText, firstVariantChange, secondVariantChange, expected);
        }

        public void CoreGetMergeResult(
            List<string> origText, 
            List<IChangedLine> firstVariantChange, 
            List<IChangedLine> secondVariantChange, 
            List<IMergedLine> expected)
        {
            // arraange
            IMergedLinesCreator mtl = Kernel.Get<IMergedLinesCreator>();

            // act
            var result = mtl.GetMergeResult(
                origText,
                firstVariantChange,
                secondVariantChange, 
                "fvn", "svn");

            //assert
            Assert.AreEqual(expected.Count, result.Count, "Количество строк в результате");
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i].IsHaveConflict, result[i].IsHaveConflict, "Конфликт строк");
                Assert.AreEqual(expected[i].OriginalLineIndex, result[i].OriginalLineIndex, "Оригинальный индекс");
                Assert.AreEqual(expected[i].MergeConflict.FirstVariantLines.Count, 
                    result[i].MergeConflict.FirstVariantLines.Count, "Кол-во конфликтующих строк в первом варианте");
                Assert.AreEqual(expected[i].MergeConflict.SecondVariantLines.Count,
                    result[i].MergeConflict.SecondVariantLines.Count, "Кол-во конфликтующих строк во втором варианте");

                for (int j = 0; j < result[i].MergeConflict.FirstVariantLines.Count; j++)
                {
                    Assert.AreEqual(expected[i].MergeConflict.FirstVariantLines[j],
                        result[i].MergeConflict.FirstVariantLines[j], "Первая строка конфликтов");
                }

                for (int j = 0; j < result[i].MergeConflict.SecondVariantLines.Count; j++)
                {
                    Assert.AreEqual(expected[i].MergeConflict.SecondVariantLines[j],
                        result[i].MergeConflict.SecondVariantLines[j], "Вторая строка конфликтов");
                }
            }

            Assert.AreEqual(1, 1, "Статус результирующей строки");
        }
    }
}
