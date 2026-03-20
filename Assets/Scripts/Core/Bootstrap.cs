using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private SkillButton skillButton;
    [SerializeField] private SkillController skillController;

    void Start()
    {
        if (skillController == null || skillButton == null)
        {
            Debug.LogError("[Bootstrap] SkillController and SkillUI not registered in references");
            return;
        }
        
        skillController.Initialize(skillButton);
    }
}
