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
   [https://github.com/Sweetyamnesia/BrainBoostVR/blob/main/portfolio-project/Menu.png]

2. **Exercise Scene**
   - Interactive objects in the VR environment
   - Immediate visual and audio feedback for user actions
   - Floating UI showing current score or progress (optional)
   [https://github.com/Sweetyamnesia/BrainBoostVR/blob/main/portfolio-project/Time.png]

3. **Tutorial Scene**
   - Demonstrates VR interactions (grabbing objects, teleportation, camera rotation)
   - Step-by-step instructions or highlights on controllers
   [https://github.com/Sweetyamnesia/BrainBoostVR/blob/main/portfolio-project/Tutorial.png]

4. **End of Exercise**
   - Summary of performance (score, correct/incorrect actions)
   - Options: Restart, Main Menu, Exit
   [https://github.com/Sweetyamnesia/BrainBoostVR/blob/main/portfolio-project/Final_Screen.png]

## 3️⃣ API Considerations

- Firebase will be used to **store and retrieve user scores**.
- A custom API may be created to handle additional data storage or retrieval needs not covered by Firebase.
- API interactions:
  - Write scores after each exercise.
  - Read scores for progress display at the end of exercises.
  - Optional: store session metadata for analysis.
- Ensure secure authentication and minimal latency for a smooth VR experience.

# System Architecture

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

# Components, Classes & Database Design

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

## Sequences diagrams

1️⃣ Use Case 1 – User Authentication (Login)
@startuml
actor User
participant "Unity VR App" as Unity
participant "Firebase Auth" as Firebase

User -> Unity: Launches VR app
Unity -> Firebase: Request authentication (email/password)
Firebase -> Firebase: Validate credentials
Firebase --> Unity: Return auth token (JWT)
Unity -> User: Displays "Authentication successful"
@enduml

2️⃣ Use Case 2 – Save User Score
@startuml
actor User
participant "Unity VR App" as Unity
participant "Custom API" as API
participant "SQL Database" as DB
participant "Firebase Firestore" as Firestore

User -> Unity: Completes an exercise
Unity -> API: POST /scores { userID, score }
API -> DB: INSERT score into database
DB --> API: Score saved confirmation
API -> Firestore: Sync score for backup
Firestore --> API: Sync confirmation
API --> Unity: Response 200 OK (Score saved)
Unity -> User: Displays feedback and score saved message
@enduml

3️⃣ Use Case 3 – View Performance History
@startuml
actor User
participant "Unity VR App" as Unity
participant "Custom API" as API
participant "SQL Database" as DB

User -> Unity: Opens "Performance" menu
Unity -> API: GET /scores?userID=123
API -> DB: SELECT scores for userID=123
DB --> API: Returns list of scores
API --> Unity: Sends scores data
Unity -> User: Displays performance stats and history
@enduml

# 📑 APIs – BrainBoostVR

## 1️⃣ External APIs

| API | Purpose | Notes |
|-----|---------|-------|
| **Firebase Authentication** | Handle user accounts and login/logout securely. | Chosen for easy integration with Unity and secure token-based authentication. |
| **Firebase Firestore** | Store and retrieve user scores and session data. | Real-time database, scalable, works well with VR app. |

> **Note:** For the MVP, no additional external APIs are required. Future extensions (analytics dashboards, notifications, etc.) may involve third-party APIs.

---

## 2️⃣ Internal (Custom) API

The **Custom API** is mandatory to handle communication between Unity and the SQL database, while also optionally syncing with Firebase.  

### 2.1 API Endpoints Overview

| Endpoint | Method | Input | Output | Description |
|----------|--------|-------|--------|-------------|
| `/login` | POST | `{ "email": "...", "password": "..." }` | `{ "authToken": "...", "userID": "..." }` | Authenticate user and start session. |
| `/scores` | POST | `{ "userID": "...", "exerciseID": "...", "score": 85 }` | `{ "status": "success" }` | Save exercise score to SQL database. Optionally sync with Firebase. |
| `/scores` | GET | `{ "userID": "..." }` | `{ "scores": [ { "exerciseID": "...", "score": 85, "timestamp": "..." }, ... ] }` | Retrieve all scores for a specific user. |
| `/sessions` | POST | `{ "userID": "...", "startTime": "...", "endTime": "..." }` | `{ "status": "success" }` | Log user session duration. |
| `/users` | GET | `{ "userID": "..." }` | `{ "userID": "...", "email": "...", "createdAt": "..." }` | Retrieve user information (profile). |

### 2.2 Notes

- All requests/responses use **JSON** format.  
- Authentication tokens are required for endpoints affecting user data (`/scores`, `/sessions`).  
- The API ensures **low-latency** communication for smooth VR experience.  
- Future endpoints may include analytics, exercise progress summaries, or advanced leaderboard queries.

+-------------------+
|   VR Application  |
|   (Unity / C#)    |
+-------------------+
          |
          v
+-------------------+
|   Custom API      |
|  (REST / C#)      |
+-------------------+
   |            |
   v            v
+--------+   +--------+
| SQL DB |   | Firebase|
| (Users,|   | (Auth &|
| Scores,|   | Scores)|
| Sessions)  +--------+
+--------+

# 5️⃣ Plan SCM and QA Strategies

## SCM (Source Code Management) Strategy

- **Version Control Tool**: Git (via GitHub or GitLab repository).  
- **Branching Strategy**:  
  - `main` branch: Always contains stable and tested code.  
  - `feature/*` branches: Created for individual tasks or experiments; merged into `main` after testing.  
- **Commit Practices**:  
  - Regular commits with clear messages describing changes.  
  - Follow conventional commit standards (e.g., `feat:`, `fix:`, `docs:`).  
- **Code Review**:  
  - Not applicable (solo project), but self-review commits before merging to `main`.

---

## QA (Quality Assurance) Strategy

- **Testing Strategy**:  
  - Unit tests for key C# classes (e.g., `ExerciseManager`, `ScoreManager`).  
  - Integration tests for API interactions between Unity and the Custom API / Firebase.  
  - Manual testing for VR user flows to validate interactions, tutorial experience, and scoring display.  
- **Testing Tools**:  
  - Unity Test Framework for automated unit tests.  
  - Postman for testing API endpoints.  
  - Manual test cases documented in a spreadsheet or Notion board.  
- **Deployment Pipeline**:  
  - Staging builds deployed to Oculus Quest 2 for internal QA.  
  - Production build for demo day or MVP release after QA approval.  
- **Continuous Feedback**:  
  - Log issues and bugs in GitHub Issues or a task management tool.  
  - Regular testing after new features are integrated to prevent regressions.
