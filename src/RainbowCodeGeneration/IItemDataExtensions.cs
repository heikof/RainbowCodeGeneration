using System.Linq;
using Rainbow.Model;

namespace RainbowCodeGeneration
{
    public static class ItemDataExtensions
    {
        public static string GetSharedField(this IItemData item, string fieldName)
        {
            if (item == null || !item.SharedFields.Any())
                return string.Empty;
            return item.SharedFields.FirstOrDefault(f => f.NameHint == fieldName)?.Value ?? string.Empty;
        }
    }
}
