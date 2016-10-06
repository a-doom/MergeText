using System.Collections.Generic;
using System.Linq;

namespace MergeTextLib
{
    /// <summary>
    /// Класс переводящий информацию об измененной строке в список строк
    /// </summary>
    public sealed class MergedLineToStrListConverter : IMergedLineToStrListConverter
    {
        public List<string> Convert(IMergedLine line)
        {
            var result = new List<string>();

            if (line.Status == LineStatus.Survived)
            {
                result.Add(line.Text);
            }

            if (line.IsHaveConflict)
            {
                result.Add(line.MergeConflict.GetFrameAbove());
                result.AddRange(line.MergeConflict.FirstVariantLines);
                result.Add(line.MergeConflict.GetFrameBetween());
                result.AddRange(line.MergeConflict.SecondVariantLines);
                result.Add(line.MergeConflict.GetFrameBelow());
            }
            else
            {
                if (line.AddedLinesAfter.Any())
                {
                    result.AddRange(line.AddedLinesAfter);
                }
            }

            return result;
        }
    }
}
