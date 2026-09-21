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

**BrainBoostVR** is a virtual reality application created as a solo portfolio project. Its goal is to offer immersive cognitive exercises that help users improve memory, focus, and spatial awareness.  

The project combines:

* A VR environment with interactive objects and intuitive locomotion.
* Real-time scoring and feedback for each exercise.
* A backend API to store users, sessions, exercises, and scores.
* Firebase Anonymous Authentication for secure session management.
* Persistent storage via a MySQL database.

The project was developed following an MVP approach with Agile-inspired sprints, ensuring rapid iteration and testing.

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

Open the project in Unity 6.

Ensure XR Plugin Management is installed (Oculus / OpenXR for Oculus Quest 2).

Open the Main Scene or Menu Scene and explore the VR environment:

Move with teleport or joystick.

Interact with objects (grab, release).

Follow UI panels for instructions and feedback.

### 2. Backend API

Navigate to the BrainBoostVR_API folder.

### 3. Restore dependencies:
```bash
dotnet restore
```

Configure appsettings.json with your MySQL connection:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BrainBoostVR;User=root;Password=YOUR_PASSWORD;"
  }
}
```

### 4. Run the API locally:
```bash
dotnet run
```

## Project Structure
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

## API Endpoints
Users

POST /api/users → Create/register a user
```json
{
  "firebaseUID": "string",
  "name": "string"
}
```

## Scores

POST /api/scores → Submit a score
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
GET /api/scores/{userID} → Retrieve all scores for a user

## Sessions

POST /api/sessions → Create a session
```json
{
  "userID": 1,
  "startTime": "2025-10-28T14:00:00",
  "endTime": "2025-10-28T14:05:00",
  "durationMinutes": 5
}
```
GET /api/sessions/{userID} → Retrieve all sessions for a user

## Database Models
| Model    | Description                                           |
| -------- | ----------------------------------------------------- |
| User     | Stores each user with `firebaseUID` and name          |
| Score    | Tracks exercise results linked to users and exercises |
| Session  | Records VR session start/end time and duration        |
| Exercise | Stores exercise-specific performance metrics          |

## VR Features & Interactions
### Locomotion
- Teleportation & joystick movement
- Collision handling via CharacterController

### Object Interaction
- Grab/release using XR Grab Interactable
- Feedback for correct/incorrect placements

### UI
- Main Menu (Play, Tutorial, Quit)
- Tutorial panels with voice instructions & subtitles
- Real-time score panels
- End-of-session performance summary

### Audio
- Spatialized environmental sounds
- Feedback sounds for user actions

## Testing
### VR Manual Tests
- Tested on Oculus Quest 2
- Validated object interaction, teleportation, and scoring mechanics

### API Tests
- Postman used to test endpoints
- Verified MySQL storage for users, scores, sessions

### Integration
- Unity ↔ API communication verified
- Firebase anonymous authentication tested

### Trello Board
BrainBoostVR Trello
 – Track tasks, sprints, and backlog
