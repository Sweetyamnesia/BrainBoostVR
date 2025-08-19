# 🧠 BrainBoostVR – Project Documentation

## 📑 Table of Contents
- [0️⃣ Team & Personal Background](#0️⃣-team--personal-background)
- [1️⃣ Research & Ideation](#1️⃣-research--ideation)
- [2️⃣ Idea Evaluation](#2️⃣-idea-evaluation)
- [3️⃣ Decision & MVP Definition](#3️⃣-decision--mvp-definition)
- [4️⃣ Documentation & Risks](#4️⃣-documentation--risks)
  - [❌ Rejected Ideas](#-rejected-ideas)
  - [⚠️ Risks and Mitigation](#-risks-and-mitigation)
  - [📌 Core Features (MVP)](#-core-features-mvp)
  - [⚡ MVP Scope Justification](#-mvp-scope-justification)

## 0️⃣ Team & Personal Background

This section presents the personal and professional background behind the project, highlighting how previous experiences and skills are leveraged in the development of BrainBoostVR.  

**Team:** Solo developer  
**Roles:** Temporary Project Manager, Researcher, Designer, Developer, Tester  

**🎓 Academic Background:**  
- Bachelor’s degree in Psychology  
- Master’s in Educational Digital Devices  
- Current training in Computer Science fundamentals  

**💼 Professional Experience:**  
- Project assistant, multimedia instructional designer, digital training project manager, digital mediator, digital advisor in public services  

**🛠 Skills:** Pedagogy, UX/UI design, Unity, C# (in progress), web development, AI & NoCode, project management, video editing  

**🌟 Interests & Motivation:**  
Virtual Reality, serious games, cognitive training, intergenerational learning, improving elderly health, making technology more accessible  

---

## 1️⃣ Research & Ideation

This part describes the preparatory work and inspiration that led to the creation of BrainBoostVR. It includes academic research, personal initiatives, and the identification of the problem the project aims to solve.  

**💡 Sources of Inspiration:**  
- Preparatory research on VR for elderly patients  
- Master’s thesis on VR for learning safety  

**📚 Context & Preparatory Work:**  
- **Exploratory Research:** Initial idea was to interview health professionals and video game designers to explore VR for seniors, aiming to promote intergenerational relationships and playful learning. Due to the scope of the master’s program, this step was not carried out.  
- **Master’s Thesis:** Investigated transfer of learning between experts and non-experts in laser safety, comparing immersive VR experiences with practical laboratory work to assess effectiveness of skill transfer and safe practices.  
- **Project Awareness & Inspiration:** Created a dedicated Instagram (for observation and idea tracking) page and followed VR companies to gather ideas, observe trends, and map potential users. A mindmap was also made to identify all target groups, which helped narrow the scope for the MVP.  

**🔍 Identified Problem:**  
Patients in cognitive or motor rehabilitation often lack motivating and engaging solutions. Traditional methods are monotonous, reducing adherence and effectiveness.  

**📝 Techniques Used:**  
- Mindmapping of target users, stakeholders, and potential features  
- Market research and social media observation  
- Persona creation to understand needs and pain points  
- Focused scope: restricted initial MVP to core exercises for healthcare professionals  

**💭 Initial Idea:**  
VR application to support professionals and caregivers in rehabilitation, with immersive exercises and progress tracking.  

---

## 2️⃣ Idea Evaluation

Once the initial idea was clarified, the next step was to evaluate its relevance and feasibility. This section presents the evaluation criteria, anticipated challenges, and decisions regarding the target audience for the MVP.  

**📊 Evaluation Criteria:**  
Feasibility, potential impact, alignment with skills, user relevance  

**⚠️ Challenges & Risks:**  
Learning Unity and C#, integrating Firebase and Cloudinary, optimizing for standalone headsets, VR ergonomics, testing with users  

**🎯 Target Prioritization:**  
Healthcare facilities and professionals for MVP; restricted scope to core functionalities to ensure feasibility  

---

## 3️⃣ Decision & MVP Definition

After evaluating the different options, the decision was made to focus on a minimal viable product (MVP) that could showcase the potential of VR in rehabilitation while remaining technically achievable as a solo developer.  

**🏷 Project Name:**  
BrainBoostVR  

**📣 Pitch:**  
A VR application for cognitive and motor rehabilitation, providing immersive, interactive, and motivating exercises in a safe environment.  

**🛠 MVP Features:**  
- Basic VR navigation and interaction  
- Interactive objects with real-time visual and audio feedback  
- Simple progression in difficulty (easy → medium → hard)  
- Basic tracking of scores or success indicators  

**💻 Technical Stack:**  
Unity XR Interaction Toolkit, C#, Firebase (Authentication, Firestore), Cloudinary for media  

**🎯 Professional Benefits:**  
- Enhances patient engagement and motivation  
- Provides professionals with progress tracking  
- Hands-on learning in Unity, VR development, and C#  

---

## 4️⃣ Documentation & Risks

This section documents the rejected ideas, identified risks, and core features of the MVP. It also provides justifications for scope limitation to ensure the feasibility of the project.  

### ❌ Rejected Ideas  
Some initial ideas were discarded because they were too ambitious for an MVP. For instance, advanced multi-user features and overly complex VR interactions were set aside to maintain focus on a minimal but functional prototype.  

---

### ⚠️ Risks and Mitigation  
The development of a VR rehabilitation application raises both **technical risks** (performance, backend integration, bugs) and **human-centered risks** (usability, accessibility, VR comfort).  
The following table summarizes these risks and the planned solutions to mitigate them:  

| Risk | Mitigation / Solution |
|------|--------------------|
| The user does not understand how to interact with the objects | Add visual and voice instructions at the start of the exercise, and possibly a short tutorial or a pre-exercise tutorial window showing controls and interactions. |
| Difficulty too high or too low for the user | Provide multiple difficulty levels and adjust the number of objects and allotted time. |
| Bugs in scripts (validation, timer, score) | Test each feature separately before integrating; implement error logging in Unity. |
| VR comfort issues (nausea, fatigue) | Limit abrupt movements, adjust exercise duration, and add breaks if necessary. |
| Difficulty remembering the sequence (visual and audio exercise) | Provide hints or repetitions; optionally allow slowing down the sequence. |
| Score tracking problems (Firebase / Cloudinary issues) | Test integration early; implement temporary local saves before sending data. |
| VR performance issues / lag on Quest 2 | Profile the scene, optimize assets, limit the number of objects displayed simultaneously. |
| Accessibility / comfort for elderly users | Use high contrast, clear texts, intuitive controls, and test with users if possible. |

---

### 📌 Core Features (MVP)  
The MVP is centered around **essential exercises** that demonstrate the potential of VR for cognitive training while remaining feasible for a solo project.  
Each feature has been evaluated in terms of feasibility, impact, alignment with the technical stack, and scalability:  

| Feature / Exercise | Feasibility | Potential Impact | Technical Alignment | Scalability | Justification / Notes |
|-------------------|------------|----------------|------------------|------------|--------------------|
| Object placement memory exercise | High | High | Medium | Medium | Core exercise for cognitive training; feasible with basic Unity interactions and scoring system. |
| Visual & audio sequence reproduction exercise | Medium | High | Medium | Medium | Encourages multi-sensory learning; requires additional scripts for sequence tracking and scoring. |
| Multiple difficulty levels | High | Medium | High | High | Allows personalized progression and better adherence; feasible to implement with adjustable timers and object counts. |
| Real-time feedback (score & voice guidance) | Medium | High | Medium | Medium | Improves engagement and motivation; requires integration of audio and scoring scripts. |
| Tutorial / pre-exercise instruction | High | High | High | High | Ensures user understands VR interactions (controllers, teleportation, object manipulation); can reduce errors and frustration.|

---

### ⚡ MVP Scope Justification  
Originally, the project aimed to include a full-fledged platform for both professionals and patients, with dashboards, session history, notifications, and analytics.  
However, given the constraints, the MVP focuses on **core VR exercises and minimal backend features**.  
The table below shows which ideas were reduced or postponed, and what alternatives were considered:  

| Idea / Feature | Reason for Limited Implementation | Notes / Alternative |
|----------------|---------------------------------|------------------|
| Full web platform for users and professionals (dashboard, session history, tracking progress) | Scope too large for MVP | Implement only a minimal backend API to save and retrieve scores from VR exercises |
| Notifications & offline mode | Adds complexity outside core VR exercises | Postpone to future versions |
| Detailed analytics and visual reports | Requires advanced backend and UI | Keep basic API endpoints to allow score retrieval; detailed visualization can come later |
| Terminal C prototype | Not directly relevant for VR-focused MVP | Kept as reference; demonstrates concept of user/session management |
