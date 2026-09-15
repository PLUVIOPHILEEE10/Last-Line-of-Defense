Last Line of Defense（最后防线）

一款使用 Unity 6 和 C# 开发的单人第一人称生存射击游戏。玩家需要管理生命值与弹药，击败五波逐渐增强的敌人并完成最终防守。

A single-player first-person survival shooter developed with Unity 6 and C#. The player must manage health and ammunition while surviving five increasingly difficult enemy waves.

项目状态 / Project Status

已完成可玩原型，包含完整的开始、战斗、胜利、失败及重新开始流程。

Completed playable prototype with a full gameplay loop, including combat, victory, defeat, pause, and restart states.

核心功能 / Core Features
第一人称移动、鼠标视角、冲刺与跳跃
First-person movement, mouse look, sprinting, and jumping
基于射线检测的射击与伤害判定
Raycast-based shooting and damage detection
弹匣、储备弹药、自动射击及换弹系统
Magazine, reserve ammunition, automatic firing, and reloading systems
基于 NavMesh 的敌人寻路、玩家追踪与近距离攻击
NavMesh-based enemy pathfinding, player tracking, and close-range attacks
五波递增难度挑战，敌人数量从3名逐步增加至7名
Five progressively harder waves, with enemy counts increasing from three to seven
敌人的生命值、移动速度和攻击伤害随波次提升
Enemy health, movement speed, and attack damage scale with each wave
四个随机出生点及可靠的 NavMesh 出生位置检测
Four randomized spawn points with NavMesh position validation
生命值、弹药、准星、波次及胜负状态 UI
UI for health, ammunition, crosshair, wave progress, victory, and defeat
命中反馈、受击提示及程序化音效
Hit feedback, damage feedback, and procedurally generated sound effects
暂停、恢复、重新开始及 Windows 构建功能
Pause, resume, restart, and Windows build functionality
技术栈 / Technologies
Unity 6000.6.0f1
C#
Universal Render Pipeline（URP）
Unity Input System
AI Navigation / NavMeshAgent
TextMeshPro
Unity UI
Unity Test Framework
操作方式 / Controls
操作 / Action	按键 / Key
移动 / Move	W、A、S、D
冲刺 / Sprint	Left Shift
跳跃 / Jump	Space
控制视角 / Look	Mouse
射击 / Fire	Left Mouse Button
换弹 / Reload	R
暂停或继续 / Pause or Resume	Esc
胜利或失败后重开 / Restart after Game Over	Enter
游戏目标 / Objective

玩家需要在封闭式战斗场景中击败五波敌人。随着波次推进，敌人的数量、生命值、移动速度和攻击伤害会逐步增加。清除全部五波敌人即可获胜，生命值降至零则游戏失败。

The player must defeat five waves of enemies in an enclosed combat arena. Enemy count, health, movement speed, and attack damage increase as the game progresses. Clearing all five waves results in victory, while losing all health results in defeat.

项目结构 / Project Structure
Assets：游戏场景、脚本、UI、材质、预制体及 NavMesh 数据
Packages：Unity项目依赖
ProjectSettings：Unity项目设置
运行项目 / How to Run
使用 Unity 6000.6.0f1 或兼容的 Unity 6 版本打开项目。
Open the project with Unity 6000.6.0f1 or a compatible Unity 6 version.
打开 Assets/Scenes/MainLevel.unity。
Open Assets/Scenes/MainLevel.unity.
点击 Unity 顶部的播放按钮。
Press the Play button in the Unity Editor.
生成Windows版本 / Build for Windows

在Unity顶部菜单中选择：

Last Line of Defense → Build Windows Game

生成的游戏位于：

Builds/Windows/LastLineOfDefense.exe

In the Unity menu, select:

Last Line of Defense → Build Windows Game

The Windows build will be generated at:

Builds/Windows/LastLineOfDefense.exe

项目职责 / Responsibilities

完成玩法设计、场景搭建、功能实现、敌人AI、波次系统、UI配置、问题调试及完整游戏流程验证。

Responsible for gameplay design, level setup, gameplay implementation, enemy AI, wave progression, UI configuration, debugging, and end-to-end gameplay validation.
