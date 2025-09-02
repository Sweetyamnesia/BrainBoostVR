# BrainBoostVR – Technical Documentation

## 1️⃣ User Stories

| User Story | Priority (MoSCoW) | Notes |
|------------|-----------------|-------|
| As a user, I want to navigate the VR environment using controllers, so that I can move freely and interact with objects. | Must Have | Core interaction for all exercises. |
| As a user, I want to interact with objects in the exercise, so that I can complete cognitive tasks. | Must Have | Includes grabbing, moving, or selecting objects. |
| As a user, I want immediate visual and audio feedback during exercises, so that I understand if I am performing actions correctly. | Must Have | Essential for engagement and learning. |
| As a user, I want to see my performance score at the end of each exercise, so that I can track my progress. | Must Have | Requires integration with Firebase for score storage. |
| As a user, I want to access a tutorial before starting exercises, so that I know how to use the VR controllers and interact with objects. | Must Have | Tutorial guides basic movement, object interaction, and camera rotation. |
| As a user, I want to be able to pause or exit exercises at any time, so that I can control my session comfortably. | Should Have | Optional but improves accessibility. |
| As a user, I want to have multiple difficulty levels for exercises, so that I can progressively challenge myself. | Could Have | Planned for future updates. |

## 2️⃣ Mockups / Interface Overview

The MVP does not yet include fully developed UI screens, but the following sketches represent the **main interface and user flow in VR**:

1. **Main Menu / Hub**
   - Options: Start Exercise, Tutorial, Quit
   - Display brief instructions for controller usage

2. **Exercise Scene**
   - Interactive objects in the VR environment
   - Immediate visual and audio feedback for user actions
   - Floating UI showing current score or progress (optional)

3. **Tutorial Scene**
   - Demonstrates VR interactions (grabbing objects, teleportation, camera rotation)
   - Step-by-step instructions or highlights on controllers

4. **End of Exercise**
   - Summary of performance (score, correct/incorrect actions)
   - Options: Restart, Main Menu, Exit

**Design Notes**
- Simple wireframes in Figma or Balsamiq recommended for main VR menus and floating UI elements.
- Focus on clear navigation, minimal text, and accessibility for elderly users.
- Mockups should show **object placement**, menu layout, and tutorial highlights.

## 3️⃣ API Considerations

- Firebase will be used to **store and retrieve user scores**.
- A custom API may be created to handle additional data storage or retrieval needs not covered by Firebase.
- API interactions:
  - Write scores after each exercise.
  - Read scores for progress display at the end of exercises.
  - Optional: store session metadata for analysis.
- Ensure secure authentication and minimal latency for a smooth VR experience.

## 4️⃣ Notes on Development

- Focus on creating **self-written C# scripts** for interactions and scoring.
- Use Unity XR Interaction Toolkit for VR mechanics.
- Keep tutorials and feedback systems **lightweight and clear** for first MVP.
- Plan to **iterate mockups and UI** after initial VR testing to improve usability.

# BrainBoostVR – System Architecture

## 1️⃣ Overview
The MVP consists of the following main components:

- **VR Application (Unity / C#)**  
  - Handles all user interactions, exercises, scoring, and tutorials.  
  - Runs on Oculus Quest 2.

- **Firebase (Backend as a Service)**  
  - **Authentication:** User accounts.  
  - **Firestore Database:** Stores scores and session data.

- **Custom API (Mandatory)**  
  - Connects Unity to Firebase.  
  - Provides endpoints for reading/writing scores and optional session metadata.  
  - Supports future extensions (analytics, dashboards).

---

## 2️⃣ Data Flow
1. User performs an exercise in the VR environment.  
2. VR app computes score and exercise completion.  
3. Score and session data are sent to the Custom API.  
4. Custom API updates Firebase Firestore.  
5. VR app retrieves stored scores and session data for progress display.

---

## 3️⃣ Architecture Diagram
      +-------------------+
      |   VR Application  |
      |   (Unity / C#)    |
      +-------------------+
                |
                v
      +-------------------+
      |   Custom API      |
      | (Mandatory)       |
      +-------------------+
                |
                v
      +-------------------+
      |   Firebase BaaS   |
      | Auth & Firestore  |
      +-------------------+


---

## 4️⃣ Notes
- Oculus Quest 2 runs the VR app directly.  
- Custom API ensures clean separation between front-end (Unity VR) and backend (Firebase).  
- Data flow is optimized for low latency to maintain smooth VR interactions.  
- Future scalability: The Custom API can be extended for analytics, dashboards, or additional data storage requirements without modifying the VR application.

# BrainBoostVR – Components, Classes & Database Design

## 1️⃣ Components Overview
The MVP system consists of three main layers:

- **VR Front-End (Unity + C#)**  
  - Handles VR interactions, exercises, tutorials, and local scoring.
- **Custom API**  
  - Manages communication between Unity and Firebase.
  - Ensures a clean separation between the VR app and backend services.
- **Firebase Firestore Database**  
  - Stores users, scores, and session data in a document-oriented structure.

---

## 2️⃣ Key Classes (Unity + API)

### **2.1 Unity (C#) Classes**

| Class Name       | Description                                | Key Attributes                            | Key Methods                                |
|------------------|-----------------------------------------|------------------------------------------|------------------------------------------|
| `PlayerController` | Manages player movement & interactions. | `playerID`, `position`, `controllerInput` | `MovePlayer()`, `GrabObject()`, `Teleport()` |
| `ExerciseManager` | Handles exercises & cognitive tasks.    | `exerciseID`, `difficulty`, `currentScore` | `StartExercise()`, `ValidateAnswer()`, `EndExercise()` |
| `ScoreManager`    | Manages scoring & feedback system.      | `currentScore`, `maxScore`              | `UpdateScore()`, `ShowFeedback()`, `ResetScore()` |
| `UIManager`      | Controls VR menu navigation & HUD.       | `menuPanels`, `tutorialUI`              | `ShowMainMenu()`, `ShowScorePanel()`, `ToggleTutorial()` |
| `ApiClient`      | Handles requests to the custom API.     | `baseUrl`, `authToken`                  | `PostScore()`, `GetScores()`, `HandleError()` |

---

### **2.2 Custom API (Backend) Classes**

| Class Name     | Description                              | Key Attributes            | Key Methods              |
|---------------|-------------------------------------|------------------------|-----------------------|
| `User`       | Represents a registered VR user.    | `userID`, `email`      | `CreateUser()`, `GetUser()` |
| `Score`      | Stores exercise results.            | `userID`, `exerciseID`, `score`, `timestamp` | `SaveScore()`, `GetScores()` |
| `Session`    | Tracks user sessions.              | `sessionID`, `startTime`, `endTime` | `StartSession()`, `EndSession()` |

---

## 3️⃣ Database Design (Firebase Firestore)

BrainBoostVR uses **Firestore**, a **document-oriented NoSQL database**.  
Data is stored in **collections** containing **documents** with flexible fields.

### **3.1 Collections & Documents**

#### **Collection: users**
```json
{
  "userID": "uid123",
  "email": "user@example.com",
  "createdAt": "2025-08-21"
}
```

#### **Collection: scores**
```json
{
  "scoreID": "sc001",
  "userID": "uid123",
  "exerciseID": "ex001",
  "score": 85,
  "timestamp": "2025-09-10T14:30:00Z"
}
```

#### **Collection: sessions**
```json
{
  "sessionID": "sess001",
  "userID": "uid123",
  "startTime": "2025-09-10T14:00:00Z",
  "endTime": "2025-09-10T14:30:00Z",
  "duration": 1800
}
```

### **3.2 Firestore Structure**
```bash
/users
   └── userID
        • email
        • createdAt

/scores
   └── scoreID
        • userID
        • exerciseID
        • score
        • timestamp

/sessions
   └── sessionID
        • userID
        • startTime
        • endTime
        • duration
```

## 4️⃣ VR UI Components
| UI Component    | Description                     | Interactions                         |
| --------------- | ------------------------------- | ------------------------------------ |
| **Main Menu**   | Central hub for navigation.     | Start exercise, view tutorial, quit. |
| **Tutorial UI** | Guides users through VR basics. | Highlights controllers and actions.  |
| **Score Panel** | Displays real-time scoring.     | Updates dynamically after actions.   |
| **End-Screen**  | Shows summary of performance.   | Retry, go to menu, or exit.          |

## 5️⃣ Summary
- Unity manages VR interactions, exercises, and UI.
- A Custom API securely communicates between Unity and Firebase.
- Firestore stores users, scores, and sessions using a flexible document model.
- Front-end UI focuses on clarity, accessibility, and smooth navigation.

