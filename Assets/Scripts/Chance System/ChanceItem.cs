using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChanceSystem
{
    [System.Serializable]
    public class ChanceItem<T>
    {
        // The weight value of this item, corresponding directly with the chance it is selected.
        [SerializeField]
        private float _weight;
        // The corresponding property to the weight value of this item.
        public float weight
        {
            get
            {
                return _weight;
            }

            set
            {
                _weight = value;
            }
        }

        // The Object of generic type T that this class encapsulates. 
        [SerializeField]
        private T _item;
        // The corresponding properety to the item Object of generic type T.
        public T item
        {
            get
            {
                return _item;
            }

            set
            {
                _item = (T)value;
            }
        }

        public ChanceItem()
        {
            _weight = 0f;
            _item = default(T);
        }

        public ChanceItem(float w, T i)
        {
            _weight = w;
            _item = i;
        }
    }
}
