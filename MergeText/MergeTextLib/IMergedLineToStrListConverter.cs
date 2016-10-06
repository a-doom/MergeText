using System.Collections.Generic;
using MergeTextLib;

namespace MergeTextLib
{
    /// <summary>
    /// Интерфейс класса, переводящего информацию об измененной строке в список строк
    /// </summary>
    public interface IMergedLineToStrListConverter
    {
        List<string> Convert(IMergedLine line);
    }
}
