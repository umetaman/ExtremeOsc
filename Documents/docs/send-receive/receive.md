---
title: Receive OSC (OscServer)
sidebar_position: 1
---

## Supported Reception Methods

**⭕ Accepts a class using `[OscPackable]` or implementing `ExtremeOsc.IOscPackable` as an argument**

```csharp
[OscCallback("/example")]
public void OnExample(string address, ExampleData data)
{

}
```

**⭕ Accepts `object[]` as an argument**

```csharp
[OscCallback("/example/objects")]
public void OnExampleObjects(string address, object[] objects)
{

}
```

**⭕ Argument types match the order of received data**

```csharp
[OscCallback("/example/arguments")]
public void OnExampleArguments(string address, int value0, float value1, string value2, bool value3)
{

}
```

**⭕ No arguments**

```csharp
[OscCallback("/example/noargument")]
public void OnExampleNoArgument(string address)
{

}
```

**⭕ Specify multiple addresses for a single function**

```csharp
[OscCallback("/example")]
[OscCallback("/example/another")]
private void OnExample(string address, ExampleData data)
{

}
```

## Unsupported Reception Methods

**Constraints**

- Only one callback function per address
- Only one `[OscPackable]` class can be used as an argument

**❎ Assigning an address to multiple functions**

```csharp
[OscCallback("/example")]
private void OnExample(string address)
{

}

// ❎
[OscCallback("/example/another")]
private void OnExampleAnother(string address)
{

}
```

**❎ Using multiple `[OscPackable]` classes as arguments**

```csharp
[OscCallback("/example/")]
private void OnExample(string address, ExampleData data, /* ❎ */ExampleData2 data2)
{

}
```