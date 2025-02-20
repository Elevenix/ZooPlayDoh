using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Timelapse : MonoBehaviour
{
    [SerializeField] private WebcamCapturePhoto webcam;
    [SerializeField] private Image screenshot;
    [SerializeField] private Image timelapse;
    [SerializeField] private InputField inputTownName;
    [SerializeField] private Text nameTown;
    [SerializeField] private SwitchScreen switchScreen;

    [Header("Timelapse Info"), Space(6)]
    [SerializeField] private float frameBySeconds = .5f;
    [SerializeField] private Animator animPhotoAdded;

    private List<Sprite> _sprites = new List<Sprite>();
    private int _indexSprite = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeScreenAddInTimelapse();
        }
    }

    public void RestoreExistingDirectory()
    {
        _sprites.Clear();
        StopAllCoroutines();

        string filePath = Application.persistentDataPath + "/" + inputTownName.text + "/";
        if (Directory.Exists(filePath))
        {
            print("---File exist !---");
            foreach (string path in Directory.GetFiles(filePath))
            {
                Sprite newSprite = CreateSpriteFromPath(path);
                _sprites.Add(newSprite);
                print(path);
            }

            if (_sprites.Count > 1)
            {
                StartCoroutine(LoopTimelapseCoroutine());
                switchScreen.OpenTimelapseWindow(true);
            }
        }
    }

    public void AddCaptureInTimelapse()
    {
        if(screenshot.sprite != null)
        {
            switchScreen.OpenTimelapseWindow(true);
            _sprites.Add(screenshot.sprite);

            if (_sprites.Count == 1)
                StartCoroutine(LoopTimelapseCoroutine());
            animPhotoAdded.SetTrigger("Action");
        }
    }

    public void SetFps(string value)
    {
        string number = value.Replace(".", ",");
        float result = float.Parse(number);
        if(result > 0)
        {
            frameBySeconds = result;
        }
    }

    private void TakeScreenAddInTimelapse()
    {
        webcam.CapturePhoto();
        AddCaptureInTimelapse();
    }

    private Sprite CreateSpriteFromPath(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        // Créer un Sprite et l'afficher dans l'UI
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private IEnumerator LoopTimelapseCoroutine()
    {
        nameTown.text = "Timelapse : " + inputTownName.text;
        while (true)
        {
            timelapse.sprite = _sprites[_indexSprite % _sprites.Count];
            _indexSprite++;
            yield return new WaitForSeconds(frameBySeconds);
        }
    }
}
