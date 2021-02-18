# foldup
Windows app that synchronizes a target folder with the original.

Use this application to quickly copy working directories to a backup folder.  Backups are defined 
in a small configuration file.  Each entry becomes a command line argument for the program.

For example the following configuration entry:
```json
[
  {
    "title": "apiserver",
    "description": "The server's source code",
    "source": "C:\\code\\source\\APIServer",
    "dest": "C:\\users\\me\\OneDrive\\APIServer",
    "ignoreFolders": [ ".git", ".vs", "packages", "bin", "obj" ]
  }
]
```
will enable the command line argument "--apiserver" so you can make a backup by typing
```foldup --apiserver```

Foldup will create/update/delete files and folders in the destination directory to match the source.

### Update Feb 2021
Foldup no longer creates log files.  Just redirect the standard output to wherever it's needed.