---
title: OSCの読み込み
sidebar_position: 0
---

任意のタイミングで`byte[]`からOSCを読み出したいときは、`ExtremeOsc.OscReader`を使います。

低レベルAPIとして実装しているので、少しわかりにくいかもしれません。

---

例えば、次のようなOSCを読み込みたいとき

```csharp title="OSC"
/example ,ifsT 12345 123.45f "Hello, World!"
```

`ExtremeOsc.OscReader`を使って、ひとつずつ値を読み込みます。

```csharp title="値ごとに読み出す"
// 受信したbyte[]
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

// タグタイプが直接値を示すものは、タグタイプ用のオフセットを使います。
bool value3 = OscReader.ReadBoolean(buffer, offsetTagTypes);
offsetTagTypes++;
```

```csharp title="事前に読み出してから取得する"
var reader = OscReader.Read(buffer);

// indexを指定する
int value0 = reader.GetAsInt32(0);
float value1 = reader.GetAsFloat(1);
string value2 = reader.GetAsString(2);
bool value3 = reader.GetAsBoolean(3);

// object[]として取得する
object[] values = reader.GetAsObjects();
```

以上のコードは、`[OscPackable]`を使えば自動的に生成されるので、ほとんどの場合は必要ないと思います。

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