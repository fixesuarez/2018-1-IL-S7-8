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
            var (_,exists) = FindNodeInBucket( idx, key );
            if( exists != null ) throw new InvalidOperationException();
            _buckets[idx] = new Node( key ) { Value = value, Next = _buckets[idx] };
            ++_count;
        }

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
                    _buckets[idx] = new Node( key ) { Value = value, Next = _buckets[idx] };
                    ++_count;
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

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
