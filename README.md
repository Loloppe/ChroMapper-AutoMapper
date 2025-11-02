# AutoMapper

## ChroMapper plugin

Completely new automapper, audio to onsets and light generation algorithm.

### How to use

Press Tab while mapping, an AutoMapper text icon should appear on the menu on the right side of the screen.  
Clicking that icon will open the AutoMapper menu.  
Pressing the Audio button will generate the map based on the audio file.  
Pressing the Map button will remap the existing notes, or the selected notes.  
Pressing the Light button will generate lights based on the existing notes, or the selected notes.

### Features

-   Lightshow Generator (based on note timings)
-   Map Generator from Notes
-   Map Generator from Audio
-   Generate note in a specific audio range
-   Generate note as timing only
-   Note selection for each algorithm

### Options

**Audio**:
0.01 is the default value for an onset (note) to be placed by the algorithm
-   Max Speed : The minimum distance between two note generated.
-   Double Threshold : The onset value need to be above this value to generate a double note on same beat.
-   Max double speed : Remove double also known as gallops if there is a singular note too close to the double (in beat).
-   Onset Sensitivity : Threshold Multiplier for the average of an onset. If the onset is above that average, the onset is kept.
-   Indistinguishable Range : Value divided by to find the "Number of set of samples to ignore after an onset". Setting it higher will reduce that immunity period.
-   Audio Range : Min and Max value (in beat) where notes will be generated.

**Map** :
-   Bottom Row Only : All the notes are generated bottom row.
-   Up and Down Only : All the notes are either down or up cut direction (still flow).
-   Randomize Line : Randomise the line index of each note.
-   Fused Double Only : Make double notes the same line and layer.
-   Timing Only : Only dot notes, all aligned bottom left.
-   Wrist Limiter : When enabled, remove extreme wrist rotation.

**Light**:
-   Speed : Color will switch value every x (beat).
-   Offset : Add an offset from first note for the color switch (in beat).
-   Boost : Boost will turn on/off every x (beat).
-   Use Chroma : Generate Chroma lights based on Onsets detection.
-   Ignore Bombs : Skip bombs while generating lights.
-   Nerf Strobes : Reduce fast color swap, remove fast strobe spam, etc.
-   Use Boost Events : Apply Boosts events every 8 beats (on/off)

### Credits

[Lolighter](https://github.com/Loloppe/Lolighter) by [Loloppe](https://github.com/Loloppe)
[ChroMapper-Lolighter](https://github.com/KivalEvan/ChroMapper-Lolighter) by [Kival Evan](https://github.com/KivalEvan/)
