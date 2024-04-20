using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWwise_VolumeControl : MonoBehaviour
{
    public float minValue = 20f;
    public float maxValue = 100f;
    public AnimationCurve backgroundAnimCurve;
    [Space]
    [SerializeField] AK.Wwise.RTPC backgroundMusicVolume_RTPC;
    [SerializeField][Range(0, 100)] float bgMusicVolume = 100;

    public void DoVolumeMuteRoutine()
    {
        StartCoroutine(BackgroundMusic_VolumeCurve(2f));
    }

    private IEnumerator BackgroundMusic_VolumeCurve(float hitDuration)
    {
        float timer = 0f;

        while (timer < hitDuration)
        {
            float lerp = timer / hitDuration;
            float eval = backgroundAnimCurve.Evaluate(lerp);

            backgroundMusicVolume_RTPC.SetValue(gameObject, Mathf.Lerp(minValue, maxValue, eval));

            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
