using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChanceSystem
{
    [System.Serializable]
    public class RandomItem<T>
    {
        [SerializeField]
        private List<ChanceItem<T>> _chanceItems;

        public List<ChanceItem<T>> chanceItems
        {
            get
            {
                return _chanceItems;
            }

            set
            {
                _chanceItems = (List<ChanceItem<T>>)value;
            }
        }

        private float _totalWeight;

        public float totalWeight
        {
            get
            {
                return _totalWeight;
            }
        }

        public RandomItem()
        {
            _chanceItems = new List<ChanceItem<T>>();
        }

        public RandomItem(List<ChanceItem<T>> startingItems)
        {
            _chanceItems = startingItems;
            CalcWeight();
        }

        public float CalcWeight()
        {
            _totalWeight = 0f;
            foreach (ChanceItem<T> item in _chanceItems)
            {
                _totalWeight += item.weight;
            }
            return _totalWeight;
        }

        public void NormalizeWeight()
        {
            CalcWeight();
            if (_totalWeight == 1f)
                return;
            foreach (ChanceItem<T> item in _chanceItems)
            {
                item.weight /= _totalWeight;
            }

            _totalWeight = 1f;
        }

        public void AddItem(ChanceItem<T> newItem)
        {
            if (newItem.weight > 0 && newItem.item != null)
            {
                chanceItems.Add(newItem);
            }
            CalcWeight();
        }

        public void AddItem(List<ChanceItem<T>> newItems)
        {
            foreach (ChanceItem<T> newItem in newItems)
            {
                if (newItem.weight > 0 && newItem.item != null)
                {
                    chanceItems.Add(newItem);
                }
            }
            CalcWeight();
        }

        public T GetItem()
        {
            float selector = Random.Range(0, totalWeight);
            for (int i = 0; i < chanceItems.Count; i++)
            {
                float internalChance = 0f;

                for (int k = 0; k <= i; k++)
                {
                    internalChance += chanceItems[k].weight;
                }

                if (selector < internalChance)
                {
                    return chanceItems[i].item;
                }
            }
            return default(T);
        }

        override public string ToString()
        {
            string result = "";
            result += "RANDOM ITEM::\n\n\n";
            foreach (ChanceItem<T> item in chanceItems)
            {
                result += "Weight: " + item.weight + "\nItem: " + item.item;
                result += "\n\n";
            }

            return result;
        }
    }
}

