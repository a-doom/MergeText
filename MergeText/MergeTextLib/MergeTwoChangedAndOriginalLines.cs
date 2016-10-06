using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeTextLib
{
    /// <summary>
    /// Класс, производящий слияние двух строк с исходной
    /// </summary>
    public class MergeTwoChangedAndOriginalLines : IMergeTwoChangedAndOriginalLines
    {
        public IComparer<String> Comparer { private set; get; }

        public MergeTwoChangedAndOriginalLines(IComparer<String> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException();
            }

            Comparer = comparer;
        }

        public IMergedLine Merge(
            string origLine, 
            IChangedLine line1, 
            IChangedLine line2,
            string firstVariantName,
            string secondVariantName)
        {
            var origLineIndex = line1.OriginalLineIndex;
            LineStatus status = LineStatus.Survived;

            if(line1.Status == LineStatus.Removed || line2.Status == LineStatus.Removed)
                status = LineStatus.Removed;

            if(line1.Status == LineStatus.FirstEmptyRow || line2.Status == LineStatus.FirstEmptyRow)
                status = LineStatus.FirstEmptyRow;

            IMergeConflict mc = new MergeConflict(firstVariantName, secondVariantName)
            {
                FirstVariantLines = line1.AddedLinesAfter,
                SecondVariantLines = line2.AddedLinesAfter
            };

            var result = new MergedLine(
                origLine,
                origLineIndex,
                null,
                status,
                mc)
            {
                IsHaveConflict = false
            };

            // Проверяем, есть ли конфликт
            if (mc.FirstVariantLines.Count != 0 && mc.SecondVariantLines.Count == 0)
            {
                result.AddedLinesAfter = mc.FirstVariantLines;
            }
            else if (mc.FirstVariantLines.Count == 0 && mc.SecondVariantLines.Count != 0)
            {
                result.AddedLinesAfter = mc.SecondVariantLines;
            }
            else if (mc.FirstVariantLines.Count != 0 && mc.SecondVariantLines.Count != 0)
            {
                if (CheckConflict(mc, Comparer))
                {
                    result.AddedLinesAfter = mc.SecondVariantLines;
                }
                else
                {
                    result.IsHaveConflict = true;
                }
            }

            return result;
        }

        private bool CheckConflict(IMergeConflict mc, IComparer<string> comparer)
        {
            if (mc.FirstVariantLines.Count != mc.SecondVariantLines.Count)
                return false;

            for (int i = 0; i < mc.FirstVariantLines.Count; i++)
            {
                if(comparer.Compare(mc.FirstVariantLines[i], mc.SecondVariantLines[i]) != 0)
                    return false;
            }

            return true;
        }
    }
}
