using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventHandle : MonoBehaviour
{
    //can only write static action or static function
    public static Action<string> onOpenTextMechaine = delegate { };

} 
