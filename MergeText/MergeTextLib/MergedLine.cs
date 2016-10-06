namespace MergeTextLib
{
    /// <summary>
    /// Класс, хранящий в себе результаты слияния 
    /// двух строк измененных текстов со строкой исходного
    /// </summary>
    public class MergedLine : ChangedLine, IMergedLine
    {
        public MergedLine(
            string text, 
            int? originalLineIndex,
            int? changedLineIndex, 
            LineStatus status,
            IMergeConflict mergeConflict)
            : base(text, originalLineIndex, changedLineIndex, status)
        {
            MergeConflict = mergeConflict;
            IsHaveConflict = false;
        }

        public IMergeConflict MergeConflict { get; set; }
        public bool IsHaveConflict { get; set; }
    }
}
