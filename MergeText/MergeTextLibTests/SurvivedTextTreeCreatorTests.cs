using System;
using System.Collections.Generic;
using System.Linq;
using MergeTextLib;
using NUnit.Framework;

namespace MergeTextLibTests
{
    [TestFixture]
    class SurvivedTextTreeCreatorTests
    {
        [Test]
        public void GetTextLineTest1_GetTotalWeight()
        {
            // arraange
            var origTextList = new List<string>()
            {
                "a","b"
            };

            var changTextList = new List<string>()
            {
                "a","b"
            };

            ISurvivedTextTreeCreator tlc = new SurvivedTextTreeCreator(new StrComparer());

            // act
            var result = tlc.GetTextLine(origTextList, changTextList);
            
            //assert
            Assert.AreEqual(2, result.GetTotalWeight());
        }

        [Test]
        public void GetTextLineTest2()
        {
            // arraange
            var origTextList = new List<string>()
            {
                "a","b","c"
            };

            var changTextList = new List<string>()
            {
                "a","c","b"
            };

            var expected = new List<SurvivedTextLine>()
            {
                new SurvivedTextLineLeaf("a",0,0),
                new SurvivedTextLineLeaf("b",1,2),
            };

            ISurvivedTextTreeCreator tlc = new SurvivedTextTreeCreator(new StrComparer());

            // act
            var result = tlc.GetTextLine(origTextList, changTextList).GetThisAndNextValues().ToList();

            //assert
            CoreCompare(expected, result);
        }

        [Test]
        public void GetTextLineTest3()
        {
            // arraange
            var origTextList = new List<string>()
            {
                "0","1","2","3","4","5"
            };

            var changTextList = new List<string>()
            {
                "0","2","3","1","4","5"
            };

            var expected = new List<SurvivedTextLine>()
            {
                new SurvivedTextLineLeaf("0",0,0),
                new SurvivedTextLineLeaf("2",2,1),
                new SurvivedTextLineLeaf("3",3,2),
                new SurvivedTextLineLeaf("4",4,4),
                new SurvivedTextLineLeaf("5",5,5),
            };

            ISurvivedTextTreeCreator tlc = new SurvivedTextTreeCreator(new StrComparer());

            // act
            var result = tlc.GetTextLine(origTextList, changTextList).GetThisAndNextValues().ToList();

            //assert
            CoreCompare(expected, result);
        }

        [Test]
        public void GetTextLineTest_4()
        {
            // arraange
            var origTextList = new List<string>()
            {
                "0","1","2","3","4","5","6","7","8"
            };

            var changTextList = new List<string>()
            {
                "0","1","2","5","3","6","4","7","8"
            };

            var expected = new List<SurvivedTextLine>()
            {
                new SurvivedTextLineLeaf("0",0,0),
                new SurvivedTextLineLeaf("1",1,1),
                new SurvivedTextLineLeaf("2",2,2),
                new SurvivedTextLineLeaf("3",3,4),
                new SurvivedTextLineLeaf("4",4,6),
                new SurvivedTextLineLeaf("7",7,7),
                new SurvivedTextLineLeaf("8",8,8),
            };

            ISurvivedTextTreeCreator tlc = new SurvivedTextTreeCreator(new StrComparer());

            // act
            var result = tlc.GetTextLine(origTextList, changTextList).GetThisAndNextValues().ToList();

            //assert
            CoreCompare(expected, result);
        }

        private void CoreCompare(List<SurvivedTextLine> expected, List<SurvivedTextLine> result)
        {
            Assert.AreEqual(expected.Count, result.Count, "Количество строк в результате");
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i].Text, result[i].Text,
                    String.Format("Текст в строке {0} ({1})", i, expected[i].Text));
                Assert.AreEqual(expected[i].OriginalLineIndex, result[i].OriginalLineIndex,
                    String.Format("Индекс строки в исходном тексте {0} ({1})", i, expected[i].Text));
                Assert.AreEqual(expected[i].ChangedLineIndex, result[i].ChangedLineIndex,
                    String.Format("Индекс строки в измененном тексте {0} ({1})", i, expected[i].Text));
            }
        }

        [Test]
        public void GetTextLineTest5_EmptyChanged()
        {
            // arraange
            var origTextList = new List<string>(){"0","1"};
            var changTextList = new List<string>(){};
            ISurvivedTextTreeCreator tlc = new SurvivedTextTreeCreator(new StrComparer());

            // act
            SurvivedTextLine res = tlc.GetTextLine(origTextList, changTextList);

            var resultCount = res.GetThisAndNextValues().ToList().Count;

            //assert
            Assert.AreEqual(0, resultCount);
        }

        [Test]
        public void GetTextLineTest5_EmptyOrig()
        {
            // arraange
            var origTextList = new List<string>(){};
            var changTextList = new List<string>() {"0", "1"};
            var expectedCount = 0;
            ISurvivedTextTreeCreator tlc = new SurvivedTextTreeCreator(new StrComparer());

            // act
            SurvivedTextLine res = tlc.GetTextLine(origTextList, changTextList);
            var resultCount = res.GetThisAndNextValues().ToList().Count;

            //assert
            Assert.AreEqual(expectedCount, resultCount);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTextLineTest5_NullOrig()
        {
            // arraange
            List<string> origTextList = null;
            var changTextList = new List<string>() { "0", "1" };

            // act
            ISurvivedTextTreeCreator tlc = new SurvivedTextTreeCreator(new StrComparer());
            tlc.GetTextLine(origTextList, changTextList);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTextLineTest5_NullChanged()
        {
            // arraange
            List<string> origTextList = new List<string>() { "0", "1" };;
            List<string> changTextList = null;

            // act
            ISurvivedTextTreeCreator tlc = new SurvivedTextTreeCreator(new StrComparer());
            tlc.GetTextLine(origTextList, changTextList);
        }
    }
}
