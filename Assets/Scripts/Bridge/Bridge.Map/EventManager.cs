using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using TMPro;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private Image eventImage;
    [SerializeField] private Text eventText;
    [SerializeField] private Transform optionsParent;
    [SerializeField] private GameObject optionButtonPrefab;

    private MultiStepEvent currentEvent;
    private EventStep currentStep;

    private void Awake() => Instance = this;

    public void StartEvent(MultiStepEvent evt)
    {
        currentEvent = evt;
        ShowStep(evt.GetStep("start")); // イベントは"start"から始める
    }

    public void ShowStep(EventStep step)
    {
        currentStep = step;
        eventPanel.SetActive(true);

        eventText.text = step.Text;
        step.EventImageRef.LoadAssetAsync().Completed += handle =>
        {
            eventImage.sprite = handle.Result;
        };

        foreach (Transform child in optionsParent)
            Destroy(child.gameObject);

        foreach (var option in step.Options)
        {
            var btnObj = Instantiate(optionButtonPrefab, optionsParent);
            var btn = btnObj.GetComponent<Button>();
            var txt = btnObj.GetComponentInChildren<Text>();
            txt.text = option.Text;

            btn.onClick.AddListener(() =>
            {
                OnOptionSelected(option);
            });
        }
    }

    private void OnOptionSelected(EventOption option)
    {
        if (option.EndsEvent)
        {
            EndEvent(option.Id);
        }
        else
        {
            var nextStep = currentEvent.GetStep(option.NextStepId);
            if (nextStep != null)
            {
                ShowStep(nextStep);
            }
            else
            {
                Debug.LogWarning($"NextStep '{option.NextStepId}' not found in {currentEvent.EventId}");
                EndEvent(option.Id);
            }
        }
    }

    private void EndEvent(string lastOptionId)
    {
        eventPanel.SetActive(false);
        Debug.Log($"Event {currentEvent.EventId} ended with option {lastOptionId}");

        MapManager.Instance.CompleteLastEvent(lastOptionId);
    }
}
