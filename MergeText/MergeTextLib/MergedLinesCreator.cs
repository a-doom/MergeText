using System;
using System.Collections.Generic;
using System.Linq;

namespace MergeTextLib
{
    /// <summary>
    /// Класс, производящий слияние строк двух текстов с исходным
    /// </summary>
    public sealed class MergedLinesCreator : IMergedLinesCreator
    {
        public IMergeTwoChangedAndOriginalLines Merge { get; private set; }

        public MergedLinesCreator(IMergeTwoChangedAndOriginalLines merge)
        {
            Merge = merge;
        }

        public List<IMergedLine> GetMergeResult(
            List<String> originalText, 
            List<IChangedLine> firstVariatChanges,
            List<IChangedLine> secondVariatChanges,
            string firstVariantName,
            string secondVariantName)
        {
            var result = new List<IMergedLine>();

            if (firstVariatChanges.Any(line => line.Status == LineStatus.FirstEmptyRow)
                && secondVariatChanges.Any(line => line.Status == LineStatus.FirstEmptyRow))
            {
                result.Add(Merge.Merge(
                    "",
                    firstVariatChanges.First(line => line.Status == LineStatus.FirstEmptyRow),
                    secondVariatChanges.First(line => line.Status == LineStatus.FirstEmptyRow),
                    firstVariantName,
                    secondVariantName));
            }

            for (int origTextIndex = 0; origTextIndex < originalText.Count; origTextIndex++)
            {
                result.Add(Merge.Merge(
                    originalText[origTextIndex],
                    firstVariatChanges.First(line => line.OriginalLineIndex == origTextIndex),
                    secondVariatChanges.First(line => line.OriginalLineIndex == origTextIndex),
                    firstVariantName,
                    secondVariantName));
            }

            return result;
        }
    }
}
