using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SkillSelectionManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject skillSelectionPanel;
    [SerializeField] private Transform skillsContainer;
    [SerializeField] private Button startLevelButton;
    [SerializeField] private GameObject skillButtonPrefab;

    [Header("Skills")]
    [SerializeField] private SkillData[] availableSkills;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip skillSelectSound;
    [SerializeField] private AudioClip startLevelSound;

    private SkillData selectedSkill;
    private SkillSelectionButton[] skillButtons;

    public static SkillSelectionManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ShowSkillSelection();
        CreateSkillButtons();
        startLevelButton.onClick.AddListener(StartLevel);
        startLevelButton.interactable = false;
    }

    public void ShowSkillSelection()
    {
        skillSelectionPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideSkillSelection()
    {
        skillSelectionPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void CreateSkillButtons()
    {
        skillButtons = new SkillSelectionButton[availableSkills.Length];

        for (int i = 0; i < availableSkills.Length; i++)
        {
            GameObject buttonObj = Instantiate(skillButtonPrefab, skillsContainer);
            SkillSelectionButton skillButton = buttonObj.GetComponent<SkillSelectionButton>();

            skillButton.Setup(availableSkills[i], this);
            skillButtons[i] = skillButton;
        }
    }

    public void SelectSkill(SkillData skill)
    {
        selectedSkill = skill;

        if (skillSelectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(skillSelectSound);
        }

        foreach (var button in skillButtons)
        {
            button.SetSelected(button.GetSkillData() == skill);
        }

        startLevelButton.interactable = true;
        Debug.Log($"Selected skill: {skill.skillName}");
    }
    public void DebugPlayerComponents()
    {
        Player player = FindFirstObjectByType<Player>();
        if (player == null) return;

        Debug.Log("=== CHECKING PLAYER COMPONENTS ===");

        Dash_Skill[] dashSkills = player.GetComponents<Dash_Skill>();
        Debug.Log($"Dash_Skill components found: {dashSkills.Length}");

        foreach (var dash in dashSkills)
        {
            Debug.Log($"Dash_Skill component: {dash.name}, enabled: {dash.enabled}");
        }

        Skill[] skills = player.GetComponents<Skill>();
        Debug.Log($"Total Skill components found: {skills.Length}");

        SkillManager skillManager = player.GetComponent<SkillManager>();
        if (skillManager != null)
        {
            Debug.Log($"SkillManager found: {skillManager.name}, enabled: {skillManager.enabled}");
        }

        Debug.Log("=== END DEBUG ===");
    }
    private void StartLevel()
    {
        if (selectedSkill == null) return;

        if (startLevelSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(startLevelSound);
        }

        Debug.Log("BEFORE removing components:");
        DebugPlayerComponents(); // Add this

        // Remove ALL existing skill components FIRST
        RemoveAllSkillComponents();

        Debug.Log("AFTER removing components:");
        DebugPlayerComponents(); // Add this

        // ONLY use the static PlayerSkills system - NO COMPONENTS
        PlayerSkills.selectedSkill = selectedSkill.skillType;
        PlayerSkills.hasSelectedSkill = true;

        HideSkillSelection();
        Debug.Log($"Starting level with {selectedSkill.skillName} - PlayerSkills.selectedSkill = {PlayerSkills.selectedSkill}");
    }

    // Make sure this method actually removes components
    private void RemoveAllSkillComponents()
    {
        Player player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogError("No Player found!");
            return;
        }

        // Get ALL MonoBehaviour components
        MonoBehaviour[] allComponents = player.GetComponents<MonoBehaviour>();

        foreach (var component in allComponents)
        {
            // Check if it's a skill-related component
            if (component is Skill || component is SkillManager)
            {
                Debug.Log($"🗑️ Destroying component: {component.GetType().Name}");
                component.enabled = false;
                DestroyImmediate(component);
            }
        }

        Debug.Log("✅ All skill components forcefully removed!");
    }
}