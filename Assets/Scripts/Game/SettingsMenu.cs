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




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        


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
                QualitySettings.shadowProjection = ShadowProjection.CloseFit;
                PlayerPrefs.SetInt("ShadowSetting", 1);
                shadowsToggleLow.isOn = true;

                break;
            case 2: // Medium
                QualitySettings.shadowDistance = 20f;
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                QualitySettings.shadowProjection = ShadowProjection.CloseFit;
                PlayerPrefs.SetInt("ShadowSetting", 2);
                shadowsToggleMedium.isOn = true;
                break;
            case 3: // High
                QualitySettings.shadowDistance = 50f;
                QualitySettings.shadowResolution = ShadowResolution.High;
                QualitySettings.shadowProjection = ShadowProjection.CloseFit;
                PlayerPrefs.SetInt("ShadowSetting", 3);
                shadowsToggleHigh.isOn = true;
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
                graphicsToggleLow.isOn = true;
                break;
            case 2: // Medium
                QualitySettings.SetQualityLevel(2, true);
                PlayerPrefs.SetInt("GraphicsSetting", 2);
                graphicsToggleMedium.isOn = true;
                break;
            case 3: // High
                QualitySettings.SetQualityLevel(5, true);
                PlayerPrefs.SetInt("GraphicsSetting", 3);
                graphicsToggleHigh.isOn = true;
                break;
            default:
                Debug.LogWarning("Invalid graphics setting selected.");
                break;
        }
        Debug.Log("Graphics set to: " + setting);



    }
}
