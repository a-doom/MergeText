using System;
using System.Collections.Generic;
using System.Linq;
using MergeTextLib;

namespace MergeTextLib
{
    public sealed class SurvivedTextLineComposite : SurvivedTextLine
    {
        private List<SurvivedTextLine> _textLines = new List<SurvivedTextLine>();

        public Dictionary<SurvivedTextLine, int> WeightKeeper { get; set; }

        public List<SurvivedTextLine> TextLines
        {
            get
            {
                return _textLines;
            }
            set
            {
                List<SurvivedTextLine> tmp = value as List<SurvivedTextLine>;
                if (tmp == null)
                {
                    throw new ArgumentNullException();
                }
                _textLines = value;
            }
        }

        public SurvivedTextLineComposite(string text, int originalLineIndex, int changedLineIndex) 
            : base(text, originalLineIndex, changedLineIndex) { }

        public override int GetTotalWeight()
        {
            int result = 0;
            if (WeightKeeper == null)
            {
                result = CalcTotalWeight();
            }
            else
            {
                if (WeightKeeper.ContainsKey(this))
                {
                    result = WeightKeeper[this];
                }
                else
                {
                    result = CalcTotalWeight();
                    WeightKeeper.Add(this, result);
                }
            }
            return result;
        }

        private int CalcTotalWeight()
        {
            int result = Weight;
            foreach (var textLine in TextLines)
            {
                result += textLine.GetTotalWeight();
            }
            return result;
        }

        public override IEnumerable<SurvivedTextLine> GetThisAndNextValues()
        {
            yield return this;
            if (TextLines.Any())
            {
                foreach (var val in GetMaxWeightLine().GetThisAndNextValues())
                {
                    yield return val;
                }
            }
        }

        /// <summary>
        /// Предоставляет строку с наибольшим весом
        /// </summary>
        public SurvivedTextLine GetMaxWeightLine()
        {
            if (TextLines.Any())
            {
                var maxWeight = TextLines.Max(line => line.GetTotalWeight());
                var muxWeightLines = TextLines.Where(line => line.GetTotalWeight() == maxWeight);

                // Если есть две строки с одинаковым весом - выбираем ту, которая выше
                var minIndex = muxWeightLines.Min(line => line.OriginalLineIndex);
                return muxWeightLines.First(line => line.OriginalLineIndex == minIndex);
            }
            return null;
        }
    }
}
