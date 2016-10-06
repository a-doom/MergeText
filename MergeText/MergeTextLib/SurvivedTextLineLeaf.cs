using System.Collections.Generic;
using MergeTextLib;

namespace MergeTextLib
{
    public sealed class SurvivedTextLineLeaf : SurvivedTextLine
    {
        public SurvivedTextLineLeaf(string text, int originalLineIndex, int changedLineIndex) 
            : base(text, originalLineIndex, changedLineIndex) { }

        public override IEnumerable<SurvivedTextLine> GetThisAndNextValues()
        {
            yield return this;
        }
    }
}
