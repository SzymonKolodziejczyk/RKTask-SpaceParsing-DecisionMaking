# RKTask-SpaceParsing&DecisionMaking
 Or, as I would like to call it:
# 🤖 Dynamic NPC Pathfinding System

Welcome to the **Dynamic NPC Pathfinding System**! This project showcases an NPC pathfinding system with customizable behaviors, allowing for smooth and realistic movement along a sequence of nodes. The system is designed to be highly flexible and configurable via **ScriptableObjects** in Unity.

## 🗂️ Table of Contents

- [About the Project](#about-the-project)
- [Features](#features)
- [Customization Options](#customization-options)
- [Screenshots](#screenshots)
- [Contact](#contact)

![ProjectView](https://github.com/user-attachments/assets/0a84936a-9afd-4f64-ba7d-cceff26adf4f)

## 📖 About the Project

This system simulates dynamic NPC movement through a series of nodes, providing realistic behaviors such as looking around, pausing for thoughts, and smoothly rotating toward new destinations. The NPC's behaviors are controlled through **ScriptableObjects**, making it easy to configure and extend the system within the Unity Inspector.

## 🛠️ Features

### 1. 🛤️ **Dynamic Pathfinding**
- NPC follows a looping path through a sequence of nodes.
- Continuously moves along the path and interacts with waypoints for specific actions. However, there is a way to make it limited to only go for all the nodes without looping, which is left in the NPCController script.

### 2. 🎭 **Node-Based Actions**
- **Thinking:** The NPC pauses at nodes to display random thoughts.
- **Rotating:** The NPC looks around by rotating at a node.
- Both actions are fully customizable via **ScriptableObjects**, including the probability of each action occurring.
- Additionally, if the chance of the above actions is less than 100%, then the remaining percent goes for the action of doing nothing. A simple and neat way to prevent errors!

### 3. 🎲 **Random Actions While Moving**
- NPC performs subtle "look-around" movements while moving between nodes.
- The random chance for these actions can be configured independently of the node-based behaviors.

### 4. 👁️ **Vision Cone**
- A visual representation of the NPC's field of view.
- The vision cone dynamically rotates with the NPC, giving real-time feedback on its awareness.

### 5. 🎯 **Smooth Movement & Rotation**
- The NPC smoothly rotates towards its next destination while moving along the path.
- This creates natural and realistic NPC behavior as it navigates the environment.

### 6. 🛠️ **ScriptableObject Customization**
- Configure the NPC's behavior easily in the Unity Inspector:
  - Set node positions, movement speed, and the probability of random actions.
  - Adjust the chance for each node-based action (thinking, rotating).
  - Modify NPC pathfinding parameters and dynamic behaviors.

### 7. 🚫 **Non-Stacking Actions**
- Behaviors are managed to prevent overlapping actions.
- Smoothly transitions between actions without conflicting animations or delays.

## ⚙️ Customization Options

Using **ScriptableObjects**, you can tweak various NPC behaviors directly in the Unity Inspector:
- **Node Positions:** Define custom paths for the NPC.
- **Movement Speed:** Control how fast the NPC moves between nodes.
- **Action Probabilities:** Configure the likelihood of "thinking" or "rotating" actions at nodes.
- **Look-Around Actions:** Set the probability for random look-around behaviors while moving.
- **Vision Cone Parameters:** Adjust the NPC's field of view for different scenarios.

## 📸 Screenshots

![4](https://github.com/user-attachments/assets/299c1108-1b11-48f7-863e-7af00166272d)
How it looks before it's populated

![1](https://github.com/user-attachments/assets/3a003405-6244-4db3-9cab-5ea462d056f1)
Example of how it works in game #1

![2](https://github.com/user-attachments/assets/2ad9e689-8571-40b0-9945-25b5f03a9f92)
Example of how it works in game #2

## 📫 Contact

I'm excited to share this prototype and would love to hear your thoughts or suggestions. Feel free to reach out:

- **Email:** `szymonnkolodziejczyk@gmail.com`
- **[Connect on LinkedIn](https://www.linkedin.com/in/szymon-ko%C5%82odziejczyk-89bb95190/)** - Visit my LinkedIn profile.

Thank you for checking out the **Dynamic NPC Pathfinding System**!
