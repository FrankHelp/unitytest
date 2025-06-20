using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json; // Use Newtonsoft.Json

public class DialogueHandler : MonoBehaviour
{
    private List<ChatMessage> messages;
    [SerializeField] public Button recordingButton;

    private OpenAIChatGPT chatGPT;
    private TTSService tts;
    [SerializeField] public WhisperTranscriber _transcriber;

    private bool isRecording;
    private AudioClip recordedClip;

    private PromptHandler prompts;

    void OnEnable()
    {
        // Events abonnieren
        _transcriber.OnTranscriptionSuccess += HandleTranscriptionSuccess;
        _transcriber.OnTranscriptionError += HandleTranscriptionError;
    }

    void OnDisable()
    {
        // Events wieder abmelden (wichtig um Memory Leaks zu vermeiden!)
        _transcriber.OnTranscriptionSuccess -= HandleTranscriptionSuccess;
        _transcriber.OnTranscriptionError -= HandleTranscriptionError;
    }

    void Start()
    {
        tts = gameObject.AddComponent<TTSService>();
        chatGPT = gameObject.AddComponent<OpenAIChatGPT>();
        prompts = new PromptHandler();

        // Erstelle das Dictionary mit den Nachrichten
        messages = new List<ChatMessage>();

        recordingButton.onClick.AddListener(ToggleRecording);
        sendMessage(prompts.GetCurrentUserPrompt());
    }

    public void sendMessage(string userPrompt)
    {
        if(prompts.CheckForPromptSwitch())
        {
            messages.Add(new ChatMessage("system", prompts.GetCurrentSystemPrompt()));
            messages.Add(new ChatMessage("user", prompts.GetCurrentUserPrompt()));
            Debug.Log("Promptswitch!");
        }else
        {
            messages.Add(new ChatMessage("system", prompts.GetCurrentSystemPrompt()));
            messages.Add(new ChatMessage("user", userPrompt));
        }
        StartCoroutine(chatGPT.GetChatGPTResponse(messages, OnResponseReceived));
    }

    void OnResponseReceived(string jsonResponse)
    {
         // Deserialisiere die JSON-Antwort
        var response = JsonConvert.DeserializeObject<OpenAIStructuredResponse>(jsonResponse);
        string full_response = "";
        var contentString = response.choices[0].message.content;
        var languageParts = JsonConvert.DeserializeObject<LanguageParts>(contentString);
        // Debug.Log("response: " + languageParts);
        // Gehe alle parts durch
        foreach (var part in languageParts.parts)
        {
            if (part.language == "de")
            {
                tts.RequestTTSde(part.text); // Text auf Deutsch an TTS senden
                full_response += part.text;
            }
            else if (part.language == "fr")
            {
                tts.RequestTTSfr(part.text); // Text auf Französisch an TTS senden
                full_response += part.text;
            }
            else
            {
                Debug.LogWarning($"Unbekannte Sprache: {part.language}");
            }
        }

        messages.Add(new ChatMessage("assistant", full_response));
        Debug.Log("ChatGPT Response: " + full_response);
    }

    void ToggleRecording()
    {
        if (!isRecording)
        {
            // Starte Aufnahme
            recordedClip = Microphone.Start(null, false, 60, 16000); // 60 Sekunden, 16 kHz
            recordingButton.GetComponentInChildren<Text>().text = "Stop Recording";
        }
        else
        {
            // Stoppe Aufnahme
            Microphone.End(null);

            // Konvertiere AudioClip in WAV-Daten
            byte[] wavData = ConvertAudioClipToWav(recordedClip);
            _transcriber.SendAudioRequest(wavData);

            recordingButton.GetComponentInChildren<Text>().text = "Start Recording";
        }
        isRecording = !isRecording;
    }

    byte[] ConvertAudioClipToWav(AudioClip clip)
    {
        return WavUtility.FromAudioClip(clip);
    }

    private void HandleTranscriptionSuccess(string result)
    {
        Debug.Log("Transkription erhalten: " + result);
        sendMessage(result);
    }

    // Event-Handler für Fehler
    private void HandleTranscriptionError(string error)
    {
        Debug.LogError("Transkription fehlgeschlagen: " + error);
        // Fehlerbehandlung (z. B. Fehlermeldung anzeigen)
    }
}
[System.Serializable]
public class OpenAIStructuredResponse
{
    public List<Choice> choices { get; set; }
}

[System.Serializable]
public class Choice
{
    public Message message { get; set; }
}

[System.Serializable]
public class Message
{
    public string role { get; set; }
    public string content { get; set; } // Achtung: Hier wird das JSON-Schema erwartet!
}

[System.Serializable]
public class LanguageParts
{
    public List<Part> parts { get; set; }
}

[System.Serializable]
public class Part
{
    public string language { get; set; }
    public string text { get; set; }
}