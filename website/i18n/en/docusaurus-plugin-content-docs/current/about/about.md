---
sidebar_position: 0
id: about-extreme-osc
title: What is ExtremeOsc?
slug: /
description: ExtremeOsc is C# implemetation of OSC (Open Sound Control) for Unity.
---

# What is ExtremeOSC?

**ExtremeOSC** is a library for handling OSC (Open Sound Control) in Unity.

## Main Features

ExtremeOSC utilizes SourceGenerator to automatically generate boilerplate code that was previously required for reading and writing OSC signals.

- Automatically adds methods to convert classes or structs into OSC signals.
- Allows functions in classes to specify an OSC address, enabling automatic callback execution for OSC signals.

## Supported Types

ExtremeOSC supports the fundamental types specified in the [OpenSoundControl Specification 1.0](https://opensoundcontrol.stanford.edu/spec-1_0.html).

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
