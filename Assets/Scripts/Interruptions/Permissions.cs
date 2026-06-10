using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
//using UnityEngine.iOS;

public class Permissions : MonoBehaviour
{
    [SerializeField] RawImage preview;
    WebCamTexture webcam;

    private void Start()
    {
        webcam = new WebCamTexture();
        preview.texture = webcam;
    }

    private void Update()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("Already obtained permission");
            webcam.Play();
        }
        else
            Permission.RequestUserPermission(Permission.Camera);

        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
            Debug.Log("Already obtained permission");
        else
            Permission.RequestUserPermission(Permission.Microphone);

        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))    // GPS ad alta precisione, CoarseLocation = scarsa precisione
            Debug.Log("Already obtained permission");
        else
            Permission.RequestUserPermission(Permission.FineLocation);

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead)) // Leggere elementi nella scheda SD
            Debug.Log("Already obtained permission");
        else
            Permission.RequestUserPermission(Permission.ExternalStorageRead);

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))    // Scrittura elementi nella scheda SD
            Debug.Log("Already obtained permission");
        else
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

        // da Android 13+ esistono, prima no, erano automatici
        if (Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS")) // consentire all'app di inviare notifiche
            Debug.Log("Already obtained permission");
        else
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");

        if (Permission.HasUserAuthorizedPermission("android.permission.READ_MEDIA_IMAGES")) // consentire all'app di visualizzare immagini nel dispositivo
            Debug.Log("Already obtained permission");
        else
            Permission.RequestUserPermission("android.permission.READ_MEDIA_IMAGES");

        if (Permission.HasUserAuthorizedPermission("android.permission.READ_MEDIA_VIDEO")) // consentire all'app di visualizzare video nel dispositivo
            Debug.Log("Already obtained permission");
        else
            Permission.RequestUserPermission("android.permission.READ_MEDIA_VIDEO");

        if (Permission.HasUserAuthorizedPermission("android.permission.READ_MEDIA_AUDIO")) // consentire all'app di riprodurre audio nel dispositivo
            Debug.Log("Already obtained permission");
        else
            Permission.RequestUserPermission("android.permission.READ_MEDIA_AUDIO");

        PermissionCallbacks callbacks = new PermissionCallbacks();
        Permission.RequestUserPermission(Permission.Camera, callbacks);

        callbacks.PermissionGranted += permission =>
        {
            // l'utente ha accettato i permessi quindi posso sbloccare quell'azione
        };

        callbacks.PermissionDenied += permission =>
        {
            // l'utente ha negato i permessi quindi rimane bloccata l'azione e respawno il popup

        };

        callbacks.PermissionDeniedAndDontAskAgain += permission =>
        {
            // l'utente ha negato i permessi e non vuole che glielo chiedo più
        };

        // poi quando utilizziamo le risorse

        if (!Input.location.isEnabledByUser)
            Debug.Log("GPS disabilitato");
        else
            Input.location.Start();

        //Microphone.Start();
    }

    private void NewPhoto()
    {
        Texture2D photo = new Texture2D(webcam.width, webcam.height);
        photo.SetPixels32(webcam.GetPixels32());
        photo.Apply();
    }
}
