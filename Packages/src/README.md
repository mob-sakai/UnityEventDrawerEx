# Unity Event Drawer Extension <!-- omit in toc -->

[![](https://img.shields.io/npm/v/com.coffee.event-drawer-ex?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.coffee.event-drawer-ex/)
[![](https://img.shields.io/github/v/release/mob-sakai/UnityEventDrawerEx?include_prereleases)](https://github.com/mob-sakai/UnityEventDrawerEx/releases)
[![](https://img.shields.io/github/release-date/mob-sakai/UnityEventDrawerEx.svg)](https://github.com/mob-sakai/UnityEventDrawerEx/releases)  
![](https://img.shields.io/badge/Unity-2018.4+-57b9d3.svg?style=flat&logo=unity)
[![](https://img.shields.io/github/license/mob-sakai/UnityEventDrawerEx.svg)](https://github.com/mob-sakai/UnityEventDrawerEx/blob/main/LICENSE.txt)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-orange.svg)](http://makeapullrequest.com)
[![](https://img.shields.io/github/watchers/mob-sakai/UnityEventDrawerEx.svg?style=social&label=Watch)](https://github.com/mob-sakai/UnityEventDrawerEx/subscription)
[![](https://img.shields.io/twitter/follow/mob_sakai.svg?label=Follow&style=social)](https://twitter.com/intent/follow?screen_name=mob_sakai)

<< [📝 Description](#-description-) | [📌 Key Features](#-key-features) | [⚙ Installation](#-installation) | [🚀 Getting Started](#-getting-started) | [🤝 Contributing](#-contributing) >>

## 📝 Description <!-- omit in toc -->

![image](https://user-images.githubusercontent.com/12690315/40921624-f43811b6-684a-11e8-96e4-83964730a358.png)

This package extends the `UnityEventDrawer` to display runtime calls in the inspector.

### What is "runtime calls"?

*Button.onClick* and *Toggle.OnChangeValue* etc. are known as [UnityEvent](https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html).  
UnityEvent has two types of callbacks:

* Persistent call
  * You can add callbacks from the inspector.
  * You can add callbacks from a script using *UnityEventTools.AddPersistentListener*.
  * It will be serialized and displayed in the inspector.  
    ![persistent](https://user-images.githubusercontent.com/12690315/40887795-f5eb7ac4-6788-11e8-9e73-6831e3eab08f.png)
* **Runtime call**
  * You can add a callback from a script with *UnityEvent.AddListener*.
  * It will be **not** serialized and displayed in the inspector.
    ![runtime](https://user-images.githubusercontent.com/12690315/40887784-c8c2027a-6788-11e8-83f7-07e38e187cba.png)

### Display your runtime calls

This package extends *UnityEventDrawer* to display runtime calls in the inspector.  
If the runtime call is an instance method, its target is also displayed.  
This plugin supports all events that inherit `UnityEvent<T0> - UnityEvent<T0, T1, T2, T3>` as well as *UnityEvent*.  
Even if the persistent call is empty, *UnityEvent* is displayed compactly in the inspector.

```cs
public class TestBehavior : MonoBehaviour
{
	[System.Serializable] public class TransformUnityEvent : UnityEngine.Events.UnityEvent<Transform>{};

	[SerializeField] TransformUnityEvent onYourCustomEvent = new TransformUnityEvent();
	
	void OnEnable()
	{
		onYourCustomEvent.AddListener(TestTransform);
	}
	
	void TestTransform(Transform t)
	{
		Debug.Log("TestTransform has called : " + t);
	}
}
```
![image](https://user-images.githubusercontent.com/12690315/40887986-d0c2af58-678b-11e8-953c-63116ab2b433.png)

If you like a style of development that makes heavy use of runtime calls (MVP pattern, etc.), this package is for you!

<br><br>

- [📌 Key Features](#-key-features)
- [⚙ Installation](#-installation)
  - [Install via OpenUPM](#install-via-openupm)
  - [Install via UPM (with Package Manager UI)](#install-via-upm-with-package-manager-ui)
  - [Install via UPM (Manually)](#install-via-upm-manually)
  - [Install as Embedded Package](#install-as-embedded-package)
- [🚀 Getting Started](#-getting-started)
- [🤝 Contributing](#-contributing)
  - [Issues](#issues)
  - [Pull Requests](#pull-requests)
  - [Support](#support)
- [License](#license)
- [Author](#author)
- [See Also](#see-also)

<br><br>

## 📌 Key Features

* Displays runtime calls in inspector
* Expands/collapses the runtime call view
* Displays instance method, its target is also displayed
* If the persistent call is empty, displays it compactly
* Supports dark skin
* Supports all components as well as uGUI components like `Button` and `Toggle`  
  ![](https://user-images.githubusercontent.com/12690315/40947741-55bcb0be-689f-11e8-9d86-6d0364ebd155.png)
* Supports `EventTrigger`  
  ![](https://user-images.githubusercontent.com/12690315/41216786-c12a1c10-6d90-11e8-8a13-00e5b27de573.png)

<br><br>

## ⚙ Installation

### Install via OpenUPM

- This package is available on [OpenUPM](https://openupm.com) package registry.
- This is the preferred method of installation, as you can easily receive updates as they're released.
- If you have [openupm-cli](https://github.com/openupm/openupm-cli) installed, then run the following command in your project's directory:
  ```
  openupm add com.coffee.event-drawer-ex
  ```
- To update the package, use Package Manager UI (`Window > Package Manager`) or run the following command with `@{version}`:
  ```
  openupm add com.coffee.event-drawer-ex@1.1.0
  ```

### Install via UPM (with Package Manager UI)

- Click `Window > Package Manager` to open Package Manager UI.
- Click `+ > Add package from git URL...` and input the repository URL: `https://github.com/mob-sakai/UnityEventDrawerEx.git`  
  ![](https://github.com/user-attachments/assets/f88f47ad-c606-44bd-9e86-ee3f72eac548)
- To update the package, change suffix `#{version}` to the target version.
  - e.g. `https://github.com/mob-sakai/UnityEventDrawerEx.git#1.1.0`

### Install via UPM (Manually)

- Open the `Packages/manifest.json` file in your project. Then add this package somewhere in the `dependencies` block:
  ```json
  {
    "dependencies": {
      "com.coffee.event-drawer-ex": "https://github.com/mob-sakai/UnityEventDrawerEx.git",
      ...
    }
  }
  ```

- To update the package, change suffix `#{version}` to the target version.
  - e.g. `"com.coffee.event-drawer-ex": "https://github.com/mob-sakai/UnityEventDrawerEx.git#1.1.0",`

### Install as Embedded Package

1. Download a source code zip file from [Releases](https://github.com/mob-sakai/UnityEventDrawerEx/releases) and extract it.
2. Place it in your project's `Packages` directory.  
   ![](https://github.com/user-attachments/assets/af639cfa-d0b4-4370-acb9-3fe4db451f47)
- If you want to fix bugs or add features, install it as an embedded package.
- To update the package, you need to re-download it and replace the contents.

<br><br>

## 🚀 Getting Started

1. [Install the package](#-installation).

2. Add a runtime call, such as `Button.onClick.AddListener (method)`.

3. Information about the runtime call is displayed in the inspector.  
   ![image](https://user-images.githubusercontent.com/12690315/40921624-f43811b6-684a-11e8-96e4-83964730a358.png)

4. Enjoy!

<br><br>

## 🤝 Contributing

### Issues

Issues are incredibly valuable to this project:

- Ideas provide a valuable source of contributions that others can make.
- Problems help identify areas where this project needs improvement.
- Questions indicate where contributors can enhance the user experience.

### Pull Requests

Pull requests offer a fantastic way to contribute your ideas to this repository.  
Please refer to [CONTRIBUTING.md](https://github.com/mob-sakai/UnityEventDrawerEx/tree/main/CONTRIBUTING.md)
and [develop branch](https://github.com/mob-sakai/UnityEventDrawerEx/tree/develop) for guidelines.

### Support

This is an open-source project developed during my spare time.  
If you appreciate it, consider supporting me.  
Your support allows me to dedicate more time to development. 😊

[![](https://user-images.githubusercontent.com/12690315/50731629-3b18b480-11ad-11e9-8fad-4b13f27969c1.png)](https://www.patreon.com/join/2343451?)  
[![](https://user-images.githubusercontent.com/12690315/66942881-03686280-f085-11e9-9586-fc0b6011029f.png)](https://github.com/users/mob-sakai/sponsorship)

<br><br>

## License

* MIT

## Author

* ![](https://user-images.githubusercontent.com/12690315/96986908-434a0b80-155d-11eb-8275-85138ab90afa.png) [mob-sakai](https://github.com/mob-sakai) [![](https://img.shields.io/twitter/follow/mob_sakai.svg?label=Follow&style=social)](https://twitter.com/intent/follow?screen_name=mob_sakai) ![GitHub followers](https://img.shields.io/github/followers/mob-sakai?style=social)

## See Also

* GitHub page : https://github.com/mob-sakai/UnityEventDrawerEx
* Releases : https://github.com/mob-sakai/UnityEventDrawerEx/releases
* Issue tracker : https://github.com/mob-sakai/UnityEventDrawerEx/issues
* Change log : https://github.com/mob-sakai/UnityEventDrawerEx/blob/main/CHANGELOG.md
