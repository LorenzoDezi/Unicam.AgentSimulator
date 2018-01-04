using System;
using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Model;
using Unicam.AgentSimulator.Utility;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts
{
    public class BusInputController : InputController
    {

        

        //TODO: Definire diverse proprietà, da concordare

        protected override string[] SanitizePositionValues(string[] positionValues)
        {
            return UTMUtility.ParseLongLatToUTM(positionValues, StreetMaker.EdinburghUTMOrigin);
        }
       
    }

}
