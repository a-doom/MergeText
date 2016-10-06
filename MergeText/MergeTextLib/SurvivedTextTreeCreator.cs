using System;
using System.Collections.Generic;
using System.Linq;
using MergeTextLib;

namespace MergeTextLib
{
    //1) Возьмем изначальный и измененный текст.
    //2) Строим по узлу SurvivedTextLine на каждую строку оригинального документа, которая есть в измененном.
    //3) Строим по узлу на каждую строку из оригинального текста, 
    //       которая идет после строки из предыдущего пункта, которая есть ниже в измененном тексте
    //4) Рекурсивно повторяем 3 пункт, получаем дерево.
    
    public sealed class SurvivedTextTreeCreator : ISurvivedTextTreeCreator
    {
        public IComparer<string> Comparer { get; private set; }

        Dictionary<Tuple<int, int>, SurvivedTextLine> _textLineCoreResults = null;
        Dictionary<Tuple<int, int>, int> _indexOfWithComparerResults = null;
        Dictionary<SurvivedTextLine, int> _weightKeeper = null;

        public SurvivedTextTreeCreator(IComparer<string> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException();

            Comparer = comparer;
        }

        public SurvivedTextLine GetTextLine(List<String> originalText, List<String> changedText)
        {
            if (originalText == null || changedText == null)
                throw new ArgumentNullException();
            _textLineCoreResults = new Dictionary<Tuple<int, int>, SurvivedTextLine>();
            _indexOfWithComparerResults = new Dictionary<Tuple<int, int>, int>();
            _weightKeeper = new Dictionary<SurvivedTextLine, int>();

            SurvivedTextLineComposite result = new SurvivedTextLineComposite("", 0, 0);

            for (int origLineIndex = 0; origLineIndex < originalText.Count; origLineIndex++)
            {
                var origStrLine = originalText[origLineIndex];

                var changedLineIndex = GetIndexOfWithComparer(origLineIndex, changedText, origStrLine, 0, Comparer);
                if (changedLineIndex >= 0)
                {
                    result.TextLines.Add(GetTextLineCore(
                        originalText, changedText, origLineIndex, changedLineIndex, Comparer));
                }
            }

            if (result.TextLines.Count <= 0)
            {
                return new SurvivedTextLineDummy();
            }

            return result.GetThisAndNextValues().Skip(1).First();
        }


        private SurvivedTextLine GetTextLineCore(
            List<String> originalText, 
            List<String> changedText,
            int origLineIndex, 
            int changedLineIndex, 
            IComparer<string> comparer)
        {
            var key = new Tuple<int, int>(origLineIndex, changedLineIndex);
            if (_textLineCoreResults.ContainsKey(key))
            {
                return _textLineCoreResults[key];
            }

            SurvivedTextLineComposite result =
                new SurvivedTextLineComposite(originalText[origLineIndex], origLineIndex, changedLineIndex)
                {
                    WeightKeeper = _weightKeeper
                };

            for (int origLineIndexTmp = origLineIndex + 1; origLineIndexTmp < originalText.Count; origLineIndexTmp++)
            {
                var origStrLine = originalText[origLineIndexTmp];

                var changedLineIndexTmp = GetIndexOfWithComparer(origLineIndexTmp, changedText, origStrLine, changedLineIndex + 1, comparer);
                if (changedLineIndexTmp >= 0)
                {
                    result.TextLines.Add(GetTextLineCore(
                        originalText, changedText, origLineIndexTmp, changedLineIndexTmp, comparer));
                }
            }

            _textLineCoreResults.Add(new Tuple<int, int>(origLineIndex, changedLineIndex), result);
            return result;
        }


        private int GetIndexOfWithComparer(int origLineIndex, List<string> strList, string searchLine, int fromSearchIndex, IComparer<string> comparer)
        {
            var key = new Tuple<int, int>(origLineIndex, fromSearchIndex);
            if (_indexOfWithComparerResults.ContainsKey(key))
            {
                return _indexOfWithComparerResults[key];
            }

            for (int i = fromSearchIndex; i < strList.Count; i++)
            {
                if (comparer.Compare(strList[i], searchLine) == 0)
                {
                    _indexOfWithComparerResults.Add(new Tuple<int, int>(origLineIndex, fromSearchIndex), i);
                    return i;
                }
            }

            _indexOfWithComparerResults.Add(new Tuple<int, int>(origLineIndex, fromSearchIndex), -1);
            return -1;
        }
    }
}
