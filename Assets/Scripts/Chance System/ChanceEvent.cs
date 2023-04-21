using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChanceSystem
{
    public delegate void ChanceEventDelegate();

    public class ChanceEvent
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

        // The event associated with this weight value. 
        [SerializeField]
        private ChanceEventDelegate _chanceEvent;
        // The corresponding properety to the event.
        public ChanceEventDelegate chanceEvent
        {
            get
            {
                return _chanceEvent;
            }

            set
            {
                _chanceEvent = value;
            }
        }

        public ChanceEvent()
        {
            _weight = 0f;
            _chanceEvent = default(ChanceEventDelegate);
        }

        public ChanceEvent(float w, ChanceEventDelegate i)
        {
            _weight = w;
            _chanceEvent = i;
        }
    }
}
