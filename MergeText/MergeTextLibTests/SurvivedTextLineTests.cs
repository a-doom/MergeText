using System.Collections.Generic;
using MergeTextLib;
using NUnit.Framework;

namespace MergeTextLibTests
{
    [TestFixture]
    class SurvivedTextLineTests
    {
        [Test]
        public void GetWeightTest1()
        {
            // arraange
            SurvivedTextLine tl = new SurvivedTextLineComposite("a",0,0)
            {
                TextLines = new List<SurvivedTextLine>
                {
                    new SurvivedTextLineLeaf("b",1,0),
                    new SurvivedTextLineLeaf("c",2,0),
                    new SurvivedTextLineLeaf("d",3,0)
                }
            };

            // act
            var res = tl.GetTotalWeight();

            //assert
            Assert.AreEqual(4, res);
        }

        [Test]
        public void GetMaxWeightLine()
        {

            // arraange
            SurvivedTextLineComposite tlc_a = new SurvivedTextLineComposite("a", 1, 0);
            SurvivedTextLineLeaf tll_b = new SurvivedTextLineLeaf("b", 2, 0);
            SurvivedTextLineLeaf tll_d = new SurvivedTextLineLeaf("d", 3, 0);
            SurvivedTextLineLeaf tll_e = new SurvivedTextLineLeaf("e", 4, 0);
            tlc_a.TextLines = new List<SurvivedTextLine> {tll_b, tll_d, tll_e};

            // act
            var res = tlc_a.GetMaxWeightLine();

            //assert
            Assert.AreEqual(tll_b, res);
        }

        [Test]
        public void GetMaxWeightLine2()
        {

            // arraange
            SurvivedTextLineComposite tlc_a = new SurvivedTextLineComposite("a", 1, 0);
            SurvivedTextLineLeaf tll_b = new SurvivedTextLineLeaf("b", 2, 0);
            SurvivedTextLineLeaf tll_d = new SurvivedTextLineLeaf("d", 3, 0);
            SurvivedTextLineComposite tll_e = new SurvivedTextLineComposite("e", 4, 0)
            {
                TextLines = new List<SurvivedTextLine>{new SurvivedTextLineLeaf("f", 5, 0)}
            };
            tlc_a.TextLines = new List<SurvivedTextLine> { tll_b, tll_d, tll_e };

            // act
            var res = tlc_a.GetMaxWeightLine();

            //assert
            Assert.AreEqual(tll_e, res);
        }
    }
}
