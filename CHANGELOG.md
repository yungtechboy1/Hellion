## Hellion ChangeLog

### Update #3 (23/12/2016) (In progress...)

- Common
  - Add ResourceTable reader (to read files like propItem.txt, propSkills.txt, etc...)
  - Add WldFile structure

- WorldServer
  - Data loading
    - Items (propItem.txt)
  - Inventory
    - Viewable equiped items (Current player and others)
  - Player
    - Save informations in database at logout


### Update #2 (16/12/2016)


- Common
  - New incoming packet management using attributes
  - Change folder structure

- World Server
    - Data loading
        - Defines loading
        - Map loading (.dyo / .rgn)
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
