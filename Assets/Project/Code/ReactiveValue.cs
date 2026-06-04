using System;
using System.Collections.Generic;

namespace Project.Code
{
    public class ReactiveValue<T> : IReadOnlyValue<T>
    {
        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                {
                    return;
                }

                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        private event Action<T> OnValueChanged;
        private T _value;

        public ReactiveValue(T value)
        {
            _value = value;
        }

        public ReactiveValue()
        {
            _value = default;
        }

        public IDisposable Subscribe(Action<T> cb, bool invokeImmediately = true)
        {
            if (cb == null)
            {
                throw new ArgumentNullException(nameof(cb));
            }
            
            OnValueChanged += cb;
            if(invokeImmediately)
            {
                cb.Invoke(_value);
            }
            
            return new Subscription(() => OnValueChanged -= cb);
        }
        

        private sealed class Subscription : IDisposable
        {
            private Action _unsubscribe;
            
            public Subscription(Action unsubscribe)
            {
                _unsubscribe = unsubscribe;
            }

            public void Dispose()
            {
                _unsubscribe?.Invoke();
                _unsubscribe = null;
            }
        }
    }
}