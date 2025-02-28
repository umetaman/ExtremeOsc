---
title: OSCの書き込み
sidebar_position: 0
---

任意のタイミングで`byte[]`にOSCを書き込みたいときは、`ExtremeOsc.OscWriter`を使います。

低レベルAPIとして実装しているので、少しわかりにくいかもしれません。

---

例えば、次のようなOSCを書き込みたいとき

```csharp title="OSC"
/example ,ifsT 12345 123.45f "Hello, World!"
```

`ExtremeOsc.OscWriter`を使って、ひとつずつ値を書き込みます。

```csharp title="値ごとに書き込む"
int value0 = 12345;
float value1 = 123.45f;
string value2 = "Hello, World!";
bool value3 = false;

// 送信するbyte[]
byte[] buffer;

int offset = 0;
OscWriter.WriteString(buffer, "/example", ref offset);

// address + ','
int offsetTagTypes = offset + 1;
// タグタイプ
OscWriter.WriteString(buffer, ",ifsT")

OscWriter.WriteInt32(buffer, value0, ref offset);
offsetTagTypes++;
OscWriter.WriteFloat(buffer, value1, ref offset);
offsetTagTypes++;
OscWriter.WriteStringUtf8(buffer, value2, ref offset);
offsetTagTypes++;
OscWriter.WriteBoolean(buffer, value3, offsetTagTypes);
offsetTagTypes++;
```

```csharp title="可変長の引数として書き込む"
OscWriter.Write(buffer, "/example", value0, value1, value2, value3);
```

```csharp title="引数なし"
OscWriter.Write(buffer, "/example");
```

以上のコードは、`[OscPackable]`を使えば自動的に生成されるので、ほとんどの場合は必要ないと思います。

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