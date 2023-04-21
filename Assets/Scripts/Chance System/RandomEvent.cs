using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChanceSystem
{
    public class RandomEvent
    {
        [SerializeField]
        private List<ChanceEvent> _chanceEvents;

        public List<ChanceEvent> chanceEvents
        {
            get
            {
                return _chanceEvents;
            }

            set
            {
                _chanceEvents = (List<ChanceEvent>)value;
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

        public RandomEvent()
        {
            _chanceEvents= new List<ChanceEvent>();
        }

        public RandomEvent(List<ChanceEvent> startingEvents)
        {
            _chanceEvents = startingEvents;
            CalcWeight();
        }

        private float CalcWeight()
        {
            _totalWeight = 0f;
            foreach (ChanceEvent chanceEvent in _chanceEvents)
            {
                _totalWeight += chanceEvent.weight;
            }
            return _totalWeight;
        }

        public void NormalizeWeight()
        {
            if (_totalWeight == 1f)
                return;
            foreach (ChanceEvent chanceEvent in _chanceEvents)
            {
                chanceEvent.weight /= _totalWeight;
            }
            //CalcWeight();
            _totalWeight = 1f;
        }

        public void AddEvent(ChanceEvent newEvent)
        {
            if (newEvent.weight > 0 && newEvent.chanceEvent != null)
            {
                chanceEvents.Add(newEvent);
            }
            CalcWeight();
        }

        public void AddEvent(List<ChanceEvent> newEvents)
        {
            foreach (ChanceEvent newEvent in newEvents)
            {
                if (newEvent.weight > 0 && newEvent.chanceEvent != null)
                {
                    chanceEvents.Add(newEvent);
                }
            }
            CalcWeight();
        }

        public ChanceEventDelegate GetEvent()
        {
            float selector = Random.Range(0, totalWeight);
            for (int i = 0; i < chanceEvents.Count; i++)
            {
                float internalChance = 0f;

                for (int k = 0; k <= i; k++)
                {
                    internalChance += chanceEvents[k].weight;
                }

                if (selector < internalChance)
                {
                    return chanceEvents[i].chanceEvent;
                }
            }
            return default(ChanceEventDelegate);
        }

        override public string ToString()
        {
            string result = "";
            result += "RANDOM EVENT::\n\n\n";
            foreach (ChanceEvent chanceEvent in chanceEvents)
            {
                result += "Weight: " + chanceEvent.weight + "\nItem: " + chanceEvent.chanceEvent;
                result += "\n\n";
            }

            return result;
        }
    }
}

