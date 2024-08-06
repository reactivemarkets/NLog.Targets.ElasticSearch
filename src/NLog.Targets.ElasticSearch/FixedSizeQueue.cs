using System;
using System.Collections.Generic;
using System.Text;

namespace NLog.Targets.ElasticSearch
{
    /// <summary>
    /// A thread-safe fixed size queue.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    public class FixedSizeQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly int _maxSize;
        private readonly object _lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedSizeQueue{T}"/> class.
        /// </summary>
        /// <param name="maxSize">The maximum size of the queue.</param>
        /// <exception cref="ArgumentException">Thrown when maxSize is less than or equal to zero.</exception>
        public FixedSizeQueue(int maxSize)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentException("Max size must be greater than zero", nameof(maxSize));
            }
            _maxSize = maxSize;
        }

        /// <summary>
        /// Adds an item to the queue. If the queue is full, the oldest item is removed.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Enqueue(T item)
        {
            lock (_lockObject)
            {
                if (_queue.Count == _maxSize)
                {
                    _queue.Dequeue();
                }
                _queue.Enqueue(item);
            }
        }

        /// <summary>
        /// Removes and returns the item at the beginning of the queue.
        /// </summary>
        /// <returns>The item removed from the beginning of the queue.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public T Dequeue()
        {
            lock (_lockObject)
            {
                if (_queue.Count == 0)
                {
                    throw new InvalidOperationException("Queue is empty.");
                }
                return _queue.Dequeue();
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the queue.
        /// </summary>
        public int Count
        {
            get
            {
                lock (_lockObject)
                {
                    return _queue.Count;
                }
            }
        }

        /// <summary>
        /// Removes all items from the queue.
        /// </summary>
        public void Clear()
        {
            lock (_lockObject)
            {
                _queue.Clear();
            }
        }

        /// <summary>
        /// Determines whether the queue contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the queue.</param>
        /// <returns>true if item is found in the queue; otherwise, false.</returns>
        public bool Contains(T item)
        {
            lock (_lockObject)
            {
                return _queue.Contains(item);
            }
        }

        /// <summary>
        /// Returns the item at the beginning of the queue without removing it.
        /// </summary>
        /// <returns>The item at the beginning of the queue.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public T Peek()
        {
            lock (_lockObject)
            {
                if (_queue.Count == 0)
                {
                    throw new InvalidOperationException("Queue is empty.");
                }
                return _queue.Peek();
            }
        }

        /// <summary>
        /// Copies the elements of the queue to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the queue.</returns>
        public T[] ToArray()
        {
            lock (_lockObject)
            {
                return _queue.ToArray();
            }
        }
    }
}
