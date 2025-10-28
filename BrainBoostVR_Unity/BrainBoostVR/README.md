# BrainBoostVR – Portfolio Project

![Unity Version](https://img.shields.io/badge/Unity-6.x-blue)
![.NET Version](https://img.shields.io/badge/.NET-8.x-lightgrey)
![MySQL Version](https://img.shields.io/badge/MySQL-8.x-orange)

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [Prerequisites](#prerequisites)
3. [Installation](#installation)
4. [Project Structure](#project-structure)
5. [API Endpoints](#api-endpoints)
6. [Database Models](#database-models)
7. [VR Features & Interactions](#vr-features--interactions)
8. [Testing](#testing)
9. [Trello Board](#trello-board)
10. [Contributing](#contributing)

---

## Project Overview

**BrainBoostVR** is a virtual reality application designed for cognitive exercises.

The project includes:

* VR environment with object interaction, locomotion, and tutorials.
* Real-time scoring and feedback.
* REST API backend storing users, sessions, exercises, and scores.
* Integration with Firebase Anonymous Authentication for secure session management.
* MySQL database for persistent storage.

This project was implemented solo, following an MVP approach and Agile-inspired sprints.

---

## Prerequisites

* **Unity**: Version 6.x (with XR Plugin Management)
* **XR SDK**: Version 8–9 (Oculus/VR support)
* **.NET SDK**: 8.x (for backend API)
* **MySQL**: 9.x (or compatible)
* **Firebase Project**: Anonymous Authentication enabled
* **Postman** (optional) for API testing

---

## Installation

### 1. Unity VR App

1. Clone the repository:

```bash
git clone https://github.com/YourUsername/BrainBoostVR.git
cd BrainBoostVR
```

2. Open the project in Unity 6.
3. Ensure XR Plugin Management is installed:

   * Oculus / OpenXR for Oculus Quest 2.
4. Open the `Main Scene` or `Menu Scene` and test the VR environment:

   * Locomotion (teleport and joystick)
   * Object interaction (grab/release)
   * UI panels and feedback

### 2. Backend API

1. Navigate to the `BrainBoostVR_API` folder.
2. Restore dependencies:

```bash
dotnet restore
```

3. Configure `appsettings.json` with your MySQL connection:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BrainBoostVR;User=root;Password=YOUR_PASSWORD;"
  }
}
```

4. Run the API locally:

```bash
dotnet run
```

---

## Project Structure

```
BrainBoostVR/
├─ Assets/
│  ├─ Scripts/
│  ├─ Prefabs/
│  ├─ Scenes/
│  └─ Materials/
├─ BrainBoostVR_API/
│  ├─ Controllers/
│  ├─ Models/
│  ├─ Services/
│  └─ Program.cs
├─ README.md
```

---

## API Endpoints

### Users

* `POST /api/users` → Create/register a user

```json
{
  "firebaseUID": "string",
  "name": "string"
}
```

### Scores

* `POST /api/scores` → Submit a score

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

* `GET /api/scores/{userID}` → Retrieve all scores for a user

### Sessions

* `POST /api/sessions` → Create a session

```json
{
  "userID": 1,
  "startTime": "2025-10-28T14:00:00",
  "endTime": "2025-10-28T14:05:00",
  "durationMinutes": 5
}
```

* `GET /api/sessions/{userID}` → Retrieve all sessions for a user

---

## Database Models

| Model    | Description                                         |
| -------- | --------------------------------------------------- |
| User     | Represents a user with `firebaseUID` and `name`     |
| Score    | Stores exercise results linked to user and exercise |
| Session  | Tracks VR sessions with start/end time and duration |
| Exercise | Stores exercise-specific performance metrics        |

---

## VR Features & Interactions

### Locomotion

* Teleport & joystick movement
* Collision handling with `CharacterController`

### Object Interaction

* Grab/release via XR Grab Interactable
* Feedback for correct/incorrect placements

### UI

* Main Menu (Play, Tutorial, Quit)
* Tutorial panels with voice instructions & subtitles
* Score panels updated in real-time
* End of session feedback

### Audio

* Spatialized environmental sounds
* Feedback sounds for actions

---

## Testing

### VR Manual Tests

* Tested on Oculus Quest 2
* Object interaction, teleportation, and scoring

### API Tests

* Tested endpoints using Postman
* Verified MySQL storage for users, scores, sessions

### Integration

* Unity → API communication verified
* Firebase authentication tested for anonymous login

---

## Trello Board

* [BrainBoostVR Trello](https://trello.com/invite/b/68c9259b7662ef9076f2547a/ATTId5870ec9e79f1251921aa733dc4183c8F35B15D2/brainboostvr) – Track tasks, sprints, and backlog

---

## Contributing

This is a solo project. Contributions are not expected, but feedback and suggestions are welcome via issues.
