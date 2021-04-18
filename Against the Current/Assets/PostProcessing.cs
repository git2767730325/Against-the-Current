using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    public PostProcessProfile postProcessProfile;
    public LensDistortion lensDistortion;
    public ChromaticAberration chromaticAberration;
    public Vignette vignette;
    private void Awake()
    {
        //postProcessProfile = gameObject.GetComponent<PostProcessProfile>();
        lensDistortion=postProcessProfile.GetSetting<LensDistortion>();
        chromaticAberration = postProcessProfile.GetSetting<ChromaticAberration>();
        vignette = postProcessProfile.GetSetting<Vignette>();
    }

    public void LensDistortionAdd(bool value)
    {
        if(value)
        {
            if (lensDistortion.intensity > -40)
                lensDistortion.intensity.value -=1;
        }
        else
        {
            if (lensDistortion.intensity < 0)
                lensDistortion.intensity.value +=1;
        }
    }
    //方法,红色背景
    public void StartRedCurtain()
    {
        if(vignette.intensity<1)
        vignette.intensity.value += 0.1f;
    }
    public void StopRedCurtain()
    {
        if(vignette.intensity.value!=0)
        vignette.intensity.value = 0;
    }
    public void StartVague()
    {
        if (chromaticAberration.intensity < 1)
            chromaticAberration.intensity.value += 0.1f;
    }
    public void StopVague()
    {
        if(chromaticAberration.intensity!=0)
        chromaticAberration.intensity.value = 0f; ;
    }


}
