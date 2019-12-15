# SpaceShooterPro
LEFT SHIFT TURBO
* Working and consumes more "fuel"

SHIELD STRENGTH
* Shields color indicate 3 strengths (Green, Yellow Red)

AMMO COUNT
* Ammo limited to 15 at start and at refill
* Starts off with 99 at Astroid
* Screen indicator when Low on Ammo
* Screen indicator when Out of Ammo
* Clicking sound when trying to fire when out of ammo

AMMO COLLECTIBLE
* PowerUp created to refill ammo (15)

HEALTH COLLECTIBLE
* RED CROSS health collectible adds life if less then 3

SECONDARY FIRE WEAPON
* New Projectile is a laser "burst" shooting 7 lasers in different directions
* Last 5 seconds
* Using Weighted Random in SpawnManager to lesson probability of laser burst

Thruster HUD
* Visual bar in upper left showing "fuel" level
* Moving ship reduces fuel
* When not moving, cooldown of 1.5 secs before recharge starts
* If fuel runs out, thrusters turn off until recharge starts

Camera Shake
* CameraShake script added to Main Camera
* Camera parented to CameraContainer to reset back to 0,0,0 after shake.  This solved a problem when shaking on Player death and camera left at shaken position off center
