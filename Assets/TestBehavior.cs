using UnityEngine;

public class TestBehavior : MonoBehaviour
{
	[System.Serializable] public class TransformUnityEvent : UnityEngine.Events.UnityEvent<Transform> {};

	[SerializeField] TransformUnityEvent onYourCustomEvent = new TransformUnityEvent();

	void OnEnable()
	{
		onYourCustomEvent.AddListener(TestTransform);
	}

	void TestTransform(Transform t)
	{
		Debug.Log("TestTransform has called : " + t);
	}


#if true
	[SerializeField] UnityEngine.UI.Button button = null;

	public void TestPersistant()
	{
		Debug.Log("TestPersistant has called : Add runtime call");
		button.onClick.AddListener(TestRuntime);
	}

	void TestRuntime()
	{
		Debug.Log("TestRuntime has called");
	}
#endif
}
