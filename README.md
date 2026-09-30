# MetroDCord

Finally... a native Metro-style Discord desktop client tailored specifically for the Windows 8.x family using the purist **Metro UI** design language.

## About the Project

**MetroDCord** bridges the gap for retro-computing enthusiasts, Windows 8.1 users, and members of the legacy Microsoft customization scene. While other community clients have drifted towards modern UWP architectures for Windows 10/11, MetroDCord is built from the ground up to preserve the 2012–2014 aesthetic, featuring horizontal panning typography, live tiles, and high-performance native rendering.

Built using **Visual Studio 2015**, **C#**, and **WinRT (XAML)** targeting the Windows 8.1 SDK.

## Key Features

- **Purist Metro UI:** Authentic styling matching the original Windows 8/8.1 design guidelines (using Segoe UI, panoramic `Hub`/`Pivot` structures, and solid accent palettes).
- **Anti-Flag/Safe Authentication:** Bypasses aggressive Discord telemetry blocks by injecting valid `X-Super-Properties` and matching user agents to avoid account bans.
- **Supermium Integration:** Solves legacy Internet Explorer 11 engine white-screen issues by chaining a portable instance of the Chromium-powered *Supermium* browser for zero-friction hybrid authentication.
- **Built-in Modularity (Plugin System):** Extensible architecture inspired by Vencord, allowing the integration of custom behavior modifications, image decoders, and hidden Discord experiment tools.
- **Native Custom Themes:** XAML-based `ResourceDictionary` styling framework to dynamically swap UI accents instantly without performance overhead.

## Project Structure & Architecture

```text
├── MetroDCord.Core/          # Underlying Rest API, WebSockets, and Authentication engines
│   ├── Network/              # HttpClient handlers, User-Agent Spoofing, and Header Injection
│   ├── Updates/              # Silent Supermium update and file lock handlers
│   └── Plugins/              # Extensibility interfaces (IMetroDCordPlugin)
├── MetroDCord.UI/            # Native WinRT XAML Pages and Controls
│   ├── Views/                # WelcomePage, MainPanel, and Chat view layouts
│   └── Styles/               # Centralized Metro UI Resource Dictionaries
└── Assets/                   # Splash screens, Live Tile assets, and logos
```

## How It Works (The Authentication Flow)

1. **Initialization:** Upon first launch, MetroDCord presents a native greeting screen offering standard Web Login or direct Token Injection.
2. **The Hybrid WebView Workaround:** When selecting Web Login, MetroDCord silently orchestrates an external Chromium process utilizing bundled Supermium files. 
3. **Token Interception:** A specialized lightweight background handler intercepts the authenticated storage token from Discord and securely binds it to Windows local storage.
4. **Gateway Protocol Connection:** Once the token is active, a persistent WebSocket worker handles the continuous text channel streams and heartbeats.

## Contributing

MetroDCord is a community-driven open-source project. If you are part of the legacy Windows ecosystem groups or the **NovaRevival** scene, you are highly encouraged to contribute!

1. Fork the Project.
2. Spin up your Windows 8.1 development VM.
3. Open the solution in **Visual Studio 2015** (or Visual Studio 2017 with Win8.1 SDKs enabled).
4. Create your Feature Branch (`git checkout -b feature/AmazingFeature`).
5. Commit your Changes (`git commit -m 'Add some AmazingFeature'`).
6. Push to the Branch (`git push origin feature/AmazingFeature`).
7. Open a Pull Request.

## Disclaimer

MetroDCord is a third-party application developed solely for educational and computational preservation purposes. It is not affiliated with, authorized, or maintained by Discord Inc. Modifying your client experience technically breaches Discord's Terms of Service, exercise standard caution and use alternate accounts during testing stages.

***

*Proudly developed by SouSand Artworks Studio & The MetroDCord Team.*
