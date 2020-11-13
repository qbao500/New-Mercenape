using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Waves")]
public class WaveSO : ScriptableObject
{
    [System.Serializable]
    public class Group
    {
        public string group;
        public int shred;
        public int mower;
    }
    public List<Group> groups;
}
