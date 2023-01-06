TABLE OF CONTENT
1. Creating Nodes script & Scriptable Object
2. Creating Tree Scriptable Object & relation with AIBrain.cs
3. Setting a Tree
4. Animator
5. Animation

============================================================

NODE TYPES

Abstract Type
Control
Execution

Specific Type
[Control]
Sequence
Fallback
Decorator
Parallel

[Execution]
Condition
Action

============================================================

1. Creating Nodes script & Scriptable Object
Note: Please search if desired behaviour already exists before creating a new script

1.1. Creation path
1.1.1. Create > Scriptable > Behaviour Tree > T

1.2. <execution> nodes (except decorator) 
1.2.1. Don't need further concretisation

1.3. <execution> and <decorator> nodes REQUIRE concretisation 
1.3.1. Concretisation's parent MUST be of desired specific type (e.g. Inverter: Decorator)
1.3.1.1. Parent.parent (Node.cs) is ScriptableObject // Thus no need think about it
1.3.2. Please follow Proposed [CreateAssetMenu] nomenclature
1.3.2.1. [CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node <abstract type>/<specific Type>/<name>", fileName = "<specific type>SO_<name>")]
1.3.2.1.1. Note: <filename> of <decorator> should follow one more step: "<specific type>SO_<decorator type>_<name>"
1.3.2.1.1.1. e.g. filename = "DecoratorSO_Inverter_Is target near"

____________________________________________________________


2. Creating Tree Scriptable Object & relation with AIBrain.cs

2.1. Creation Path
2.1.1. Create > Scriptable > Behaviour Tree > Tree
2.1.2. Please rename the generated scriptable object appropriately to avoid confusion as to which AI uses it

2.2. A single AIBrain.cs manages a single tree
2.2.1. AIBrain.cs calls the Tick() of its tree every Fixed Update


____________________________________________________________


3. Setting a Tree
Note: root node MUST always be of type <Control>
note: tree must be linked to its respective AIBrain monobehaviour

Two possibilities exist to set up a tree: 
(3.1.) Working a selection of scriptable objects and linking the root node to a tree's Scriptable Object
(3.2.) Working with the UI visualisation (TO BE IMPLEMENTED)

3.1. Working with a selection of scriptable objects
(TLDR: Create node + Link children to <control> node + Set root in tree)
Note: Establish the logical steps before-hand to simplify linking the nodes and avoid logic mistakes
3.1.1. Create the required <execution> scriptable object
3.1.1.1. Reminder: Some behaviour may already exist. Please avoid duplication.
3.1.2. Create the required <control> scriptable objects
3.1.2.1. Reminder: <decorator> nodes require concretisation
3.1.3. Place the <execution> scriptable objects inside of the "children" list of the appropriate <control> scriptable object
3.1.3.1. Reminder: Execution is in order
3.1.3.2. Reminder: A <control> node's children can be composed of other <control> nodes
3.1.4. Set root node reference into it's specific treeSO
3.1.5. Set the treeSO reference into it's specific AIBrain

3.2. Working with the UI visualisation !!!TO BE IMPLEMENTED!!!
3.2.1. !!!TO BE IMPLEMENTED!!!


____________________________________________________________


4. Animator

4.1. Animator MUST be copied from template folder
4.1.1. Assets > Animations > Enemy > BT_Enemy > _TEMPLATE_Animator_NAME (copy the directory)
4.1.1.1. Delete "_TEMPLATE_"
4.1.1.2. Set "NAME" to the enemy's name for EVERY asset in both the directory and in its sub-directory

4.2. Animator State
4.2.1. Name MUST follow TWO rules
4.2.1.1. Nomenclature: AnimatorStateName == animation.everything before the "_" (see 5.1.1.)
4.2.1.2. Weapon animation is played by getting the state which name corresponds to the "AnimatorStateName" part of the animation's name
4.2.2. Create transition link to "Iddle" state ONLY


____________________________________________________________


5. Animation
AnimatorStateName_EnemyName

5.1. Name MUST follow TWO rules
5.1.1. Nomenclature: AnimatorStateName_EnemyName
5.1.1.1. Weapon animation is played by getting the state which name corresponds to the "AnimatorStateName" part of the animation's name
5.1.2. Create transition link to "Iddle" state ONLY





