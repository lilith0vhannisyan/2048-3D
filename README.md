# 2048 3D - Unity Puzzle Game

A 3D recreation of the classic 2048 puzzle logic, featuring smooth animations and a persistent scene management system.

## 🚀 Key Features
**Custom Grid Logic:** Implemented a 4x4 grid system using 2D arrays and directional movement algorithms.
* **Tweened Animations:** Used **DOTween** for smooth tile sliding and scaling effects.
* **Persistent Systems:** Developed a `MenuManager` using the **Singleton Pattern** and `DontDestroyOnLoad` to handle transitions between Menu, Game, and Win/Loss states.
* **Save System:** (Optional: add this if you use PlayerPrefs) High-score persistence using `PlayerPrefs`.

## 🛠️ Technical Implementation
* **Language:** C#
* **Engine:** Unity 2022.x / 2023.x
* **Architecture:** Decoupled UI logic from gameplay using UnityEvents.

## 🎮 How to Play
1. Use **Arrow Keys** to slide the tiles.
2. Matching tiles merge into the next level.
3. Reach the goal tile to win!
