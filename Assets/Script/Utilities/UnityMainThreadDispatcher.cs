namespace Script.Utilities
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> queue = new Queue<Action>();

        void Update()
        {
            lock (queue)
            {
                while (queue.Count > 0)
                    queue.Dequeue().Invoke();
            }
        }

        public static void Enqueue(Action action)
        {
            lock (queue)
            {
                queue.Enqueue(action);
            }
        }
    }

}