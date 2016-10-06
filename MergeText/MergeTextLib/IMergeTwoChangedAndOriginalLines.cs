using System;
using System.Collections.Generic;

namespace MergeTextLib
{
    /// <summary>
    /// Интерфейс класса, производящего слияние двух строк с исходной
    /// </summary>
    public interface IMergeTwoChangedAndOriginalLines
    {
        IMergedLine Merge(
            String origLine,
            IChangedLine line1,
            IChangedLine line2,
            string firstVariantName,
            string secondVariantName);
    }
}
