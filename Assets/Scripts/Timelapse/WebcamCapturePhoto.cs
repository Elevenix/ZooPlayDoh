using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WebcamCapturePhoto : MonoBehaviour
{
    [Header("UI"), Space(6)]

    [SerializeField] private InputField inputTown;
    public RawImage display; // Affiche la webcam sur un UI RawImage
    public Image capturedImageDisplay; // Affiche la photo capturée en Sprite
    [SerializeField] private Animator animWebcamTaken;

    private WebCamTexture webcamTexture;
    private string _filePath = "";

    void Start()
    {
        // Démarrer la webcam
        webcamTexture = new WebCamTexture();
        display.texture = webcamTexture;
        webcamTexture.Play();
    }

    public void CapturePhoto()
    {
        string nameTown = inputTown.text;
        nameTown = nameTown.Replace(" ", "");

        if (nameTown.Length != 0)
        {
            // Créer une texture pour stocker l'image de la webcam
            Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
            photo.SetPixels(webcamTexture.GetPixels());
            photo.Apply();

            // Convertir en bytes (PNG)
            byte[] bytes = photo.EncodeToPNG();
            string filePath = Application.persistentDataPath + "/" + nameTown + "/";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            int indexPhoto = Directory.GetFiles(filePath).Length;
            filePath = Path.Combine(filePath, "capture_" + indexPhoto + ".png");
            _filePath = filePath;

            // Enregistrer l'image sur le disque
            File.WriteAllBytes(filePath, bytes);
            UnityEngine.Debug.Log("Photo sauvegardée à : " + filePath);

            // Charger l'image enregistrée et l'affecter à un sprite
            StartCoroutine(LoadAndSetSprite(filePath));

            animWebcamTaken.SetTrigger("Action");
        }
    }

    public void DeleteCapture()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
            capturedImageDisplay.sprite = null;
        }
    }

    public void OpenFileExplorerCaptures()
    {
        string path = Application.persistentDataPath;

#if UNITY_STANDALONE_WIN
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "explorer",
            Arguments = path.Replace("/", "\\"), // Remplace les "/" par des "\"
            UseShellExecute = true
        };
        Process.Start(psi);
#elif UNITY_STANDALONE_OSX
        Process.Start("open", path);
#elif UNITY_STANDALONE_LINUX
        Process.Start("xdg-open", path);
#endif
    }

    private System.Collections.IEnumerator LoadAndSetSprite(string filePath)
    {
        // Charger l'image en tant que texture
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        // Créer un Sprite et l'afficher dans l'UI
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        capturedImageDisplay.sprite = newSprite;

        yield return null;
    }

    private void OnDestroy()
    {
        webcamTexture.Stop();
    }
}
