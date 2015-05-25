using System;
using System.Linq;
using System.Collections.Generic;
using SimpleJSON;

namespace SimpleJSON
{
    public static class JSONExtensions
    {
        #region Extensions

        public static JSONArray FoldList<T>(this List<T> list)
            where T : IJsonSavable
        {
            JSONArray array = new JSONArray();

            for(int i = 0; i < list.Count; i++)
            {
                JSONClass item = list[i].ExportState();
                array.Add(item);
            }

            return array;
        }

        public static List<T> UnfoldJsonArray<T>(this JSONArray array)
            where T : IJsonSavable, new()
        {
            List<T> result = new List<T>();

            foreach (JSONNode child in array.Childs)
            {
                T newItem = new T();
                newItem.ImportState(child.AsObject);

                result.Add(newItem);
            }

            return result;
        }

        #endregion Extensions
    }
}