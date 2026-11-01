# QA and known limitations

## Effect list (all one-shot)

| Effect ID | Result | Development price |
| --- | --- | --- |
| weapon_axe | Give Axe | 50 |
| weapon_water | Give Holy Water | 50 |
| weapon_cross | Give Japanese-style Cross | 50 |
| weapon_none | Remove subweapon / clear HUD icon | 50 |
| hearts_max | Set hearts to 99 | 50 |
| hearts_none | Set hearts to 0 | 50 |
| health_full | Set health to 12 | 100 |
| kill_player | Native lethal-health path | 200 |

Prices are placeholders, not approved live pricing. No speed modifiers, enemy
spawns, invincibility, or multi-jump effects are offered by this pack.

## Evidence

- Automated BGB checks passed: idle gameplay matches alpha17; eight commands and
  native death/respawn; duplicate sequence suppression; pause/context/invalid
  request guards; active-projectile guard.
- Dedicated BizHawk 2.9.1, cold-booted with controller inputs, passed all eight
  commands through the unchanged official Lua connector using a local wire host.
- The maintainer reported successful local SDK effect-button tests on
  2026-11-01. This is manual user validation, separate from the automated checks.
- No Twitch purchase, viewer-facing live session, or normal-client integration
  has been validated. No MiSTer Crowd Control connection is claimed.

## Behavior and review focus

The ROM processes an ABI1 mailbox at C8E0..C8EF in its main-thread gameplay hook.
The connector submits context and command before committing a request sequence.
The ROM acknowledges only after applying or rejecting the command. One request
is outstanding at a time. The pack serializes local effect handling.

Title, pause/menu, death, and room-transition states reject or disable effects.
Weapon changes also reject temporarily while the player/weapon is busy; existing
projectiles are not deleted. A request created for another room/screen context
is rejected rather than applying later in the new room. SDK may retry temporary
failures according to its scheduling; the ROM does not queue deferred changes.

Hearts use packed BCD; health uses the native HUD/lethal-health path. Kill causes
normal death and respawn, with alpha17's unlimited retries. CC weapon requests
are exact even if the practice menu's Japanese weapon rules are selected.

Timeout after sequence commit reports permanent/uncertain failure, never success
or an automatic retry of a potentially executed Kill. It attempts cancellation;
this is not an exactly-once guarantee over reset, rollback, disconnect, or process
failure. Avoid loading emulator states/resetting during active requests.

Suggested reviewer checks: all eight effects during normal gameplay; equipment
request while an Axe/Cross is active; pause/title/transition rejection; native
Kill and respawn; connection interruption with an in-flight request; repeated
requests from the SDK. The shared Axe/Cross art can temporarily retain an outgoing
projectile's silhouette during a native pickup; this is inherited alpha17 behavior.

No claim of exhaustive all-room or concurrency/failure testing is made.

## Dedicated CC revision - 2026-11-01

Menu access is disabled; native Start pause/resume, feature routines and unlimited
retries remain. Title: CROWD CONTROL - BY SIREJ. These files supersede the earlier
alpha1. Eight command tests and menu lockout/resume pass. Revised SDK pack compiles;
The maintainer confirmed this revised build: menu disabled, all eight SDK commands
working, and updated title credit displayed correctly. Viewer-session testing
remains outstanding.

## Revised CC edition acceptance - 2026-11-01

The maintainer manually verified the current submission build:

- Practice menu cannot be opened.
- All eight effects work through the SDK test controls.
- Title displays CROWD CONTROL - BY SIREJ.

These checks apply to the menu-disabled ROM in this bundle, not only the earlier
prototype. Normal-client integration and viewer-purchase/live-session behavior
have not been tested. This is a review candidate, not an approved release.
