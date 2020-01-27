***New Enemy Movement***

5 out of the 6 enemy types will randomly move one of 3 ways:

1.  Straight down

2.  40 degrees left

3.  40 degrees right

***Player Ammo***

![](.//media/image1.png){width="2.791809930008749in"
height="0.7153149606299213in"}

![](.//media/image2.png){width="4.257163167104112in"
height="2.2362259405074365in"}***Wave System***

Spawn Manager controls:

-   Which wave is Boss wave

-   Wave Multiplier

-   Wave Increment

Wave Multiplier and Increment work together to essentially add 5 enemies
to each wave incrementally starting with 10 in the first wave
(Multiplier + Increment).

Wave increments will continue until Boss wave is reached.

Wave number displayed before each wave.

![](.//media/image3.png){width="0.5986111111111111in"
height="0.7701388888888889in"}***Negative Pickup***

"Slow Down" pickup will slow the player down for 5 seconds and flash the
screen.

![](.//media/image4.png){width="1.0694444444444444in"
height="1.3406474190726159in"}***New Enemy Type***

Nickname "Zig-Zag", new Enemy has zig-zag motion as it moves down the
screen.

Shoots "Beam" randomly. When fired, beam flashes quickly rather than a
solid beam.

Effects Player same as "normal" enemy laser beam.

![](.//media/image8.png){width="2.865972222222222in"
height="4.952083333333333in"}![](.//media/image9.png){width="3.2416666666666667in"
height="4.065972222222222in"}***Balanced Spawning***

Probabilities for Enemies and Power Ups is controlled in the Spawn
Manager.

***Enemy with Shield***

![](.//media/image10.png){width="0.6701388888888888in"
height="0.8659722222222223in"}New enemy type. Takes 1 hit to remove
shield.

Shield power-down sound with hit.

***Aggressive Enemy Type***

![](.//media/image11.png){width="0.6430555555555556in"
height="0.8152777777777778in"}

Nickname "Enemy Diver" will try to ram the player as it gets within a
certain range.

Ramming range controlled by Child of Player with a Collider2D.

![](.//media/image12.png){width="0.7513888888888889in"
height="0.7513888888888889in"} ***Smart Enemy***

Nickname "Front\_Back" will fire up or down depending on whether Y
position of player is greater than or equal to enemy Y position.

***Enemy Pickups***

Enemy continuously uses Raycast to see if Power Up is within 6 units to
decide whether to fire laser.

Even accidentally hitting a Power Up will destroy the power up.

Power Up flashes and plays sound when destroyed.

***Pickup Collect***

Press C at any time to move all Power Ups on the screen to the player -
including "Negative" Power Ups.

They will move to player at 2x normal speed and can still be destroyed
by the Enemies.

Message on Asteroid screen displays instruction for 5 seconds or until
asteroid is destroyed.

![](.//media/image13.png){width="0.7569444444444444in"
height="0.9548611111111112in"}***Enemy Avoid Shot***

Nickname "Dodger" appears to use satellite to detect Player lasers.

Uses a Child Laser Detector with Collider2D to detect whether Player
laser in range.

If Player laser in range, uses X position of laser to angle 40 degrees
left or right.

![](.//media/image14.png){width="0.5090277777777777in"
height="0.7895833333333333in"}***Homing Projectile***

Nickname "Homer". Power Up will make all Player lasers find closest
enemy -- including Boss.

Lasts for 5 seconds. If no enemies remain, will eventually fly out of
range and be destroyed by script.

***Boss AI***

![](.//media/image15.png){width="1.187671697287839in"
height="0.9490441819772528in"}Boss starts by moving down the screen to
the center, then randomly moves ("Dodges") around the center of the
screen.

Player cannot fire while boss is moving down the screen.

New music for Boss Wave.

Boss shoots new projectiles.

-   Projectiles come in waves. \<Quantity and time between waves
    controlled in Inspector\>

-   Time between each shot within a wave controlled within inspector
    (e.g. .5)

-   Projectiles move toward player position at the time projectile was
    fired.

-   Boss takes 20 hits to kill \<controlled in Inspector\>

-   Killing the boss:

    -   A series of explosions takes place.

    -   Game Over screen displays "Victory!" and offers same Restart
        options as Game Over.

![](.//media/image16.png){width="5.590565398075241in"
height="2.250115923009624in"}