
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GIBBUS
{
    public class MissionConfigurationUI : MonoBehaviour
    {
        private const int PANEL_COUNT = 5;

        [Header("NAVIGATION BUTTONS")]
        [SerializeField] private Button launchSystemButton;
        [SerializeField] private Button architectureButton;
        [SerializeField] private Button sciencePayloadButton;
        [SerializeField] private Button engineeringBudgetButton;
        [SerializeField] private Button approvalButton;

        [Header("CONTENT PANELS")]
        [SerializeField] private GameObject launchSystemPanel;
        [SerializeField] private GameObject architecturePanel;
        [SerializeField] private GameObject sciencePayloadPanel;
        [SerializeField] private GameObject engineeringBudgetPanel;
        [SerializeField] private GameObject approvalPanel;

        [Header("NAVIGATION CONTROLS")]
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;

        [Header("PROGRESS INDICATOR")]
        [SerializeField] private Image progressFill;

        [Header("APPROVAL")]
        [SerializeField] private Button acknowledgeConfigurationButton;

        [SerializeField]
        private UnityEvent onConfigurationApproved;

        private GameObject[] panels;
        private Button[] navigationButtons;

        private int currentPanelIndex;

        private void Awake()
        {
            panels = new GameObject[]
            {
                launchSystemPanel,
                architecturePanel,
                sciencePayloadPanel,
                engineeringBudgetPanel,
                approvalPanel
            };

            navigationButtons = new Button[]
            {
                launchSystemButton,
                architectureButton,
                sciencePayloadButton,
                engineeringBudgetButton,
                approvalButton
            };

            SetupNavigation();
            SetupControls();
            SetupApproval();
        }

        private void Start()
        {
            ShowPanel(0);
        }

        private void SetupNavigation()
        {
            for (int i = 0; i < PANEL_COUNT; i++)
            {
                int index = i;

                if (navigationButtons[index] == null)
                    continue;

                navigationButtons[index].onClick.AddListener(
                    () => ShowPanel(index)
                );
            }
        }

        private void SetupControls()
        {
            if (previousButton != null)
                previousButton.onClick.AddListener(PreviousPanel);

            if (nextButton != null)
                nextButton.onClick.AddListener(NextPanel);
        }

        private void SetupApproval()
        {
            if (acknowledgeConfigurationButton != null)
            {
                acknowledgeConfigurationButton.onClick.AddListener(
                    AcknowledgeConfiguration
                );
            }
        }

        public void ShowPanel(int index)
        {
            if (index < 0 || index >= PANEL_COUNT)
                return;

            currentPanelIndex = index;

            for (int i = 0; i < PANEL_COUNT; i++)
            {
                if (panels[i] != null)
                    panels[i].SetActive(i == index);
            }

            UpdateNavigationControls();
            UpdateProgress();
        }

        public void NextPanel()
        {
            if (currentPanelIndex < PANEL_COUNT - 1)
                ShowPanel(currentPanelIndex + 1);
        }

        public void PreviousPanel()
        {
            if (currentPanelIndex > 0)
                ShowPanel(currentPanelIndex - 1);
        }

        private void UpdateNavigationControls()
        {
            if (previousButton != null)
                previousButton.interactable = currentPanelIndex > 0;

            if (nextButton != null)
                nextButton.interactable =
                    currentPanelIndex < PANEL_COUNT - 1;
        }

        private void UpdateProgress()
        {
            if (progressFill == null)
                return;

            progressFill.fillAmount =
                (currentPanelIndex + 1f) / PANEL_COUNT;
        }

        public void AcknowledgeConfiguration()
        {
            if (currentPanelIndex != PANEL_COUNT - 1)
            {
                Debug.LogWarning(
                    "[GIBBUS] Configuration approval attempted " +
                    "outside the Approval panel."
                );

                return;
            }

            onConfigurationApproved?.Invoke();
        }
    }
}
