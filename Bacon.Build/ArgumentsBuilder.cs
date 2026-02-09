using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Bacon.Build;

public ref struct ArgumentsBuilder
{
    //TODO: This might be too safe ...
    private static readonly SearchValues<char> SearchChars = SearchValues.Create("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");

    private char[]? _arrayToReturnToPool;
    private Span<char> _buffer;
    private int _used;

    public ArgumentsBuilder(Span<char> initialBuffer)
    {
        _arrayToReturnToPool = null;
        _buffer = initialBuffer;
        _used = 0;
    }

    public ArgumentsBuilder(int initialCapacity)
    {
        _arrayToReturnToPool = ArrayPool<char>.Shared.Rent(initialCapacity);
        _buffer = _arrayToReturnToPool;
        _used = 0;
    }

    public int Length => _used;

    public int Capacity => _buffer.Length;

    public void EnsureCapacity(int capacity)
    {
        Debug.Assert(capacity >= 0);

        if ((uint)capacity > (uint)_buffer.Length)
        {
            Grow(capacity);
        }
    }

    public override string ToString()
    {
        return _used > 0 ? _buffer[.._used].ToString() : string.Empty;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddSpaceIfRequired()
    {
        if (_used > 0)
        {
            Append(' ');
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendWithSeparator(string value, char before = ' ', char after = ' ')
    {
        int pos = _used;
        int used = pos + value.Length + 2; // Yes can be 1 too much if not adding before.

        if (used > _buffer.Length)
        {
            Grow(used);
        }

        if (_used > 0)
        {
            _buffer[pos++] = before;
        }

        value.CopyTo(_buffer[pos..]);
        _buffer[pos + value.Length] = after;
        _used = pos;
    }

    public void Append<T>(T value)
        where T : ISpanFormattable
    {
        int pos = _used;

        for (int i = 0; ; ++i)
        {
            if (value.TryFormat(_buffer[pos..], out int charsWritten, "D", NumberFormatInfo.InvariantInfo))
            {
                _used = pos + charsWritten;
                return;
            }

            if (i >= 2)
            {
                throw new InvalidOperationException("Could not write value ...");
            }

            Grow(_buffer.Length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char value)
    {
        int pos = _used;

        if ((uint)pos < (uint)_buffer.Length)
        {
            _buffer[pos] = value;
            _used = pos + 1;
        }
        else
        {
            GrowAndAppend(value);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(string value)
    {
        int pos = _used;
        if (value.Length == 1 && (uint)pos < (uint)_buffer.Length)
        {
            _buffer[pos] = value[0];
            _used = pos + 1;
        }
        else
        {
            AppendSlow(value);
        }
    }

    public void AutoQuoteAppend(string value)
    {
        if (!value.ContainsAnyExcept(SearchChars))
        {
            AppendSlow(value);
            return;
        }

        Append('"');

        if (!value.Contains('"'))
        {
            AppendSlow(value);
        }
        else
        {
            AppendEscaped(value);
        }

        Append('"');
    }

    public void SafeAppend(string value)
    {
        if (!value.ContainsAnyExcept(SearchChars))
        {
            AppendSlow(value);
            return;
        }

        if (!value.Contains('"'))
        {
            AppendSlow(value);
        }
        else
        {
            AppendEscaped(value);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char value, int count)
    {
        int used = _used + count;

        if (used > _buffer.Length)
        {
            Grow(used);
        }

        Span<char> dst = _buffer.Slice(_used, count);
        for (int i = 0; i < dst.Length; i++)
        {
            dst[i] = value;
        }

        _used = used;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(ReadOnlySpan<char> value)
    {
        int pos = _used;
        int used = pos + value.Length;

        if (used > _buffer.Length)
        {
            Grow(used);
        }

        value.CopyTo(_buffer[_used..]);
        _used += used;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<char> AppendSpan(int length)
    {
        int pos = _used;
        int used = pos + length;

        if (used > _buffer.Length)
        {
            Grow(used);
        }

        _used = used;
        return _buffer.Slice(pos, length);
    }

    private void AppendSlow(string value)
    {
        int pos = _used;
        int used = pos + value.Length;

        if (used > _buffer.Length)
        {
            Grow(used);
        }

        value.CopyTo(_buffer[pos..]);
        _used = used;
    }

    private void AppendEscaped(ReadOnlySpan<char> value)
    {
        int pos = _used;
        int used = pos + value.Length + 8; // Can be anything, just some space to start with

        if (used > _buffer.Length)
        {
            Grow(used);
        }

        int extra = _buffer.Length - pos - value.Length;

        foreach (char c in value)
        {
            if (c == '"')
            {
                if (extra == 0)
                {
                    _used = pos;
                    Grow(_buffer.Length);
                    extra = _buffer.Length - pos - value.Length;
                }

                --extra;
                _buffer[pos++] = '\\';
            }

            _buffer[pos++] = c;
        }

        _used = pos;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowAndAppend(char value)
    {
        Grow(1);
        Append(value);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow(int capacity)
    {
        Debug.Assert(capacity > 0);
        Debug.Assert(capacity < _buffer.Length, "Grow called incorrectly, no resize is needed.");

        char[] poolArray = ArrayPool<char>.Shared.Rent((int)Math.Max((uint)capacity, (uint)_buffer.Length * 2));

        _buffer[.._used].CopyTo(poolArray);

        char[]? toReturn = _arrayToReturnToPool;
        _buffer = _arrayToReturnToPool = poolArray;

        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        char[]? toReturn = _arrayToReturnToPool;
        this = default;

        if (toReturn != null)
        {
            ArrayPool<char>.Shared.Return(toReturn);
        }
    }
}