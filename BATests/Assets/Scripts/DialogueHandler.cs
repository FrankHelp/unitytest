using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json; // Use Newtonsoft.Json
using ChatGPTIntegration;

public class DialogueHandler : MonoBehaviour
{
    private List<ChatMessage> messages;
    [SerializeField] public Button recordingButton;

    private OpenAIChatGPT chatGPT;
    private TTSService tts;
    private WhisperTranscriber _transcriber;

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
        _transcriber = gameObject.AddComponent<WhisperTranscriber>();
        prompts = new PromptHandler();

        // Erstelle das Dictionary mit den Nachrichten
        messages = new List<ChatMessage>();

        recordingButton.onClick.AddListener(ToggleRecording);
        sendMessage(prompts.GetCurrentUserPrompt());
    }

    public void sendMessage(string userPrompt)
    {
        if(userPrompt == null)
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

        // ---------------------------------------------------------------
        var parts = ResponseDeserializer.GetParts(jsonResponse);
        string full_response = "";
        if (parts != null)
        {
            tts.announceParts(parts.Count);
            foreach (var part in parts)
            {
                if (part.language == "de")
                {
                    tts.RequestTTSde(part.text); // Text auf Deutsch an TTS senden
                    full_response += part.text;
                }
                else if (part.language == "fr") // HIER fr
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

        // Get tool calls
        var toolCalls = ResponseDeserializer.GetToolCalls(jsonResponse);
        if (toolCalls != null)
        {
            foreach (var toolCall in toolCalls)
            {
                if(toolCall.function.name == "switch_prompt")
                {
                    prompts.switch_prompt();
                    if(parts == null)
                    {
                        sendMessage(null);
                    }
                }
                Debug.Log($"Tool Call: {toolCall.function.name}, Arguments: {toolCall.function.arguments}");
                
                // Optionally deserialize arguments into a specific class
                // var arguments = ResponseDeserializer.GetToolCallArguments<YourArgumentClass>(toolCall);
                // if (arguments != null)
                // {
                //     // Use arguments
                // }
            }
        }
        // -----------------------------------
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