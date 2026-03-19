using System.Numerics;
using Bacon.Build;

namespace Bacon.Tests;

[Syntax("opt {args}")]
public partial class TestAllTypesOptionalArguments : TestArguments
{
    [Parameter("string")]
    public string? String { get; }

    [Parameter("byte")]
    public byte? Byte { get; }

    [Parameter("sbyte")]
    public sbyte? SByte { get; }

    [Parameter("ushort")]
    public ushort? UShort { get; }

    [Parameter("short")]
    public short? Short { get; }

    [Parameter("uint")]
    public uint? UInt { get; }

    [Parameter("int")]
    public int? Int { get; }

    [Parameter("ulong")]
    public ulong? ULong { get; }

    [Parameter("long")]
    public long? Long { get; }

    [Parameter("uint128")]
    public UInt128? UInt128 { get; }

    [Parameter("int128")]
    public Int128? Int128 { get; }

    [Parameter("half")]
    public Half? Half { get; }

    [Parameter("float")]
    public float? Float { get; }

    [Parameter("double")]
    public double? Double { get; }

    [Parameter("decimal")]
    public Decimal? Decimal { get; }

    [Parameter("bigInteger")]
    public BigInteger? BigInteger { get; }

    [Parameter("char")]
    public char? Char { get; }

    [Parameter("bool")]
    public bool? Bool { get; }

    [Parameter("enum")]
    public TestEnum? Enum { get; }

    [Parameter("formattable")]
    public TestFormattable? Formattable { get; }
}