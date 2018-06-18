using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class TestBehavior : MonoBehaviour
{
	[System.Serializable] public class TransformUnityEvent : UnityEngine.Events.UnityEvent<Transform>
	{

	};

	[System.Serializable]
	public class Nested
	{
		public TransformUnityEvent nestedEvent = new TransformUnityEvent();
		public TransformUnityEvent[] nestedEventArray = new TransformUnityEvent[]{};
		public List<TransformUnityEvent> nestedEventList = new List<TransformUnityEvent>();
	}

	public TransformUnityEvent onYourCustomEvent = new TransformUnityEvent();
	public TransformUnityEvent[] onYourCustomEventArray = new TransformUnityEvent[]{};
	public List<TransformUnityEvent> onYourCustomEventList = new List<TransformUnityEvent>();

	public Nested nested = new Nested();
	public Nested[] nestedArray = new Nested[]{};
	public List<Nested> nestedList = new List<Nested>{};

	void OnEnable()
	{
		onYourCustomEvent.AddListener(TestTransform);

		var evt = GetComponent<EventTrigger>();
		var entry = new EventTrigger.Entry(){ eventID = EventTriggerType.PointerClick, callback = new EventTrigger.TriggerEvent() };
		entry.callback.AddListener(ClickEvent);
		evt.triggers.Add(entry);

		entry = new EventTrigger.Entry(){ eventID = EventTriggerType.Drag, callback = new EventTrigger.TriggerEvent() };
		entry.callback.AddListener(DragEvent);
		evt.triggers.Add(entry);
	}

	void OnDisable()
	{
		var evt = GetComponent<EventTrigger>();
		evt.triggers.Clear();
	}

	void TestTransform(Transform t)
	{
		Debug.Log("TestTransform has called : " + t);
	}


	#if true
	[SerializeField] string text;
	[SerializeField] UnityEngine.UI.Button button = null;

	public void TestPersistant()
	{
		Debug.Log("TestPersistant has called : Add runtime call", this);
		button.onClick.AddListener(TestRuntime);
	}

	void TestRuntime()
	{
		Debug.Log("TestRuntime has called", this);
	}

	void ClickEvent(BaseEventData ev)
	{
		Debug.Log("ClickEvent has called " + ev, this);
	}

	void DragEvent(BaseEventData ev)
	{
		Debug.Log("DragEvent has called " + ev, this);
	}
	#endif
}
