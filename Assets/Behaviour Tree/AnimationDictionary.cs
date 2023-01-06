using UnityEngine;
using System;

namespace AnimationContainer
{
    /// <summary>
    /// Animation clip<br/>
    /// WeaponManager<br/>
    /// </summary>
    [Serializable]
    public struct LinkedValues
    {
        public AnimationClip animClip;
        public WeaponManager weaponManager;
    }

    [Serializable]
    public class Container
    {
        [SerializeField] private LinkedValues[] values;

        private LinkedValues[] Values { get => values; }

        public int Length => values.Length;

        public int GetIndex(WeaponManager key)
        {
            int index = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].weaponManager == key)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public int GetIndex(AnimationClip key)
        {
            int index = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].animClip == key)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public LinkedValues GetValuesAt(AnimationClip key) => Values[GetIndex(key)];

        public LinkedValues GetValuesAt(WeaponManager key) => Values[GetIndex(key)];

        public LinkedValues GetValuesAt(int index) => Values[index];


    }
}

