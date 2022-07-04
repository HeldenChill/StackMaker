using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    public abstract class AbstractStack : MonoBehaviour
    {
        public enum Status
        {
            Active = 0,
            Deactive = 1
        }
        public enum ModelType
        {
            High = 0,
            Low = 1
        }

        private Status state = Status.Active;
        private ModelType type = ModelType.High;

        public Status State { 
            get => state; 
            set
            {
                state = value;
            }
        }
        public ModelType Type
        {
            get => type;
            set
            {
                type = value;
            }
        }

        public virtual bool Interact(Player player)
        {
            if(State == Status.Active)
            {
                state = Status.Deactive;
                return true;
            }
            else 
            {
                return false;
            }
        }

        protected void AddObjectStatus(GameObject obj)
        {
            obj.transform.parent = gameObject.transform.parent;
            obj.transform.localPosition = gameObject.transform.localPosition;
        }
    }
}
