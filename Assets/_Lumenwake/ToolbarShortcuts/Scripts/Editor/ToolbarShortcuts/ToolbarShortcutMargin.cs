using System.Collections.Generic;
using UnityEditor.Toolbars;

namespace ToolbarShortcuts.Editor
{
    public static class ToolbarShortcutMargin
    {
        public static IEnumerable<MainToolbarElement> CreateSlots(int slotCount)
        {
            for (int i = 0; i < slotCount; i++)
                yield return CreateSlot();
        }

        static MainToolbarButton CreateSlot()
        {
            var content = new MainToolbarContent(string.Empty, null, string.Empty);
            return new MainToolbarButton(content, () => { })
            {
                enabled = false
            };
        }
    }
}
