# agAIo - Evolving bots eating each other

We all love the online massive multiplayer game [agar.io](http://agar.io) where you need to grow your cell large by other players' cells. This is a simulation where cells controlled by AIs play against each other, which emerges to a race between "genomes" (versions of code), loosely modelling natural selection. The game is extremely easy and fun to play - just start the simulation and watch the colourful circles! :)

## Details
Each cell is controlled by one of the few AI scripts we devised. New small cells pop up constantly and large cells have to eat small cells in order not to lose mass. Resorces (space and total mass of organisms) are scarce so different versions of AI code will necessarily differ in fitness. Cells can split at any time, and with each split some characteristics of the cells (such as maximum speed or acceleration) evolve. Moreover, AIs can identify neighbouring cells as "friendly" (having the same code), thus are able to cooperate and spread their "genome" instead of ingesting each other. 

## Technology
Game mechanics are written in C#, using the Unity framework. AIs were written in Lua script. We used [MoonSharp](https://www.moonsharp.org), a Lua interpreter for Unity to hook the scripts into our simulation.

## Authors
Made at [HackCambridge Ternary](https://hackcambridge.com/), on 20-21 Jan 2018 by Team TungusGames.

Members:

Tamara Eszterhás

Péter Mernyei

Benedek Stadler

Tamás Tardos

## Screenshot
Labels on the left show the total mass for each colour-coded script/"species".

![Screensot 5](Screenshots/agAIo5.PNG)
