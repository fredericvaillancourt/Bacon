using System.Collections;
using System.Runtime.CompilerServices;

namespace Bacon.Generator;

internal readonly struct EquatableArray<T> : IEquatable<EquatableArray<T>>, IEnumerable<T>
    where T : IEquatable<T>
{
    private readonly T[] _array;

    public EquatableArray(T[]? array)
    {
        _array = array ?? [];
    }

    public ref readonly T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref _array[index];
    }

    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array.Length;
    }

    public bool Equals(EquatableArray<T> other)
    {
        return _array.SequenceEqual(other._array);
    }

    public override bool Equals(object? obj)
    {
        return obj is EquatableArray<T> other && Equals(other);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return ((IEnumerable<T>)_array).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _array.GetEnumerator();
    }

    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right)
    {
        return !left.Equals(right);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = default;

        foreach (T item in _array)
        {
            hashCode.Add(item);
        }

        return hashCode.ToHashCode();
    }

    public static implicit operator EquatableArray<T>(T[]? array)
    {
        return new EquatableArray<T>(array);
    }
}