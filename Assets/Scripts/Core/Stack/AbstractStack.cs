using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    public abstract class AbstractStack : MonoBehaviour
    {
        public enum Type
        {
            Add = 0,
            Subtract = 1
        }
        private Type typeStack = Type.Add;
        public Type TypeStack { get => typeStack; }

        public abstract void Interact(Player player);
    }
}
