using System;
using System.Collections.Generic;

namespace MergeTextLib
{
    /// <summary>
    /// Интерфейс класса, содержащего в себе информацию о конфликте 
    /// слияния двух строк измененных файлов со строкой исходного файла
    /// </summary>
    public interface IMergeConflict
    {
        string FirstVariantName { get; set; }
        string SecondVariantName { get; set; }

        List<string> FirstVariantLines { get; set; }
        List<string> SecondVariantLines { get; set; }

        String GetFrameAbove();
        String GetFrameBetween();
        String GetFrameBelow();
    }
}
