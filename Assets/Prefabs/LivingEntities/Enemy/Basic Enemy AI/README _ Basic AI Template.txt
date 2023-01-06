HOW TO USE

Step 01 : CREATE PREFAB VARIANT ========================
+ [MB2 on prefab > Create > Prefab Variant]
+ Change Name
+ Create a new [FloatVariableSO] for maxHP & link to variant


Step 02: ANIMATOR ===================================== (MAY NEED TO IMPLEMENT [Animation Override Controller] INSTEAD)
+ [Animation > Enemy > Basic Enemy AI]
+ CTRL + D ... [_TEMPLATE _ AOC & ANIMATIONS]
+ Rename Folder
+ Rename Animator Override Controller (suggested)
+ Rename Animations (suggested)
+ On newly created prefab variant
	++ [_AOC _ MyAiName] > Change all animations for current folder's animations
	++ Sprites of animations MUST be set manually


Step 03: SETTING UP WEAPON MANAGERS ===================
+ A maximum of one weapon manager can be placed on each state
	++ The same weapon holder can be used for both state
	++ Only the main weapon of each weapon manager is currently used by the FSM
	++ [TheBaseEnemyContext.cs] will clone the main weapon's scriptable object OnStart


Step 04: SETTING UP BEHAVIOURS ========================
+ Plug N' Play system

	++ [Basic Enemy Context.cs] - Defining terms
		+++ My Starting State : Which state enemy starts in

		+++ Start at max speed : For, example, ambush type ai (i.e. mimic)

		+++  Weapon Holder for each state
			++++ For Range
			++++ For attack cooldown

		+++ R/A_On Attack Exit boolean
			++++ Shorthand for AI who need to switch state after an attack

		+++ R/A_Anim Execute Attack boolean
			++++ Shorthand for state who uses animation to dictate behaviour execution


	++ Children of main object
		+++ Behaviours _ State _ 1
			++++ Behaviour slots for WITH and WITHOUT token

		+++ Behaviours _ State _ 2	
			++++ Behaviour slots for WITH and WITHOUT token

		+++ Behaviours _ Death / On Hit
			++++ Storage unit for [LivingEntity.cs]'s Events on death & on hit			

		+++ Behaviours _ Passive
			++++ Passive behaviours which are not dictated by any state management
			
	
Step 05: Enjoy! =======================================