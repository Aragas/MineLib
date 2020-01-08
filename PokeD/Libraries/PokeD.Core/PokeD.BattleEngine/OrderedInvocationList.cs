using System;
using System.Collections.Generic;

namespace PokeD.BattleEngine
{
    public enum InvocationOrder { Normal, Begin, End }

    public class OrderedInvocationList : IDisposable
    {
        private object _listLock { get; } = new object();

        private List<Action> InvocationListBegin= new List<Action>();
        private List<Action> InvocationListNormal = new List<Action>();
        private List<Action> InvocationListEnd = new List<Action>();


        public void Add(Action action)
        {
            lock (_listLock)
            {
                InvocationListNormal.Add(action);
            }
        }
        public void Add(Action action, InvocationOrder InvocationOrder)
        {
            lock (_listLock)
            {
                switch (InvocationOrder)
                {
                    case InvocationOrder.Begin:
                        InvocationListBegin.Add(action);
                        break;

                    case InvocationOrder.Normal:
                        InvocationListNormal.Add(action);
                        break;

                    case InvocationOrder.End:
                        InvocationListEnd.Add(action);
                        break;
                }
            }
        }
        public void Remove(Action action)
        {
            lock (_listLock)
            {
                InvocationListNormal.Remove(action);
            }
        }
        public void Remove(Action action, InvocationOrder InvocationOrder)
        {
            lock (_listLock)
            {
                switch (InvocationOrder)
                {
                    case InvocationOrder.Begin:
                        InvocationListBegin.Remove(action);
                        break;

                    case InvocationOrder.Normal:
                        InvocationListNormal.Remove(action);
                        break;

                    case InvocationOrder.End:
                        InvocationListEnd.Remove(action);
                        break;
                }
            }
        }


        public void Invoke()
        {
            lock (_listLock)
            {
                foreach (var action in InvocationListBegin)
                    action.Invoke();

                foreach (var action in InvocationListNormal)
                    action.Invoke();

                foreach (var action in InvocationListEnd)
                    action.Invoke();
            }
        }


        public void Dispose()
        {
            lock (_listLock)
            {
                InvocationListBegin.Clear();
                InvocationListNormal.Clear();
                InvocationListEnd.Clear();
            }
        }
    }
    public class OrderedInvocationList<T> : IDisposable
    {
        private object _listLock { get; } = new object();

        private List<Action<T>> InvocationListBegin = new List<Action<T>>();
        private List<Action<T>> InvocationListNormal = new List<Action<T>>();
        private List<Action<T>> InvocationListEnd = new List<Action<T>>();


        public void Add(Action<T> action)
        {
            lock (_listLock)
            {
                InvocationListNormal.Add(action);
            }
        }
        public void Add(Action<T> action, InvocationOrder InvocationOrder)
        {
            lock (_listLock)
            {
                switch (InvocationOrder)
                {
                    case InvocationOrder.Begin:
                        InvocationListBegin.Add(action);
                        break;

                    case InvocationOrder.Normal:
                        InvocationListNormal.Add(action);
                        break;

                    case InvocationOrder.End:
                        InvocationListEnd.Add(action);
                        break;
                }
            }
        }
        public void Remove(Action<T> action)
        {
            lock (_listLock)
            {
                InvocationListNormal.Remove(action);
            }
        }
        public void Remove(Action<T> action, InvocationOrder InvocationOrder)
        {
            lock (_listLock)
            {
                switch (InvocationOrder)
                {
                    case InvocationOrder.Begin:
                        InvocationListBegin.Remove(action);
                        break;

                    case InvocationOrder.Normal:
                        InvocationListNormal.Remove(action);
                        break;

                    case InvocationOrder.End:
                        InvocationListEnd.Remove(action);
                        break;
                }
            }
        }


        public void Invoke(T obj)
        {
            lock (_listLock)
            {
                foreach (var action in InvocationListBegin)
                    action.Invoke(obj);

                foreach (var action in InvocationListNormal)
                    action.Invoke(obj);

                foreach (var action in InvocationListEnd)
                    action.Invoke(obj);
            }
        }


        public void Dispose()
        {
            lock (_listLock)
            {
                InvocationListBegin.Clear();
                InvocationListNormal.Clear();
                InvocationListEnd.Clear();
            }
        }
    }
    public class OrderedInvocationList<T1, T2> : IDisposable
    {
        private object _listLock { get; } = new object();

        private List<Action<T1, T2>> InvocationListBegin = new List<Action<T1, T2>>();
        private List<Action<T1, T2>> InvocationListNormal = new List<Action<T1, T2>>();
        private List<Action<T1, T2>> InvocationListEnd = new List<Action<T1, T2>>();


        public void Add(Action<T1, T2> action)
        {
            lock (_listLock)
            {
                InvocationListNormal.Add(action);
            }
        }
        public void Add(Action<T1, T2> action, InvocationOrder InvocationOrder)
        {
            lock (_listLock)
            {
                switch (InvocationOrder)
                {
                    case InvocationOrder.Begin:
                        InvocationListBegin.Add(action);
                        break;

                    case InvocationOrder.Normal:
                        InvocationListNormal.Add(action);
                        break;

                    case InvocationOrder.End:
                        InvocationListEnd.Add(action);
                        break;
                }
            }
        }
        public void Remove(Action<T1, T2> action)
        {
            lock (_listLock)
            {
                InvocationListNormal.Remove(action);
            }
        }
        public void Remove(Action<T1, T2> action, InvocationOrder InvocationOrder)
        {
            lock (_listLock)
            {
                switch (InvocationOrder)
                {
                    case InvocationOrder.Begin:
                        InvocationListBegin.Remove(action);
                        break;

                    case InvocationOrder.Normal:
                        InvocationListNormal.Remove(action);
                        break;

                    case InvocationOrder.End:
                        InvocationListEnd.Remove(action);
                        break;
                }
            }
        }


        public void Invoke(T1 obj1, T2 obj2)
        {
            lock (_listLock)
            {
                foreach (var action in InvocationListBegin)
                    action.Invoke(obj1, obj2);

                foreach (var action in InvocationListNormal)
                    action.Invoke(obj1, obj2);

                foreach (var action in InvocationListEnd)
                    action.Invoke(obj1, obj2);
            }
        }


        public void Dispose()
        {
            lock (_listLock)
            {
                InvocationListBegin.Clear();
                InvocationListNormal.Clear();
                InvocationListEnd.Clear();
            }
        }
    }
}
