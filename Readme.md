# 🧠 SPL Bot Hub  
*A lightweight framework for building C# bots for SPL games.*
<img width="631" height="539" alt="image" src="https://github.com/user-attachments/assets/76822d5f-9a2a-42d6-8571-d2ae9d03ed26" />

---

## 🚀 What This Framework Gives You

- **🔥 Plug-and-play bots** — drop a .NET DLL into `/Bots` and the app loads it automatically  
- **🔌 Connect/Disconnect SBOX** — controlled directly from the UI  
- **📡 WebSocket integration** — bots send and receive game events from SBOX  
- **📡 Real-time logs** — instantly see Bot ↔ SBOX communication  
- **⚡ Multiple bots supported** — pick any bot at runtime  
- **🔁 Hot-reload** — rebuild your bot DLL and click *Reload Bots*  
- **🛠 Minimal boilerplate** — implement one method and your bot is ready  
- **🤖 Built-in AI Service** — framework includes an AI service that wraps **OpenAI APIs** for generating bot moves  
- **🔑 OpenAI API Key support** — configurable through `appsettings.json`  

---

## 🛠 Before You Start — Update Your SPL API Key

The framework requires your **OpenAI API key** for generating bot moves.

Update it in:

```
BotHub/appsettings.json
```

Example:

```json
{
  "ApiKey": "YOUR_API_KEY_HERE"
}
```

Without this key, the inbuilt AI service will not work.

---

## 🎮 How the Framework Works

The BotHub host application handles:

- Opening the WebSocket connection to SBOX  
- Parsing incoming messages  
- Providing them to the selected bot  
- Sending bot  moves back to SBOX  
- Updating the log panel in real time  
- Giving bots access to the built-in OpenAI-backed AI service  

**You only write game logic.  
Everything else is handled for you.**

---

## ✨ Create Your First Bot (3 Easy Steps)

### **1. Add a bot class inside the `Bots` project**

This example shows the typical flow:

- Build an AI prompt  
- Ask the inbuilt OpenAI-powered AI service  
- Create a `BotGuessMessage`  
- Send it through `IBot.SendMessageToSBox()`  

```csharp

public class SampleBot(ISboxClient sBoxClient, IAIService aIService, string? name = null)
    : BotBase(sBoxClient, aIService, name)
{
    protected override async void HandleCommand(CommandMessage command)
    {
        // Build AI prompt from game history
        string prompt = BuildPrompt(state, command);

        // Use the built-in OpenAI wrapper to generate a guess
        string guess = await AIService.AskAsync(prompt);

        // Construct the message for SBOX
        BotGuessMessage botGuess = new()
        {
            MatchId = command.MatchId,
            GameId = command.GameId,
            Otp = command.Otp,
            Guess = guess
        };

        // Convert this instance to IBot to send the response
        IBot bot = this;

        await bot.SendMessageToSBox(botGuess);
    }
}
```

This is **all you need** to build a working bot.

---

### **2. Build the solution**

Your bot DLL is automatically copied into:

```
BotHub/bin/Debug/net8.0-windows/Bots/
```

The host application automatically discovers and loads all bot DLLs.

---

### **3. Run the BotHub Application**

Inside the UI:

1. Click **Connect to SBOX**  
2. Select your bot  
3. Click **Start Bot**  
4. Join the your room at:  
   **http://sbox-live.solitontech.local**  
5. Watch your bot respond to commands  
6. View all traffic in the real-time log viewer
7. Option to filter logs by GameID to debug your bot responses per game. 

---

## ✔ That’s It! 

Happy bot building! 🤖🔥
