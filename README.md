User documentation for HoMM3: Battle edition.

Getting started:
1. Install VLC player via ```sudo apt install vlc``` or ```sudo dnf install vlc```
2. Clone project to your machine
3. Open terminal in repository
4. Run ```dotnet run```

Manual:

After setting up the application, you will notice following window:
![image](https://github.com/user-attachments/assets/451f8cfa-6304-4e96-bf38-68569e1c049a)
There, you can choose to start the game, change settings or exit the application.

The settings window:
![image](https://github.com/user-attachments/assets/2cf15194-9ece-42d7-8c19-f1aaa15eecac)
The only available option is to off/on the sound.

The play window:
![image](https://github.com/user-attachments/assets/5edd566f-bc6e-4213-a517-3b22751ffcc0)
The game, possibly in the future, may have multiplayer mode, but for now singleplayer is only one available.

The starting conditions window:
![image](https://github.com/user-attachments/assets/575b8c42-815c-46f1-b039-97ecd4fd178d)
There you can set up number and types of units.
Also, some presets are available:
1. Random start. Chooses unit types uniformally random.
2. Balanced start. Chooses by 1 unit of each level to both sides
3. Testing start. Gives one side 999 Angels, and 1 Peasant to another team.
The number of units depends on the level of unit: while units like Peasant or Skeleton may appear in ranges 70-140, unit like Angel or Bone Dragon will form for a stack of range 10-20.

The following documentation will show Balanced start scenarion.
The battlefield:
![image](https://github.com/user-attachments/assets/a6b33af8-69b7-4419-9fdd-c6a6e6ac6645)
The type of game is hot-seat PvP. The team which loses all its units looses the battle.
Units appeared in the queue in the order of their speed.

We can move units by clicking on the hexagones of the grey colour.
The unit which move is now is showen in the queue in the beggining, and also marked by the yellow hexagon on the battlefield.
While all units have the same musical accompaniment, Zombie and Bone Dragon have different sound of attack.

Technical notes:
The GUI is made using GTK.
The source code is made in OOP style.
Assets used in the game almost fully AI-generated, except some musical effects.
