---
sidebar_position: 0
id: about-extreme-osc
title: ExtremeOSCとは
slug: /
description: ExtremeOsc is C# implemetation of OSC (Open Sound Control) for Unity.
---

# ExtremeOSCとは

**ExtremeOsc**は、UnityでOSC(Open Sound Control)を扱うためのライブラリです。

## 主な機能

OSCの読み書きにあたって、今まで書かざるを得なかったボイラープレート的なコードをSourceGeneratorによって自動生成します。

- クラスまたは構造体に対して、自動的にOSC信号へ変換するメソッドを追加
- クラスの関数にOSCのアドレスを指定すると、自動的にOSC信号に対してコールバックを実行

## サポートしている型

[OpenSoundControl Specification 1.0](https://opensoundcontrol.stanford.edu/spec-1_0.html) にある基本的な型をサポートしています。

| Tag | C# Type |
| --- | --- |
| i | int |
| h | long |
| f | float |
| s | string |
| S | Symbol |
| b | byte[] |
| d | double |
| c | char |
| r | UnityEngine.Color32 |
| t | TimeTag (DateTime) |
| T | bool (true) |
| F | bool (false) |
| N | Nil |
| I | Infinitum |
| m | MIDI as int |