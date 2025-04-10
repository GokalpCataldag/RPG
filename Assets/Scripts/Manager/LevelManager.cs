using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private int currentExp;
    private int level;
    private int expToNextLevel;
    public int GetLevel { get { return level+1; } }
    public static LevelManager instance;
    public Image ExpBar;
    public Text LevelText;
    public GameObject LevelUpVFX;
    private Transform Player;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        level = 0;
        currentExp = 0;
        expToNextLevel = 100;
        ExpBar.fillAmount = 0f;
        UpdateLevelText();
        Player = GameObject.Find("Player").gameObject.transform;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddExp(25);
        }
    }
    public void AddExp(int amount)
    {
        currentExp += amount;
        ExpBar.fillAmount = (float)currentExp / expToNextLevel;
        if(currentExp >= expToNextLevel)
        {
            level++;
            GameObject LevelUpVFXClone =Instantiate(LevelUpVFX, Player.position, Quaternion.identity);
            LevelUpVFXClone.transform.SetParent(Player);
            UpdateLevelText();
            currentExp -= expToNextLevel;
            ExpBar.fillAmount = 0f;
        }
    }
    private void OnEnable()
    {
        EnemyHealth.onDeath += AddExp;
    }
    private void OnDisable() 
    {
        EnemyHealth.onDeath -= AddExp;
    }
    void UpdateLevelText()
    {
        LevelText.text = GetLevel.ToString();
    }
}
