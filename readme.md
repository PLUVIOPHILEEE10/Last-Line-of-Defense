
# Last Line of Defense（最后防线）

一款使用 Unity 6 和 C# 开发的单人第一人称生存射击游戏。玩家需要管理生命值与弹药，击败五波逐渐增强的敌人并完成最终防守。

A single-player first-person survival shooter developed with Unity 6 and C#. The player must manage health and ammunition while surviving five increasingly difficult enemy waves.

## 项目状态 / Project Status

已完成可玩原型，包含完整的战斗、胜利、失败、暂停及重新开始流程。

Completed playable prototype with combat, victory, defeat, pause, and restart systems.

## 核心功能 / Core Features

- 第一人称移动、鼠标视角、冲刺与跳跃
- 基于射线检测的射击和伤害判定
- 弹匣、储备弹药、自动射击及换弹系统
- 基于 NavMesh 的敌人寻路、追踪及近距离攻击
- 五波递增难度挑战，敌人数量从3名增加至7名
- 敌人的生命值、速度和伤害随波次提升
- 四个随机出生点及 NavMesh 位置检测
- 生命值、弹药、准星、波次和胜负状态 UI
- 命中反馈、程序化音效、暂停及重新开始功能

---

- First-person movement, mouse look, sprinting, and jumping
- Raycast-based shooting and damage detection
- Ammunition, automatic firing, and reloading systems
- NavMesh-based enemy pathfinding, tracking, and attacks
- Five increasingly difficult enemy waves
- Progressive enemy health, speed, and damage scaling
- Randomized enemy spawning with NavMesh validation
- Health, ammunition, crosshair, wave, victory, and defeat UI
- Hit feedback, procedural audio, pause, and restart systems

## 技术栈 / Technologies

- Unity 6000.6.0f1
- C#
- Universal Render Pipeline（URP）
- Unity Input System
- AI Navigation / NavMeshAgent
- TextMeshPro
- Unity UI

## 操作方式 / Controls

| 操作 / Action | 按键 / Key |
|---|---|
| 移动 / Move | W, A, S, D |
| 冲刺 / Sprint | Left Shift |
| 跳跃 / Jump | Space |
| 控制视角 / Look | Mouse |
| 射击 / Fire | Left Mouse Button |
| 换弹 / Reload | R |
| 暂停或继续 / Pause or Resume | Esc |
| 游戏结束后重开 / Restart | Enter |

## 游戏目标 / Objective

玩家需要击败五波逐渐增强的敌人。清除全部敌人即可获胜，生命值降至零则游戏失败。

Defeat five increasingly difficult enemy waves. Clear every wave to win; losing all health results in defeat.

## 如何运行 / How to Run

1. 使用 Unity 6000.6.0f1 或兼容的 Unity 6 版本打开项目。
2. 打开 `Assets/Scenes/MainLevel.unity`。
3. 点击 Unity 顶部的播放按钮。

---

1. Open the project with Unity 6000.6.0f1 or a compatible Unity 6 version.
2. Open `Assets/Scenes/MainLevel.unity`.
3. Press the Play button in the Unity Editor.

## Windows构建 / Windows Build

在Unity顶部菜单中选择：

`Last Line of Defense → Build Windows Game`

生成位置：

`Builds/Windows/LastLineOfDefense.exe`

## 项目职责 / Responsibilities

负责玩法设计、场景搭建、功能实现、敌人AI、波次系统、UI配置、问题调试及完整流程验证。

Responsible for gameplay design, level setup, gameplay implementation, enemy AI, wave progression, UI configuration, debugging, and end-to-end validation.
