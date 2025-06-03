using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider FpsSlider;
    [SerializeField]
    private TextMeshProUGUI fpsText;
    [SerializeField]
    private Toggle shadowsToggleHigh;
    [SerializeField]
    private Toggle shadowsToggleMedium;
    [SerializeField]
    private Toggle shadowsToggleLow;
    [SerializeField]
    private Toggle graphicsToggleHigh;
    [SerializeField]
    private Toggle graphicsToggleMedium;
    [SerializeField]
    private Toggle graphicsToggleLow;
    private Color defaultColor = Color.white;
    [SerializeField]
    private Color selectedColor;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadFpsLimit();
        int shadowSetting = PlayerPrefs.GetInt("ShadowSetting", 2);
        setShadows(shadowSetting);
        int graphicsSetting = PlayerPrefs.GetInt("GraphicsSetting", 2);
        setGraphics(graphicsSetting);



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadFpsLimit() 
    { 
        int fps = PlayerPrefs.GetInt("FpsCap", 120);
        Application.targetFrameRate = fps;
        fpsText.text = fps.ToString();
        FpsSlider.value = fps;
    }
    public void ChangeFpsCap()
    {
        int fps = Mathf.RoundToInt(FpsSlider.value);
        Application.targetFrameRate = fps;
        PlayerPrefs.SetInt("FpsCap", fps);
        Debug.Log("FPS Cap set to: " + fps);
        fpsText.text = fps.ToString();
    }
    public void setShadows(int setting)
    {
        switch (setting)
        {
            case 1: // Low
                QualitySettings.shadowDistance = 10f;
                QualitySettings.shadowResolution = ShadowResolution.Low;
                QualitySettings.shadows = ShadowQuality.HardOnly; // Assuming low shadows means hard shadows only
                PlayerPrefs.SetInt("ShadowSetting", 1);
                shadowsToggleLow.GetComponent<Image>().color = selectedColor;
                shadowsToggleMedium.GetComponent<Image>().color = defaultColor;
                shadowsToggleHigh.GetComponent<Image>().color = defaultColor;
                break;
            case 2: // Medium
                QualitySettings.shadowDistance = 20f;
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                QualitySettings.shadows = ShadowQuality.HardOnly; // Assuming medium shadows means all shadows
                PlayerPrefs.SetInt("ShadowSetting", 2);
                shadowsToggleLow.GetComponent<Image>().color = defaultColor;
                shadowsToggleMedium.GetComponent<Image>().color = selectedColor;
                shadowsToggleHigh.GetComponent<Image>().color = defaultColor;
                break;
            case 3: // High
                QualitySettings.shadowDistance = 50f;
                QualitySettings.shadowResolution = ShadowResolution.High;
                QualitySettings.shadows = ShadowQuality.All; // Assuming high shadows means all shadows
                PlayerPrefs.SetInt("ShadowSetting", 3);
                shadowsToggleLow.GetComponent<Image>().color = defaultColor;
                shadowsToggleMedium.GetComponent<Image>().color = defaultColor;
                shadowsToggleHigh.GetComponent<Image>().color = selectedColor;
                break;
            default:
                Debug.LogWarning("Invalid shadow setting selected.");
                return;
        }
        Debug.Log("Shadows set to: " + setting);
    }
    public void setGraphics(int setting)
    {
        switch (setting)
        {
            case 1: // Low
                QualitySettings.SetQualityLevel(0, true);
                PlayerPrefs.SetInt("GraphicsSetting", 1);
                graphicsToggleLow.GetComponent<Image>().color = selectedColor;
                graphicsToggleMedium.GetComponent<Image>().color = defaultColor;
                graphicsToggleHigh.GetComponent<Image>().color = defaultColor;
                break;
            case 2: // Medium
                QualitySettings.SetQualityLevel(2, true);
                PlayerPrefs.SetInt("GraphicsSetting", 2);
                graphicsToggleLow.GetComponent<Image>().color = defaultColor;
                graphicsToggleMedium.GetComponent<Image>().color = selectedColor;
                graphicsToggleHigh.GetComponent<Image>().color = defaultColor;
                break;
            case 3: // High
                QualitySettings.SetQualityLevel(5, true);
                PlayerPrefs.SetInt("GraphicsSetting", 3);
                graphicsToggleLow.GetComponent<Image>().color = defaultColor;
                graphicsToggleMedium.GetComponent<Image>().color = defaultColor;
                graphicsToggleHigh.GetComponent<Image>().color = selectedColor;
                break;
            default:
                Debug.LogWarning("Invalid graphics setting selected.");
                break;
        }
        Debug.Log("Graphics set to: " + setting);
    }
}
