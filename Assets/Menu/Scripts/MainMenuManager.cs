using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Sequence Manager")]
        [Tooltip("Choose a type of intro.")] public Intro introState;
        public enum Intro { OneLiner_FadingMenu, FadingMenu, MenuOnly };

        [Header("Buttons")]
        [Space(10)] [Tooltip("Enable to change the texts manually.")] public bool manualModeButtons;
        [Tooltip("Choose a type of intro.")] public Buttons buttonsAppearance;
        public enum Buttons { Rounded, Rounded_Outlined, Rounded_AlwaysFilled, Squared, Squared_Outlined, Squared_AlwaysFilled };

        [Header("Colors")]
        [Space(10)] [Tooltip("Enable to change the colors manually.")] public bool manualModeColor;
        public Color32 mainColor;
        public float alpha_godrays = 0.13f;
        public float alpha_particleSlowNormal = 0.5f;
        public float alpha_particleHuge = 0.05f;

        [Header("Intro Sequence")]
        [Space(10)] [Tooltip("Enable to change the text manually.")] public bool manualModeIntroText;
        [SerializeField] string introTextContent = "It is never too late to be who you might have been.";

        [Header("Scene")]
        [Space(10)] [SerializeField] string sceneToLoad;
        [SerializeField] float delayBeforeLoading = 3f;

        [Header("Home Panel")]
        [Space(10)] [Tooltip("Enable to change the texts manually.")] public bool manualModeTexts;
        [SerializeField] string play = "Play";
        [SerializeField] string settings = "Options";
        [SerializeField] string quit = "Quit";

        [Header("Audio")]
        [SerializeField] bool customSoundtrack;
        [SerializeField] AudioClip customSoundtrackAudio;
        [SerializeField] float defaultVolume = 0.7f;
        [SerializeField] AudioClip sound_click;
        [SerializeField] AudioClip sound_hover;
        [SerializeField] AudioClip sound_loadScene;
        
        [Header("---- References")]
        [Space(50)] public Animator main_animator;
        [SerializeField] Image background_sprite;
        [Space(10)] [SerializeField] TextMeshProUGUI introText;
        public CanvasGroup homePanel;
        [SerializeField] TextMeshProUGUI homePanel_text_play;
        [SerializeField] TextMeshProUGUI homePanel_text_options;
        [SerializeField] TextMeshProUGUI homePanel_text_quit;

        [Space(10)] [SerializeField] Sprite buttonRounded;
        [SerializeField] Sprite buttonRoundedOutlined;
        [SerializeField] Sprite buttonSquared;
        [SerializeField] Sprite buttonSquaredOutlined;

        [Space(10)] [SerializeField] ParticleSystem[] particles;
        [SerializeField] Image[] godrays_sprite;
        [SerializeField] Image[] buttons;
        [SerializeField] Animator[] buttonsAnimators;
        [SerializeField] RuntimeAnimatorController buttonsAnimator_darkText;
        [SerializeField] RuntimeAnimatorController buttonsAnimator_lightText;
        [SerializeField] RuntimeAnimatorController buttonsAnimator_alwaysFilled;

        [Space(10)] [SerializeField] AudioSource audioSource;
        [SerializeField] AudioSource audioSourceSountrack;
        [SerializeField] AudioClip demo_soundtrack;
        [SerializeField] AudioClip demo_soundtrack_shorter;
        [SerializeField] Slider volumeSlider;

        private void Awake() => IntroSequence();

        private void Start()
        {
            if (!manualModeTexts) UpdateTexts();
            if (!manualModeButtons) UpdateButtons();

            SetStartVolume();
        }

        #region Levels
        public void LoadLevel()
        {
            main_animator.enabled = true;
            main_animator.SetTrigger("LoadScene");

            StartCoroutine(WaitToLoadLevel());
        }

        IEnumerator WaitToLoadLevel()
        {
            yield return new WaitForSeconds(delayBeforeLoading);

            SceneManager.LoadScene(sceneToLoad);
        }

        public void Quit() => Application.Quit();
        #endregion

        #region Audio

        public void SetVolume(float _volume)
        {
            AudioListener.volume = _volume;
            PlayerPrefs.SetFloat("Volume", _volume);
        }

        void SetStartVolume()
        {
            if (!PlayerPrefs.HasKey("Volume")) PlayerPrefs.SetFloat("Volume", defaultVolume);

            LoadVolume();
        }

        public void LoadVolume()
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }

        public void UIClick()
        {
            audioSource.PlayOneShot(sound_click);
        }

        public void UIHover()
        {
            audioSource.PlayOneShot(sound_hover);
        }

        public void UISpecial()
        {
            audioSource.PlayOneShot(sound_loadScene);
        }

        #endregion

        private void IntroSequence()
        {
            switch (introState)
            {
                case Intro.MenuOnly or Intro.FadingMenu:
                    PlayIntro("MenuOnly", customSoundtrack ? customSoundtrackAudio : demo_soundtrack_shorter);
                    break;
                default:
                    if (introState == Intro.OneLiner_FadingMenu && !manualModeTexts && introText != null) introText.text = introTextContent;
            
                    PlayIntro("OneLiner_FadingMenu", customSoundtrack ? customSoundtrackAudio : demo_soundtrack);
                    break;
            }
        }

        private void PlayIntro(string triggerName, AudioClip clip)
        {
            main_animator.SetTrigger(triggerName);
            audioSourceSountrack.clip = clip;
            audioSourceSountrack.Play();
        }

        private void UpdateButtons()
        {
            Sprite spriteToApply = null;
            RuntimeAnimatorController animatorToApply = null;

            switch (buttonsAppearance)
            {
                case Buttons.Rounded:
                    spriteToApply = buttonRounded;
                    animatorToApply = buttonsAnimator_darkText;
                    break;
                case Buttons.Squared:
                    spriteToApply = buttonSquared;
                    animatorToApply = buttonsAnimator_darkText;
                    break;
                case Buttons.Squared_Outlined:
                    spriteToApply = buttonSquaredOutlined;
                    animatorToApply = buttonsAnimator_lightText;
                    break;
                case Buttons.Rounded_Outlined:
                    spriteToApply = buttonRoundedOutlined;
                    animatorToApply = buttonsAnimator_lightText;
                    break;
                case Buttons.Rounded_AlwaysFilled:
                    spriteToApply = buttonRounded;
                    animatorToApply = buttonsAnimator_alwaysFilled;
                    break;
                case Buttons.Squared_AlwaysFilled:
                    spriteToApply = buttonSquared;
                    animatorToApply = buttonsAnimator_alwaysFilled;
                    break;
            }

            for (int i = 0; i < buttons.Length; i+=1)
            {
                buttons[i].sprite = spriteToApply;
                buttonsAnimators[i].runtimeAnimatorController = animatorToApply;
            }
        }


        private void UpdateTexts()
        {
            if (homePanel_text_play != null)
                homePanel_text_play.text = play;

            if (homePanel_text_options != null)
                homePanel_text_options.text = settings;

            if (homePanel_text_quit != null)
                homePanel_text_quit.text = quit;
        }

        // Updates every time the inspector script is refreshed
        public void UIEditorUpdate()
        {
            #region Colors
            if (!manualModeColor)
            {
                // Apply the color to the background
                background_sprite.color = mainColor;

                // Get the main color with a lower alpha value
                Color newColor_godrays = mainColor;
                newColor_godrays.a = alpha_godrays;

                // Godrays
                if (godrays_sprite.Length > 0)
                {
                    // Apply the new color to the godrays
                    for (int i = 0; i < godrays_sprite.Length; i+=1)
                    {
                        godrays_sprite[i].color = newColor_godrays;
                    }
                }

                // Get the main color with a lower alpha value for Slow and Normal Particles
                Color newColor_slowNormal = mainColor;
                newColor_slowNormal.a = alpha_particleSlowNormal;

                // Get the main color with a lower alpha value for Huge Particles
                Color newColor_huge = mainColor;
                newColor_huge.a = alpha_particleHuge;

                // Apply to Particles
                if (particles.Length > 0)
                {
                    // Apply the new color to the godrays
                    for (int i = 0; i < particles.Length; i+=1)
                    {

                        var main1 = particles[i].main;
                        main1.startColor = new ParticleSystem.MinMaxGradient(i != 2 ? newColor_slowNormal : newColor_huge);
                    }
                }
            }
            #endregion

            if (!manualModeTexts) UpdateTexts();

            if (!manualModeButtons) UpdateButtons();

            #region Particles
            if (particles.Length > 0)
            {
                // Prewarm the particles
                for (int i = 0; i < particles.Length; i+=1)
                {
                    if(introState == Intro.MenuOnly)
                    {
                        var main1 = particles[i].main;
                        main1.prewarm = true;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            var main1 = particles[i].main;
                            main1.prewarm = true;
                        }
                        else if (i > 0)
                        {
                            var main1 = particles[i].main;
                            main1.prewarm = false;
                        }
                    }
                }
            }
            #endregion
        }

        public void _fadingAnimationIsDone()
        {
            main_animator.enabled = false;
            homePanel.blocksRaycasts = true;
        }
    }
}
