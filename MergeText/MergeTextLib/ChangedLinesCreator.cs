using System;
using System.Collections.Generic;
using System.Linq;

namespace MergeTextLib
{
    /// <summary>
    /// Класс, сравнивающий оригинальную и измененную строку. Создает класс измененной строки.
    /// </summary>
    public sealed class ChangedLinesCreator : IChangedLinesCreator
    {
        public ISurvivedTextTreeCreator SurvivedTextTreeCreator { get; private set; }

        public ChangedLinesCreator(ISurvivedTextTreeCreator survivedTextTreeCreator)
        {
            if (survivedTextTreeCreator == null)
            {
                throw new ArgumentNullException();
            }

            SurvivedTextTreeCreator = survivedTextTreeCreator;
        }

        public List<IChangedLine> GetChangeOriginalLines(
            List<String> originalText,
            List<String> newText)
        {
            if (originalText == null
                || newText == null)
            {
                throw new ArgumentNullException();
            }

            List<SurvivedTextLine> survivedTextLines = SurvivedTextTreeCreator.GetTextLine(
                originalText, newText).GetThisAndNextValues().ToList();

            List<IChangedLine> result = new List<IChangedLine>();

            // Нулевая строка, для строк, вставленных перед первой
            result.Add(new ChangedLine("", null, null, LineStatus.FirstEmptyRow));

            // Проходим по оригинальным строкам и ищем удаленные
            for (int origLineIndex = 0; origLineIndex < originalText.Count; origLineIndex++)
            {
                var origStrLine = originalText[origLineIndex];
                IChangedLine col = new ChangedLine(origStrLine, origLineIndex, null, LineStatus.Removed);

                if (survivedTextLines.Any(line => line.OriginalLineIndex == origLineIndex))
                {
                    var changedLineIndex = survivedTextLines.Find(
                        line => line.OriginalLineIndex == origLineIndex).ChangedLineIndex;

                    col.Status = LineStatus.Survived;
                    col.ChangedLineIndex = changedLineIndex;
                }

                result.Add(col);
            }

            // Проходим по новым строкам и ищем добавленные
            for (int newLineIndex = 0; newLineIndex < newText.Count; newLineIndex++)
            {
                if (!result.Any(line => line.ChangedLineIndex != null && line.ChangedLineIndex == newLineIndex))
                {
             
//  ==>>
//                    // Находим следующую выжившую строку после нашей новой (в новом тексте)
//                    var nextSurviveLineIndex = result.FindIndex(line => line.ChangedLineIndex > newLineIndex);
//                    // И вставляем в строчку перед ней новые строки
//                    if (nextSurviveLineIndex - 1 >= 0)
//                    {
//                        result[nextSurviveLineIndex - 1].AddedLinesAfter.Add(newText[newLineIndex]);
//                    }
//                    if (nextSurviveLineIndex - 1 >= 0)
//                    {
//                        result[nextSurviveLineIndex - 1].AddedLinesAfter.Add(newText[newLineIndex]);
//                    }
//                    else
//                    {
//                        if (newLineIndex > result.Max(line => line.ChangedLineIndex))
//                        {
//                            result.Last().AddedLinesAfter.Add(newText[newLineIndex]);
//                        }
//                        else
//                        {
//                            result[0].AddedLinesAfter.Add(newText[newLineIndex]);
//                        }
//                    }

                    // Находим предыдущую строку (удаленную, нет - не важно) и вставляем в нее.
                    var nextSurviveLineIndex = result.OrderByDescending(line => line.ChangedLineIndex)
                        .FirstOrDefault(line => line.ChangedLineIndex < newLineIndex);
                    // И вставляем в строчку перед ней новые строки
                    if (nextSurviveLineIndex != null && nextSurviveLineIndex.ChangedLineIndex < newLineIndex)
                    {
                        nextSurviveLineIndex.AddedLinesAfter.Add(newText[newLineIndex]);
                    }
                    else
                    {
                        if (newLineIndex > result.Max(line => line.ChangedLineIndex))
                        {
                            result.Last().AddedLinesAfter.Add(newText[newLineIndex]);
                        }
                        else
                        {
                            result[0].AddedLinesAfter.Add(newText[newLineIndex]);
                        }
                    }
//  <==
                }
            }

            return result;
        }
    }
}
