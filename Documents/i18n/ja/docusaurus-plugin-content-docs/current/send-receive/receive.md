---
title: OSCの受信（OscServer）
sidebar_position: 1
---

## サポートしている受信方法

**⭕ `[OscPackable]`を使用しているクラス、または`ExtremeOsc.IOscPackable`を実装しているクラスを引数にとる**

```csharp
[OscCallback("/example")]
public void OnExample(string address, ExampleData data)
{

}
```

**⭕ `object[]`を引数にとる**

```csharp
[OscCallback("/example/objects")]
public void OnExampleObjects(string address, object[] objects)
{

}
```

**⭕ 引数の型の順番が受信したデータと一致している**

```csharp
[OscCallback("/example/arguments")]
public void OnExampleArguments(string address, int value0, float value1, string value2, bool value3)
{

}
```

**⭕ 引数なし**

```csharp
[OscCallback("/example/noargument")]
public void OnExampleNoArgument(string address)
{

}
```

**⭕ 1つの関数につき複数のアドレスを指定する**

```csharp
[OscCallback("/example")]
[OscCallback("/example/another")]
private void OnExample(string address, ExampleData data)
{

}
```

## サポートしていない受信方法

**制約**

- アドレス1つにきコールバック関数は1つのみ
- `[OscPackable]`クラスは引数に1つまで

**❎ アドレスを複数の関数に指定する**

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

**❎ 複数の`[OscPackable]`クラスを引数に取る**

```csharp
[OscCallback("/example/")]
private void OnExample(string address, ExampleData data, /* ❎ */ExampleData2 data2)
{

}
```