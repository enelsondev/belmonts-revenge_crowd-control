Hi! I'd like to submit a new Game Boy effect pack for review and eventual normal Crowd Control client use.

Project: Castlevania II: Belmont's Revenge - Crowd Control alpha1
Maintainer: SiREJ / enelsondev
Pack ID: BelmontsRevengeCC

This uses a separate patched US practice-ROM build, BizHawk 2.9.1 (Gambatte), and the official SDK Lua connector. Eight one-shot effects are implemented: Axe, Holy Water, Cross, remove subweapon, 99 hearts, zero hearts, full health, and Kill Christopher.

The pack compiles with SDK 5.0.9776.23265. Automated ROM/connector tests passed, and I've successfully tested all eight effects on this revised build using the SDK's local effect controls, confirmed that the practice menu is inaccessible, and verified the Crowd Control title credit. No viewer-purchase session has been tested yet.

Attached are the C# source, compiled pack, generated JSON, IPS patch, setup instructions, hashes, and QA notes. No ROMs or save states are included. The private development repository will remain private.

Could you advise on review/registration, how best to package the patched-ROM requirement and BizHawk connector setup for the normal client, and the process for a limited live test? Prices are provisional; please let me know any icon, metadata, or other release requirements.

Thanks!

## Dedicated CC revision - 2026-11-01

Menu access is disabled; native Start pause/resume, feature routines and unlimited
retries remain. Title: CROWD CONTROL - BY SIREJ. These files supersede the earlier
alpha1. Eight command tests and menu lockout/resume pass. Revised SDK pack compiles;
Captain confirmed this revised build: menu disabled, all eight SDK commands
working, and updated title credit displayed correctly. Viewer-session testing
remains outstanding.
