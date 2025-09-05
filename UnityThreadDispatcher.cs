using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Automapper
{
    public class UnityThreadDispatcher : MonoBehaviour
    {
        private static UnityThreadDispatcher _instance; 
        private readonly Queue<Action> _executionQueue = new Queue<Action>();
        private readonly object _lock = new object();

        public static UnityThreadDispatcher Instance()
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("UnityThreadDispatcher");
                _instance = go.AddComponent<UnityThreadDispatcher>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }

        void Update()
        {
            lock (_lock)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue().Invoke();
                }
            }
        }

        public void Enqueue(Action action)
        {
            lock (_lock)
            {
                _executionQueue.Enqueue(action);
            }
        }

        public Task EnqueueAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();
            
            Enqueue(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            
            return tcs.Task;
        }
    }
}