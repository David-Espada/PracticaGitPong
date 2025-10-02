using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class FadeController : MonoBehaviour
{
   public CanvasGroup fadeGroup;     
   public float fadeDuration = 0.9f; 
   void Awake()
   {
       if (fadeGroup == null)
           fadeGroup = GetComponent<CanvasGroup>();
   }
   void Start()
   {
       
       StartCoroutine(Fade(1f, 0f));
   }
  
   public void FadeOutAndLoad(string sceneName)
   {
       StartCoroutine(FadeOutAndLoadCoroutine(sceneName));
   }
   private IEnumerator Fade(float from, float to)
   {
       float elapsed = 0f;
       fadeGroup.alpha = from;
       fadeGroup.blocksRaycasts = true; 
       while (elapsed < fadeDuration)
       {
           elapsed += Time.unscaledDeltaTime; 
           float t = Mathf.Clamp01(elapsed / fadeDuration);
           fadeGroup.alpha = Mathf.Lerp(from, to, t);
           yield return null;
       }
       fadeGroup.alpha = to;
       fadeGroup.blocksRaycasts = (to > 0.9f);
   }
   private IEnumerator FadeOutAndLoadCoroutine(string sceneName)
   {
       yield return StartCoroutine(Fade(0f, 1f)); 
       SceneManager.LoadScene(sceneName);
   }
}