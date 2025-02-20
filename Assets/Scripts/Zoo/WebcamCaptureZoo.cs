using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;

public class WebcamCaptureZoo : MonoBehaviour
{
    [SerializeField] private ZooManager zoo;
    [SerializeField] private float toleranceColorTransparency = 0.1f;
    [SerializeField] private Color colorTestTransparency;

    [Header("UI"), Space(6)]

    [SerializeField] private GameObject TakePhotoWindow;
    [SerializeField] private GameObject buttonOpenPhotoWindow;
    public RawImage display; // Affiche la webcam sur un UI RawImage
    public Image capturedImageDisplay; // Affiche la photo capturée en Sprite
    public InputField captureNameFile; // Change le nom de la capture
    public InputField captureSize; // Change la taille de la capture

    private WebCamTexture webcamTexture;
    private Sprite _actualSprite;

    void Start()
    {
        OpenPhotoMenu(false);

        // Démarrer la webcam
        webcamTexture = new WebCamTexture();
        display.texture = webcamTexture;
        webcamTexture.Play();
    }

    public void CapturePhoto()
    {
        string namePhoto = captureNameFile.text.Replace(" ", "");

        if (namePhoto.Length != 0)
        {
            // Créer une texture pour stocker l'image de la webcam
            Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
            photo.SetPixels(webcamTexture.GetPixels());
            photo.Apply();

            // Convertir en bytes (PNG)
            byte[] bytes = photo.EncodeToPNG();

            // path directory & capture
            string filePath = Application.persistentDataPath + "/Zoo_photos/";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = Path.Combine(filePath, namePhoto + "_capture.png");

            // Enregistrer l'image sur le disque
            File.WriteAllBytes(filePath, bytes);
            UnityEngine.Debug.Log("Photo sauvegardée à : " + filePath);

            // Charger l'image enregistrée et l'affecter à un sprite
            StartCoroutine(LoadAndSetSprite(filePath));
        }
    }

    public void CreateNewPlayer()
    {
        if(_actualSprite != null)
        {
            string sizeStr = captureSize.text.Replace(".", ",");
            float size = float.Parse(sizeStr);
            zoo.AddPlayer(captureNameFile.text, _actualSprite, size);
            // reset buttons
            capturedImageDisplay.sprite = null;
            _actualSprite = null;
            captureNameFile.text = "";
            OpenPhotoMenu(false);
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

    public void OpenPhotoMenu(bool value)
    {
        TakePhotoWindow.SetActive(value);
        buttonOpenPhotoWindow.SetActive(!value);
    }

    private System.Collections.IEnumerator LoadAndSetSprite(string filePath)
    {
        // Charger l'image en tant que texture
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        // Créer un Sprite et l'afficher dans l'UI
        Sprite newSprite = TransparentColor(texture, colorTestTransparency);

        capturedImageDisplay.sprite = newSprite;

        _actualSprite = newSprite;

        yield return null;
    }

    /// <summary>
    /// Delete the colorToRemove in the texture
    /// </summary>
    /// <param name="texture"> The texture to modify </param>
    /// <param name="colorToRemove"> The color to remove in the image </param>
    /// <returns>A Sprite</returns>
    private Sprite TransparentColor(Texture2D texture, Color colorToRemove)
    {
        Texture2D newTexture = new Texture2D(texture.width, texture.height);

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color pixel = texture.GetPixel(x, y);
                if (Vector3.Distance(new Vector3(pixel.r, pixel.g, pixel.b), new Vector3(colorToRemove.r, colorToRemove.g, colorToRemove.b)) < toleranceColorTransparency)
                {
                    pixel.a = 0;
                }
                newTexture.SetPixel(x, y, pixel);
            }
        }
        newTexture.Apply();
        return Sprite.Create(newTexture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private void OnDestroy()
    {
        webcamTexture.Stop();
    }
}
