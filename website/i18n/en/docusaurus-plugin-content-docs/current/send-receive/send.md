---
title: Sending OSC (OscClient)
sidebar_position: 0
description: ExtremeOsc is a C# implementation of OSC (Open Sound Control) for Unity.
---

## Supported Sending Methods

**⭕ Using a class with `[OscPackable]` or implementing `ExtremeOsc.IOscPackable`**

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
client.Send("/example", data);
```

**⭕ Using `object[]` containing [supported types](/docs/about/#supported-types)**

```csharp
var objects = new object[]
{
    12345,
    123.45f,
    "Hello, World!",
};

client.Send("/example", objects);
```

**⭕ No arguments**

```csharp
client.Send("/example");
```

## Unsupported Sending Methods

**❎ Using a nested class with `[OscPackable]`**

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

**❎ Using classes or types not included in [supported types](/docs/about/#supported-types)**

:::info
Types included in UnityEngine are planned to be supported in the future.
:::
