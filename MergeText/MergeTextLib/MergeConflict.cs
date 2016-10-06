using System.Collections.Generic;

namespace MergeTextLib
{
    /// <summary>
    /// Класс, содержащий в себе информацию о конфликте 
    /// слияния двух строк измененных файлов со строкой исходного файла
    /// </summary>
    public sealed class MergeConflict : IMergeConflict
    {
        public string FirstVariantName { get; set; }
        public string SecondVariantName { get; set; }
        public List<string> FirstVariantLines { get; set; }
        public List<string> SecondVariantLines { get; set; }

        public MergeConflict(string firstVariantName, string secondVariantName)
        {
            FirstVariantName = firstVariantName;
            SecondVariantName = secondVariantName;
            FirstVariantLines = new List<string>();
            SecondVariantLines = new List<string>();
        }

        public string GetFrameAbove()
        {
            return ">>>>> " + FirstVariantName;
        }

        public string GetFrameBetween()
        {
            return "======";
        }

        public string GetFrameBelow()
        {
            return "<<<<< " + SecondVariantName;
        }
    }
}
