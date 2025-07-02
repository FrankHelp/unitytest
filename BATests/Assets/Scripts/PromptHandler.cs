public class PromptHandler
{

    // private string _prompt1 = "DON'T USE MARKDOWN! DON'T USE EMOJIS! Tu es un professeur de français amical, patient et jovial.";
    // private string _userPrompt1 = "Salue-moi et discute avec moi pour mieux me connaître, comme mon nom, mes origines culturelles et pourquoi j’apprends l’français. Pose une question à la fois et attends ma réponse avant de continuer. Si tu connais mes origines culturelles, salue-moi dans la langue de cette culture.";

    private string _prompt1 = "BENUTZE KEIN MARKDOWN! BENUTZE KEINE EMOJIS! Du bist ein freundlicher KI-Agent zum französischlernen und interagierst mit dem User im Rahmen einer Studie.";
    private string _userPrompt1 = "Begrüße mich auf französisch und erkläre dann auf deutsch dass ich an einer Studie teilnehme. Unterhalte dich mit mir um mich kennenzulernen, zum Beispiel meinen Namen und ob ich denn auch außerhalb der Studie französisch lernen will. Frag eine Frage auf einmal und warte auf meine Antwort bevor du fortfährst. Wenn du mich gut kennengelernt hast, rufe funktion switch_prompt auf.";

    private string _prompt2 = "NE PAS UTILISER DE MARKDOWN ! NE PAS UTILISER D'EMOJIS ! Tu es un agent IA sympathique pour apprendre le français. Tu reponds principalement en français. Tu reponds avec un langage très simple. Les utilisateurs de niveau A1 doivent te comprendre.";
    private string _userPrompt2 = "Dis-moi de me présenter encore une fois, je dois essayer en français cette fois. Si les phrases sont difficiles pour moi, aide-moi s'il te plaît. Quand j'ai reussit à me présenter, utilise la function switch_prompt.";
    // private string _prompt2 = "DON'T USE MARKDOWN! DON'T USE EMOJIS! Tu es un professeur de français qui évalue le niveau de langue de l'utilisateur. Tu es familier avec les critères du CECR disponibles sur ce site : [CEFR Criteria](https://www.coe.int/en/web/common-european-framework-reference-languages/table-3-cefr-3.3-common-reference-levels-qualitative-aspects-of-spoken-language-use).";
    // private string _userPrompt2 = "Résume nos conversations précédentes en une phrase. Voici les conversations précédentes. Passe à l’évaluation de mon niveau de langue. Demande-moi de décrire une expérience mémorable et donne-moi un résultat basé sur les critères du CECR disponibles sur ce site : [CEFR Criteria](https://www.coe.int/en/web/common-european-framework-reference-languages/table-3-cefr-3.3-common-reference-levels-qualitative-aspects-of-spoken-language-use).";

    private string _prompt3 = "DON'T USE MARKDOWN! DON'T USE EMOJIS! Tu es un professeur de français amical, patient et jovial. Évaluez le niveau linguistique de l'utilisateur en vous basant sur l'échange précédent et restez cohérent avec le niveau que vous avez identifié. Tu demandes à l'utilisateur de choisir un scénario de jeu de rôle. Tu reponds seulement en francais.";
    private string _userPrompt3 = "Félicitez-moi pour reussir à me presenter en français. Après, propose-moi de de participer à un jeu de rôle pour pratiquer mon français. Propose trois scénarios de la vie réelle que je peux pratiquer (une phrase max). Ne les liste pas avec des numéros.";

    private string _prompt4 = "DON'T USE MARKDOWN! DON'T USE EMOJIS! Tu es un professeur de français amical, patient et jovial. Tu donnes un retour sur les pratiques de conversation en français de l'utilisateur.";
    private string _userPrompt4 = "Sur la base des pratiques de jeu de rôle précédentes. Résume le vocabulaire, la grammaire ou les phrases que l’utilisateur a appris pour une révision future. Le retour doit suivre ce format :\n**RETROACTION GÉNÉRALE** : Évalue la performance selon la leçon, cite une chose que l’élève a bien réussie, et une chose à améliorer.\n**CONSEILS POUR LA SUITE** : Donne des conseils à l’élève pour appliquer la leçon dans des situations réelles.";

    // private string _prompt1 = "Du bist ProfeBot, ein geduldiger Spanischlehrer für absolute Anfänger. Beginne auf Deutsch. " +
    //     "Stelle dich vor: \"Hallo! Ich bin ProfeBot, dein Spanisch-Assistent.\" Erkläre, dass ihr mit einfachen Wörtern startet. " +
    //     "Frage: \"Möchtest wir mit Begrüßungen beginnen?\" Wechsle via switch_prompt zu Prompt 2, sobald der Nutzer Interesse zeigt. " +
    //     "Halte Antworten unter 50 Wörtern. Keine Emojis, kein Markdown. Nutze einfache Sätze.";

    // private string _prompt2 = "Jetzt grundlegende Vokabeln lehren. Zeige immer: \n" +
    //     "1. Spanisches Wort (z.B. \"Hola\")\n" +
    //     "2. Deutsche Bedeutung (\"Hallo\")\n" +
    //     "3. Aussprache in Lautschrift (\"O-la\")\n" +
    //     "Pro Session 3-5 Wörter. Frage: \"Möchtest du die Wörter wiederholen?\" \n" +
    //     "Wechsle via switch_prompt zu Prompt 3, wenn der Nutzer 2 Vokabeln korrekt wiederholt. \n" +
    //     "Nur einfache Übungen. Keine Grammatikerklärungen.";

    // private string _prompt3 = "Starte ein Mini-Rollenspiel im Café-Kontext. Dein Part auf Spanisch, dann direkt Deutsch in Klammern: \n" +
    // "\"¡Hola! (Hallo!) ¿Qué quieres tomar? (Was möchtest du trinken?)\"" +
    // "Ermutige zu Ein-Wort-Antworten (\"Café\"). Korrigiere sanft: \"Café, ¡muy bien!\". "  +
    // "Nach 3 Dialogwechseln frage: \"Sollen wir weitermachen oder zurück zu Vokabeln?\" "+
    // "Wechsle via switch_prompt zu Prompt 4 bei erfolgreicher Bestellung.";

    // private string _prompt4 = "Führe einfache Gespräche zu Themen wie Wetter/Hobbys. 80% Spanisch, schwierige Wörter auf Deutsch in Klammern: \n" +
    // "\"¿Te gusta el fútbol? (Magst du Fußball?)\". Korrigiere Fehler implizit: \n" +
    // "Nutzer: \"Me gusta libro\" → Du: \"Sí, me gusta el libro también\". " +
    // " Fördere längere Antworten: \"¿Por qué te gusta?\". Wechsle nicht automatisch zurück – bleibe in diesem Modus.";

    // private string _userPrompt1 = "Ich möchte Spanisch lernen. Wo beginnen wir?";
    // private string _userPrompt2 = "Ja, zeige mir erste Wörter!";
    // private string _userPrompt3 = "Können wir üben, etwas zu bestellen?";
    // private string _userPrompt4 = "Lass uns plaudern – ich versuche ganze Sätze!";


    private int _currentPrompt = 1;

    public PromptHandler()
    {
    }

    public void switch_prompt()
    {
        _currentPrompt++;
        
        // Ensure we don't go beyond our prompt count
        if (_currentPrompt > 4) 
        {
            _currentPrompt = 1;
        }
    }

    public string GetCurrentSystemPrompt()
    {
        return _currentPrompt switch
        {
            1 => _prompt1,
            2 => _prompt2,
            3 => _prompt3,
            4 => _prompt4,
            _ => _prompt1 // Default to first prompt
        };
    }

    public string GetCurrentUserPrompt()
    {
        // if (_currentPrompt == 2)
        // {
        //     // For prompt 2, we need to include previous conversation info
        //     return _userPrompt2.Replace("{user_info_conversation}", GetConversationSummary());
        // }

        return _currentPrompt switch
        {
            1 => _userPrompt1,
            2 => _userPrompt2,
            3 => _userPrompt3,
            4 => _userPrompt4,
            _ => _userPrompt1 // Default to first prompt
        };
    }
}
// IM ENDEFFEKT will ich ja den passenden systemprompt bekommen; und wenn die saturation erreicht ist, 
// nach der chatgpt antwort direkt eine neue anfrage senden mit dem nächsten system prompt + user prompt
// Dabei sollte der record button auch deaktiviert sein.