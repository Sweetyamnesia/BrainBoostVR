# Project Charter – BrainBoostVR

## 📑 Table of Contents
- [0️⃣ Define Project Objectives](#0️⃣-define-project-objectives)
- [1️⃣ Stakeholders and Team Roles](#1️⃣-stakeholders-and-team-roles)
- [2️⃣ Project Scope](#2️⃣-project-scope)
- [3️⃣ Project Risks](#3️⃣-project-risks)
- [4️⃣ Develop a High-Level Plan](#4️⃣-develop-a-high-level-plan)

---

## 0️⃣ Define Project Objectives

The purpose of BrainBoostVR is to create an immersive VR application that helps users train and improve their cognitive skills through interactive exercises. 

The MVP will enable users to complete exercises that provide immediate feedback, track their performance with a scoring system using Firebase, and navigate the VR environment easily through a simple tutorial, ensuring an engaging and effective cognitive training experience.

### SMART Objectives
- **Specific & Measurable**: Implement interactive exercises with feedback and scoring to track user performance.
- **Achievable**: Integrate Firebase for score tracking and basic data persistence to allow progress saving.
- **Relevant**: Exercises designed to train memory, attention, and problem-solving skills in an immersive VR environment.
- **Time-bound**: Complete MVP with exercises, scoring, and tutorial by Oct 29, 2025, for internal validation and final demo preparation.

---

## 1️⃣ Stakeholders and Team Roles

### Stakeholders

- **Internal**:  
  - You (VR Developer): Build VR exercises, implement C# scripts, integrate scoring system  
  - Tutors / Supervisors: Provide guidance, review progress, give feedback  

- **External**:  
  - End-users (students, trainees): Use the VR application, provide feedback on exercises and usability  

### Team Roles

- **Project Lead / Developer**: Oversees project progress, plans tasks, builds VR environment, implements exercises, scoring, and tutorial  
- **Tester / QA**: Tests functionality, ensures exercises work as intended, reports issues  

---

## 2️⃣ Project Scope

### In-Scope
- Development of core interactive exercises targeting cognitive skills (memory, attention, problem-solving)
- Integration of a scoring system using Firebase for progress tracking and basic data persistence
- Creation of a tutorial to guide users in navigating the VR environment and understanding exercise mechanics
- Deployment of the MVP for testing on Oculus Quest 2

### Out-of-Scope
- Advanced VR features such as multiplayer functionality or complex social interactions
- Web or mobile versions of the application
- Long-term user analytics beyond basic score tracking
- Integration of third-party APIs not directly required for core exercises

---

## 3️⃣ Project Risks

| Risk | Impact | Likelihood | Mitigation Strategy |
|------|--------|------------|-------------------|
| **Learning C# deeply enough to write custom scripts** | High | High | Dedicate focused time each week to practice C# within Unity. Analyze built-in scripts for structure but ensure all core features are implemented with **self-written** scripts. |
| **Unity–Firebase integration complexity** | High | Medium | Set up Firebase SDK for Unity early; start with small test scripts for read/write. Use official tutorials before integrating into the real project. |
| **Need to develop a custom API** | Medium | Medium | Clarify API requirements early (scores, logs, session data). Implement after mastering Firebase basics to avoid redundancy. |
| **Time constraints for MVP completion** | High | Medium | Prioritize custom VR exercises and scoring system first. Track progress weekly; cut secondary features if needed. |
| **VR hardware performance (Quest 2)** | Medium | Low | Frequently test builds on Quest 2 to detect performance issues early. Optimize assets and scripts iteratively. |

---

## 4️⃣ Develop a High-Level Plan

The project will follow a structured development process divided into five key stages, each with defined objectives, timelines, and deliverables. This ensures steady progress toward building the MVP and preparing for the final demo.

| **Stage**                                    | **Timeline**              | **Key Activities**                                                                                                                                    | **Deliverables**                |
| -------------------------------------------- | ------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------- |
| **Stage 1 – Idea Development**               | **Completed**             | Brainstorming, concept definition, feasibility check.                                                                                                 | Finalized project idea & vision |
| **Stage 2 – Project Charter**                | **Aug 20 – Aug 31, 2025** | Define objectives, stakeholders, scope, risks, and high-level plan.                                                                                   | Project Charter Document        |
| **Stage 3 – Technical Documentation**        | **Sep 1 – Sep 14, 2025**  | Define architecture, Firebase integration plan, API requirements, and interaction flow.                                                               | Technical Documentation         |
| **Stage 4 – MVP Development & Execution**    | **Sep 15 – Oct 29, 2025** | Build VR environment, implement exercises, develop custom C# scripts, integrate scoring with Firebase, create tutorial, and conduct internal testing. | MVP ready for validation        |
| **Stage 5 – Project Closure & Landing Page** | **Oct 30 – Nov 14, 2025** | Finalize MVP, deploy landing page, prepare and rehearse final pitch & demo.                                                                           | Final Demo & Landing Page       |
