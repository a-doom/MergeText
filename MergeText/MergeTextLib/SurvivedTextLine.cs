using System.Collections.Generic;

namespace MergeTextLib
{
    /// <summary>
    /// Строка текста, уцелевшая после изменения
    /// </summary>
    public abstract class SurvivedTextLine
    {
        protected int Weight { get; set; }
        public string Text { get; protected set; }
        public int OriginalLineIndex { get; protected set; }
        public int ChangedLineIndex { get; protected set; }

        /// <summary>
        /// Создание строки текста
        /// </summary>
        /// <param name="text">Текст в строке</param>
        /// <param name="originalLineIndex">Номер строки в тексте</param>
        /// <param name="changedLineIndex">Номер строки в новом тексте</param>
        /// <param name="weight">Изначальный вес</param>
        public SurvivedTextLine(string text, int originalLineIndex, int changedLineIndex)
        {
            Text = text;
            OriginalLineIndex = originalLineIndex;
            ChangedLineIndex = changedLineIndex;
            Weight = 1;
        }

        /// <summary>
        /// Подсчет веса строк, следующих за данной
        /// </summary>
        public virtual int GetTotalWeight()
        {
            return Weight;
        }

        /// <summary>
        /// Перечислитель строк, следующих за данной
        /// </summary>
        public abstract IEnumerable<SurvivedTextLine> GetThisAndNextValues();

        public override string ToString()
        {
            return "'" + Text.Substring(0, 10) + "'" + ", " + Weight;
        }

        public override bool Equals(object obj)
        {
            SurvivedTextLine incomingLine = obj as SurvivedTextLine;
            if (incomingLine == null)
                return false;

            return this.OriginalLineIndex == incomingLine.OriginalLineIndex
                && this.ChangedLineIndex == incomingLine.ChangedLineIndex
                && this.Weight == incomingLine.Weight
                && this.Text == incomingLine.Text;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + OriginalLineIndex.GetHashCode()
                   + ChangedLineIndex.GetHashCode() + Weight.GetHashCode() + Text.GetHashCode();
        }

    }
}
