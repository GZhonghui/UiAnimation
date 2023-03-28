using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UiAnimation
{
    public static class UiAnimationLua
    {
        public static int GetLuaExport(GameObject go, string selected, out List<string> exportList)
        {
            int index = -1;

            exportList = new List<string>();
            var exist = new HashSet<string>();

            if (selected != null && selected != "")
            {
                index = 0;
                exist.Add(selected);
                exportList.Add(selected);
            }

            var lua = go.GetComponent<LA_LuaComponent>();
            if (lua != null)
            {
                var luaTable = lua.GetLuaInstanceTable();
                if (luaTable != null)
                {
                    var exportAnims = luaTable.Get<XLua.LuaTable>(UiAnimationDefine.luaExportName);

                    if (exportAnims != null) foreach (var key in exportAnims.GetKeys())
                    {
                        exportAnims.Get(key, out string animName);

                        if (animName != null && animName != "" && !exist.Contains(animName))
                        {
                            exist.Add(animName);
                            exportList.Add(animName);
                        }
                    }
                }
            }

            return index;
        }
    }
}