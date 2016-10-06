using System.Collections.Generic;

namespace MergeTextLib
{
    /// <summary>
    /// Класса, хранящий в себе результаты слияния одной строки измененного текста с исходным
    /// </summary>
    public class ChangedLine : IChangedLine
    {
        public List<string> AddedLinesAfter{ get; set; }
        public string Text { get; set; }
        public int? OriginalLineIndex { get; set; }
        public int? ChangedLineIndex { get; set; }

        public LineStatus Status { get; set; }

        public ChangedLine(string text, int? originalLineIndex, int? changedLineIndex, LineStatus status)
        {
            AddedLinesAfter = new List<string>();

            Text = text;
            OriginalLineIndex = originalLineIndex;
            ChangedLineIndex = changedLineIndex;
            Status = status;
        }
    }
}
