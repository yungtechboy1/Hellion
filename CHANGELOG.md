## Hellion ChangeLog

### Update #7 (27/01/2017)

- Common
  - Create `Hellion.Database` project to handle everyting realated witht the database.

- World
  - Npc data loading
  - Npc oral dialog
  - Teleport command

### Update #6 (13/01/2017)

- Common
  - NPC Dialog structure

- World
  - Add all GM and admin commands verification
  - Monster moves
  - Dialog loader

### Update #5 (06/01/2017)

- World
  - Monster visibility

### Update #4 (30/12/2016)

- Common
  - Update to .NET Core 1.1
  - Remove unused dependencies
  - Rgn file structure

- Tools
  - DefineToConst : Converts a flyff header file (.h) to a C# const file.
  
- Cluster
  - FIX: HP/MP/FP at character creation
  
- World
  - Chat system (normal chat)
  - Load region from .rgn files
  - Begin of monster respawner


### Update #3 (23/12/2016)

- Common
  - Add ResourceTable reader (to read files like propItem.txt, propSkills.txt, etc...)
  - Add WldFile structure

- LoginServer
  - Fix bug "Account already connected"

- WorldServer
  - Data loading
    - Items (propItem.txt)
  - Inventory
    - Viewable equiped items (Current player and others)
    - Item move
    - Equip/Unequip
  - Player
    - Save informations in database at logout


### Update #2 (16/12/2016)


- Common
  - New incoming packet management using attributes
  - Change folder structure

- World Server
    - Data loading
        - Defines loading
        - Map loading (.dyo)
    - Player login
    - Player visibility with other players
    - Player moves
    - NPC

### Update #1 (09/12/2016)

- InterServer Communication
- Login Server
    - Connect
    - Disconnect
- Cluster Server
    - Character List
    - Create character
    - Delete character
    - Login Protect (On/Off)
