using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ITI.Work
{
    public class ITIDictionary<TKey,TValue> : IEnumerable<KeyValuePair<TKey,TValue>>
    {
        int _count;
        Node[] _buckets;
        const int _fillLimit = 10;


        class Node
        {
            public readonly TKey Key;
            public TValue Value;
            public Node Next;

            public Node( TKey key )
            {
                Key = key;
            }
        }

        public ITIDictionary()
        {
            _buckets = new Node[7];
        }

        public int Count => _count;

        public void Add( TKey key, TValue value )
        {
            int idx = GetBucketIndex( key );
            var (_, exists) = FindNodeInBucket( idx, key );
            if( exists != null ) throw new InvalidOperationException();
            AddNewNode( key, value, idx );
        }

        void AddNewNode( TKey key, TValue value, int idx )
        {
            _buckets[idx] = new Node( key ) { Value = value, Next = _buckets[idx] };
            ++_count;
            if( _buckets.Length * _fillLimit < _count )
            {
                ResizeBuckets();
            }
        }

        void ResizeBuckets()
        {
            var oldBuckets = _buckets;
            _buckets = new Node[GetNextAllocSize( oldBuckets.Length )];

            IEnumerable<Node> FromOldBuckets( Node[] old )
            {
                for( int i = 0; i < old.Length; ++i )
                {
                    Node current = old[i];
                    while( current != null )
                    {
                        Node next = current.Next;
                        yield return current;
                        current = next;
                    }
                }
            }

            foreach( var n in FromOldBuckets( oldBuckets ) )
            {
                int newIdx = GetBucketIndex( n.Key );
                n.Next = _buckets[newIdx];
                _buckets[newIdx] = n;
            }
        }

        static int GetNextAllocSize( int length ) => length * 2;

        public void Remove( TKey key )
        {
            int idx = GetBucketIndex( key );
            var (previous,exists) = FindNodeInBucket( idx, key );
            if( exists == null ) return;
            if( previous == null )
            {
                _buckets[idx] = exists.Next;
            }
            else
            {
                previous.Next = exists.Next;
            }
            --_count;
        }

        public TValue this[ TKey key ]
        {
            get
            {
                int idx = GetBucketIndex( key );
                var (_, exists) = FindNodeInBucket( idx, key );
                if( exists != null ) return exists.Value;
                throw new KeyNotFoundException();
            }
            set
            {
                int idx = GetBucketIndex( key );
                var (_, exists) = FindNodeInBucket( idx, key );
                if( exists != null )
                {
                    exists.Value = value;
                }
                else
                {
                    AddNewNode( key, value, idx );
                }
            }
        }

        int GetBucketIndex( TKey key )
        {
            int h = key?.GetHashCode() ?? 0;
            return Math.Abs( h % _buckets.Length );
        }

        (Node Previous, Node Node) FindNodeInBucket( int idx, TKey key )
        {
            Node n = _buckets[idx];
            Node previous = null;
            while( n != null )
            {
                if( n.Key?.Equals( key ) ?? key == null ) break;
                previous = n;
                n = n.Next;
            }
            return (previous, n);
        }

        class E : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            readonly ITIDictionary<TKey, TValue> _papa;
            int _currentIdx;
            Node _currentNode;

            public E( ITIDictionary<TKey, TValue> papa )
            {
                _papa = papa;
            }

            public KeyValuePair<TKey, TValue> Current => new KeyValuePair<TKey, TValue>( _currentNode.Key, _currentNode.Value );

            object IEnumerator.Current => Current;

            public void Dispose() {}

            public bool MoveNext()
            {
                _currentNode = _currentNode?.Next;
                while( _currentNode == null )
                {
                    if( _currentIdx == _papa._buckets.Length - 1 ) return false;
                    ++_currentIdx;
                    _currentNode = _papa._buckets[_currentIdx];
                }
                Debug.Assert( _currentNode != null && _currentIdx < _papa._buckets.Length );
                return true;
            }

            public void Reset() => throw new NotSupportedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new E( this );
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
