using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Coffee.EditorExtensions
{
    [RequireComponent(typeof(Button))]
    [ExecuteAlways]
    public class Demo : MonoBehaviour
    {
        private void OnEnable()
        {
            if (TryGetComponent(out Button button))
            {
                button.onClick.AddListener(OnClick_Instance);
                button.onClick.AddListener(OnClick_Static);
                button.onClick.AddListener(() => Debug.Log("Hello (ramda)"));
            }

            if (TryGetComponent(out EventTrigger trigger))
            {
                var ev = trigger.triggers.Find(x => x.eventID == EventTriggerType.PointerClick)?.callback;
                ev?.AddListener(OnClick_Instance);
                ev?.AddListener(OnClick_Static);
                ev?.AddListener(_ => Debug.Log("Hello (ramda)"));
            }
        }

        private void OnDisable()
        {
            if (TryGetComponent(out Button button))
            {
                button.onClick.RemoveAllListeners();
            }

            if (TryGetComponent(out EventTrigger trigger))
            {
                var ev = trigger.triggers.Find(x => x.eventID == EventTriggerType.PointerClick)?.callback;
                ev?.RemoveAllListeners();
            }
        }

        private void Register(UnityEvent ev)
        {
            ev.AddListener(OnClick_Instance);
            ev.AddListener(OnClick_Static);
            ev.AddListener(() => Debug.Log("Hello (ramda)"));
        }

        private void Unregister(UnityEvent ev)
        {
        }

        private void OnClick_Instance()
        {
            Debug.Log("Hello (instance)");
        }


        private void OnClick_Instance(BaseEventData ev)
        {
            Debug.Log("Hello (instance)");
        }

        private static void OnClick_Static()
        {
            Debug.Log("Hello (static)");
        }

        private static void OnClick_Static(BaseEventData ev)
        {
            Debug.Log("Hello (static)");
        }
    }
}
