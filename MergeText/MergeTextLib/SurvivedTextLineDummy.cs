using System.Collections.Generic;
using MergeTextLib;

namespace MergeTextLib
{
    public sealed class SurvivedTextLineDummy : SurvivedTextLine
    {
        public SurvivedTextLineDummy() : 
            base("", -1, -1)
        {
        }

        public override IEnumerable<SurvivedTextLine> GetThisAndNextValues()
        {
            yield break;
        }
    }
}
