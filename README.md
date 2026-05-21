<div align="center">

# PrograMEMEin' 🤓

### An ASP.NET Core web app that automatically posts developer jokes & memes to Instagram.

**Because someone had to automate the dev humour pipeline.**

<br/>

[![C#](https://img.shields.io/badge/C%23-85%25-239120?style=flat-square&logo=csharp&logoColor=white)](https://github.com/georgidelchev/programemein/search?l=c%23)
[![HTML](https://img.shields.io/badge/HTML-13%25-E34F26?style=flat-square&logo=html5&logoColor=white)](https://github.com/georgidelchev/programemein/search?l=html)
[![Instagram](https://img.shields.io/badge/Integration-Instagram%20API-E1306C?style=flat-square&logo=instagram&logoColor=white)](https://developers.facebook.com/docs/instagram-api/)
[![Build](https://img.shields.io/github/actions/workflow/status/georgidelchev/programemein/dotnet.yml?style=flat-square&label=CI&logo=github-actions&logoColor=white)](https://github.com/georgidelchev/programemein/actions)
[![License](https://img.shields.io/badge/License-MIT-6366F1?style=flat-square)](LICENSE)
[![Stars](https://img.shields.io/github/stars/georgidelchev/programemein?style=flat-square&color=F59E0B)](https://github.com/georgidelchev/programemein/stargazers)

<br/>

*The name is a pun — "programming" + "meme" + "in'" — because naming things is still the hardest problem in CS.*

<br/>

</div>

---

## 😂 About

**PrograMEMEin'** is an ASP.NET Core web application with a singular, glorious purpose: automatically uploading funny developer jokes and programming memes to an Instagram page — completely hands-free.

The app manages a library of dev jokes and meme content, then pushes them to Instagram on a schedule via the Instagram Graph API. No manual posting. No copy-paste. Just pure, automated programmer humour delivered to the masses while you sleep.

It's funny *and* it's a proper integration project. The best kind.

---

## ✨ Features

### 🤖 Automated Instagram Publishing
- Connects to the **Instagram Graph API** (via Facebook's developer platform) using OAuth
- Automatically uploads jokes and meme content to a configured Instagram page on a schedule
- Handles image formatting, captions, and post metadata
- Background job scheduling for hands-free, recurring posts

### 📚 Content Management
- Web interface to browse, add, edit, and manage the joke/meme library
- Content categorisation and tagging
- Preview content before it goes live

### 🔐 Authentication & Security
- ASP.NET Core Identity for admin access
- Secure OAuth token management for the Instagram API integration
- Role-based access to content management features

### ⚙️ Infrastructure
- **GitHub Actions CI** — automated build and test pipeline on every push
- Clean `src/` project structure separating concerns clearly

---

## 🏗️ Architecture

```
programemein/
└── src/
    ├── PrograMEMEin.Web/          # ASP.NET Core MVC — controllers, views, admin UI
    ├── PrograMEMEin.Services/     # Business logic — content management, Instagram posting
    ├── PrograMEMEin.Data/         # DbContext, EF Core, migrations
    └── PrograMEMEin.Data.Models/  # Domain entities (Joke, Meme, PostSchedule…)
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core MVC |
| Language | C# (.NET) |
| ORM | Entity Framework Core |
| Auth | ASP.NET Core Identity |
| Social Integration | Instagram Graph API (Facebook Developer Platform) |
| Database | MS SQL Server |
| CI/CD | GitHub Actions |
| IDE | Visual Studio |

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (local or remote)
- A [Facebook Developer account](https://developers.facebook.com/) with an Instagram Business page connected

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/georgidelchev/programemein.git
   cd programemein/src
   ```

2. **Configure credentials** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=PrograMEMEin;Trusted_Connection=True;"
     },
     "Instagram": {
       "AccessToken": "your-instagram-graph-api-token",
       "PageId": "your-instagram-page-id"
     }
   }
   ```

3. **Apply migrations and run**
   ```bash
   dotnet ef database update
   dotnet run
   ```

4. Add some jokes to the content library and watch them appear on Instagram automatically. 🎉

---

## 👤 Author

**Georgi Delchev**

[![GitHub](https://img.shields.io/badge/GitHub-georgidelchev-181717?style=flat-square&logo=github)](https://github.com/georgidelchev)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-delchevgeorgi-0A66C2?style=flat-square&logo=linkedin)](https://www.linkedin.com/in/delchevgeorgi/)
[![Facebook](https://img.shields.io/badge/Facebook-georgi.d99-1877F2?style=flat-square&logo=facebook)](https://www.facebook.com/georgi.d99/)

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

<div align="center">
<sub>Built with ☕, ASP.NET Core, and an unhealthy appreciation for developer humour</sub>
</div>
