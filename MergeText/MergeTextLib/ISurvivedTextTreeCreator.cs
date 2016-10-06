using System;
using System.Collections.Generic;

namespace MergeTextLib
{
    public interface ISurvivedTextTreeCreator
    {
        SurvivedTextLine GetTextLine(List<String> originalText, List<String> changedText);
    }
}
