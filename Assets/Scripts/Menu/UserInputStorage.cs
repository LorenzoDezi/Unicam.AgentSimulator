using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Menu
{
    public class UserInputStorage : MonoBehaviour
    {

        public static UserInputStorage storage;

        public List<string> inputPaths;

        void Awake()
        {

            //Applying singleton design pattern
            if (storage == null)
            {
                storage = this;
            }
            else if (storage != this)
            {
                this.inputPaths = storage.inputPaths;
                Destroy(storage.gameObject);
                storage = this;
            }
            DontDestroyOnLoad(this.gameObject);

        }


    }
}


