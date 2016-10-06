using System;
using System.Collections.Generic;
using MergeTextLib;

namespace MergeTextLib
{
    /// <summary>
    /// Интерфейс класса, сравнивающего оригинальную и измененную строку, и создающего класс измененной строки
    /// </summary>
    public interface IChangedLinesCreator
    {
        List<IChangedLine> GetChangeOriginalLines(
            List<String> originalText,
            List<String> newText);
    }
}
