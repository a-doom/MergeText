namespace MergeTextLib
{
    /// <summary>
    /// Интерфейс класса, хранящего в себе результаты слияния 
    /// двух строк измененных текстов со строкой исходного
    /// </summary>
    public interface IMergedLine : IChangedLine
    {
        IMergeConflict MergeConflict { get; set; }
        bool IsHaveConflict { get; set; }
    }
}
