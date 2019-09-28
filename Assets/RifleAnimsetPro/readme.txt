Rifle Animset Pro v.1.15
--------------------


This is a complete set of motion capture animations, to build a seamless, third person perspective character with rifle movement for your game.

It comes with example Mecanim Controller graph. It also includes an example controller, made with Playmaker (Playmaker is needed to use it; If you do not have Playmaker, you will have to script your own controller for included Mecanim graph, or just use the animations however you like).

It consists of over 80 animations from which you can make game mechanics for:

- Walking
- Running
- Shooting
- Getting hit and dying
- Grenade throwing
- Aiming with Additive animations
- Melee attacks
- and more!

It also contains:
- Skinned model of a modern day soldier (26,536 tris, 13,813 verts)
- Skinned model of a dummy, to preview animations
- Simple mesh of a rifle, a placeholder

--------------------

Animations are cut to ready-to-use clips. They have an animated Root bone, so you can use Root Motion and move the character around just with animations. Animations use Unity's Humanoid rig (they can be played on any humanoid character), but they can be easly converted to Generic or Legacy rig. If you wish to have all those animations in place (with no Root Motion), just uncheck "Enable Root Motion" in your character prefab in the scene, or delete the animation from Root bone (Legacy).

The skeleton that animations are baked on to is a standard, Motionbuilder compatible skeleton. It has Motionbuilder hierarchy and naming convention. Hips, Neck and Head have Z axes pointing forward, so you can use Look At constraints etc.

--------------------

Few tips:
- All animations can be retargeted to any Humanoid
- XBox 360 controller is supported, if you add 2 additional Axes in the Input panel: RightStickHorizontal (mapped to 4th axis (Joysticks)) and RightStickVertical (mapped to 5th axis (Joysticks)). Check InputForX360GamePad.jpg to see how it should look
- Not all animations are used by the controller - build your own or expand this one! You are the game developer :)


--------------------
List of animations
--------------------

Rifle_Idle

Rifle_TurnR_90
Rifle_TurnL_90
Rifle_TurnR_180
Rifle_TurnL_180
Rifle_TurnR_90Loop
Rifle_TurnL_90Loop

Rifle_WalkFwdStart
Rifle_WalkFwdLoop
Rifle_WalkFwdStop_LU
Rifle_WalkFwdStop_RU

Rifle_WalkBwdStart
Rifle_WalkBwdLoop
Rifle_WalkBwdStop_RU
Rifle_WalkBwdStop_LU

Rifle_StrafeRightStart
Rifle_StrafeRightLoop
Rifle_StrafeRightStop_LU
Rifle_StrafeRightStop_RU

Rifle_StrafeLeftStart
Rifle_StrafeLeftLoop
Rifle_StrafeLeftStop_RU
Rifle_StrafeLeftStop_LU

Rifle_StrafeLeft45Loop
Rifle_StrafeRight135Loop

Rifle_WalkFwdStart90_L
Rifle_WalkFwdStart90_R
Rifle_WalkFwdStart180_R
Rifle_WalkFwdStart180_L

Rifle_ShootOnce
Rifle_ShootGrenade
Rifle_ShootBurst
Rifle_ShootBurstLong
Rifle_ShootLoop_Additive

Rifle_Reload_2

Rifle_Hit_L_1
Rifle_Hit_L_2
Rifle_Hit_R_2
Rifle_Hit_C_1

Rifle_Death_L
Rifle_Death_R

Rifle_Melee_Hard
Rifle_Melee_Kick

Rifle_Grenade_Throw_Single
Rifle_Grenade_Throw_Start
Rifle_Grenade_Throw_Loop
Rifle_Grenade_Throw_Cancel
Rifle_Grenade_Throw_Far
Rifle_Grenade_Throw_Close

Rifle_Look_CC_Additive
Rifle_Look_45L_Additive
Rifle_Look_45R_Additive
Rifle_Look_45U_Additive
Rifle_Look_45D_Additive
Rifle_Look_45LU_Additive
Rifle_Look_45RU_Additive
Rifle_Look_45LD_Additive
Rifle_Look_45RD_Additive
Rifle_Look_90L_Additive
Rifle_Look_90R_Additive
Rifle_Look_90U_Additive
Rifle_Look_90D_Additive

--------------------
Update 1.1
--------------------
Rifle_FallingLoop

Rifle_Idle2Crouch
Rifle_CrouchLoop
Rifle_Crouch2Idle

Rifle_Idle_GunDown
Rifle_Idle_GunDown2Idle

Rifle_Climb_1m
Rifle_Vault_1m

Rifle_OpenDoor

Rifle_Death_3

Rifle_RunFwdLoop
Rifle_RunBwdLoop
Rifle_StrafeRunRightLoop
Rifle_StrafeRunLeftLoop
Rifle_StrafeRun45LeftLoop
Rifle_StrafeRun135LeftLoop

--------------------


See AnimationDescriptions.pdf to get full description of each animation.




--------------------
Created by Kubold
kuboldgames@gmail.com
http://www.kubold.com
https://www.facebook.com/kuboldgames