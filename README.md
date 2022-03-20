# AutoMapper

## ChroMapper plugin

This is a stripped-down port of Lolighter, now as a plugin for ChroMapper.

Explanation: https://www.youtube.com/watch?v=tH3t3J9Aymw

### Features

-   Lightshow Generator
-   Map Generator from Notes
-   Map Generator from Audio
-   Generate note as timing note only
-   Specific note selection for each algorithm

### Options

Mapping value:
0.01 is the default value for onset to be placed by the algorithm
-   Max Speed = The minimum distance between two note generated.
-   Double Threshold = The onset value need to be above this value to generate a double note on same beat.
-   Max double speed = Remove double also known as gallops if there is a note too close to the double (in beat).

Detection value:
-   Onset Sensitivity = Threshold Multiplier for the average of an onset. If the onset is above that average, the onset is kept.
-   Indistinguishable Range = Value divided by to find the "Number of set of samples to ignore after an onset". Setting it higher will reduce that immunity period.

Light value:
-   Speed = Color will switch value every x (beat).
-   Offset = Add an offset from first note for the color switch (in beat).
-   Boost = Boost will turn on/off every x (beat).

### Credits

[Lolighter](https://github.com/Loloppe/Lolighter) by [Loloppe](https://github.com/Loloppe)
[ChroMapper-Lolighter](https://github.com/KivalEvan/ChroMapper-Lolighter) by [Kival Evan](https://github.com/KivalEvan/)
