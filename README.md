# Belmont's Revenge - Crowd Control alpha1 submission

Public review candidate, prepared 2026-11-01. Author/maintainer: SiREJ (enelsondev).
Game pack ID: BelmontsRevengeCC. Platform: Game Boy. Connector: GBConnector / BizHawk Lua.
This is a separate Crowd Control build based on the accepted alpha17 practice ROM.
Experimental alpha18/19 movement-speed changes are excluded.

## Contents

- BelmontsRevengeCC.cs: effect pack source for SDK review/loading.
- BelmontsRevengeCC.dll: compiled pack (SDK 5.0.9776.23265).
- BelmontsRevengeCC.json and en-US.json: generated pack/menu metadata.
- belmont-crowd-control-alpha1.ips: patch for the clean US reference ROM.
- QA.md: behavior, evidence, limitations, and suggested review tests.
- DISCORD-TICKET.md: proposed ticket text; not an already submitted request.
- manifest.json and SHA256SUMS.txt: artifact identities.

No ROM, BIOS, save states, emulator, SDK, or private repository history is included.

## Required versions and patching

Tested with BizHawk 2.9.1, Gambatte core, the official SDK 5.0.9776.23265,
and its unchanged connector.lua (Lua connector version 2.3.0).
That connector rejects BizHawk 2.11.1; use the tested 2.9.1 installation.

Apply the IPS to a legally obtained CLEAN US Belmont's Revenge ROM, not to an
already patched practice ROM. Use an IPS-capable patcher (for example Floating IPS).
The resulting image is 262144 bytes. Verify the hashes below before testing.

Clean US source SHA-256:
17570ceec1b22153604622c4412d048dd8f7ccb4626daf9ddea96de8a062dbf2
Patched CC alpha1 SHA-256:
5564a852dfe4ba96990a64af2bbf8a2ad7a7c75851405ac4a6c19430d6a99c26
Patched CC alpha1 MD5 (enforced by pack):
a0beaa14075fdabe3b78bd96ea0c7912

The patch retains the underlying practice feature routines and unlimited retries,
but the practice menu is inaccessible. Japanese Cross behavior/art is available
alongside US Axe and Holy Water through Crowd Control effects.
It retains the US castle layouts. No Japanese ROM is required to APPLY the patch.
Copyrighted game assets are not supplied as standalone files; distribution is patch-only.

## Reproduce the local SDK test

1. Install the official Crowd Control SDK and obtain BizHawk 2.9.1.
2. In the SDK use Load Pack Source to load BelmontsRevengeCC.cs.
3. Under Game Connector choose Emulator (Lua), then click Connect.
4. In BizHawk 2.9.1, load the patched CC alpha1 ROM and cold-boot.
5. Open Tools > Lua Console, open the SDK's ConnectorScripts/connector.lua,
   and leave the console open. Keep its support files in their original folder.
6. Start normal gameplay, unpaused. Expand Equipment or Player in the SDK,
   select an effect, and press the play/test button. Test Kill last.
7. If a request is temporarily rejected during an attack or transition, wait
   until normal gameplay resumes and test again.

The Lua connection uses localhost port 23884. Do not run another test host on
that port. Configure your controller separately in this portable BizHawk install.
No live stream or viewer purchase is needed for these SDK tests.

## Client integration requested

This pack is not registered/listed in the normal Crowd Control client yet.
We request review, supported setup/packaging guidance, and a controlled live-test
path. Public availability, prices, effect icons, and any additional assets remain
subject to the Crowd Control team's requirements. The private development repo
will remain private; this submission does not grant repository access.

## Dedicated CC revision - 2026-11-01

Menu access is disabled; native Start pause/resume, feature routines and unlimited
retries remain. Title: CROWD CONTROL - BY SIREJ. These files supersede the earlier
alpha1. Eight command tests and menu lockout/resume pass. Revised SDK pack compiles;
The maintainer confirmed this revised build: menu disabled, all eight SDK commands
working, and updated title credit displayed correctly. Viewer-session testing
remains outstanding.

## Download

[Download the alpha1 submission ZIP](https://github.com/enelsondev/belmonts-revenge_crowd-control/releases/download/alpha1/crowd-control-alpha1-submission.zip).

This is an experimental integration awaiting Crowd Control review, not an
officially supported or listed game pack. See [QA notes](QA.md) for limitations.
The [practice ROM distribution](https://github.com/enelsondev/belmonts-revenge-practice)
is maintained separately. No game ROM is distributed here.
