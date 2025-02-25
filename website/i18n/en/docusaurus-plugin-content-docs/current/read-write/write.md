---
title: Writing OSC
sidebar_position: 0
---

If you want to write OSC to `byte[]` at any given time, use `ExtremeOsc.OscWriter`.

Since it is implemented as a low-level API, it may be somewhat difficult to understand.

---

For example, if you want to write the following OSC:

```csharp title="OSC"
/example ,ifsT 12345 123.45f "Hello, World!"
```

You can use `ExtremeOsc.OscWriter` to write values one by one.

```csharp title="Writing values one by one"
int value0 = 12345;
float value1 = 123.45f;
string value2 = "Hello, World!";
bool value3 = false;

// Byte array to send
byte[] buffer;

int offset = 0;
OscWriter.WriteString(buffer, "/example", ref offset);

// address + ','
int offsetTagTypes = offset + 1;
// Tag type
OscWriter.WriteString(buffer, ",ifsT");

OscWriter.WriteInt32(buffer, value0, ref offset);
offsetTagTypes++;
OscWriter.WriteFloat(buffer, value1, ref offset);
offsetTagTypes++;
OscWriter.WriteStringUtf8(buffer, value2, ref offset);
offsetTagTypes++;
OscWriter.WriteBoolean(buffer, value3, offsetTagTypes);
offsetTagTypes++;
```

```csharp title="Writing as variable-length arguments"
OscWriter.Write(buffer, "/example", value0, value1, value2, value3);
```

```csharp title="No arguments"
OscWriter.Write(buffer, "/example");
```

In most cases, these lines of code are unnecessary as `[OscPackable]` can automatically generate them.

## OscWriter

```csharp
public static void WriteString(byte[] buffer, string value, ref int offset);
public static void WriteString(byte[] buffer, byte[] value, ref int offset);
public static void WriteStringUtf8(byte[] buffer, string value, ref int offset);
public static void WriteInt32(byte[] buffer, int value, ref int offset);
public static void WriteInt64(byte[] buffer, long value, ref int offset);
public static void WriteFloat(byte[] buffer, float value, ref int offset);
public static void WriteBlob(byte[] buffer, byte[] value, ref int offset);
public static void WriteBlob(byte[] buffer, Span<byte> value, ref int offset);
public static void WriteULong(byte[] buffer, ulong value, ref int offset);
public static void WriteDouble(byte[] buffer, double value, ref int offset);
public static void WriteColor32(byte[] buffer, Color32 value, ref int offset);
public static void WriteChar(byte[] buffer, char value, ref int offset);
public static void WriteBoolean(byte[] buffer, bool value, int offset);
public static void WriteNil(byte[] buffer, int offset);
public static void WriteNil(byte[] buffer, Nil value, int offset);
public static void WriteInfinitum(byte[] buffer, int offset);
public static void WriteInfinitum(byte[] buffer, Infinitum value, int offset);
public static void WriteTimeTag(byte[] buffer, ulong value, ref int offset);
public static void WriteMidi(byte[] buffer, int value, ref int offset);
```