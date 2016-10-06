using System.Collections.Generic;

namespace MergeTextLib
{
    /// <summary>
    /// Интерфейс класса, хранящего в себе результаты слияния одной строки измененного текста с исходным
    /// </summary>
    public interface IChangedLine
    {
        /// <summary>
        /// Список новых строк, добавленных после строки исходного текста
        /// </summary>
        List<string> AddedLinesAfter { get; set; }

        /// <summary>
        /// Статус строки
        /// </summary>
        LineStatus Status { get; set; }

        /// <summary>
        /// Текст находящийся в строке
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Индекс строки в оригинальном тексте
        /// </summary>
        int? OriginalLineIndex { get; set; }

        /// <summary>
        /// Индекс строки в измененном тексте
        /// </summary>
        int? ChangedLineIndex { get; set; }
    }
}
