<div align="center">

![Banner](./banner_nobackground.png "KitchenChaos2D")

<a name="readme-top"></a>

# KitchenChaos2D

A port of the famous [**CodeMonkey**][CodeMonkey] 10 hours Unity tutorial: KitchenChaos to 2D using [**GodotEngine**][godot_engine] & [**Aseprite**][aseprite_url] to make assets.

[![Godot version][godot_version_img]][godot_engine]
[![Game Version][game_version_img]][release_page]
[![License][repo_license_img]][repo_license_url]

</div>

## Table of content

- [KitchenChaos2D](#kitchenchaos2d)
  - [Table of content](#table-of-content)
  - [✨ How To Pronounce My Name](#-how-to-pronounce-my-name)
  - [📰 About](#-about)
  - [🔰 Get Started](#-get-started)
  - [💻 Code Changes](#-code-changes)
    - [Singleton vs Autoload](#singleton-vs-autoload)
      - [C# Singleton](#c-singleton)
      - [Autoload](#autoload)
    - [GetComponent() vs GetNode()](#getcomponent-vs-getnode)
      - [GetComponent](#getcomponent)
      - [GetNode](#getnode)
    - [EventHandler vs \[Signal\]](#eventhandler-vs-signal)
    - [In-script counter vs Timer node](#in-script-counter-vs-timer-node)
      - [In-script counter](#in-script-counter)
      - [Timer node (better option 90% of the time)](#timer-node-better-option-90-of-the-time)
  - [⚠️ License](#️-license)

## ✨ How To Pronounce My Name

My birth name is Thai, which is a Vietnamese name. I got my nick name Thaiminh2022 when I was 3 years of age playing games with a brother named Minh.

- Thai: **tie** with a **stress**.
- Minh: the word **min**

## 📰 About

- In January, [**CodeMonkey**][CodeMonkey] released one of his most comprehensive tutorial for game development for **free**.
- However, this tutorial use [**Unity**][unity_engine] as the game engine.
- This is my attempt to port the game to the godot engine in **2D**

> 📝 Notes </br>
> I create this repo to document changes I had to make to the source code for it to work with the Godot engine in case anyone have the same problem. </br>
>**If you want to know what are the changes, please prefer to the next next section**.

## 🔰 Get Started

- This repo reflects my journey of learning how to do game development using the **godot engine**.
- You are **free to use the repo as a starting point**

```bash
# HTTPS
git clone https://github.com/thaiminh2022/KitchenChaos2D.git

# github cli
gh repo clone thaiminh2022/KitchenChaos2D
```

> **! WARNING** </br>
> Some practices used in the original [**CodeMonkey**][CodeMonkey] tutorial video have to be changed because of dimension and engine differences. </br>
> **If you want to know what are the changes, please prefer to the next section**.

## 💻 Code Changes

***If you think you have better ideas for these listed problem, see contributing.***

### Singleton vs Autoload

In short, if you want to use singleton for only a scene, use normal C# singleton. If you want your singleton to persist after scene change, use autoload.

#### C# Singleton

```cs
public class ClassName {
  public static ClassName Instance {get; private set;} 

  // Enter tree get calls when this node enter scene tree, or change it's parents.
  public override void _EnterTree() {
    Instance = this;
  }
}
```

If you are using this method, you **HAVE TO** reset the singleton everytime you re-enter your scene, since godot does not do that automatically for you.
One method is to have a reseter that automatically reset everytime before you enter the scene that contains the singleton.

```cs
public class ClassName {
  public static ClassName Instance {get; private set;} 
  
  public static void ResetStatic() {
    Instance = null
  }
  // Enter tree get calls when this node enter scene tree, or change it's parents.
  public override void _EnterTree() {
    Instance = this;
  }
}

public class AnotherClassInAnotherScene {
  public override void _Ready() {
    ClassName.Instance.ResetStatic();
  }
}
```

#### Autoload

Autoload in godot is better explained using the [docs][autoload_docs]

### GetComponent() vs GetNode()

These are Unity and Godot way to basicly get the attached script of a game object, repsectively. However, there are clear differences in syntax and the way both engine work.

```cs
using UnityEngine; // GetComponent exists in UnityEngine
using Godot; // GetNode exists in Godot

// Unity
T component = GetComponent<T>();

// Godot
T node = GetNode<T>(pathToNode);
```

#### GetComponent

- Get the component attach to the current gameobject.
- If there are multiples component with the same type, it will get the first component.

> - **GetComponent returns null if there're no component, no error is thrown**
> - **GetComponent can get interface  that given to the component**

#### GetNode

- Get the children node of the node where the function is called.
- You have to specify the path, multiple component will results in the first instance.

> - **GetNode returns null if there're no component in the specified path, no error is thrown**
> - **GetNode cannot get interface  that given to the component**

**Solution** </br>
Use pattern matching instead

```cs
interface IInterface {
    // A interface
}

if (component is IInterface _interface) {
    // Do something with _interface
}
```

### EventHandler vs [Signal]

- EventHandler is a standard c# delegate use for **event**, see **Observer pattern**.
- Signal is an attribute, which is also Godot own take on **event**, use EventHandler under the hood with some differences.

```csharp
using System; // EventHandler exists in System
using Godot; // Signal exists in System  

private event EventHandler<EventArgs> OnEventHappend;

// Signal must end with EventHandler
[Signal] private delegate void EventHappenedEventHandler();
```

|                               |       EventHandler       |                   Signal                    |
| ----------------------------- | :----------------------: | :-----------------------------------------: |
| Interaction with Godot Engine |      No interaction      |          Showed in the signal tab           |
| Interaction with other nodes  |     Only in C# files     | Interact with anything using the signal tab |
| Usage in interface            | Can be use in interfaces |        Cannot be used in interfaces         |

I originally use signal as it was more conventional for Godot, however CodeMonkey later use EventHandler in an interface, which sinal can't so I switched back to EventHandler. </br>

>**Warning** </br>
> If you are using a static event, you **HAVE TO** unsubcribe before the object goes out of scope. Most common way is do it in the _ExitTree override method. </br>
> Another good option is to have a ResetScript that does that automaticly everytime before you enter the game scene.

### In-script counter vs Timer node

It's really common in game developement to have a counter varible which track the time before some actions happen. This can be done by using a in-script counter varible or the Timer node.

For more details, checkout [**Timer Docs**][timer_docs]

#### In-script counter

```cs
using Godot;

float counter = 0; // The counting time (in seconds)
float counterMax = 3; // The maxium time (in seconds)

public override void _Process(delta) {
  counter += delta;

  if (counter >= counterMax) {
    // Do some actions

    // Reset the counter
    counter = 0;
  }
}
```

This method is very common if you are comming from Unity. However, it requires a lot of boilerplate code.

#### Timer node (better option 90% of the time)

```cs
using Godot;

[Export] Timer timer; // drag and setup the timer node in the editor


override void _Ready() {
  timer.Timeout += TimerTimeout;
}

private void TimerTimeout() {
  // This will be call after setuped amount of second in the editor.
}

private override void _ExitTree() {
  // It's also good to unsubcribe before the node goes out of scope
  timer.Timeout -= TimerTimeout;
}

```

This should be the better way. However it's only recommended to use Timer node if your wait time is **more than 0.05 seconds**. If your wait time is longer than that, please prefer the in-script method.

## ⚠️ License

MIT License

Copyright (c) 2023 Thaiminh2022

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

<!-- Game links -->
[godot_version_img]: https://img.shields.io/badge/Godot-4.2+-00ADD8?style=for-the-badge&logo=godotengine
[release_page]: https://github.com/thaiminh2022/kitchenchaos2d/releases
[game_version_img]: https://img.shields.io/badge/Version-0.1Beta-success?style=for-the-badge&logo=none

<!-- Author links -->

<!-- Others -->
[godot_engine]: https://godotengine.org
[unity_engine]: https://unity.com
[aseprite_url]: https://aseprite.org
[CodeMonkey]: https://youtube.com/@CodeMonkeyUnity

[repo_license_img]: https://img.shields.io/badge/LICENSE-MIT-yellow?style=for-the-badge&logo=none
[repo_license_url]: ./LICENSE

<!-- Godot links -->
[timer_docs]: https://docs.godotengine.org/en/stable/classes/class_timer.html
[autoload_docs]: https://docs.godotengine.org/en/stable/tutorials/scripting/singletons_autoload.html
