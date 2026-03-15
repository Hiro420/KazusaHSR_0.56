# KazusaHSR_0.56
Open-source server reimplementation for an early version of an anime game with stars and rails\
This project was in work months ago, and then was abandoned and now uploaded\
Main purpose of this project is preservation of the oldest game version we currently have

# Usage
- Download [MongoDB 4.0+](https://www.mongodb.com/try/download/community)
- Compile via Visual Studio 2022
- Download game data from [the resources repo](https://github.com/Hiro420/KazusaHSR_Data) and place them in `resources` folder, next to the executable
- Download [0.56](https://archive.org/details/20210920-2150-v-0.56-live-178389-osceregress-win-0.56.0-ostest-release.-7z) version of a certain anime game 
- Compile and place [client patch](https://github.com/Hiro420/AnimeGamePatch_0.56) in the game folder, next to the executable
- Profit

### Features
- Natural enemy/calyx spawn
- All playable characters fully working
- Working fights
- Properly implemented Ability management
- Working open-world logic
- Working challenges
- Useful commands

### Commands
- help -> lists all commands
- clear -> clears the cmd window from logs
- list -> lists all active sessions
- target -> select a session by uid to use commands on
- level -> sets your adventure level
- give \<item|avatar|lightcone\> [id] [amount] [level] -> gives specific item, character or lightcone by ID
- giveall \<items|avatars\> [level] -> same as give command, but gives everything
- avatar \<all|id\> \<lv|level|e|eidolon|sl|skill\> \<value\> -> modifies a specific character
- scene \<sceneId\> \<floorId\> -> teleport to a specific scene (floorId is optional)
- mp \<set|add|status|refill\> \<value\> -> manages your MP (status by default)

### Personal TODO List
- SP management
- Add more Commands
- Merge task executors into one
- Save fallback scene info db

Copyright© Hiro420