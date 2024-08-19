using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public void PlayCutsceneByName(string cutsceneName)
    {
        // 이름으로 GameObject를 찾아서 PlayableDirector를 가져옴
        GameObject cutsceneObject = GameObject.Find(cutsceneName);
        if (cutsceneObject != null)
        {
            PlayableDirector cutscene = cutsceneObject.GetComponent<PlayableDirector>();
            if (cutscene != null)
            {
                cutscene.Play();
            }
            else
            {
                Debug.LogWarning("PlayableDirector not found on GameObject " + cutsceneName);
            }
        }
        else
        {
            Debug.LogWarning("Cutscene " + cutsceneName + " not found.");
        }
    }
}
