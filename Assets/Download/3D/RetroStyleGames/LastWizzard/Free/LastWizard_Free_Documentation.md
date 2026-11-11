# Last Wizard Free — Unity Documentation

## Overview

A stylized fantasy interior kit with the Knight, four animations, modular architecture, furniture, treasure props and animated fire. The Demo presents a 10 x 10 m room and a 3.4 x 15 m corridor, both 10 m high, with the artist's chest/chalice, table/candle, banners, windows and lighting. The door is closed in the supplied arrangement and blocks passage. Its leaf is a separate transform; buyers can rotate it when building their own scene.

## Render Pipeline Compatibility

Built-in (BRP), Universal (URP) and High Definition (HDRP). Unity 6000.0.77f1 and Shader Graph / URP / HDRP 17.0.4. Shared RSG materials support all three pipelines; scenes contain their own native pipeline components. Each pipeline has a Demo and an Overview scene. Open the pair matching your project's installed pipeline.

## Technical Details

146 package files excluding metadata/folders; 21 FBX files (17 geometry, four animation-only); 24 FBX-imported mesh objects, plus three native product Mesh assets (the two UV wall variants and candle flame). 19 prefabs; 21 product materials plus five Overview materials; 57 product textures plus one Overview label atlas; six scenes; one Animator Controller; three Shader Graphs; no C# or assembly-definition files. Product FBX geometry totals 40,406 vertices / 44,316 triangles. Including the three native product Mesh assets: 40,448 vertices / 44,352 triangles. Overview presentation meshes add 108 vertices / 70 triangles (labels 72 / 36; curved backdrop 36 / 34), excluding repeated floor modules and built-in display primitives. LOD0 only.

## Model

Counts below are measured from Unity's imported meshes, including each submesh, and do not multiply repeated scene instances.

| Model / mesh | Vertices | Triangles | Submesh triangles |
|---|---:|---:|---|
| SK_Knight / SM_Knight | 7,284 | 9,134 | 990, 8144 |
| SK_Knight / SM_Knight_Weapon | 1,165 | 1,708 | 1708 |
| SM_Ceiling / SM_Ceiling | 105 | 72 | 72 |
| SM_Door / SM_Door | 3,790 | 3,792 | 3792 |
| SM_Door / SM_DoorHandleBack | 297 | 512 | 512 |
| SM_Door / SM_DoorFrame | 4,518 | 3,864 | 3864 |
| SM_Door / SM_DoorHandleFront | 297 | 512 | 512 |
| SM_Fireplace / SM_Fireplace | 1,910 | 1,952 | 1952 |
| SM_Floor / SM_Floor | 4 | 2 | 2 |
| SM_Wall_Arch / SM_Wall_Arch_Castle | 16 | 14 | 14 |
| SM_Wall_Bricks / SM_Wall | 4 | 2 | 2 |
| SM_Wall_Bricks_HalfUV_Left_Free / SM_Wall_Bricks_HalfUV_Left_Free | 4 | 2 | 2 |
| SM_Wall_Bricks_HalfUV_Right_Free / SM_Wall_Bricks_HalfUV_Right_Free | 4 | 2 | 2 |
| SM_Wall_WoodenPlank / SM_Wooden_Plank | 60 | 60 | 60 |
| SM_Window / SM_Wall_Window | 1,222 | 1,162 | 1162 |
| SM_Banner / SM_Banner | 758 | 808 | 808 |
| SM_Loot_Candlestick / SM_Loot_Candlestick | 4,076 | 4,260 | 4260 |
| SM_Container_Reliquary / SM_ContainerReliquaryKeyHole | 169 | 142 | 142 |
| SM_Container_Reliquary / SM_ContainerReliquaryCover | 1,665 | 1,664 | 1664 |
| SM_Container_Reliquary / SM_ContainerReliquaryHull | 2,007 | 2,082 | 2082 |
| SM_Loot_GoldChalice / SM_Loot_GoldChalice | 2,798 | 3,064 | 3064 |
| SM_Loot_PhilosophersStone / SM_Loot_PhilosophersStone | 1,768 | 2,384 | 2384 |
| SM_PoorRug / SM_Poor_Rug | 149 | 154 | 154 |
| SM_Table / SM_Table_Cloth | 590 | 853 | 853 |
| SM_Table / SM_Table | 2,868 | 2,808 | 2808 |
| SM_WallTorch / SM_WallTorch | 2,886 | 3,311 | 3311 |
| SM_VFX_CandleFlame / SM_VFX_CandleFlame | 34 | 32 | 32 |
| **Product total (including three native mesh assets)** | **40,448** | **44,352** | |

## Textures

58 PNGs including the Overview label atlas: 1 x 256x256, 3 x 512x512, 12 x 1024x1024, 1 x 2048x1024, 41 x 2048x2048. Metallic PBR: BaseColor sRGB, OpenGL Normal (+Y) linear, MAS linear (Metallic R / AO G / Smoothness A; blue unused). The Blue banner has three 2048 maps; PoorRug uses one 1024 BaseColor map. Free fire sheets retain nine original frames in 4 x 4, 2048 x 2048 atlases. Candle flame uses a 256 x 256 RGBA procedural texture and animated camera-facing segmented mesh.

## Features

Two rows of wall modules; matching full-height corridor; wall infills with UVs adjusted for half-width placement; separate door leaf; realtime lighting; torch/fireplace particles; camera-facing candle flame with sway, distortion and flicker; original Generic Knight rig with automatic looping Idle; labelled asset Overview per pipeline. Source assets preserve the Complete's paths and GUIDs.

## Included Content

Knight; brick wall; arch wall; wooden plinth; floor; ceiling/beams; door and handles/frame; window; fireplace; Blue banner; PoorRug; table; candlestick; reliquary chest with lid and keyhole; gold chalice; philosopher's stone; wall torch; fire particle prefab; candle flame prefab; left/right half-width UV wall meshes. The two infill Mesh assets retain 5 m geometry and use X scale 0.5 in the Demo and Overview.

## Demo Scene

`Free/Scenes/LastWizard_Free_Demo_BRP.unity`, `_URP.unity`, `_HDRP.unity`. All three retain the approved room/corridor placements and 21 point lights, with pipeline-specific intensity calibration. The closed door is deliberate current scene content. Each scene has one static camera and no movement controller. The corridor is closed at its far end. Fireplace, torches and the candle animate in Play. No baked lightmaps.

## Overview Scene

`Free/Scenes/LastWizard_Free_Overview_BRP.unity`, `_URP.unity`, `_HDRP.unity`. The perspective main camera shows 17 grouped stations: architecture at the back, Knight/furniture/torch/fire in the middle, and candlestick/chalice/stone close to the camera on display plinths. All 19 prefabs and both infill mesh variants are present. Floor tiles use the pack's floor module at native scale; a curved neutral backdrop and soft key/fill lighting ground the set. Plates sit below/in front of their exhibits, facing the camera, with no product-mesh or label overlap in the checked 16:9 projection.

All product instances use native scale (1,1,1), including the candle flame and fire systems. The two wall infills use their intended X scale 0.5 to make 2.5 m panels from the 5 m UV-variant mesh; their label explicitly marks the exception. Floor/ceiling/rug rotation is for display only. The Knight plays Idle. Fire is shown in the fireplace, torch and standalone station; the candlestick carries its separate animated flame child. Scene lights, plinths and label geometry require no scripts.

Each Overview has one enabled main camera and one disabled optional `CAM_Overview_Candle_Detail`. To inspect the native candle close-up, disable the main camera and enable the detail camera manually. In Play the particles animate automatically; allow several seconds to warm up before capturing.

## Setup Instructions

1. Built-in: install Shader Graph 17.0.4 before import. URP/HDRP include Shader Graph; install the matching pipeline package for your project.
2. Import `LastWizard_Free.unitypackage`.
3. Built-in: Graphics and Quality render pipelines must be None. URP: assign `Free/Settings/URP/PC_RPAsset`. HDRP: assign `Free/Settings/HDRP/HDRP High Fidelity`. Clear conflicting Quality pipeline overrides.
4. Open the matching Demo or Overview scene. Other pipeline scenes contain native components that require their corresponding Unity packages.
5. Press Play. The camera is static; the Knight plays Idle and the effects animate. Use Scene view navigation to inspect the layout or add your own controller. The supplied door collider blocks the closed doorway.

## Animations and Controller

| Clip | Seconds | FPS |
|---|---:|---:|
| A_Knight_Idle | 4.433 | 30 |
| A_Knight_Walk | 3.300 | 30 |
| A_Knight_Attack | 2.167 | 30 |
| A_Knight_Death | 2.800 | 30 |

`AC_Knight_Free.controller` is assigned to `P_Knight_Free`: Idle default, Walk, Attack and Death states. Idle/Walk restart through zero-duration self-transitions without changing original clip imports. All four clips are in place with no root curves. No Run/Jump clips or scripted StateMachineBehaviours are included. The Generic rig is custom, not a Humanoid retargeting setup.

## Notes

All geometry prefabs have authored colliders. The two pure VFX prefabs have small disabled trigger spheres; they do not block players or raycasts. Lights live in scenes. The philosopher's stone retains its authored convex MeshCollider; Unity reports a 256-polygon convex-hull limit warning and uses a partial hull. Existing URP room-light shadow-atlas/bias warnings may appear; approved lighting is retained. Shader compile messages and runtime errors are checked separately from these warnings.

Historical spelling `LastWizzard` is retained for compatibility. No gameplay code is included. Tested on Windows / NVIDIA RTX 3060 using Unity 6000.0.77f1; player builds, mobile, other operating systems and graphics APIs are not claimed. AI-generated content: Yes (confirmed by the artist 2026-10-29); the source Last Wizard pack was made with an AI-assisted workflow and finished by artists. The candle texture is procedural, not AI generated.

## Support

Contact support through the product's store page.

## License

Use under the applicable store license for the acquired assets.
