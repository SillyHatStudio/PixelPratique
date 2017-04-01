﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Pixel.Game.Management
{
    /** 
     * Time Management (countdown timer & regular timer)
     **/
    public class TimerManager : MonoBehaviour
    {
        public Text timerTextObject;
        public EnumTypes.TimerTypeEnum m_timerType;

        private float timer;
        private float elapsedMins, elapsedSecs;

        [HideInInspector]
        public bool timerLocked;

        //(variables for countdown timer)
        [Range(0, 59)]
        public int countDownMinutes;
        [Range(0, 59)]
        public int countDownSeconds;
        public float remainingMins, remainingSecs, milliseconds;
        private bool timerOver;

        private void Awake()
        {
            if (m_timerType == EnumTypes.TimerTypeEnum.Normal)
            {
                elapsedMins = 0;
                elapsedSecs = 0;
            }

            if (m_timerType == EnumTypes.TimerTypeEnum.Countdown)
            {
                remainingMins = countDownMinutes;
                remainingSecs = countDownSeconds;
                milliseconds = 100;
                timerOver = false;
            }
        }

        void Start()
        {

        }


        void Update()
        {
            if(!timerLocked)
            {
                //Compute & display elapsed time (normal timer)
                if (m_timerType == EnumTypes.TimerTypeEnum.Normal)
                {
                    timer += Time.deltaTime;

                    elapsedMins = Mathf.Floor(timer / 60);
                    elapsedSecs = timer % 60;

                    if (timerTextObject != null)
                    {
                        timerTextObject.text = elapsedMins.ToString("00") + ":" + elapsedSecs.ToString("00");
                    }
                }

                //Compute & display  remaining time (countdown timer)
                if (m_timerType == EnumTypes.TimerTypeEnum.Countdown)
                {
                    if (remainingMins <= 0 && remainingSecs <= 0 && milliseconds <= 0)
                    {
                        //Timer ends
                        //Add some code here when the timer finishes
                        timerOver = true;
                    }

                    else
                    {
                        if (!timerOver)
                        {
                            if (milliseconds <= 0)
                            {
                                if (remainingSecs <= 0)
                                {
                                    remainingMins--;
                                    remainingSecs = 59;

                                }

                                else if (remainingSecs >= 1)
                                {
                                    remainingSecs--;
                                }

                                milliseconds = 100;
                            }

                            milliseconds -= Time.deltaTime * 100;

                            if (timerTextObject != null)
                            {
                                if (remainingSecs < 10)
                                {
                                    timerTextObject.text = string.Format("{0}:0{1}", remainingMins, remainingSecs);
                                }

                                else
                                    timerTextObject.text = string.Format("{0}:{1}", remainingMins, remainingSecs);
                            }
                        }
                    }
                }
            }
        }
    }
}
