---
title: Reading OSC
sidebar_position: 0
---

If you want to read OSC from `byte[]` at any given time, use `ExtremeOsc.OscReader`.

Since it is implemented as a low-level API, it may be somewhat difficult to understand.

---

For example, if you want to read the following OSC:

```csharp title="OSC"
/example ,ifsT 12345 123.45f "Hello, World!"
```

You can use `ExtremeOsc.OscReader` to read values one by one.

```csharp title="Reading values one by one"
// Received byte[]
byte[] buffer;

int offset = 0;
string address = OscReader.ReadString(buffer, ref offset);

// address + ','
int offsetTagTypes = offset + 1;

int value0 = OscReader.ReadInt32(buffer, ref offset);
offsetTagTypes++;

float value1 = OscReader.ReadFloat(buffer, ref offset);
offsetTagTypes++;

string value2 = OscReader.ReadString(buffer, ref offset);
offsetTagTypes++;

// For values represented directly by tag types, use the tag type offset.
bool value3 = OscReader.ReadBoolean(buffer, offsetTagTypes);
offsetTagTypes++;
```

```csharp title="Pre-reading before retrieving values"
var reader = OscReader.Read(buffer);

// Specify index
int value0 = reader.GetAsInt32(0);
float value1 = reader.GetAsFloat(1);
string value2 = reader.GetAsString(2);
bool value3 = reader.GetAsBoolean(3);

// Retrieve as object[]
object[] values = reader.GetAsObjects();
```

In most cases, these lines of code are unnecessary as `[OscPackable]` can automatically generate them.

## OscReader

```csharp
public static int ReadStringLength(byte[] buffer, int offset);
public static string ReadString(byte[] buffer, ref int offset);
public static bool IsBundle(byte[] buffer, ref int offset);
public static int ReadInt32(byte[] buffer, ref int offset);
public static long ReadInt64(byte[] buffer, ref int offset);
public static float ReadFloat(byte[] buffer, ref int offset);
public static byte[] ReadBlob(byte[] buffer, ref int offset);
public static ulong ReadULong(byte[] buffer, ref int offset);
public static double ReadDouble(byte[] buffer, ref int offset);
public static Color32 ReadColor32(byte[] buffer, ref int offset);
public static char ReadChar(byte[] buffer, ref int offset);
public static bool ReadBoolean(byte[] buffer, int offset);
public static Infinitum ReadInfinitum(byte[] buffer, int offset);
public static Nil ReadNil(byte[] buffer, int offset);
public static ulong ReadTimeTagAsULong(byte[] buffer, ref int offset);
public static DateTime ReadTimeTagAsDateTime(byte[] buffer, ref int offset);
public static int ReadMidi(byte[] buffer, ref int offset);
```