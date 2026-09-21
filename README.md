# 🧠 BrainBoostVR

Development branch of **BrainBoostVR**, a virtual reality application designed around immersive cognitive exercises, interactive objects, real-time feedback, and session tracking.

This branch contains the current development version of the project, including the Unity VR application and its ASP.NET Core backend API.

---

## 💡 Overview

BrainBoostVR combines:

* A VR environment with interactive cognitive exercises
* Object-based XR interactions
* Real-time visual and audio feedback
* Score and session tracking
* Tutorial and user guidance
* Firebase Anonymous Authentication
* An ASP.NET Core REST API
* MySQL persistent storage

The application targets **Oculus Quest 2**.

---

## 🛠 Tech Stack

* **Unity 6** + XR Interaction Toolkit
* **C#**
* **ASP.NET Core / .NET 8**
* **Firebase Authentication**
* **MySQL**
* **Oculus Quest 2**
* **Postman**
* **Git / GitHub**

---

## 🏗 Architecture

```text
Oculus Quest 2
       │
       ▼
  Unity VR App
       │
    REST API
       ▼
 ASP.NET Core API
    │       │
    ▼       ▼
 MySQL   Firebase Auth
```

Unity manages the VR experience, interactions, exercises, scoring, and user interface.

The ASP.NET Core API handles communication with the database and manages persistent application data.

Firebase provides anonymous authentication and a unique Firebase UID for each authenticated user.

---

## 📁 Repository Structure

```text
BrainBoostVR/
├── BrainBoostVR_Unity/
│   └── BrainBoostVR/
│       ├── Assets/
│       │   ├── Scenes/
│       │   ├── Scripts/
│       │   ├── Prefabs/
│       │   └── Resources/
│       │
│       ├── BrainBoostVR_API/
│       │   ├── Controllers/
│       │   ├── Models/
│       │   ├── Services/
│       │   └── Program.cs
│       │
│       ├── Packages/
│       └── ProjectSettings/
│
├── portfolio-project/
│   ├── Stage_1_Report.md
│   ├── The_Project_Charter.md
│   └── Technical_documentation.md
│
└── README.md
```

### Main project directories

**`BrainBoostVR_Unity/BrainBoostVR/Assets/`**

Contains the Unity VR application, including scenes, scripts, prefabs, UI, audio, and other assets.

**`BrainBoostVR_Unity/BrainBoostVR/BrainBoostVR_API/`**

Contains the ASP.NET Core backend API.

**`portfolio-project/`**

Contains the project's research, planning, and technical documentation.

---

## 🚀 Installation

### 1. Clone the repository

```bash
git clone https://github.com/Sweetyamnesia/BrainBoostVR.git
cd BrainBoostVR
```

### 2. Open the Unity project

Open the following folder with **Unity 6.x**:

```text
BrainBoostVR_Unity/BrainBoostVR
```

Unity should restore the required packages through the Unity Package Manager.

The project is configured for XR development and Oculus Quest 2.

---

## ⚙️ Backend API Setup

Navigate to:

```text
BrainBoostVR_Unity/BrainBoostVR/BrainBoostVR_API
```

Restore the .NET dependencies:

```bash
dotnet restore
```

Configure the MySQL connection in `appsettings.json`.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BrainBoostVR;User=root;Password=YOUR_PASSWORD;"
  }
}
```

Then start the API:

```bash
dotnet run
```

> Do not commit database passwords, Firebase private credentials, or other sensitive configuration.

---

## 🔐 Firebase Authentication

BrainBoostVR uses **Firebase Anonymous Authentication**.

The authentication flow is:

```text
Unity
  │
  ▼
Firebase Anonymous Authentication
  │
  ▼
Firebase UID
  │
  ▼
ASP.NET Core API
  │
  ▼
MySQL
```

The Firebase project must have **Anonymous Authentication enabled**.

---

## 🌐 API Endpoints

### Users

Create or register a user:

```http
POST /api/users
```

Example:

```json
{
  "firebaseUID": "string",
  "name": "string"
}
```

### Scores

Submit an exercise score:

```http
POST /api/scores
```

Example:

```json
{
  "userID": 1,
  "exerciseID": 2,
  "score": 5,
  "successes": 5,
  "failures": 0,
  "durationMinutes": 2.5
}
```

Retrieve scores for a user:

```http
GET /api/scores/{userID}
```

### Sessions

Create a session:

```http
POST /api/sessions
```

Retrieve sessions for a user:

```http
GET /api/sessions/{userID}
```

---

## 🗄 Database

The main entities are:

| Entity              | Description                                          |
| ------------------- | ---------------------------------------------------- |
| **User**            | Stores application users and their Firebase identity |
| **FirebaseProfile** | Links Firebase authentication data to a user         |
| **Exercise**        | Stores exercise-related information                  |
| **Score**           | Stores exercise results and performance metrics      |
| **Session**         | Stores VR session information and duration           |

For the complete database design and relationships, see the [Technical Documentation](./portfolio-project/Technical_documentation.md).

---

## 🥽 VR Features

### Locomotion

* Teleportation
* Joystick movement
* Character collision handling

### Object Interaction

* Grab and release interactions
* XR Grab Interactable objects
* Correct / incorrect interaction feedback

### Exercises

* Interactive cognitive exercises
* Object manipulation
* Spatial interaction
* Score calculation
* Performance tracking

### User Interface

* Main menu
* Tutorial
* Exercise interface
* Real-time score feedback
* End-of-session summary
* Session history

### Audio

* Tutorial voice instructions
* Subtitles
* Interaction feedback
* Environmental audio

---

## 🧪 Testing

The project is tested at several levels.

### VR Testing

Manual testing on Oculus Quest 2 covers:

* Locomotion
* Object interactions
* Exercise behaviour
* Scoring
* Tutorial flow
* UI interactions

### API Testing

Postman can be used to test:

* User creation
* Score submission
* Session creation
* Session retrieval
* Database persistence

### Integration Testing

The complete application verifies communication between:

```text
Firebase
   ↓
Unity
   ↓
ASP.NET Core API
   ↓
MySQL
```

---

## 📚 Documentation

The repository contains three complementary project documents.

### 📋 Stage 1 Report

[Stage 1 Report](./portfolio-project/Stage_1_Report.md)

Research, ideation, MVP definition, rejected ideas, risks, and core project features.

### 📐 Project Charter

[Project Charter](./portfolio-project/The_Project_Charter.md)

Project objectives, scope, stakeholders, risks, and planning.

### ⚙️ Technical Documentation

[Technical Documentation](./portfolio-project/Technical_documentation.md)

System architecture, user stories, Unity and API classes, database design, API specifications, sequence diagrams, SCM, QA, and technical decisions.

---

## 🌐 Project Presentation

The stable project presentation is available on the `main` branch.

For the visual presentation of BrainBoostVR, visit the [BrainBoostVR Landing Page](https://sweetyamnesia.github.io/brainboostvr-landing/).

---

## 🌱 Git Branches

The project uses two main branches:

* **`development`** → active development and ongoing changes
* **`main`** → stable and presentable version

Changes are developed and tested on `development` before being promoted to `main`.

---

## 📌 Project Status

**MVP completed.**

The development branch may contain ongoing improvements to:

* XR interactions
* User experience
* Performance
* Stability
* Project presentation
