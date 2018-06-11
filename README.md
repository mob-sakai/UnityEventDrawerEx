UnityEventDrawerEx
===

![image](https://user-images.githubusercontent.com/12690315/40921624-f43811b6-684a-11e8-96e4-83964730a358.png)

This plugin extends the UnityEventDrawer to display runtime calls in the inspector.

[![](https://img.shields.io/github/release/mob-sakai/UnityEventDrawerEx.svg?label=latest%20version)](https://github.com/mob-sakai/UnityEventDrawerEx/releases)
[![](https://img.shields.io/github/release-date/mob-sakai/UnityEventDrawerEx.svg)](https://github.com/mob-sakai/UnityEventDrawerEx/releases)
![](https://img.shields.io/badge/unity-5.5%2B-green.svg)
[![](https://img.shields.io/github/license/mob-sakai/UnityEventDrawerEx.svg)](https://github.com/mob-sakai/UnityEventDrawerEx/blob/master/LICENSE.txt)

<< [Description](#Description) | [Download](https://github.com/mob-sakai/UnityEventDrawerEx/releases) | [Usage](#usage) | [Development Note](#development-note) >>

### [RELEASE NOTE ![](https://img.shields.io/github/release-date/mob-sakai/UnityEventDrawerEx.svg?label=last%20updated&style=for-the-badge)](https://github.com/mob-sakai/UnityEventDrawerEx/blob/develop/CHANGELOG.md)




<br><br><br><br>
## Description


#### What is runtime call?

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

#### Display your runtime call

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

#### Features

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
## Usage

1. Download UnityEventDrawerEx.unitypackage from [Releases](https://github.com/mob-sakai/UnityEventDrawerEx/releases).
1. Import the package into your Unity project. Select `Import Package > Custom Package` from the `Assets` menu.
1. Add a runtime call, such as `Button.onClick.AddListener (method)`.
1. Information about the runtime call is displayed in inspector.
1. Enjoy!


##### Requirement

* Unity 5.5+ *(included Unity 2018.x)*
* No other SDK are required




<br><br><br><br>
## Development Note




<br><br><br><br>
## License

* MIT
* © UTJ/UCL



## Author

[mob-sakai](https://github.com/mob-sakai)



## See Also

* GitHub page : https://github.com/mob-sakai/UnityEventDrawerEx
* Releases : https://github.com/mob-sakai/UnityEventDrawerEx/releases
* Issue tracker : https://github.com/mob-sakai/UnityEventDrawerEx/issues
* Current project : https://github.com/mob-sakai/UnityEventDrawerEx/projects/1
* Change log : https://github.com/mob-sakai/UnityEventDrawerEx/blob/master/CHANGELOG.md
