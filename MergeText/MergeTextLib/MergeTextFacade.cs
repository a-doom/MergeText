using System.Collections.Generic;

namespace MergeTextLib
{
    /// <summary>
    /// Фасад для работы с библиотекой объединения файлов
    /// </summary>
    public sealed class MergeTextFacade : IMergeTextFacade
    {
        public IMergedLinesCreator MergedLinesCreator { get; private set; }
        public IMergedLineToStrListConverter MergedToStrConverter { get; private set; }
        public IChangedLinesCreator ChangedLinesCreator { get; private set; }

        public MergeTextFacade(
            IMergedLinesCreator mergedLinesCreator,
            IMergedLineToStrListConverter mergedToStrConverter,
            IChangedLinesCreator changedLinesCreator
            )
        {
            MergedLinesCreator = mergedLinesCreator;
            MergedToStrConverter = mergedToStrConverter;
            ChangedLinesCreator = changedLinesCreator;
        }

        public List<string> GetMergedText(
            List<string> origText, 
            List<string> newTextFirst, 
            List<string> newTexSecond,
            string changedFileNameFirst,
            string changedFileNameSecond)
        {

            List<IChangedLine> changedList1 = ChangedLinesCreator.GetChangeOriginalLines(origText, newTextFirst);

            List<IChangedLine> changedList2 = ChangedLinesCreator.GetChangeOriginalLines(origText, newTexSecond);

            List<IMergedLine> mergedList = MergedLinesCreator.GetMergeResult(origText, changedList1, changedList2,
                changedFileNameFirst, changedFileNameSecond);

            var result = new List<string>();
            foreach (var mergedLine in mergedList)
            {
                result.AddRange(MergedToStrConverter.Convert(mergedLine));
            }

            return result;
        }
    }
}
