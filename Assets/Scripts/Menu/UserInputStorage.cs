using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Menu
{
    /// <summary>
    /// Used to store data between multiple scenes.
    /// </summary>
    public class UserInputStorage : MonoBehaviour
    {
        [HideInInspector]
        public static UserInputStorage Storage;
        [HideInInspector]
        
        public List<string> InputPaths;

        void Awake()
        {

            //Applying singleton design pattern
            if (Storage == null)
            {
                Storage = this;
            }
            else if (Storage != this)
            {
                this.InputPaths = Storage.InputPaths;
                Destroy(Storage.gameObject);
                Storage = this;
            }
            DontDestroyOnLoad(this.gameObject);

        }


    }
}


