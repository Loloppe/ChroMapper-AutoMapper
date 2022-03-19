# AutoMapper

## ChroMapper plugin

This is a stripped-down port of Lolighter, now as a plugin for ChroMapper.

Explanation: https://www.youtube.com/watch?v=tH3t3J9Aymw

### Features

-   Lightshow Generator
-   Map Generator from Notes
-   Map Generator from Audio

### Options

Mapping logic value:
Max Speed = The minimum distance between two note generated.
Double Threshold = The onset value need to be above this value to generate a double note on same beat.
0.01 = default value for note to be placed by the algorithm
Max double speed = Remove double also known as gallops if there is a note too close to the double (in beat).

Detection value:
Onset Sensitivity = I recommend between 1.3 to 1.5 for decent timing, it catch more data and noise the lower it is.
Indistinguishable Range = Another value that modify if a note is kept or not during the algorithm. Similar to the Onset Sensitivity, but higher will mean more notes.

I recommend to use Indistinguishable Range and Onset Sensitivity to overmap/undermap. The rest of the value are mostly just limiter on top.

### Credits

[Lolighter](https://github.com/Loloppe/Lolighter) by [Loloppe](https://github.com/Loloppe)
[ChroMapper-Lolighter](https://github.com/KivalEvan/ChroMapper-Lolighter) by [Kival Evan](https://github.com/KivalEvan/)
