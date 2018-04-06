using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Work
{
    public class ITIListInt
    {
        int[] _tab;
        int _count;

        public ITIListInt()
        {
            _tab = new int[4];
        }

        public ITIListInt( int initialCapacity )
        {
            if( initialCapacity <= 0 ) throw new ArgumentNullException( nameof( initialCapacity ), "Must be positive." );
            _tab = new int[initialCapacity];
        }

        public int Count => _count;

        public int this[ int index ]
        {
            get
            {
                if( index < 0 || index >= _count ) throw new IndexOutOfRangeException();
                return _tab[index];
            }
            set
            {
                if( index < 0 || index >= _count ) throw new IndexOutOfRangeException();
                _tab[index] = value;
            }
        }

        public void RemoveAt( int index )
        {
            if( index < 0 || index >= _count ) throw new IndexOutOfRangeException();
            Array.Copy( _tab, index + 1, _tab, index, _count - index );
            --_count;
        }

        public void InsertAt( int index, int value )
        {
            if( index < 0 || index > _count ) throw new IndexOutOfRangeException();

            if( _count == _tab.Length )
            {
                this.Add( value );
            }
            else
            {
                Array.Copy( _tab, index, _tab, index + 1, _count - index );
                _tab[index] = value;
                _count++;
            }
        }

        public int IndexOf( int i )
        {
            for( int x = 0; x < _count; ++x )
            {
                if( _tab[x] == i ) return x;
            }
            return -1;
        }

        public void Add( int i )
        {
            if( _count == _tab.Length )
            {
                var newTab = new int[_tab.Length * 2];
                Array.Copy( _tab, newTab, _count );
                _tab = newTab;
            }
            _tab[_count++] = i;
        }
    }

    public class ITIListString
    {

        public void Add( string s )
        {
        }
    }
}
