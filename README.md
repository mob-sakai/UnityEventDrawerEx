Unity Event Drawer Extension
===

![image](https://user-images.githubusercontent.com/12690315/40921624-f43811b6-684a-11e8-96e4-83964730a358.png)

Extend the `UnityEventDrawer` to display runtime calls in the inspector.

[![](https://img.shields.io/npm/v/com.coffee.event-drawer-ex?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.coffee.event-drawer-ex/)
[![](https://img.shields.io/github/v/release/mob-sakai/UnityEventDrawerEx?include_prereleases)](https://github.com/mob-sakai/UnityEventDrawerEx/releases)
[![](https://img.shields.io/github/release-date/mob-sakai/UnityEventDrawerEx.svg)](https://github.com/mob-sakai/UnityEventDrawerEx/releases)  [![](https://img.shields.io/github/license/mob-sakai/UnityEventDrawerEx.svg)](https://github.com/mob-sakai/UnityEventDrawerEx/blob/main/LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-orange.svg)](http://makeapullrequest.com)  
![](https://img.shields.io/badge/Unity%202017.1+-supported-blue.svg)  

<< [Description](#Description) | [Demo](#demo) | [Installation](#installation) | [Usage](#usage) | [Development Note](#development-note) | [Change log](https://github.com/mob-sakai/UnityEventDrawerEx/blob/main/CHANGELOG.md) >>



<br><br><br><br>

## Description

### What is runtime call?

*Button.onClick* and *Toggle.OnChangeValue* etc. are known as **UnityEvent**.  
UnityEvent has two types of callbacks:

* Persistent call
    * You can add callbacks from the inspector.
    * You can add callbacks from a script using *UnityEventTools.AddPersistentListener*.
    * It is Serialized.
    * It is displayed in the inspector.  
    ![persistent](https://user-images.githubusercontent.com/12690315/40887795-f5eb7ac4-6788-11e8-9e73-6831e3eab08f.png)
* **Runtime call**
    * You can add a callback from a script using *UnityEvent.AddListener*.
    * It is not serialized.
    * It is not displayed in the inspector.  
    ![runtime](https://user-images.githubusercontent.com/12690315/40887784-c8c2027a-6788-11e8-83f7-07e38e187cba.png)

### Display your runtime call

This plugin extends *UnityEventDrawer* to display runtime calls in inspector.  
If runtime call is an instance method, its target is also displayed.  
This plugin supports all events that inherit `UnityEvent<T0> - UnityEvent<T0, T1, T2, T3>` as well as *UnityEvent*.  
Also, when the Persistent call is empty, *UnityEvent* is displayed compactly in inspector.  

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


If you like a development style that heavily uses Runtime calls (MVP pattern, etc.), we recommend using this plugin!

### Features

* Displays runtime calls in inspector
* Expands/collapses the runtime call view
* Displays instance method, its target is also displayed
* When the Persistent call is empty, displays it compactly
* Supports pro skin
* Supports all components as well as uGUI components such as `Button` and `Toggle`  
![](https://user-images.githubusercontent.com/12690315/40947741-55bcb0be-689f-11e8-9d86-6d0364ebd155.png)
* Supports `EventTrigger`  
![](https://user-images.githubusercontent.com/12690315/41216786-c12a1c10-6d90-11e8-8a13-00e5b27de573.png)



<br><br><br><br>

## Installation

### Requirement

* Unity 5.5 or later

### (For Unity 2018.3 or later) Using OpenUPM

This package is available on [OpenUPM](https://openupm.com).  
You can install it via [openupm-cli](https://github.com/openupm/openupm-cli).
```
openupm add com.coffee.event-drawer-ex
```

### (For Unity 2018.3 or later) Using Git

Find the manifest.json file in the Packages folder of your project and add a line to `dependencies` field.

* Major version: ![](https://img.shields.io/github/v/release/mob-sakai/UnityEventDrawerEx)  
`"com.coffee.event-drawer-ex": "https://github.com/mob-sakai/UnityEventDrawerEx.git"`

To update the package, change suffix `#{version}` to the target version.

* e.g. `"com.coffee.event-drawer-ex": "https://github.com/mob-sakai/UnityEventDrawerEx.git#1.0.0",`

Or, use [UpmGitExtension](https://github.com/mob-sakai/UpmGitExtension) to install and update the package.

#### For Unity 2018.2 or earlier

1. Download a source code zip file from [Releases](https://github.com/mob-sakai/UnityEventDrawerEx/releases) page
2. Extract it
3. Import it into the following directory in your Unity project
   - `Packages` (It works as an embedded package. For Unity 2018.1 or later)
   - `Assets` (Legacy way. For Unity 2017.1 or later)



<br><br><br><br>

## Usage

1. Add a runtime call, such as `Button.onClick.AddListener (method)`.
1. Information about the runtime call is displayed in inspector.
1. Enjoy!



<br><br><br><br>

## Contributing

### Issues

Issues are very valuable to this project.

- Ideas are a valuable source of contributions others can make
- Problems show where this project is lacking
- With a question you show where contributors can improve the user experience

### Pull Requests

Pull requests are, a great way to get your ideas into this repository.  
See [sandbox/README.md](https://github.com/mob-sakai/UnityEventDrawerEx/blob/sandbox/README.md).

### Support

This is an open source project that I am developing in my spare time.  
If you like it, please support me.  
With your support, I can spend more time on development. :)

[![](https://user-images.githubusercontent.com/12690315/50731629-3b18b480-11ad-11e9-8fad-4b13f27969c1.png)](https://www.patreon.com/join/mob_sakai?)  
[![](https://user-images.githubusercontent.com/12690315/66942881-03686280-f085-11e9-9586-fc0b6011029f.png)](https://github.com/users/mob-sakai/sponsorship)



<br><br><br><br>

## License

* MIT



## Author

* ![](https://user-images.githubusercontent.com/12690315/96986908-434a0b80-155d-11eb-8275-85138ab90afa.png) [mob-sakai](https://github.com/mob-sakai) [![](https://img.shields.io/twitter/follow/mob_sakai.svg?label=Follow&style=social)](https://twitter.com/intent/follow?screen_name=mob_sakai) ![GitHub followers](https://img.shields.io/github/followers/mob-sakai?style=social)



## See Also

* GitHub page : https://github.com/mob-sakai/UnityEventDrawerEx
* Releases : https://github.com/mob-sakai/UnityEventDrawerEx/releases
* Issue tracker : https://github.com/mob-sakai/UnityEventDrawerEx/issues
* Change log : https://github.com/mob-sakai/UnityEventDrawerEx/blob/main/CHANGELOG.md