using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeTextLib
{
    /// <summary>
    /// Интерфейс класса, производящего слияние двух текстов с исходным
    /// </summary>
    public interface IMergedLinesCreator
    {
        List<IMergedLine> GetMergeResult(
            List<String> originalText, 
            List<IChangedLine> firstVariatChanges,
            List<IChangedLine> secondVariatChanges,
            string firstVariantName,
            string secondVariantName);
    }
}
