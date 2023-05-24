using System;
using System.Collections.Generic;
using UnityEngine;

namespace VariableSystem
{
    [Serializable]
    public class RemoteVariableMapper
    {
        [SerializeField] private List<Map> Maps;

        [Serializable]
        public class Map
        {
            [SerializeField] public int RemoteVariableID;
            [SerializeField] public int LocalVariableID;
        }

        private Dictionary<int, int> GetDict()
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            Maps.ForEach(x => dictionary[x.RemoteVariableID] = x.LocalVariableID);
            return dictionary;
        }

        public List<Variable> MapVariables(List<Variable> remoteVariables, List<Variable> localVariables, bool filter)
        {
            /*  int stage = 0;
              try
              {*/

            Dictionary<int, int> maps = GetDict();

            //  stage = 1;

            List<Variable> filteredList = remoteVariables.FindAll(x => maps.ContainsKey(x.ID));

            //  stage = 2;

            List<Variable> resultList = filteredList.ConvertAll(x => x.GetRemoteVariable(maps[x.ID]));

            //  stage = 3;

            if (!filter) resultList.AddRange(remoteVariables.FindAll(x => !maps.ContainsKey(x.ID)).ConvertAll(x => x.GetRemoteVariable(x.ID)));

            //  stage = 4;

            resultList.RemoveAll(x => localVariables.Find(y => Variable.Check(x, y.ID, y.GetValueType())));

            // stage = 5;

            return resultList;
            /* } catch (Exception ex)
             {
                 Debug.LogError("Mapping Stage : " + stage);
                 Debug.LogException(ex);
             }

            return null;*/
        }
    }
}