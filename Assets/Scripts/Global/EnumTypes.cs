using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixel.Game.Management
{
    /**
    * List of Enums usable from any script
    **/
    public class EnumTypes
    {
        //Time management
        public enum TimerTypeEnum
        {
            Countdown = 0, //Timer that starts from XX:XX ends at 00:00
            Normal = 1 // Timer that starts from 00:00
        }
    }
}
   
