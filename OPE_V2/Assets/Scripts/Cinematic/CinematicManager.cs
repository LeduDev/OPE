using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicManager : MonoBehaviour
{
    [Header ("Cinematic Variables")]
    [SerializeField] private string[] storyTxtPT;
    [SerializeField] private string[] storyTxtEN;
    [SerializeField] private GameObject[] storyImages;
    private bool canClick = true;
    [SerializeField] private GameObject skipCinematicBtn;
    private int storyId = 0; //Indica a "página" em que se encontra história
    private int StoryId
    {
        get
        {
            return storyId;
        }
        set
        {
            storyId = value;
            if (Settings.language == Settings.Languages.Portuguese)
            {
                UpdateCinematic(storyTxtPT);                
            }
            else if (Settings.language == Settings.Languages.English)
            {
                UpdateCinematic(storyTxtEN);
            }
        }
    }

    [Header("Cinematic UI References")]
    [SerializeField] private Text storyTxtDisplay;

    // Start is called before the first frame update
    void Start()
    {
        if (Settings.language == Settings.Languages.Portuguese)
        {
            storyTxtDisplay.text = storyTxtPT[0];
        }
        else if (Settings.language == Settings.Languages.English)
        {
            storyTxtDisplay.text = storyTxtEN[0];
        }
    }

    public void UpdateCinematic(string[] storyText)
    {
        if (StoryId < storyText.Length)
        {
            SoundManager.Instance.PlaySFX("Click");
            storyTxtDisplay.text = storyText[StoryId];
            StartCoroutine("ActiveStoryImage");
        }
        else
        {
            canClick = false;
            if (skipCinematicBtn != null)
            {
                Button skipBtn = skipCinematicBtn.GetComponent<Button>();
                if (skipBtn != null)
                {
                    skipBtn.enabled = false;
                }
            }
            SoundManager.Instance.PlaySFX("Score");
            StartCoroutine("FadeOut");
        }
    }

    public void AdvanceStory()
    {
        if (canClick)
        {
            StoryId++;
        }
    }

    private IEnumerator FadeOut()
    {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetBool("finish", true);
        yield return new WaitForSeconds(2.8f);
        this.gameObject.SetActive(false);
    }

    private IEnumerator ActiveStoryImage()
    {
        canClick = false;
        storyImages[StoryId - 1].gameObject.SetActive(true);
        yield return new WaitForSeconds(2.2f);
        canClick = true;
    }
}
