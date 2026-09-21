# 🧠 BrainBoostVR

Immersive VR application for cognitive training through interactive exercises and real-time feedback.

---

## 💡 Overview

BrainBoostVR is a virtual reality application designed to explore how immersive environments can support cognitive and motor training.

It combines **XR interaction design, cognitive exercises, real-time feedback, user authentication, and backend data persistence**.

The project was developed as a **solo portfolio project**, with a focus on **XR interaction design, user experience, and system architecture**.

---

## ✨ Key Features

* Immersive VR environment for cognitive exercises
* Interactive object-based exercises
* Real-time visual and audio feedback
* Score and session tracking
* Tutorial system for first-time users
* Anonymous user authentication
* Backend API for data persistence
* Oculus Quest 2 support

---

## 🛠 Tech Stack

* **Unity 6** + XR Interaction Toolkit
* **C#**
* **ASP.NET Core / .NET**
* **Firebase Authentication**
* **MySQL**
* **Oculus Quest 2**
* **Git / GitHub**
* **Postman**

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

Unity handles the **VR experience, interactions, exercises, scoring, and user interface**.

The backend API manages **data persistence and communication with the MySQL database**, while Firebase provides **anonymous authentication**.

---

## 🎥 Demonstration

A visual presentation of BrainBoostVR is available through the project landing page.

👉 **[View the BrainBoostVR Landing Page](https://sweetyamnesia.github.io/brainboostvr-landing/)**

---

## 📚 Documentation

The repository contains three complementary documents covering the project from different perspectives.

### 📋 Project Overview & Research

**[Stage 1 Report](./portfolio-project/Stage_1_Report.md)**

Covers the project's research, ideation, MVP definition, rejected ideas, risks, and core features.

### 📐 Project Planning

**[Project Charter](./portfolio-project/The_Project_Charter.md)**

Covers the project objectives, scope, stakeholders, risks, and high-level planning.

### ⚙️ Technical Documentation

**[Technical Documentation](./portfolio-project/Technical_documentation.md)**

Covers the system architecture, user stories, Unity and API classes, database design, API specifications, sequence diagrams, SCM, QA, and technical decisions.

For installation, configuration, API usage, and development information, see the **[Unity project README](./BrainBoostVR_Unity/BrainBoostVR/README.md)**.

---

## 🚀 Status

**MVP completed** – ongoing improvements in XR interaction, UX, performance, and overall project polish.
