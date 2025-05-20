# 🎧 Notes Apart

**Notes Apart** is a full-featured ASP.NET Core MVC web application built with **.NET 9**. It integrates the **Spotify API** to allow users to search songs, artists and albums, write reviews, favorite albums and songs, and interact with others through review likes. Admins have moderation powers to maintain a healthy community.

---

## 🚀 Features

### 🎶 Music Discovery (via Spotify API)
- Fetch **artists**, **albums**, and **songs** dynamically from the Spotify catalog
- View detailed pages for each song or album

### 👤 User Functionality
- Register & log in using ASP.NET Identity
- **Post reviews** on songs and albums
- **Like other users’ reviews**
- **Add songs and albums to favorites**, displayed on the user's profile
- **Can delete own reviews**
- View their own **public profile** with favorites and reviews
  
### 🛡️ Admin Features
- **Delete inappropriate reviews** from any user while accessing the admin panel page
- Only admins can **see the delete button** on reviews they don't own outside the admin panel
- Maintain a safe and moderated user experience 

---

##  Used Technologies

- **ASP.NET Core MVC (.NET 9)**
- **Entity Framework Core**
- **Spotify Web API Integration**
- **SQL Server** 
- **ASP.NET Identity** for user authentication and roles
- **Bootstrap 5** for UI/UX
- **JavaScript**

---

###  Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- Visual Studio 2022+ / VS Code
- Spotify Developer Account & API credentials
- SQL Server / SQLite

###  Setup Instructions

﻿# Spotify API Setup

To use the Spotify integration, follow these steps:

1. Go to: https://developer.spotify.com/dashboard
2. Log in with a Spotify account
3. Create a new app and copy the:
   - Client ID
   - Client Secret
4. Open the file: `appsettings.json`
5. Replace:
   - `"ClientId": "YOUR_SPOTIFY_CLIENT_ID"`
   - `"ClientSecret": "YOUR_SPOTIFY_CLIENT_SECRET"`

This is required for the app to seed artists, albums, and songs using the Spotify API.
