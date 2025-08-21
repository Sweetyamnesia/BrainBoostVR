# Project Charter – BrainBoostVR

## 📑 Table of Contents
- [0️⃣ Define Project Objectives](#0️⃣-define-project-objectives)
- [1️⃣ Stakeholders and Team Roles](#1️⃣-stakeholders-and-team-roles)
- [2️⃣ Project Scope](#2️⃣-project-scope)
- [3️⃣ Project Risks](#3️⃣-project-risks)
- [4️⃣ Develop a High-Level Plan](#4️⃣-develop-a-high-level-plan)

---

## 0️⃣ Define Project Objectives

The purpose of **BrainBoostVR** is to create an immersive VR application that helps users train and improve their cognitive skills through interactive exercises. 

The MVP will enable users to complete exercises that provide immediate feedback, track their performance with a scoring system using **Firebase**, and navigate the VR environment easily through a simple tutorial, ensuring an engaging and effective cognitive training experience.

**SMART Objectives**
- **Specific**: Build a VR application focused on cognitive exercises.
- **Measurable**: Deliver a functional MVP with at least two working exercises and Firebase integration.
- **Achievable**: Use Unity, C#, and Firebase to create a realistic, feasible project.
- **Relevant**: Address the growing need for engaging cognitive training tools.
- **Time-bound**: Complete MVP and presentation by **November 14, 2025**.

---

## 1️⃣ Stakeholders and Team Roles

### **Stakeholders**

- **Internal**:  
  - **Myself (VR Developer)**: Responsible for development, scripting, integration and testing. 
  - **Tutors / Supervisors**: Provide guidance, review deliverables, and give feedback.

- **External**:  
  - **End-users (students, trainees)**: Use the VR application, test exercises, and provides usability feedback.

### **Team Roles**

- **Project Lead / Developer** *(Myself)* -> Plan tasks, build the VR environment, implement exercises, integrate Firebase, and prepare the tutorial. 
- **Tester / QA** *(Myself + Tutors)* -> Conduct testing to ensure exercises and scoring systems function correctly.

---

## 2️⃣ Project Scope

### **In-Scope**
- Development of **core interactive exercises** to train cognitive skills *(memory, attention, problem-solving)*.
- Integration of a **scoring system** using Firebase for tracking progress.
- Creation of a **tutorial** guiding users through VR navigation and exercises.
- Deployment of the MVP for **Oculus Quest 2** testing.

### **Out-of-Scope**
- Multiplayer features or social interactions.
- Mobile or web versions of the app.
- Advanced analytics beyond basic score tracking.
- Integration of third-party APIs unrelated to the core MVP.

---

## 3️⃣ Project Risks

| **Risk** | **Impact** | **Likelihood** | **Mitigation Strategy** |
|------|--------|------------|-------------------|
| **Learning C# deeply enough to write custom scripts** | High | High | Dedicate focused weekly time to learning C#. Start by analyzing built-in in Unity scripts, but implement **custom-solutions** for exercises, tutorials, and scoring. |
| **Unity–Firebase integration complexity** | High | Medium | Set up Firebase early with a small test project. Use official Firebase Unity documentation and tutorials. |
| **Custom API requirements** | Medium | Medium | Clarify API needs early. Implement API after mastering Firebase basics to avoid redundancy. |
| **Time constraints for MVP completion** | High | Medium | Focus on **core features first** (exercices, scoring, tutorial). Cut nonessential tasks if delays occur. |
| **VR hardware performance issues** | Medium | Low | Regularly test on Quest 2 during development. Optimize scripts and assets progressively. |

---

## 4️⃣ Develop a High-Level Plan

The project will follow a structured, week-by-week plan from now until the **Demo Day**. Each phase includes clear objectives, learning milestones, and deliverables to ensure consistent progress.

| **Week** | **Dates**          | **Phase**                 | **Objectives / Planned Activities** | **Expected Deliverables** |
|----------|--------------------|---------------------------|--------------------------------------|---------------------------|
| **Week 1** | **Aug 21 – Aug 31** | **Unity & VR Learning** | Complete the **Unity VR Pathway**, understand key features for VR environment creation and user interaction. | Completed VR Pathway + summary notes. |
| **Week 2** | **Sept 1 – Sept 7** | **Targeted C# Learning** | Learn essential C# concepts for Unity: variables, functions, events, and object manipulation in VR. | Basic custom C# scripts tested. |
| **Week 3** | **Sept 8 – Sept 14** | **Technical Design** | Define the app’s architecture, plan Firebase integration, and structure VR exercises. | Architecture diagram + start of technical documentation. |
| **Week 4** | **Sept 15 – Sept 21** | **MVP Development Start** | Set up the VR environment, import first assets, and prepare the base scene. | Initial VR environment prototype. |
| **Week 5** | **Sept 22 – Sept 28** | **Interactions Development** | Implement basic VR interactions: movement, selection, and object manipulation. | Testable version with core interactions. |
| **Week 6** | **Sept 29 – Oct 5** | **Firebase Integration** | Configure Firebase SDK in Unity, test data read/write with mock scripts. | Firebase fully integrated and functional. |
| **Week 7** | **Oct 6 – Oct 12** | **First VR Exercise** | Develop the first cognitive VR exercise and validate its functionality. | Fully working first exercise. |
| **Week 8** | **Oct 13 – Oct 19** | **Additional Exercises** | Add one or two more exercises and improve scene navigation between them. | Expanded MVP with multiple exercises. |
| **Week 9** | **Oct 20 – Oct 26** | **Scoring System** | Develop and connect the scoring system to Firebase to track progress. | Functional scoring + data persistence. |
| **Week 10** | **Oct 27 – Nov 2** | **Internal Testing & Optimization** | Test the MVP on Oculus Quest 2, fix bugs, and optimize performance. | Stable and functional MVP. |
| **Week 11** | **Nov 3 – Nov 9** | **Finalization of MVP** | Optimize VR performance, integrate the tutorial, and finalize exercises. | MVP ready for demo preparation. |
| **Week 12** | **Nov 10 – Nov 14** | **Landing Page & Demo Day Prep** | Create the landing page, prepare the final presentation, and rehearse the pitch. | Published landing page + completed pitch deck. |
