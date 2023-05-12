using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTransitionLogic : MonoBehaviour
{
    [SerializeField] MenuNav functions;

    public void IsPlaying() { functions.StartedPlaying(); }

    public void NotPlaying() { functions.StoppedPlaying(); }
}
