---
title: OSCの送信（OscClient）
sidebar_position: 0
description: ExtremeOsc is C# implemetation of OSC (Open Sound Control) for Unity.
---

## サポートしている送信方法

**⭕ `[OscPackable]`を使用しているクラス、または`ExtremeOsc.IOscPackable`を実装しているクラス**

```csharp
[OscPackable]
public partial class ExampleData
{
    [OscElementAt(0)]
    public int IntValue { get; set; }
    [OscElementAt(1)]
    public float FloatValue { get; private set; }
    [OscElementAt(2)]
    public string StringValue;

    public ExampleData()
    {
        IntValue = 0;
        FloatValue = 0.0f;
        StringValue = string.Empty;
    }
}

var data = new ExampleData();

var client = new OscClient("127.0.0.1", 5555);
client.Send("/exmaple", data);
```

**⭕ [サポートしている型](/docs/#サポートしている型)を含む`object[]`**

```csharp
var objects = new object[]
{
    12345,
    123.45f,
    "Hello, World!",
};

client.Send("/example", objects)
```

**⭕ 引数なし**

```csharp
client.Send("/example");
```

## サポートしていない送信方法

**❎ ネストされた`[OscPackable]`を使用しているクラス**

```csharp
public class ParentClass
{
    // ❎
    [OscPackable]
    public class ExampleData
    {
        // :(
    }
}
```

**❎ [サポートしている型](/docs/#サポートしている型)にはないクラス・型**

:::info
UnityEngineに含まれる型は、今後サポートする予定です。
:::