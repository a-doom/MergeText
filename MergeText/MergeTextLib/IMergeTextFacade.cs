using System.Collections.Generic;

namespace MergeTextLib
{
    /// <summary>
    /// Интерфейс фасада для работы с библиотекой объединения файлов
    /// </summary>
    public interface IMergeTextFacade
    {
        List<string> GetMergedText(
            List<string> origText,
            List<string> newTextFirst,
            List<string> newTexSecond,
            string changedFileNameFirst,
            string changedFileNameSecond);
    }
}
